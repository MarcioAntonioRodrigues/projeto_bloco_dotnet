using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SocialNetwork.Api.Models
{
    public class ProfileBindingModel
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime BirthDate { get; set; }
        public string PicutreUrl { get; set; }
        public string AccountId { get; set; }
    }
}