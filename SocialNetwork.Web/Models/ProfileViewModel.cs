using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SocialNetwork.Web.Models
{
    public class ProfileViewModel
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime BirthDate { get; set; }
    }
}