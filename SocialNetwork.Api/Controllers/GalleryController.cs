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
    [RoutePrefix("api/Gallery")]
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

        // GET: api/Gallery
        [Route("GetGalleriesById/{id}")]
        public IEnumerable<GalleryBindModel> GetGalleriesById(int id)
        {
            var dataGallery = _dataContext.Gallery.Where(g => g.ProfileId == id).ToList();

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
        public List<ImageBindModel> Get(int id)
        {
            Gallery gallery = _dataContext.Gallery.Where(g => g.GalleryId == id).FirstOrDefault();
            List<Image> images = _dataContext.Image.Where(i => i.GalleryId == id).ToList();
            List<ImageBindModel> bindImages = new List<ImageBindModel>();
            foreach (var i in images)
            {
                ImageBindModel image = new ImageBindModel()
                {
                    Title = i.Title,
                    Subtitle = i.Subtitle,
                    Url = i.Url,
                    GalleryId = i.GalleryId,
                    ImageId = i.ImageId
                };
                bindImages.Add(image);
            }

            return bindImages;
        }

        // POST: api/Gallery
        public void Post(GalleryBindModel Gallery)
        {
            var accountId = User.Identity.GetUserId();

            var profile = _dataContext.Profile.Where(p => p.AccountId == accountId).FirstOrDefault();
            var profileId = profile.Id;

            var Gal = new Gallery()
            {
                Name = Gallery.Name,
                ProfileId = profileId
            };

            _dataContext.Gallery.Add(Gal);
            _dataContext.SaveChanges();
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
