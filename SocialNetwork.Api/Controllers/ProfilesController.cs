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
    public class ProfilesController : ApiController
    {
        // GET: api/Profiles
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET: api/Profiles/5
        public string Get(int id)
        {
            return "value";
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

            var repository = new ProfileRepository();
            repository.Create(profile);

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
    }
}
