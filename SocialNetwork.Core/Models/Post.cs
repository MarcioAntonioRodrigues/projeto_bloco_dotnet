using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocialNetwork.Core.Models
{
    public class Post
    {
        public int PostId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string PictureUrl { get; set; }
        public string Category { get; set; }
        public string Distance { get; set; }
        public string AverageSpeed { get; set; }
        public int ProfileId { get; set; }
    }
}
