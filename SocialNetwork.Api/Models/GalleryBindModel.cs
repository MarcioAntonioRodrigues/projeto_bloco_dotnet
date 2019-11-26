using SocialNetwork.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SocialNetwork.Api.Models
{
    public class GalleryBindModel
    {
        public int GalleryId { get; set; }

        public string Name { get; set; }
        public ICollection<Image> Images { get; set; }
        public Profile Profile { get; set; }
        public int ProfileId { get; set; }
    }
}