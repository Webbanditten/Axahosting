using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AxaHosting.Model
{
    public class WebHotel
    {
        public int Id { get; set; }
        public int ProductId { get; set; }
        public string Owner { get; set; }
        public string Domain { get; set; }
        public string FtpUsername { get; set; }
        public string FtpPassword { get; set; }
        public DateTime LastUpdated { get; set; } = DateTime.Now;
        public DateTime CreatedDate { get; set; } = DateTime.Now;
        public int IisId { get; set; }
        public int HMailDomainId { get; set; }
        public string AppPoolName { get; set; }
        public int ServerId { get; set; }
        public virtual Server Server { get; set; }
        public virtual Product Product { get; set; }
        
    }
}
