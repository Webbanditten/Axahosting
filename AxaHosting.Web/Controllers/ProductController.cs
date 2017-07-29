using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AxaHosting.Model;
using AxaHosting.Service;
using AxaHosting.Web.utils;
using AxaHosting.Web.ViewModels;
using Microsoft.Owin.Security;
using Hangfire;

namespace AxaHosting.Web.Controllers
{
    public class ProductController : Controller
    {
        private readonly IProductService _productService;
        private readonly IDomainService _domainService;
        private readonly IDatabaseService _databaseService;
        private readonly IWebHotelService _webHotelService;
        private readonly IServerService _serverService;

        public ProductController(IProductService productService, IServerService serverService, IDomainService domainService, IDatabaseService databaseService, IWebHotelService webHotelService)
        {
            this._productService = productService;
            this._domainService = domainService;
            this._databaseService = databaseService;
            this._webHotelService = webHotelService;
            this._serverService = serverService;
        }
        
        public ActionResult Index(ProductType? type)
        {
            var model = new ProductViewModels
            {
                Products =
                    type != null
                        ? _productService.GetProducts().Where(p => p.ProductType == type).ToList()
                        : _productService.GetProducts().ToList()
            };
            ViewBag.Type = type;
            return View(model);
        }

        [Authorize]
        public ActionResult Order(int id)
        {
            var product = _productService.GetProduct(id);
           
            if (product != null)
            {
                if (product.ProductType == ProductType.WebHotel)
                {
                    var databaseProducts = _productService.GetProductsByType(ProductType.Database);
                    return View("OrderWebHotel", new WebHotelOrderViewModel {Product = product, DatabaseProducts = databaseProducts.ToList()});
                }
                else if(product.ProductType == ProductType.Database)
                {

                    return View("OrderDatabase", product);
                }
                return HttpNotFound();
            }
            else
            {
                return RedirectToAction("Index");
            }
        }

        public ActionResult IsDomainTaken(string domain)
        {
            _domainService.Connect();
            return Json(_domainService.IsDomainTaken(domain));
        }

        [Authorize]
        public ActionResult Generate(int amount)
        {
            var user = FormAuthUtil.GetUser(Request.Cookies);
            var product = _productService.GetProduct(1);
            for (var i  = 0; i <= amount; i++)
            {
                Random random = new Random();
                const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
                var domainName = new string(Enumerable.Repeat(chars, 8)
                  .Select(s => s[random.Next(s.Length)]).ToArray()); ;
                domainName = domainName + ".com";
                // Create the website on our local setup
                var webhotel = _webHotelService.Create(product, domainName, user.Username);
                _webHotelService.Save();

                // Create the site on remote DNS 
                _domainService.Connect();
                _domainService.CreateRecordsForDomain(webhotel);
                
            }
            return View();
        }

        [Authorize]
        [HttpPost]
        public ActionResult ConfirmOrder(OrderViewModel model)
        {
            // Connect to DNS
            _domainService.Connect();

            var user = FormAuthUtil.GetUser(Request.Cookies);
            var product = _productService.GetProduct(model.ProductId);
            var databaseProducts = _productService.GetProductsByType(ProductType.Database);

            if (product != null)
            {
                if (product.ProductType == ProductType.WebHotel)
                {
                    TempData["MessageType"] = "success";
                    TempData["MessageText"] = "Your webhotel is now ready.";

                    var domainTaken = _domainService.IsDomainTaken(model.DomainName);
                    if (model.DomainName != null && domainTaken == true)
                    {
                        
                        ModelState.AddModelError("DomainName", "Your domain name is taken");
                        return View("OrderWebHotel", new WebHotelOrderViewModel { DatabaseProductId = model.DatabaseProductId, DatabaseProducts = databaseProducts.ToList(), Product = product, DomainName = model.DomainName });
                    }

                    if (model.DomainName != null)
                    {
                        // Create the website on our local setup
                        var webhotel = _webHotelService.Create(product, model.DomainName, user.Username);
                        _webHotelService.Save();

                        // Create the site on remote DNS 

                        _domainService.CreateRecordsForDomain(webhotel);

                        // Check load
                     
                        var hangfireJobId = BackgroundJob.Enqueue(() => _serverService.EvaluateLoadOfMachines());

                        // Create database for web hotel
                        if (model.DomainName != null && model.DatabaseProductId != null)
                        {
                            var dbProduct = _productService.GetProduct(model.ProductId);
                            if (dbProduct != null)
                            {
                                // create db
                                var database = _databaseService.Create(product, user.Username, model.DomainName);
                                _databaseService.Save();
                                if (database == null)
                                {
                                    TempData["MessageType"] = "error";
                                    TempData["MessageText"] =
                                        "It seems like theres no available database servers (" +
                                        product.ServerType + "), please contact an administrator.";
                                    return RedirectToAction("Index", "ControlPanel");
                                }
                                else
                                {
                                    TempData["MessageText"] = "Your webhotel and database is ready.";
                                }
                            }

                        }

                        return RedirectToAction("Index", "ControlPanel");

                    }
                    else
                    {

                        ModelState.AddModelError("DomainName", "Please enter a valid domain name");
                        return View("OrderWebHotel", new WebHotelOrderViewModel {DatabaseProductId = model.DatabaseProductId, DatabaseProducts = databaseProducts.ToList(), Product = product, DomainName = model.DomainName});
                    }

                }

                else if (product.ProductType == ProductType.Database)
                {
                    // create db
                    var database = _databaseService.Create(product, user.Username);
                    _databaseService.Save();

                    if (database == null)
                    {
                        TempData["MessageType"] = "error";
                        TempData["MessageText"] = "It seems like theres no available database servers (" +
                                                    product.ServerType + "), please contact an administrator.";
                        return RedirectToAction("Index", "ControlPanel");
                    }
                    TempData["MessageType"] = "success";
                    TempData["MessageText"] =
                        "Your database have now been created! You can view your databases by clicking databases below.";
                    return RedirectToAction("Index", "ControlPanel");
                }
                return HttpNotFound();
            }
            else
            {
                return RedirectToAction("Index");
            }
            
        }

       
    }
}