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
    public class ImagesController : Controller
    {
        // GET: Images
        public ActionResult Index()
        {
            return View();
        }

        // GET: Images/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: Images/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Images/Create
        [HttpPost]
        public async Task<ActionResult> Create(ImageViewModel model)
        {
            string access_token = Session["access_token"]?.ToString();

            if (string.IsNullOrEmpty(access_token))
            {
                return RedirectToAction("Login", "Account", null);
            }

            ImageViewModel image = new ImageViewModel()
            {
                Title = model.Title,
                Subtitle = model.Subtitle
            };

            try
            {
                if (ModelState.IsValid)
                {
                    using (var client = new HttpClient())
                    {
                        client.BaseAddress = new Uri("http://localhost:24260/");
                        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", $"{access_token}");
                        var response = await client.PostAsJsonAsync("api/Images", image);

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

        // GET: Images/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: Images/Edit/5
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

        // GET: Images/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: Images/Delete/5
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
    }
}
