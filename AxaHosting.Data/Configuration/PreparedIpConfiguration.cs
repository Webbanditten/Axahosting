using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AxaHosting.Model;

namespace AxaHosting.Data.Configuration
{
    public class PreparedIpConfiguration : EntityTypeConfiguration<PreparedIp>
    {
        public PreparedIpConfiguration()
        {
            ToTable("PreparedIps");
            Property(c => c.ExternalIp).IsRequired();
            Property(c => c.InternalIp).IsRequired();
        }
    }
}
