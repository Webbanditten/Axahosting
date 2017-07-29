using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AxaHosting.Model;

namespace AxaHosting.Web.ViewModels
{
    public class ProductViewModels
    {
        public ICollection<Product> Products { get; set; } 
    }

    public class OrderViewModel
    {
        public int ProductId { get; set; }
        [Display(Name = "Domain name")]
        public string DomainName { get; set; }
        public int? DatabaseProductId { get; set; }
    }

    public class WebHotelOrderViewModel
    {
        public List<Product> DatabaseProducts { get; set; }
        public Product Product { get; set; }
        [Display(Name = "Domain name")]
        public string DomainName { get; set; }
        public int? DatabaseProductId { get; set; }
    }
}