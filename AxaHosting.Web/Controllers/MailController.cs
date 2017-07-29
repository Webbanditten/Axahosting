using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AxaHosting.Data.Repositories;
using AxaHosting.Model;
using AxaHosting.Service;
using AxaHosting.Web.utils;
using AxaHosting.Web.ViewModels;

namespace AxaHosting.Web.Controllers
{
    [Authorize]
    public class MailController : Controller
    {
        
        private readonly IMailService _mailService;
        private readonly IWebHotelService _webHotelService;

        public MailController(IMailService mailService, IWebHotelService webHotelService)
        {
            
            
            this._mailService = mailService;
            this._webHotelService = webHotelService;

        }

        public ActionResult Index(int webHotelId)
        {
            var usr = FormAuthUtil.GetUser(Request.Cookies);
            var webhotel = _webHotelService.GetWebHotel(webHotelId);
            if (webhotel.Owner == usr.Username)
            {
                var accounts = _mailService.GetAccountsForDomain(webhotel.HMailDomainId);
                var model = new MailsViewModel {Accounts = accounts, WebHotel = webhotel};
                var messageType = TempData["MessageType"];
                var messageText = TempData["MessageText"];

                if (messageText != null && messageType != null)
                {
                    model.MessageText = (string)messageText;
                    model.MessageType = (string)messageType;
                }

                return View(model);
            }
            else
            {
                return new HttpStatusCodeResult(403);
            }
        }

        public ActionResult Add(int webHotelId)
        {
            var user = FormAuthUtil.GetUser(Request.Cookies);
            var webhotel = _webHotelService.GetWebHotel(webHotelId);
            if (webhotel.Owner == user.Username)
            {
                var model = new MailsAddAccountViewModel {WebHotelId = webHotelId, WebHotel = webhotel};
                return View(model);
            }
            else
            {
                return new HttpStatusCodeResult(403);
            }
        }

        public ActionResult Edit(int webHotelId, int accountId)
        {
            var user = FormAuthUtil.GetUser(Request.Cookies);
            var webhotel = _webHotelService.GetWebHotel(webHotelId);
            if (webhotel.Owner == user.Username)
            {
                var account = _mailService.ViewAccount(webhotel.HMailDomainId, accountId);
                var model = new MailsEditAccountViewModel { WebHotel = webhotel, AccountId = accountId, Address = account.Address};
                return View(model);
            }
            else
            {
                return new HttpStatusCodeResult(403);
            }
        }

        public ActionResult Delete(int webHotelId, int accountId)
        {
            var user = FormAuthUtil.GetUser(Request.Cookies);
            var webhotel = _webHotelService.GetWebHotel(webHotelId);
            if (webhotel.Owner == user.Username)
            {
                var account = _mailService.ViewAccount(webhotel.HMailDomainId, accountId);
                var model = new MailsDeleteAccountViewModel { WebHotel = webhotel, AccountId = accountId, Address = account.Address };
                return View(model);
            }
            else
            {
                return new HttpStatusCodeResult(403);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Add(MailsAddAccountViewModel input)
        {
            var user = FormAuthUtil.GetUser(Request.Cookies);
            if (ModelState.IsValid)
            {
                var webhotel = _webHotelService.GetWebHotel(input.WebHotelId);
                if (webhotel.Owner == user.Username)
                {
                    // Add mail account
                    _mailService.CreateAccount(webhotel.HMailDomainId, input.Address, input.Password);


                    TempData["MessageType"] = "success";
                    TempData["MessageText"] = "The account has been created!";
                    return RedirectToAction("Index", new {webhotelId = webhotel.Id});
                }
                return new HttpStatusCodeResult(403);
            }
            return View(input);
           
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(MailsEditAccountViewModel input)
        {
            var user = FormAuthUtil.GetUser(Request.Cookies);
            if (ModelState.IsValid)
            {
                var webhotel = _webHotelService.GetWebHotel(input.WebHotelId);
                if (webhotel.Owner == user.Username)
                {
                    // Edit mail account
                    _mailService.ChangePasswordForAccount(webhotel.HMailDomainId, input.AccountId, input.Password);

                    TempData["MessageType"] = "success";
                    TempData["MessageText"] = "The account has been edited!";
                    return RedirectToAction("Index", new { webhotelId = webhotel.Id });
                }
                return new HttpStatusCodeResult(403);
            }
            return View(input);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(MailsDeleteAccountViewModel input)
        {
            var user = FormAuthUtil.GetUser(Request.Cookies);
            if (ModelState.IsValid)
            {
                var webhotel = _webHotelService.GetWebHotel(input.WebHotelId);
                if (webhotel.Owner == user.Username)
                {
                    // Delete mail account
                    _mailService.DeleteAccount(webhotel.HMailDomainId, input.AccountId);

                    TempData["MessageType"] = "success";
                    TempData["MessageText"] = "The account has been deleted!";
                    return RedirectToAction("Index", new { webhotelId = webhotel.Id });
                }
                return new HttpStatusCodeResult(403);
            }
            return View(input);
        }
    }
};