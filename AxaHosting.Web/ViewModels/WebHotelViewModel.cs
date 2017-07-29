using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AxaHosting.Model;

namespace AxaHosting.Web.ViewModels
{
    public class WebHotelViewModel
    {
       
        public string MessageType { get; set; }
        public string MessageText { get; set; }
        public WebHotel WebHotel { get; set; }
    }

    public class WebHotelEditViewModel
    {
        public WebHotel WebHotel { get; set; }
        [HiddenInput]
        [Required]
        public int WebHotelId { get; set; }
        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "FTP Password")]
        public string Password { get; set; }
    }

    public class WebhotelCancelViewModel
    {
        public WebHotel WebHotel { get; set; }
        [HiddenInput]
        [Required]
        public int WebHotelId { get; set; }
        [HiddenInput]
        [Required]
        public bool CancelWebhotel { get; set; }
    }
}