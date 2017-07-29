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
    public class AccountController : Controller
    {
        private readonly IActiveDirectoryService _activeDirectoryService;
      
        public AccountController(IActiveDirectoryService activeDirectoryService)
        {
            this._activeDirectoryService = activeDirectoryService;
        }

        public ActionResult Login()
        {

            return View();
        }

        [ValidateAntiForgeryToken]
        [HttpPost]
        public ActionResult Login(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = new User {Username = model.Username, Password = model.Password};
                if (_activeDirectoryService.IsValid(user))
                {
                    user = _activeDirectoryService.Get(user);
                    if (user != null)
                    {

                        var cookie = FormAuthUtil.CreateCookie(user);
                        Response.Cookies.Add(cookie);


                        return RedirectToAction("Index", "ControlPanel");
                    }
                }
            }
            ModelState.AddModelError("", "Wrong username or password");
            return View(model);

        }

        [Authorize]
        public ActionResult Index()
        {
            return View();
        }

        [Authorize]
        public ActionResult Signout()
        {
            FormsAuthentication.SignOut();
            return RedirectToAction("Login");
        }

        [Authorize]
        public ActionResult Settings()
        {
            
            var usr = FormAuthUtil.GetUser(Request.Cookies);
            var model = new SettingsViewModel()
            {
                Company = usr.Company,
                Email = usr.Email,
                LastName = usr.Surname,
                Name = usr.Name,
                PhoneNumber = usr.PhoneNumber,
                Street = usr.Street,
                City = usr.City,
                PostalCode = usr.PostalCode
            };

            return View(model);
        }

        [Authorize]
        [ValidateAntiForgeryToken]
        [HttpPost]
        public ActionResult Settings(SettingsViewModel model)
        {
            var usr = FormAuthUtil.GetUser(Request.Cookies);
            if (ModelState.IsValid)
            {

                usr.Company = model.Company;
                usr.Surname = model.LastName;
                usr.Name = model.Name;
                usr.PhoneNumber = model.PhoneNumber;
                usr.Email = model.Email;
                if (usr.Password != model.Password)
                {
                    usr.Password = model.Password;
                }
                usr.PostalCode = model.PostalCode;
                usr.City = model.City;
                usr.Street = model.Street;

                // Save
                _activeDirectoryService.Update(usr);

                var cookie = FormAuthUtil.CreateCookie(usr);
                Response.Cookies.Add(cookie);

                ViewBag.Success = true;
                return View(model);

            }

            return View(model);
        }

        [HttpPost]
        public ActionResult CheckUsername(string username)
        {
            return Json(_activeDirectoryService.UsernameExist(new User {Username = username}));
        }

        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Create(CreateUserViewModel model)
        {
            if (model.CreateUsername != null)
            {
                if (_activeDirectoryService.UsernameExist(new User {Username = model.CreateUsername}))
                {
                    ModelState.AddModelError("CreateUsername", "Username is taken, please select a new one");
                }
            }
            else
            {
                ModelState.AddModelError("CreateUsername", "Please enter a username");
            }

            if (ModelState.IsValid)
            {
                var newUsr = new User
                {
                    Company = model.Company,
                    Name = model.Name,
                    Surname = model.LastName,
                    Email = model.Email,
                    Password = model.CreatePassword,
                    PhoneNumber = model.PhoneNumber,
                    Username = model.CreateUsername,
                    Street = model.Street,
                    City = model.City,
                    PostalCode = model.PostalCode
                };
                // Create user
                try
                {
                    _activeDirectoryService.Add(newUsr);
                    
                    var cookie = FormAuthUtil.CreateCookie(newUsr);
                    Response.Cookies.Add(cookie);
                    return RedirectToAction("Index", "ControlPanel");
                }
                catch
                {
                    ModelState.AddModelError("Internal", "Something is not right with our server. Please try again.");
                }
            }
            return View(model);
        }
    }
}