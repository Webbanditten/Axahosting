using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AxaHosting.Model
{
    public class Server
    {
        public int Id { get; set; }
        public ServerType Type { get; set; }
        public string ExternalIp { get; set; }
        public string InternalIp { get; set; }
        public DateTime LastUpdated { get; set; } = DateTime.Now;
        public int Load { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
    }
}
