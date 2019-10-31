using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace SocialNetwork.Web.Models
{
    public class RegisterViewModel
    {
        [Required]
        [Display(Name = "E-mail")]
        public string Email { get; set; }
        [Required]
        [Display(Name = "Senha")]
        public string Password { get; set; }
        [Required]
        [Display(Name = "Confirme sua senha")]
        [Compare("Password", ErrorMessage = "A senha e a confirmação da senha estão diferentes.")]
        public string ConfirmPassword { get; set; }
    }
}