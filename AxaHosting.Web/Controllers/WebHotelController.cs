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
    public class WebHotelController : Controller
    {
        private readonly IWebHotelService _webHotelService;

        

        public WebHotelController(IWebHotelService webHotelService)
        {
            _webHotelService = webHotelService;
            

        }
        public ActionResult Index(int id)
        {
            var user = FormAuthUtil.GetUser(Request.Cookies);
            var webhotel = _webHotelService.GetWebHotel(id);
            if (webhotel.Owner == user.Username)
            {
                
                var model = new WebHotelViewModel { WebHotel = webhotel };
                var messageType = TempData["MessageType"];
                var messageText = TempData["MessageText"];

                if (messageText != null && messageType != null)
                {
                    model.MessageText = (string)messageText;
                    model.MessageType = (string)messageType;
                }

                return View(model);
            }
            return new HttpStatusCodeResult(403);
        }

        public ActionResult Edit(int id)
        {
            var user = FormAuthUtil.GetUser(Request.Cookies);
            var webhotel = _webHotelService.GetWebHotel(id);
            if (webhotel.Owner == user.Username)
            {
                var model = new WebHotelEditViewModel {WebHotel = webhotel, WebHotelId = id};
                return View(model);
            }
            return new HttpStatusCodeResult(403);
            
        }

        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(WebHotelEditViewModel model)
        {
            var user = FormAuthUtil.GetUser(Request.Cookies);
            if (ModelState.IsValid)
            {

                var webhotel = _webHotelService.GetWebHotel(model.WebHotelId);
                if (webhotel.Owner == user.Username)
                {
                    webhotel.FtpPassword = model.Password;
                    _webHotelService.ChangeFtp(webhotel);
                    _webHotelService.Update(webhotel);
                    _webHotelService.Save();
                    
                    TempData["MessageType"] = "success";
                    TempData["MessageText"] = "The FTP password has been changed!";


                    return RedirectToAction("Index", new { id = webhotel.Id });
                }
                return new HttpStatusCodeResult(403);
            }
            return View(model);
        }

        public ActionResult Cancel(int id, bool nope = false)
        {
            var user = FormAuthUtil.GetUser(Request.Cookies);
            if (nope == true)
            {
                TempData["MessageType"] = "info";
                TempData["MessageText"] = "Your Domain is has not been cancelled.";


                return RedirectToAction("MyProducts", "ControlPanel");
            }
            var webhotel = _webHotelService.GetWebHotel(id);
            if (webhotel.Owner == user.Username)
            {
                var model = new WebhotelCancelViewModel { WebHotel = webhotel, WebHotelId = id };
                return View(model);
            }
            return new HttpStatusCodeResult(403);

        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Cancel(WebhotelCancelViewModel model)
        {
            
            if (ModelState.IsValid)
            {
                var usr = FormAuthUtil.GetUser(Request.Cookies);
                var webhotel = _webHotelService.GetWebHotel(model.WebHotelId);
                if (webhotel.Owner == usr.Username)
                {
                    _webHotelService.Cancel(webhotel);
                    _webHotelService.Save();

                    TempData["MessageType"] = "success";
                    TempData["MessageText"] = "Your Domain is now cancelled and deleted.";


                    return RedirectToAction("Index", "ControlPanel");
                }
                return new HttpStatusCodeResult(403);
            }
            return View(model);
        }
    }
}