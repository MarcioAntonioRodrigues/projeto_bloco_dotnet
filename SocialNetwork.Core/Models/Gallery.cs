using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocialNetwork.Core.Models
{
    public class Gallery
    {
        public int GalleryId { get; set; }
        public string Name { get; set; }
        public ICollection<Image> Images { get; set; }
        public Profile Profile { get; set; }
        public int ProfileId { get; set; }
    }
}
