using SocialNetwork.Core.Models;
using SocialNetwork.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace SocialNetwork.Web.Controllers
{
    public class HomeController : Controller
    {
        public string Access_token { get; set; }

        public async Task<ActionResult> Index()
        {
            Session["UserLoged"] = System.Web.HttpContext.Current.User.Identity.Name;
            ActionResult x = await BuscarPerfil();
            Access_token = Session["access_token"]?.ToString();
            if (string.IsNullOrEmpty(Access_token))
            {
                return RedirectToAction("Register", "Account", null);
            }
            else
            {
                ProfileViewModel profileFromLogedUser = (ProfileViewModel)Session["Profile"];
                if(profileFromLogedUser == null)
                {
                    return RedirectToAction("Create", "Profile", null);
                }
                else
                {
                    ViewBag.profileFromLogedUser = profileFromLogedUser;
                    List<ProfileViewModel> profiles = await GetFriendsList();
                    List<PostViewModel> postsList = await GetAllUsersPosts();
                    foreach(var p in postsList)
                    {
                        foreach(var pf in profiles)
                        {
                            if (p.ProfileId == pf.Id)
                            {
                                p.Profile = pf;
                            }
                        }
                    }
                    ViewBag.Posts = postsList;
                    return View();
                }
                return View();
            }
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        public async Task<ActionResult> BuscarPerfil()
        {
            string access_token = Session["access_token"]?.ToString();

            if (!string.IsNullOrEmpty(access_token))
            {
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri("http://localhost:24260/");
                    client.DefaultRequestHeaders.Accept.Clear();

                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", $"{access_token}");

                    var response = await client.GetAsync("/api/Profiles/UserInfo");

                    if (response.IsSuccessStatusCode)
                    {
                        Session["Profile"] = await response.Content.ReadAsAsync<ProfileViewModel>();

                        return RedirectToAction("Index", "Home");
                    }

                    return View("Error");
                }
            }
            return RedirectToAction("Register", "Account", null);
        }

        public async Task<List<PostViewModel>> GetAllUsersPosts()
        {
            string access_token = Session["access_token"]?.ToString();
            if (string.IsNullOrEmpty(access_token))
            {
                return null;
            }
            if (ModelState.IsValid)
            {
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri("http://localhost:24260/");
                    client.DefaultRequestHeaders.Accept.Clear();
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", $"{access_token}");
                    var response = await client.GetAsync("/api/Posts/GetAllPosts");
                    if (response.IsSuccessStatusCode)
                    {
                        List<PostViewModel> postslist = await response.Content.ReadAsAsync<List<PostViewModel>>();
                        return postslist;
                    }
                    else
                    {
                        return null;
                    }
                }
            }
            return null;
        }

        // Get Frinds List
        [HttpGet]
        public async Task<List<ProfileViewModel>> GetFriendsList()
        {
            string access_token = Session["access_token"]?.ToString();

            if (string.IsNullOrEmpty(access_token))
            {
                return null;
            }
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("http://localhost:24260/");
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", $"{access_token}");
                var response = await client.GetAsync("/api/Profiles/GetFriendsList");
                if (response.IsSuccessStatusCode)
                {
                    List<ProfileViewModel>profileslist = await response.Content.ReadAsAsync<List<ProfileViewModel>>();
                    return profileslist;
                }
                return null;
            }
        }
    }
}