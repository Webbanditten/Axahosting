using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AxaHosting.Model;

namespace AxaHosting.Data.Configuration
{
    public class ProductConfiguration : EntityTypeConfiguration<Product>
    {
        public ProductConfiguration()
        {
            ToTable("Products");
            Property(c => c.ServerType).IsRequired();
            Property(c => c.ProductType).IsRequired();
            Property(c => c.Color).IsRequired().HasMaxLength(64);
            Property(c => c.Name).IsRequired().HasMaxLength(64);
            Property(c => c.MonthlyPricing).IsRequired();
        }
    }
}
