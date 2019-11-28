using SocialNetwork.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SocialNetwork.Api.Models
{
    public class ImageBindModel
    {
        public int ImageId { get; set; }
        public string Title { get; set; }
        public string Url { get; set; }
        public string Subtitle { get; set; }
        public Gallery Gallery { get; set; }
        public int GalleryId { get; set; }
    }
}