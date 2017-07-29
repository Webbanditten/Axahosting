using System.Web.Mvc;
using AxaHosting.Model;
using AxaHosting.Service;
using AxaHosting.Web.utils;
using AxaHosting.Web.ViewModels;

namespace AxaHosting.Web.Controllers
{
    [Authorize]
    public class DatabaseController : Controller
    {
        private readonly IDatabaseService _databaseService;

       

        public DatabaseController(IDatabaseService databaseService)
        {
            _databaseService = databaseService;
          

        }
        public ActionResult Index(int id)
        {
            var user = FormAuthUtil.GetUser(Request.Cookies);
            var database = _databaseService.Get(id);
            if (database.Owner == user.Username)
            {

                var model = new DatabaseViewModel { Database = database };
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
            var database = _databaseService.Get(id);
            if (database.Owner == user.Username)
            {

                var model = new DatabaseEditViewModel { Database = database, DatabaseId = id };
                return View(model);
            }
            return new HttpStatusCodeResult(403);

        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(DatabaseEditViewModel model)
        {
            var user = FormAuthUtil.GetUser(Request.Cookies);
            if (ModelState.IsValid)
            {

                var database = _databaseService.Get(model.DatabaseId);
                if (database.Owner == user.Username)
                {
                    database.Password = model.Password;
                    _databaseService.Update(database);
                    _databaseService.Save();

                    TempData["MessageType"] = "success";
                    TempData["MessageText"] = "The database password has been changed!";


                    return RedirectToAction("Index", new {id = database.Id});
                }
                return new HttpStatusCodeResult(403);
            }
            return View(model);
        }

        public ActionResult Cancel(int id, bool nope = false)
        {
            var user = FormAuthUtil.GetUser(Request.Cookies);
            if (nope)
            {
                TempData["MessageType"] = "info";
                TempData["MessageText"] = "Your Database has not been cancelled.";


                return RedirectToAction("MyProducts", "ControlPanel");
            }
            var database = _databaseService.Get(id);
            if (database.Owner == user.Username)
            {
                var model = new DatabaseCancelViewModel { Database = database, DatabaseId = id };
                return View(model);
            }
            return new HttpStatusCodeResult(403);

        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Cancel(DatabaseCancelViewModel model)
        {
            var user = FormAuthUtil.GetUser(Request.Cookies);
            if (ModelState.IsValid)
            {

                var database = _databaseService.Get(model.DatabaseId);
                if (database.Owner == user.Username)
                {
                    _databaseService.Cancel(database);
                    _databaseService.Save();

                    TempData["MessageType"] = "success";
                    TempData["MessageText"] = "Your database is now cancelled and deleted.";


                    return RedirectToAction("Index", "ControlPanel");
                }
                return new HttpStatusCodeResult(403);
            }
            return View(model);
        }
    }
}