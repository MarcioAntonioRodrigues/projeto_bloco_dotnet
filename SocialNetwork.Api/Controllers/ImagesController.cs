using Microsoft.AspNet.Identity;
using Microsoft.Azure;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using Newtonsoft.Json;
using SocialNetwork.Api.Data;
using SocialNetwork.Api.Models;
using SocialNetwork.Core.Models;
using SocialNetwork.Web.Models;
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
    [RoutePrefix("api/Images")]
    public class ImagesController : ApiController
    {
        private DataContext _dataContext;
        private BlobCreator _blobCreator;

        public ImagesController()
        {
            _dataContext = new DataContext();
            _blobCreator = new BlobCreator();
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
                model.Url = await _blobCreator.CreateBlob(result.Contents[1], model.Title);
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
            Image image = _dataContext.Image.Where(i => i.ImageId == id).FirstOrDefault();
            if(image != null)
            {
                _dataContext.Image.Remove(image);
                _dataContext.SaveChanges();
            }
        }
        [Route("GetImageById/{id}")]
        public ImageViewModel GetImageById(int id)
        {
            Image imageBind = _dataContext.Image.Where(i => i.ImageId == id).FirstOrDefault();
            ImageViewModel image = new ImageViewModel()
            {
                ImageId = imageBind.ImageId,
                Title = imageBind.Title,
                Subtitle = imageBind.Subtitle,
                Url = imageBind.Url
            };
            return image;
        }

    }
}
