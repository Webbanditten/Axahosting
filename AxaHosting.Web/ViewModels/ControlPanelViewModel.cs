using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AxaHosting.Model;

namespace AxaHosting.Web.ViewModels
{
    public class ControlPanelViewModel
    {
        public string MessageType { get; set; }
        public string MessageText { get; set; }
        public int Products { get; set; }
        public bool Admin { get; set; } = false;

    }

    public class ControlPanelProductsViewModel
    {
        public string MessageType { get; set; }
        public string MessageText { get; set; }
        public IEnumerable<WebHotel> WebHotels { get; set; }
        public IEnumerable<Database> Databases { get; set; }   
    }
}