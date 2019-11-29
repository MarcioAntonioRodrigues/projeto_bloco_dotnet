using Newtonsoft.Json;
using SocialNetwork.Core.Models;
using SocialNetwork.Web.Models;
using System;
using System.Collections.Generic;
using System.IO;
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
        public async Task<ActionResult> Index()
        {
            string access_token = Session["access_token"]?.ToString();

            if (string.IsNullOrEmpty(access_token))
            {
                return RedirectToAction("Login", "Account", null);
            }
            else
            {
                ActionResult x = await GetGalleries();
                IEnumerable<GalleryViewModel> gals = (IEnumerable<GalleryViewModel>)Session["Galleries"];
                return View(gals);
            }
        }

        // GET: Gallery/Details/5
        public async Task<ActionResult> Details(int id)
        {
            ActionResult x = await GetImagesByGalleryId(id);
            GalleryViewModel gallery = GetGallery(id);
            ICollection<ImageViewModel> images = (ICollection<ImageViewModel>)Session["Images"];
            gallery.Images = images;
            Session["GalleryId"] = gallery.GalleryId;
            return View(gallery);
        }

        // GET: Gallery/Create
        public ActionResult Create()
        {
            string access_token = Session["access_token"]?.ToString();

            if (string.IsNullOrEmpty(access_token))
            {
                return RedirectToAction("Login", "Account", null);
            }
            else
            {
                return View();
            }
        }

        // POST: Gallery/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(GalleryViewModel model)
        {
            string access_token = Session["access_token"]?.ToString();

            if (string.IsNullOrEmpty(access_token))
            {
                return RedirectToAction("Login", "Account", null);
            }

            GalleryViewModel gallery = new GalleryViewModel()
            {
                Name = model.Name
            };

            try
            {
                if (ModelState.IsValid)
                {
                    using (var client = new HttpClient())
                    {
                        client.BaseAddress = new Uri("http://localhost:24260/");
                        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", $"{access_token}");
                        var response = await client.PostAsJsonAsync("api/Gallery", gallery);

                        if (response.IsSuccessStatusCode)
                        {
                            return RedirectToAction("Index", "Gallery");
                        }
                        return View("Error");
                    }
                }
                return RedirectToAction("Index", "Gallery");
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

        public async Task<ActionResult> GetImagesByGalleryId(int id)
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
                        Session["Images"] = await response.Content.ReadAsAsync<List<ImageViewModel>>();

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

        public GalleryViewModel GetGallery(int id)
        {
            IEnumerable<GalleryViewModel> gals = (IEnumerable<GalleryViewModel>)Session["Galleries"];
            GalleryViewModel gal = gals.Where(g => g.GalleryId == id).FirstOrDefault();
            return gal;
        }

        public ActionResult AddImageToGallery()
        {
            return RedirectToAction("Create", "Images");
        }
    }
}
