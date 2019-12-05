using Newtonsoft.Json;
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
    public class PostsController : Controller
    {
        // GET: Posts
        public async Task<ActionResult> Index()
        {
            ActionResult x = await GetPostsFromUser();
            List<PostViewModel> postsList = (List<PostViewModel>)Session["Posts"];
            return View(postsList);
        }

        // GET: Posts/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: Posts/Create
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

        // POST: Posts/Create
        [HttpPost]
        public async Task<ActionResult> Create(PostViewModel model)
        {
            string access_token = Session["access_token"]?.ToString();

            if (string.IsNullOrEmpty(access_token))
            {
                return RedirectToAction("Login", "Account", null);
            }

            if (ModelState.IsValid)
            {
                using (var client = new HttpClient())
                {
                    using (var content = new MultipartFormDataContent())
                    {
                        client.BaseAddress = new Uri("http://localhost:24260/");
                        client.DefaultRequestHeaders.Accept.Clear();

                        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", $"{access_token}");

                        content.Add(new StringContent(JsonConvert.SerializeObject(model)));

                        if (Request.Files.Count > 0)
                        {
                            byte[] fileBytes;
                            using (var inputStream = Request.Files[0].InputStream)
                            {
                                var memoryStream = inputStream as MemoryStream;
                                if (memoryStream == null)
                                {
                                    memoryStream = new MemoryStream();
                                    inputStream.CopyTo(memoryStream);
                                }
                                fileBytes = memoryStream.ToArray();
                            }
                            var fileContent = new ByteArrayContent(fileBytes);
                            fileContent.Headers.ContentDisposition = new ContentDispositionHeaderValue("attachment");
                            fileContent.Headers.ContentDisposition.FileName = Request.Files[0].FileName.Split('\\').Last();

                            content.Add(fileContent);
                        }

                        var response = await client.PostAsync("/api/Posts", content);

                        if (response.IsSuccessStatusCode)
                        {
                            return RedirectToAction("Index", "Home");
                        }
                        else
                        {
                            return View("Error");
                        }
                    }
                }
            }
            return View();
        }

        // GET: Posts/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: Posts/Edit/5
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

        // GET: Posts/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: Posts/Delete/5
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

        public async Task<ActionResult> GetPostsFromUser()
        {
            string access_token = Session["access_token"]?.ToString();

            if (string.IsNullOrEmpty(access_token))
            {
                return RedirectToAction("Login", "Account", null);
            }

            if (ModelState.IsValid)
            {
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri("http://localhost:24260/");
                    client.DefaultRequestHeaders.Accept.Clear();
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", $"{access_token}");

                    var response = await client.GetAsync("/api/Posts");

                    if (response.IsSuccessStatusCode)
                    {
                        Session["Posts"] = await response.Content.ReadAsAsync<List<PostViewModel>>();
                    }
                    else
                    {
                        return View("Error");
                    }
                }
            }
            return RedirectToAction("Index");
        }
    }
}
