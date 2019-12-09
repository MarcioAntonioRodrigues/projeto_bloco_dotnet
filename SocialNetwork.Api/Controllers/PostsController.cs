using Microsoft.AspNet.Identity;
using Newtonsoft.Json;
using SocialNetwork.Api.Data;
using SocialNetwork.Core.Models;
using SocialNetwork.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;

namespace SocialNetwork.Api.Controllers
{
    [RoutePrefix("api/Posts")]
    public class PostsController : ApiController
    {
        private DataContext _dataContext;
        private BlobCreator _blobCreator;

        public PostsController()
        {
            _dataContext = new DataContext();
            _blobCreator = new BlobCreator();
        }

        // GET: api/Posts
        public List<PostViewModel> Get()
        {
            var accountId = User.Identity.GetUserId();
            Profile p = _dataContext.Profile.Where(x => x.AccountId == accountId).FirstOrDefault();

            List<Post> Postslist = _dataContext.Post.Where(post => post.ProfileId == p.Id).ToList();
            List<PostViewModel> ModeList = new List<PostViewModel>();

            foreach (var post in Postslist)
            {
                PostViewModel postModel = new PostViewModel()
                {
                    PictureUrl = post.PictureUrl,
                    Title = post.Title,
                    Description = post.Description,
                    Distance = post.Distance,
                    AverageSpeed = post.AverageSpeed,
                    Category = post.Category,
                    ProfileId = post.ProfileId
                };

                ModeList.Add(postModel);
            }

            return ModeList;
        }

        // GET: api/Posts/5
        public string Get(int id)
        {
            return "value";
        }

        // POST: api/Posts
        public async Task<IHttpActionResult> Post()
        {
            if (!Request.Content.IsMimeMultipartContent())
            {
                return BadRequest();
            }

            var result = await Request.Content.ReadAsMultipartAsync();
            var requestJson = await result.Contents[0].ReadAsStringAsync();
            var model = JsonConvert.DeserializeObject<PostViewModel>(requestJson);

            if (result.Contents.Count > 1)
            {
                model.PictureUrl = await _blobCreator.CreateBlob(result.Contents[1], model.Title);
            }

            var accountId = User.Identity.GetUserId();
            Profile p = _dataContext.Profile.Where(x => x.AccountId == accountId).FirstOrDefault();

            var post = new Post()
            {
                Title = model.Title,
                Description = model.Description,
                Category = model.Category,
                AverageSpeed = model.AverageSpeed,
                Distance = model.Distance,
                ProfileId = p.Id,
                PictureUrl = model.PictureUrl
            };

            if (post != null)
            {
                _dataContext.Post.Add(post);
                _dataContext.SaveChanges();
            }

            return Ok();
        }

        // PUT: api/Posts/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE: api/Posts/5
        public void Delete(int id)
        {
        }

        // GET: api/Posts
        [Route("GetAllPosts")]
        public List<PostViewModel> GetAllPosts()
        {
            List<Post> Postslist = _dataContext.Post.ToList();
            List<PostViewModel> ModeList = new List<PostViewModel>();

            foreach (var post in Postslist)
            {
                PostViewModel postModel = new PostViewModel()
                {
                    PictureUrl = post.PictureUrl,
                    Title = post.Title,
                    Description = post.Description,
                    Distance = post.Distance,
                    AverageSpeed = post.AverageSpeed,
                    Category = post.Category,
                    ProfileId = post.ProfileId
                };

                ModeList.Add(postModel);
            }
            return ModeList;
        }

        // GET: api/Posts
        [Route("GetPostsByUserId/{id}")]
        public List<PostViewModel> GetPostsByUserId(int id)
        {
            List<Post> Postslist = _dataContext.Post.Where(p=>p.ProfileId == id).ToList();
            List<PostViewModel> ModeList = new List<PostViewModel>();
            foreach (var post in Postslist)
            {
                PostViewModel postModel = new PostViewModel()
                {
                    PictureUrl = post.PictureUrl,
                    Title = post.Title,
                    Description = post.Description,
                    Distance = post.Distance,
                    AverageSpeed = post.AverageSpeed,
                    Category = post.Category,
                    ProfileId = post.ProfileId
                };
                ModeList.Add(postModel);
            }
            return ModeList;
        }
    }
}
