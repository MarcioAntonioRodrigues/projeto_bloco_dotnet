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
                        Session["Profile"] = await response.Content.ReadAsAsync<Profile>();

                        return RedirectToAction("Index", "Home");
                    }

                    return View("Error");
                }
            }
            return RedirectToAction("Register", "Account", null);
        }
    }
}