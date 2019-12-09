using SocialNetwork.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SocialNetwork.Web.Models
{
    public class PostViewModel
    {
        public int PostId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string PictureUrl { get; set; }
        public string Category { get; set; }
        public string Distance { get; set; }
        public string AverageSpeed { get; set; }
        public int ProfileId { get; set; }
        public ProfileViewModel Profile { get; set; }
    }
}