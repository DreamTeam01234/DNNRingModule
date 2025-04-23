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
        public ActionResult Index()
        {
            ViewBag.User = UserController.Instance.GetCurrentUserInfo();
            ViewBag.IsLoggedIn = ViewBag.User != null && ViewBag.User.UserID > 0;

            return View();
        }
        public ActionResult TestStart()
        {
            return View();
        }
    }
}

