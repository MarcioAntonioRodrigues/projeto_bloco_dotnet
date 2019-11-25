using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocialNetwork.Core.Models
{
    class Image
    {
        public int ImageId { get; set; }
        public string Title { get; set; }
        public string Url { get; set; }
        public string Subtitle { get; set; }
        public Gallery Gallery { get; set; }
    }
}
