using AxaHosting.Data.Configuration;

using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AxaHosting.Model;

namespace AxaHosting.Data
{
    public class AxaHostingEntities : DbContext
    {
        public AxaHostingEntities() : base("AxaHostingEntities") { }

       
        public DbSet<Server> Servers { get; set; }
        
        public DbSet<Product> Products { get; set; }
        public DbSet<Model.Database> Databases { get; set; }
        public DbSet<WebHotel> WebHotels { get; set; }

        public virtual void Commit()
        {
            base.SaveChanges();
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            
            modelBuilder.Configurations.Add(new ServerConfiguration());
            modelBuilder.Configurations.Add(new WebHotelConfiguration());
            
            modelBuilder.Configurations.Add(new DatabaseConfiguration());
            modelBuilder.Configurations.Add(new ProductConfiguration());
            modelBuilder.Configurations.Add(new PreparedIpConfiguration());
        }
    }
}
