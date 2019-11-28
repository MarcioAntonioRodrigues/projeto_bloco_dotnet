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
    public class GalleryController : ApiController
    {
        private DataContext _dataContext;

        public GalleryController()
        {
            _dataContext = new DataContext();
        }

        // GET: api/Gallery
        public IEnumerable<GalleryBindModel> Get()
        {
            var accountId = User.Identity.GetUserId();

            Profile p = _dataContext.Profile.Where(c => c.AccountId == accountId).FirstOrDefault();

            var dataGallery = _dataContext.Gallery.Where(g => g.ProfileId == p.Id).ToList();

            List<GalleryBindModel> ProfileGallery = new List<GalleryBindModel>();

            foreach (var gal in dataGallery)
            {
                GalleryBindModel gallery = new GalleryBindModel()
                {
                    GalleryId = gal.GalleryId,
                    Name = gal.Name,
                    ProfileId = gal.ProfileId
                };
                ProfileGallery.Add(gallery);
            }

            return ProfileGallery;
        }

        // GET: api/Gallery/5
        public List<Image> Get(int id)
        {
            Gallery gallery = _dataContext.Gallery.Where(g => g.GalleryId == id).FirstOrDefault();
            List<Image> images = _dataContext.Image.Where(i => i.GalleryId == id).ToList();



            //GalleryBindModel gal = new GalleryBindModel()
            //{
            //    Name = gallery.Name,
            //    GalleryId = gallery.GalleryId,
            //    Images = images,
            //};

            return images;
        }

        // POST: api/Gallery
        public void Post([FromBody]string value)
        {
        }

        // PUT: api/Gallery/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE: api/Gallery/5
        public void Delete(int id)
        {
        }
    }
}
