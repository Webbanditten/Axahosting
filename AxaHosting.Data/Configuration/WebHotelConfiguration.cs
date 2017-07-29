using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AxaHosting.Model;

namespace AxaHosting.Data.Configuration
{
    public class WebHotelConfiguration : EntityTypeConfiguration<WebHotel>
    {
        public WebHotelConfiguration()
        {
            ToTable("WebHotels");
            Property(c => c.ProductId).IsRequired();
            Property(c => c.Domain).IsRequired().HasMaxLength(64);
            Property(c => c.Owner).IsRequired().HasMaxLength(64);
        }
    }
}
