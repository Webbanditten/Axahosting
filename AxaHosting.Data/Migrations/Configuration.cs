namespace AxaHosting.Data.Migrations
{
    using Model;
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<AxaHosting.Data.AxaHostingEntities>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(AxaHosting.Data.AxaHostingEntities context)
        {
            /*
            context.Servers.AddOrUpdate(p=>p.ExternalIp,
                new Model.Server {
                    ExternalIp = "1.4.0.26",
                    Type = ServerType.Iis,
                    InternalIp = "10.0.10.50",
                    LastUpdated = DateTime.Now,
                    Load = 0,
                    Username = "",
                    Password = "Pa$$w0rd"
                },
                new Model.Server
                {
                    ExternalIp = "1.4.0.25",
                    Type = ServerType.MySql,
                    InternalIp = "10.0.10.103",
                    LastUpdated = DateTime.Now,
                    Load = 0,
                    Username = "functions",
                    Password = "Pa$$w0rd"
                },
                new Model.Server
                {
                    ExternalIp = "1.4.0.24",
                    Type = ServerType.MsSql,
                    InternalIp = "10.0.10.102",
                    LastUpdated = DateTime.Now,
                    Load = 0,
                    Username = "sa",
                    Password = "Pa$$w0rd"
                }
                );

            context.Products.AddOrUpdate(new Product
            {
                Color = "purple",
                ServerType = ServerType.Iis,
                MonthlyPricing = (decimal)129.99,
                MaxCpu = 200,
                MaxMailAccounts = 100,
                ProductType = ProductType.WebHotel,
                Name = "Enterprise"

            },
                new Product
                {
                    Color = "purple",
                    ServerType = ServerType.Iis,
                    MonthlyPricing = (decimal)49.99,
                    MaxCpu = 100,
                    MaxMailAccounts = 30,
                    ProductType = ProductType.WebHotel,
                    Name = "Private"

                },
                new Product
                {
                    Color = "purple",
                    ServerType = ServerType.MsSql,
                    MonthlyPricing = (decimal)29.99,
                    MaxDbGb = 10,
                    ProductType = ProductType.Database,
                    Name = "MSSQL Database"

                },
                new Product
                {
                    Color = "purple",
                    ServerType = ServerType.MySql,
                    MonthlyPricing = (decimal)14.99,
                    MaxDbGb = 10,
                    ProductType = ProductType.Database,
                    Name = "MySQL Database"

                });
            context.Commit();*/

            //  This method will be called after migrating to the latest version.

            //  You can use the DbSet<T>.AddOrUpdate() helper extension method 
            //  to avoid creating duplicate seed data. E.g.
            //
            //    context.People.AddOrUpdate(
            //      p => p.FullName,
            //      new Person { FullName = "Andrew Peters" },
            //      new Person { FullName = "Brice Lambson" },
            //      new Person { FullName = "Rowan Miller" }
            //    );
            //
        }
    }
}
