using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AxaHosting.Model;

namespace AxaHosting.Web.ViewModels
{
    public class AdminUserViewModel
    {
        public string MessageType { get; set; }
        public string MessageText { get; set; }
        public IEnumerable<User> Users { get; set; }

    }

    public class AdminResetPasswordViewModel
    {

        [Required]
        public string Username { get; set; }
        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }
    }
}