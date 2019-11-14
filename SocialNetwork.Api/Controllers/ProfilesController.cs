using Microsoft.AspNet.Identity;
using Microsoft.Azure;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using Newtonsoft.Json;
using SocialNetwork.Api.Data;
using SocialNetwork.Api.Models;
using SocialNetwork.Core.Models;
using SocialNetwork.Data.Repositories;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;

namespace SocialNetwork.Api.Controllers
{
    //[Authorize]
    [RoutePrefix("api/Profiles")]
    public class ProfilesController : ApiController
    {
        private DataContext _dataContext;

        public ProfilesController()
        {
            _dataContext = new DataContext();
        }

        // GET: api/Profiles
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET: api/Profiles/5
        public DbSet<Profile> Get(int id)
        {
            return _dataContext.Profile;
        }

        // POST: api/Profiles
        public async Task<IHttpActionResult> Post()
        {
            if(!Request.Content.IsMimeMultipartContent())
            {
                return BadRequest();
            }

            var result = await Request.Content.ReadAsMultipartAsync();

            var requestJson = await result.Contents[0].ReadAsStringAsync();

            var model = JsonConvert.DeserializeObject<ProfileBindingModel>(requestJson);

            if(result.Contents.Count > 1)
            {
                model.PicutreUrl = await CreateBlob(result.Contents[1]);
            }

            var accountId = User.Identity.GetUserId();

            var profile = new Profile()
            {
                AccountId = accountId,
                FirstName = model.FirstName,
                LastName = model.LastName,
                BirthDate = model.BirthDate
            };

            if(profile != null)
            {
                _dataContext.Profile.Add(profile);
                _dataContext.SaveChanges();

            }

            return Ok();
        }

        private async Task<string> CreateBlob(HttpContent httpContent)
        {
            var storageAccount = CloudStorageAccount.Parse(CloudConfigurationManager.GetSetting("StorageConnectionString"));

            var blobContainerName = "api-amigo-fotos";
            var blobClient = storageAccount.CreateCloudBlobClient();
            var blobContainer = blobClient.GetContainerReference(blobContainerName);

            await blobContainer.CreateIfNotExistsAsync();

            await blobContainer.SetPermissionsAsync(
                new BlobContainerPermissions
                {
                    PublicAccess = BlobContainerPublicAccessType.Blob
                });

            var fileName = httpContent.Headers.ContentDisposition.FileName;
            var byteArray = await httpContent.ReadAsByteArrayAsync();

            var blob = blobContainer.GetBlockBlobReference(GetRandomBlobName(fileName));
            await blob.UploadFromByteArrayAsync(byteArray, 0, byteArray.Length);

            return blob.Uri.AbsoluteUri;
            
        }

        private string GetRandomBlobName(string fileName)
        {
            string ext = Path.GetExtension(fileName);
            return string.Format("{0:10}_{1}{2}", DateTime.Now.Ticks, Guid.NewGuid(), ext);
        }

        // PUT: api/Profiles/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE: api/Profiles/5
        public void Delete(int id)
        {
        }

        [Route("UserInfo")]
        public Profile GetProfileById()
        {
            var accountId = User.Identity.GetUserId();
            Profile p = (from x in _dataContext.Profile where x.AccountId == accountId select x).FirstOrDefault();
            return p;
        }
    }
}
