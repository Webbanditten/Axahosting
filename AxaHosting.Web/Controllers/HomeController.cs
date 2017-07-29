using AutoMapper;


using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace AxaHosting.Web.Controllers
{
    
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        #region Websurge
        public ActionResult WebSurge()
        {
            return File(Server.MapPath("~/websurge-allow.txt"), "text/plain");
            
        }
        #endregion



    }



}