using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AxaHosting.Model;

namespace AxaHosting.Data.Configuration
{
    public class ServerConfiguration : EntityTypeConfiguration<Server>
    {
        public ServerConfiguration()
        {
            ToTable("Servers");
            Property(c => c.ExternalIp).IsRequired().HasMaxLength(15);
            Property(c => c.InternalIp).IsRequired().HasMaxLength(15);

        }
    }
}
