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
    public class GalleryController : Controller
    {
        // GET: Gallery
        public ActionResult Index()
        {
            IEnumerable<GalleryViewModel> gals = (IEnumerable<GalleryViewModel>)Session["Galleries"];
            return View(gals);
        }

        // GET: Gallery/Details/5
        public async Task<ActionResult> Details(int id)
        {
            ActionResult x = await GetGalleryById(id);
            GalleryViewModel gallery = (GalleryViewModel)Session["Gallery"];

            return View(gallery);
        }

        // GET: Gallery/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Gallery/Create
        [HttpPost]
        public ActionResult Create(FormCollection collection)
        {
            try
            {



                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: Gallery/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: Gallery/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: Gallery/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: Gallery/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        public async Task<ActionResult> GetGalleryById(int id)
        {
            string access_token = Session["access_token"]?.ToString();

            if (!string.IsNullOrEmpty(access_token))
            {
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri("http://localhost:24260/");
                    client.DefaultRequestHeaders.Accept.Clear();

                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", $"{access_token}");

                    var response = await client.GetAsync("/api/Gallery/" + id);

                    if (response.IsSuccessStatusCode)
                    {
                        Session["Gallery"] = await response.Content.ReadAsAsync<GalleryViewModel>();

                        return RedirectToAction("Details", "Gallery");
                    }

                    return View("Error");
                }
            }
            return RedirectToAction("Index", "Profile");

        }


        public async Task<ActionResult> GetGalleries()
        {
            string access_token = Session["access_token"]?.ToString();

            if (!string.IsNullOrEmpty(access_token))
            {
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri("http://localhost:24260/");
                    client.DefaultRequestHeaders.Accept.Clear();

                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", $"{access_token}");

                    var response = await client.GetAsync("/api/Gallery");

                    if (response.IsSuccessStatusCode)
                    {
                        Session["Galleries"] = await response.Content.ReadAsAsync<List<GalleryViewModel>>();

                        return RedirectToAction("Index", "Gallery");
                    }

                    return View("Error");
                }
            }
            return RedirectToAction("Index", "Profile");
        }
    }
}
