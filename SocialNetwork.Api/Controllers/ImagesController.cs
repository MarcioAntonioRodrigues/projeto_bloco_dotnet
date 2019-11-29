using Microsoft.AspNet.Identity;
using Microsoft.Azure;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using Newtonsoft.Json;
using SocialNetwork.Api.Data;
using SocialNetwork.Api.Models;
using SocialNetwork.Core.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;

namespace SocialNetwork.Api.Controllers
{
    public class ImagesController : ApiController
    {
        private DataContext _dataContext;

        public ImagesController()
        {
            _dataContext = new DataContext();
        }

        // GET: api/Images
        public List<ImageBindModel> Get()
        {
            var accountId = User.Identity.GetUserId();
            Profile p = _dataContext.Profile.Where(c => c.AccountId == accountId).FirstOrDefault();
            var dataGallery = _dataContext.Gallery.Where(g => g.ProfileId == p.Id).ToList();
            List<ImageBindModel> images = new List<ImageBindModel>();

            foreach (var g in dataGallery)
            {
                foreach(var f in g.Images)
                {
                    if(f.GalleryId == g.GalleryId)
                    {
                        ImageBindModel i = new ImageBindModel()
                        {
                            ImageId = f.ImageId,
                            Title = f.Title,
                            Subtitle = f.Subtitle,
                            Url = f.Url,
                            GalleryId = f.GalleryId
                        };

                        images.Add(i);
                    }
                }
            }

            return images;
        }

        // GET: api/Images/5
        public string Get(int id)
        {
            return "value";
        }

        // POST: api/Images
        public async Task<IHttpActionResult> Post()
        {
            if (!Request.Content.IsMimeMultipartContent())
            {
                return BadRequest();
            }

            var result = await Request.Content.ReadAsMultipartAsync();

            var requestJson = await result.Contents[0].ReadAsStringAsync();

            var model = JsonConvert.DeserializeObject<ImageBindModel>(requestJson);

            if (result.Contents.Count > 1)
            {
                model.Url = await CreateBlob(result.Contents[1], model.Title);
            }

            var image = new Image()
            {
                Title = model.Title,
                Subtitle = model.Subtitle,
                Url = model.Url,
                GalleryId = model.GalleryId
            };

            if (image != null)
            {
                _dataContext.Image.Add(image);
                _dataContext.SaveChanges();
            }

            return Ok();
        }

        // PUT: api/Images/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE: api/Images/5
        public void Delete(int id)
        {
        }

        private async Task<string> CreateBlob(HttpContent httpContent, string blobName)
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
            if (fileName == null)
            {
                return null;
            }
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
    }
}
