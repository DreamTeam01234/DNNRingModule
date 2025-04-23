using System;
using System.Linq;
using System.Web.Mvc;
using Ring.Module.DNNRingModule.Components;
using Ring.Module.DNNRingModule.Models;
using DotNetNuke.Web.Mvc.Framework.Controllers;
using DotNetNuke.Web.Mvc.Framework.ActionFilters;
using DotNetNuke.Entities.Users;
using DotNetNuke.Framework.JavaScriptLibraries;

namespace Ring.Module.DNNRingModule.Controllers
{
    public class PersonalityTestController : DnnController
    {
        [HttpGet]
        public ActionResult Index()
        {
            ViewBag.User = UserController.Instance.GetCurrentUserInfo();
            ViewBag.IsLoggedIn = ViewBag.User != null && ViewBag.User.UserID > 0;
            ViewBag.Step = TempData["Step"] != null ? Convert.ToInt32(TempData["Step"]) : 0;
            return View();
        }

        [HttpPost]
        public ActionResult Index(FormCollection form)
        {
            int step = 0;
            int.TryParse(form["step"], out step);

            if (step == 0)
            {
                TempData["Step"] = 1;
                return RedirectToAction("Index");
            }

            if (step == 1)
            {
                TempData["Step"] = 2;
                return RedirectToAction("Index");
            }

            if (step == 2)
            {
                return RedirectToAction("ThankYou");
            }

            TempData["Step"] = 0;
            return RedirectToAction("Index");
        }

        public ActionResult ThankYou()
        {
            return View();
        }
    }
}
