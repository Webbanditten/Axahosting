using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AxaHosting.Model
{
    public class Product
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public decimal MonthlyPricing { get; set; }
        public ServerType ServerType { get; set; }
        public ProductType ProductType { get; set; }
        public string Color { get; set; }
        
        public int? MaxCpu { get; set; }
        public int? MaxDbGb { get; set; }
        public int? MaxMailAccounts { get; set; }
        public bool Visible { get; set; } = true;


        public string GetServerTypeName()
        {
            switch (this.ServerType)
            {
                case ServerType.Iis:
                    return "WebHost";
                case ServerType.MsSql:
                    return "Microsoft SQL";
                case ServerType.MySql:
                    return "MySQL";
            }
            return "N/A";
        }
    }
}
