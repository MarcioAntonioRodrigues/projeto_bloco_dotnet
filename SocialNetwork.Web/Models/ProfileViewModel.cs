using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace SocialNetwork.Web.Models
{
    public class ProfileViewModel
    {
        public int Id { get; set; }
        [Display(Name = "Nome")]
        public string FirstName { get; set; }
        [Display(Name = "Sobrenome")]
        public string LastName { get; set; }
        [Display(Name = "Data de aniversário")]
        public string BirthDate { get; set; }
        [Display(Name = "Foto de perfil")]
        public string PictureUrl { get; set; }
    }
}