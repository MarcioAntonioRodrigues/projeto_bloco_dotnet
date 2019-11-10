using Microsoft.AspNet.Identity.EntityFramework;
using SocialNetwork.Api.Models;
using SocialNetwork.Core.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace SocialNetwork.Api.Data
{
    //public class MyUser: IdentityUser
    //{
    //    public virtual Profile Profile { get; set; }
    //}

    public class DataContext : DbContext
    {
        public DataContext()
            : base("DefaultConnection")
        {
        }

        public DbSet<Profile> Profile { get; set; }
    }
}