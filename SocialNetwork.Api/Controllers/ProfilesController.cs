using Microsoft.AspNet.Identity;
using SocialNetwork.Api.Data;
using SocialNetwork.Api.Models;
using SocialNetwork.Core.Models;
using SocialNetwork.Data.Repositories;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Net.Http;
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
        public IHttpActionResult Post(ProfileBindingModel model)
        {
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
