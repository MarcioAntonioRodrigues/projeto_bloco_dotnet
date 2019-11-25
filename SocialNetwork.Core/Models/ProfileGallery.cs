using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocialNetwork.Core.Models
{
    class ProfileGallery
    {
        public int ProfileId { get; set; }
        public int GalleryId { get; set; }
        public Profile Profile { get; set; }
        public Gallery Gallery { get; set; }
    }
}
