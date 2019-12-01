using Microsoft.AspNet.Identity;
using Microsoft.Azure;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using Newtonsoft.Json;
using SocialNetwork.Api.Data;
using SocialNetwork.Api.Models;
using SocialNetwork.Core.Models;
using SocialNetwork.Data.Repositories;
using SocialNetwork.Web.Models;
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
        private BlobCreator _blobCreator;

        public ProfilesController()
        {
            _dataContext = new DataContext();
            _blobCreator = new BlobCreator();
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

        // PUT: api/Profiles/5
        [Route("EditProfile")]
        public async Task<IHttpActionResult> Put()
        {
            if (!Request.Content.IsMimeMultipartContent())
            {
                return BadRequest();
            }
            var accountId = User.Identity.GetUserId();
            Profile p = _dataContext.Profile.Where(x => x.AccountId == accountId).FirstOrDefault();

            var result = await Request.Content.ReadAsMultipartAsync();
            var requestJson = await result.Contents[0].ReadAsStringAsync();
            var model = JsonConvert.DeserializeObject<ProfileBindingModel>(requestJson);

            if (result.Contents.Count > 1)
            {
                model.PicutreUrl = await _blobCreator.CreateBlob(result.Contents[1], "Foto de perfil");
            }

            if (p != null)
            {
                p.FirstName = model.FirstName;
                p.LastName = model.LastName;
                p.BirthDate = model.BirthDate;
                if(model.PicutreUrl != null)
                {
                    p.PicutreUrl = model.PicutreUrl;
                }

                _dataContext.Profile.Add(p);
                _dataContext.Entry(p).State = EntityState.Modified;
                _dataContext.SaveChanges();
            }

            return Ok();
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
                model.PicutreUrl = await _blobCreator.CreateBlob(result.Contents[1], "Foto de perfil");
            }

            var accountId = User.Identity.GetUserId();

            var profile = new Profile()
            {
                AccountId = accountId,
                FirstName = model.FirstName,
                LastName = model.LastName,
                BirthDate = model.BirthDate,
                PicutreUrl = model.PicutreUrl
            };

            if (profile != null)
            {
                _dataContext.Profile.Add(profile);
                _dataContext.SaveChanges();

            }

            Image image = new Image()
            {
                Title = "Foto de perfil",
                Url = model.PicutreUrl
            };

            List<Image> images = new List<Image>();
            images.Add(image);

            Gallery gallery = new Gallery()
            {
                Name = "Fotos de perfil",
                Images = images,
                ProfileId = profile.Id
            };

            Gallery userGallery = _dataContext.Gallery.Add(gallery);

            profile.Galleries.Add(userGallery);

            _dataContext.Entry(profile).State = System.Data.Entity.EntityState.Modified;
            _dataContext.SaveChanges();

            return Ok();
        }

        [Route("GetBlobs")]
        public async Task<List<string>> GetBlobs()
        {
            var storageAccount = CloudStorageAccount.Parse(CloudConfigurationManager.GetSetting("StorageConnectionString"));

            var accountId = User.Identity.GetUserId();

            var blobContainerName = accountId;
            var blobClient = storageAccount.CreateCloudBlobClient();
            CloudBlobContainer container = blobClient.GetContainerReference(blobContainerName);

            await container.CreateIfNotExistsAsync();

            await container.SetPermissionsAsync(new BlobContainerPermissions { PublicAccess = BlobContainerPublicAccessType.Blob });

            BlobContinuationToken continuationToken = null;
            List<IListBlobItem> blobItems = new List<IListBlobItem>();

            do
            {
                var response = await container.ListBlobsSegmentedAsync(continuationToken);
                continuationToken = response.ContinuationToken;
                blobItems.AddRange(response.Results);
            }
            while (continuationToken != null);

            List<string> urisList = new List<string>();
             foreach(var item in blobItems)
            {
                urisList.Add(item.Uri.ToString());
            }

            return urisList;
        }

        // DELETE: api/Profiles/5
        public void Delete(int id)
        {
        }

        [Route("UserInfo")]
        public Profile GetProfile()
        {
            var accountId = User.Identity.GetUserId();
            Profile p = (from x in _dataContext.Profile where x.AccountId == accountId select x).FirstOrDefault();
            return p;
        }

        [Route("GetProfileById/{id}")]
        public Profile GetProfileById(int id)
        {
            Profile p = (from x in _dataContext.Profile where x.Id == id select x).FirstOrDefault();
            return p;
        }

        [Route("GetFriendsList")]
        public List<ProfileViewModel> GetFriendsList()
        {
            var profile = _dataContext.Profile;
            List<ProfileViewModel> profilesList = new List<ProfileViewModel>();
            foreach(var p in profile)
            {
                ProfileViewModel profileBind = new ProfileViewModel()
                {
                    BirthDate = p.BirthDate.ToString("dd/mm/yyyy"),
                    FirstName = p.FirstName,
                    LastName = p.LastName,
                    PictureUrl = p.PicutreUrl
                };
                profilesList.Add(profileBind);
            }
            return profilesList;
        }
    }
}
