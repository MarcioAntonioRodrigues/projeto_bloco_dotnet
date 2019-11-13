using SocialNetwork.Core.Models;
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

        public ActionResult Index()
        {
             Access_token = Session["access_token"]?.ToString();

            if (string.IsNullOrEmpty(Access_token))
            {
                return RedirectToAction("Login", "Account", null);
            }
            else
            {
                return View(VerifyProfileAsync());
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

        public async Task<ActionResult> VerifyProfileAsync()
        {
            Access_token = Session["access_token"]?.ToString();

            if (!string.IsNullOrEmpty(Access_token))
            {
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri("http://localhost:24260/");
                    client.DefaultRequestHeaders.Accept.Clear();

                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", $"{Access_token}");

                    var response = await client.GetAsync("/api/Profiles/UserInfo");

                    if (response.IsSuccessStatusCode)
                    {
                        var profile = await response.Content.ReadAsAsync<Profile>();

                        if (profile != null)
                        {
                            ViewBag.profile = true;
                        }
                        else
                        {
                            ViewBag.profile = false;
                        }
                    }
                    else
                    {
                        return View("Error");
                    }
                }
            }
            return RedirectToAction("Index", "Home");
        }
    }
}