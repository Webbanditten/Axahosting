using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AxaHosting.Model;

namespace AxaHosting.Data.Configuration
{
    public class DatabaseConfiguration : EntityTypeConfiguration<Database>
    {
        public DatabaseConfiguration()
        {
            ToTable("Databases");
            Property(c => c.Name).IsRequired().HasMaxLength(64);
            Property(c => c.Username).IsRequired().HasMaxLength(64);
            Property(c => c.Password).IsRequired().HasMaxLength(64);
            Property(c => c.Owner).IsRequired().HasMaxLength(64);
        }
    }
}
