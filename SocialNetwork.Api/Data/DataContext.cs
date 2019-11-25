using Microsoft.AspNet.Identity.EntityFramework;
using SocialNetwork.Api.Models;
using SocialNetwork.Core.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Linq;
using System.Web;

namespace SocialNetwork.Api.Data
{
    public class DataContext : DbContext
    {
        public DataContext()
            : base("DefaultConnection")
        {
        }

        public DbSet<Profile> Profile { get; set; }
        public DbSet<Gallery> Gallery { get; set; }
        public DbSet<Friend> Friend { get; set; }
        public DbSet<Image> Image { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();

            //Relacionamento um para muitos: Perfil X Galeria onde um perfil poderá ter muitas galerias
            modelBuilder.Entity<Gallery>()
               .HasRequired(s => s.Profile)
               .WithMany(c => c.Galleries)
               .HasForeignKey(s => s.ProfileId);

            //Relacionamento um para muitos: Galeria X Imagem onde uma galeria poderá ter muitas imagens
            modelBuilder.Entity<Image>()
              .HasRequired(s => s.Gallery)
              .WithMany(c => c.Images)
              .HasForeignKey(s => s.GalleryId);

            //Relacionamento um para muitos: Perfil X Amigo onde um perfil poderá ter muitos amigos
            modelBuilder.Entity<Friend>()
              .HasRequired(s => s.Profile)
              .WithMany(c => c.Friends)
              .HasForeignKey(s => s.ProfileId);

            base.OnModelCreating(modelBuilder);
        }
    }
}