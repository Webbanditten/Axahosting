using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AxaHosting.Model;
using AxaHosting.Service;
using AxaHosting.Web.utils;
using AxaHosting.Web.ViewModels;

namespace AxaHosting.Web.Controllers
{
    [Authorize]
    public class ControlPanelController : Controller
    {
        private readonly IWebHotelService _webHotelService;
        private readonly IDatabaseService _databaseService;
        private readonly IActiveDirectoryService _activeDirectoryService;
        

        public ControlPanelController(IWebHotelService webHotelService, IDatabaseService databaseService, IActiveDirectoryService activeDirectoryService)
        {
            _webHotelService = webHotelService;
            _databaseService = databaseService;
            _activeDirectoryService = activeDirectoryService;
            

        }


        // GET: ControlPanel
        public ActionResult Index()
        {
            var user = FormAuthUtil.GetUser(Request.Cookies);
            var model = new ControlPanelViewModel
            {
                Products = _webHotelService.GetWebHotelsByOwner(user.Username).Count() + _databaseService.GetDatabasesByOwner(user.Username).Count()
            };

            var messageType = TempData["MessageType"];
            var messageText = TempData["MessageText"];

            if (messageText != null && messageType != null)
            {
                model.MessageText = (string)messageText;
                model.MessageType = (string)messageType;
            }
            model.Admin = _activeDirectoryService.IsUserInRole(user, "webadmin");
            return View(model);
        }

       

        public ActionResult MyProducts()
        {
            var user = FormAuthUtil.GetUser(Request.Cookies);
            var databases = _databaseService.GetDatabasesByOwner(user.Username);
            var webhotels = _webHotelService.GetWebHotelsByOwner(user.Username);

            var model = new ControlPanelProductsViewModel {Databases = databases, WebHotels = webhotels};

            var messageType = TempData["MessageType"];
            var messageText = TempData["MessageText"];

            if (messageText != null && messageType != null)
            {
                model.MessageText = (string)messageText;
                model.MessageType = (string)messageType;
            }

            return View(model);
        }
    }
}