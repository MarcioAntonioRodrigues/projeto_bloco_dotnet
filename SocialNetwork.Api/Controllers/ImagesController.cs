using Microsoft.AspNet.Identity;
using SocialNetwork.Api.Data;
using SocialNetwork.Api.Models;
using SocialNetwork.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
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
        public void Post([FromBody]string value)
        {
        }

        // PUT: api/Images/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE: api/Images/5
        public void Delete(int id)
        {
        }
    }
}
