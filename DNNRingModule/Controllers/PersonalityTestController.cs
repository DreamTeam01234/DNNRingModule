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
            return View();
        }
    }
}
