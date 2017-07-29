using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using AxaHosting.Model;
using AxaHosting.Service;
using AxaHosting.Web.utils;
using AxaHosting.Web.ViewModels;
using Newtonsoft.Json;


namespace AxaHosting.Web.Controllers
{
    [Authorize]
    public class AdminController : Controller
    {
        private readonly IActiveDirectoryService _activeDirectoryService;
        
        public AdminController(IActiveDirectoryService activeDirectoryService)
        {
            this._activeDirectoryService = activeDirectoryService;

        }

        public ActionResult ViewUsers()
        {
            var user = FormAuthUtil.GetUser(Request.Cookies);
            if (_activeDirectoryService.IsUserInRole(user, "webadmin"))
            {
                var users = _activeDirectoryService.GetUsersInOu();
                var model = new AdminUserViewModel {Users = users};
                var messageType = TempData["MessageType"];
                var messageText = TempData["MessageText"];

                if (messageText != null && messageType != null)
                {
                    model.MessageText = (string)messageText;
                    model.MessageType = (string)messageType;
                }
                return View(model);
            }
            return new HttpUnauthorizedResult();
        }

        public ActionResult ResetPassword(string username)
        {
            var signedIn = FormAuthUtil.GetUser(Request.Cookies);
            if (_activeDirectoryService.IsUserInRole(signedIn, "webadmin"))
            {
                var user = _activeDirectoryService.Get(new User { Username = username });
                var model = new AdminResetPasswordViewModel { Username = user.Username };
                return View(model);
            }
            return new HttpUnauthorizedResult();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ResetPassword(AdminResetPasswordViewModel input)
        {
            var signedIn = FormAuthUtil.GetUser(Request.Cookies);
            if (_activeDirectoryService.IsUserInRole(signedIn, "webadmin"))
            {
                if (ModelState.IsValid)
                {
                    var user = _activeDirectoryService.Get(new User { Username = input.Username });
                    user.Password = input.Password;
                    _activeDirectoryService.Update(user);

                    TempData["MessageType"] = "success";
                    TempData["MessageText"] = "Users password has been changed!";

                    return RedirectToAction("ViewUsers");
                }
                
                return View(input);
            }
            return new HttpUnauthorizedResult();
        }
    }
}