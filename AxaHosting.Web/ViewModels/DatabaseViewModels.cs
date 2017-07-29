using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AxaHosting.Model;

namespace AxaHosting.Web.ViewModels
{
    public class DatabaseViewModel
    {
       
        public string MessageType { get; set; }
        public string MessageText { get; set; }
        public Database Database { get; set; }
    }

    public class DatabaseEditViewModel
    {
        public Database Database { get; set; }
        [HiddenInput]
        [Required]
        public int DatabaseId { get; set; }
        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Database account password")]
        public string Password { get; set; }
    }

    public class DatabaseCancelViewModel
    {
        public Database Database { get; set; }
        [HiddenInput]
        [Required]
        public int DatabaseId { get; set; }
        [HiddenInput]
        [Required]
        public bool CancelDatabase { get; set; }
    }

}