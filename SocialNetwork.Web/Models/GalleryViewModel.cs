using SocialNetwork.Core.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace SocialNetwork.Web.Models
{
    public class GalleryViewModel
    {
        public int GalleryId { get; set; }
        [Display(Name = "Nome")]
        public string Name { get; set; }
        [Display(Name = "Imagens")]
        public ICollection<ImageViewModel> Images { get; set; }
        public Profile Profile { get; set; }
        public int ProfileId { get; set; }
    }
}