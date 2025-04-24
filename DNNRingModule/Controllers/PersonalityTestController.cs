using System;
using System.Linq;
using System.Web.Mvc;
using Ring.Module.DNNRingModule.Components;
using Ring.Module.DNNRingModule.Models;
using DotNetNuke.Web.Mvc.Framework.Controllers;
using DotNetNuke.Web.Mvc.Framework.ActionFilters;
using DotNetNuke.Entities.Users;
using DotNetNuke.Framework.JavaScriptLibraries;
using DotNetNuke.Data;
using System.Data.SqlClient;
using System.Collections.Generic;

namespace Ring.Module.DNNRingModule.Controllers
{
    public class PersonalityTestController : DnnController
    {
        [HttpGet]
        public ActionResult Index()
        {
            ViewBag.User = UserController.Instance.GetCurrentUserInfo();
            ViewBag.IsLoggedIn = ViewBag.User != null && ViewBag.User.UserID > 0;
            ViewBag.Step = TempData["Step"];
            return View();
        }

        [HttpPost]
        public ActionResult Index(FormCollection form)
        {
            int step = 0;
            int.TryParse(form["step"], out step);
            if (step == 2)
            {
                SaveAnswersToDatabase(form);
                return RedirectToAction("ThankYou");
            }

            TempData["Step"] = step + 1;
            return RedirectToAction("Index");
        }

        private void SaveAnswersToDatabase(FormCollection form)
        {
            var user = UserController.Instance.GetCurrentUserInfo();

            var answers = new PersonalityTestAnswer
            {
                FilledOutBy = user.UserID,
                FilledOutAt = DateTime.Now,
                Answer1 = form["q1"],
                Answer2 = form["q2"],
                Answer3 = form["q3"],
                Answer4 = form["q4"],
                Answer5 = form["q5"],
                Answer6 = form["q6"],
                Answer7 = form["q7"],
                Answer8 = form["q8"],
                Answer9 = form["q9"],
                Answer10 = form["q10"]
            };

            var parameters = new Dictionary<string, object>
            {
                { "@FilledOutBy", answers.FilledOutBy },
                { "@FilledOutAt", answers.FilledOutAt },
                { "@Answer1", answers.Answer1 },
                { "@Answer2", answers.Answer2 },
                { "@Answer3", answers.Answer3 },
                { "@Answer4", answers.Answer4 },
                { "@Answer5", answers.Answer5 },
                { "@Answer6", answers.Answer6 },
                { "@Answer7", answers.Answer7 },
                { "@Answer8", answers.Answer8 },
                { "@Answer9", answers.Answer9 },
                { "@Answer10", answers.Answer10 }
            };

            var db = DataProvider.Instance();
            db.ExecuteNonQuery(
                $"INSERT INTO PersonalityTest " +
                "(FilledOutBy, FilledOutAt, Answer1, Answer2, Answer3, Answer4, Answer5, Answer6, Answer7, Answer8, Answer9, Answer10) " +
                "VALUES (@FilledOutBy, @FilledOutAt, @Answer1, @Answer2, @Answer3, @Answer4, @Answer5, @Answer6, @Answer7, @Answer8, @Answer9, @Answer10)",
                parameters
            );
        }

        public ActionResult ThankYou()
        {
            return View();
        }
    }
}
