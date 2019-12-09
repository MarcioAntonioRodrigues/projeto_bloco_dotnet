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
        //FriendsList view
        public async Task<ActionResult> FriendsList()
        {
            ActionResult x = await GetFriendsList();
            List<ProfileViewModel> profilesList = (List<ProfileViewModel>)Session["ProfilesList"];
            string access_token = Session["access_token"]?.ToString();

            if (string.IsNullOrEmpty(access_token))
            {
                return RedirectToAction("Login", "Account", null);
            }
            else
            {
                return View(profilesList);
            }
        }

        //Create Profile view
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

        //Create Profile view
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

        //Proflie details
        public ActionResult Details()
        {
            ProfileViewModel profile = (ProfileViewModel)Session["Profile"]; ;
            return View(profile);
        }

        //Search profile
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

        //Search profile 2
        public async Task<ActionResult> BuscarPerfil2()
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

                        return RedirectToAction("Edit", "Profile");
                    }

                    return View("Error");
                }
            }
            return RedirectToAction("Login", "Account", null);
        }

        //Search profile 2
        public async Task<ActionResult> GetProfileById(int id)
        {
            string access_token = Session["access_token"]?.ToString();

            if (!string.IsNullOrEmpty(access_token))
            {
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri("http://localhost:24260/");
                    client.DefaultRequestHeaders.Accept.Clear();

                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", $"{access_token}");

                    var response = await client.GetAsync("/api/Profiles/GetProfileById/" + id);

                    if (response.IsSuccessStatusCode)
                    {
                        Session["ProfileById"] = await response.Content.ReadAsAsync<Profile>();

                        return RedirectToAction("Edit", "ProfileFromListPage");
                    }

                    return View("Error");
                }
            }
            return RedirectToAction("Login", "Account", null);
        }

        //Edit profile view
        public ActionResult Edit()
        {
            ProfileViewModel profile = (ProfileViewModel)Session["Profile"];
            return View(profile);
        }

        //Edit profile
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> EditarPerfil(ProfileViewModel model)
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

                        var response = await client.PutAsync("/api/Profiles/EditProfile", content);

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

        // Get Frinds List
        [HttpGet]
        public async Task<ActionResult> GetFriendsList()
        {
            string access_token = Session["access_token"]?.ToString();

            if (string.IsNullOrEmpty(access_token))
            {
                return RedirectToAction("Login", "Account", null);
            }

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("http://localhost:24260/");
                client.DefaultRequestHeaders.Accept.Clear();

                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", $"{access_token}");

                var response = await client.GetAsync("/api/Profiles/GetFriendsList");

                if (response.IsSuccessStatusCode)
                {
                    Session["ProfilesList"] = await response.Content.ReadAsAsync<List<ProfileViewModel>>();

                    return RedirectToAction("FriendsList", "Profile");
                }
                return View("Error");
            }
        }

        public async Task<ActionResult> ProfileFromListPage(int id)
        {
            ActionResult x = await GetProfileById(id);
            Profile p = (Profile)Session["ProfileById"];
            ProfileViewModel profile = new ProfileViewModel()
            {
                Id = p.Id,
                FirstName = p.FirstName,
                LastName = p.LastName,
                PictureUrl = p.PicutreUrl,
                BirthDate = p.BirthDate.ToString("dd/mm/yyyy")
            };
            return View(profile);
        }
    }
}