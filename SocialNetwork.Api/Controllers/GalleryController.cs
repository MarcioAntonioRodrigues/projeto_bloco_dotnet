using Microsoft.AspNet.Identity;
using SocialNetwork.Api.Data;
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
        public IEnumerable<Gallery> Get()
        {
            var accountId = User.Identity.GetUserId();

            Profile p = _dataContext.Profile.Where(c => c.AccountId == accountId).FirstOrDefault();

            var dataGallery = _dataContext.Gallery.Where(g => g.ProfileId == p.Id).ToList();

            List<Gallery> ProfileGallery = new List<Gallery>();

            foreach (var gal in dataGallery)
            {
                ProfileGallery.Add(gal);
            }

            return ProfileGallery;
        }

        // GET: api/Gallery/5
        public string Get(int id)
        {
            return "value";
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
