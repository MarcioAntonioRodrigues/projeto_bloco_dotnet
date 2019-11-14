using Newtonsoft.Json;
using SocialNetwork.Core.Models;
using SocialNetwork.Web.Models;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace SocialNetwork.Web.Controllers
{
    public class ProfileController : Controller
    {
        //GET: Profile
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

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(ProfileViewModel model)
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

                        if(Request.Files.Count > 0)
                        {
                            byte[] fileBytes;
                            using (var inputStream = Request.Files[0].InputStream)
                            {
                                var memoryStream = inputStream as MemoryStream;
                                if(memoryStream == null)
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

                        var response = await client.PostAsync("/api/Profiles", content);

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

        public ActionResult Details()
        {
            string defaultProfileUrl = "https://marcioarmazenamento.blob.core.windows.net/api-amigo-fotos/profile.jpg";
            Profile p = (Profile)Session["Profile"];
            ProfileViewModel profile = new ProfileViewModel();
            profile.FirstName = p.FirstName;
            profile.LastName = p.LastName;
            profile.BirthDate = p.BirthDate.ToString("dd/MM");
            if(p.PicutreUrl != null)
            {
                ViewBag.PictureUrl = p.PicutreUrl;
            }
            else
            {
                ViewBag.PictureUrl = defaultProfileUrl;
            }

            return View(profile);
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

                        return RedirectToAction("Details", "Profile");
                    }

                    return View("Error");
                }
            }
            return RedirectToAction("Login", "Account", null);
        }

        public ActionResult Edit()
        {
            return View();
        }

        public async Task<ActionResult> EditarPerfil()
        {
            return null;
        }


    }
}