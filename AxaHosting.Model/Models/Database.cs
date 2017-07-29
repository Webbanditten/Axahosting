using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AxaHosting.Model
{
    public class Database
    {
        public int Id { get; set; }
        public string Owner { get; set; }
        public int ProductId { get; set; }
        public DatabaseType DatabaseType { get; set; }
        public string Name { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public DateTime LastUpdated { get; set; } = DateTime.Now;
        public DateTime CreatedDate { get; set; } = DateTime.Now;
        public int ServerId { get; set; }
        public virtual Server Server { get; set; }
        public virtual Product Product { get; set; }
        
    }
}
