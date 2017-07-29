using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AxaHosting.Model;
using AxaHosting.Model.Models;

namespace AxaHosting.Web.ViewModels
{
    public class MailsViewModel
    {
        public string MessageType { get; set; }
        public string MessageText { get; set; }
        public WebHotel WebHotel { get; set; }
        public IEnumerable<MailAccount> Accounts { get; set; } 
    }

    public class MailsAddAccountViewModel
    {
        [Required]
        [HiddenInput]
        public int WebHotelId { get; set; }
        public WebHotel WebHotel { get; set; }
        [Required]
        [Display(Name = "Alias")]
        public string Address { get; set; }
        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }

    public class MailsEditAccountViewModel
    {
        [Required]
        [HiddenInput]
        public int WebHotelId { get; set; }
        public WebHotel WebHotel { get; set; }
        [Required]
        public int AccountId { get; set; }
        public string Address { get; set; }
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }

    public class MailsDeleteAccountViewModel
    {
        [Required]
        [HiddenInput]
        public int WebHotelId { get; set; }
        public WebHotel WebHotel { get; set; }
        [Required]
        public int AccountId { get; set; }
        public string Address { get; set; }
    }


}