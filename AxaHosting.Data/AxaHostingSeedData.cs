
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AxaHosting.Model;

namespace AxaHosting.Data
{
    public class AxaHostingSeedData : DropCreateDatabaseIfModelChanges<AxaHostingEntities>
    {
        
        protected override void Seed(AxaHostingEntities context)
        {
            GetServers().ForEach(c => context.Servers.Add(c));
            GetProducts().ForEach(c => context.Products.AddOrUpdate(c));
            context.Commit();
        }

        private static List<Server> GetServers()
        {
            return null;
        }
        private static List<Product> GetProducts()
        {

            var products = new List<Product>
            {
                new Product
                {
                    Color = "purple",
                    ServerType = ServerType.Iis,
                    MonthlyPricing = (decimal) 129.99,
                    MaxCpu = 200,
                    MaxMailAccounts = 100,
                    ProductType = ProductType.WebHotel,
                    Name = "Enterprise"

                },
                new Product
                {
                    Color = "purple",
                    ServerType = ServerType.Iis,
                    MonthlyPricing = (decimal) 49.99,
                    MaxCpu = 100,
                    MaxMailAccounts = 30,
                    ProductType = ProductType.WebHotel,
                    Name = "Private"

                },
                new Product
                {
                    Color = "purple",
                    ServerType = ServerType.MsSql,
                    MonthlyPricing = (decimal) 29.99,
                    MaxDbGb = 10,
                    ProductType = ProductType.Database,
                    Name = "MSSQL Database"

                },
                new Product
                {
                    Color = "purple",
                    ServerType = ServerType.MySql,
                    MonthlyPricing = (decimal) 14.99,
                    MaxDbGb = 10,
                    ProductType = ProductType.Database,
                    Name = "MySQL Database"

                }
            };
            return products;
        }
        

    }
}
