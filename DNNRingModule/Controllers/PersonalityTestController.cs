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
            ViewBag.Step = (int?)Session["Step"] ?? 0;

            var answers = Session["PersonalityTestAnswers"] as PersonalityTestAnswer;
            if (answers != null)
            {
                ViewBag.Answers = new
                {
                    q1 = answers?.Answer1,
                    q2 = answers?.Answer2,
                    q3 = answers?.Answer3,
                    q4 = answers?.Answer4,
                    q5 = answers?.Answer5,
                    q6 = answers?.Answer6,
                    q7 = answers?.Answer7,
                    q8 = answers?.Answer8,
                    q9 = answers?.Answer9,
                    q10 = answers?.Answer10
                };
            }
            return View();
        }

        [HttpPost]
        public ActionResult Index(FormCollection form)
        {
            int step = 0;
            int.TryParse(form["step"], out step);

            var answers = Session["PersonalityTestAnswers"] as PersonalityTestAnswer;
            if (answers == null)
            {
                answers = new PersonalityTestAnswer
                {
                    FilledOutBy = UserController.Instance.GetCurrentUserInfo().UserID,
                    FilledOutAt = DateTime.Now
                };
            }

            // VISSZA
            if (step == 0)
            {
                answers.Answer6 = form["q6"];
                answers.Answer7 = form["q7"];
                answers.Answer8 = form["q8"];
                answers.Answer9 = form["q9"];
                answers.Answer10 = form["q10"];
            }
            else if (step == 1)
            {
                answers.Answer1 = form["q1"];
                answers.Answer2 = form["q2"];
                answers.Answer3 = form["q3"];
                answers.Answer4 = form["q4"];
                answers.Answer5 = form["q5"];
            }
            else if (step == 2)
            {
                answers.Answer6 = form["q6"];
                answers.Answer7 = form["q7"];
                answers.Answer8 = form["q8"];
                answers.Answer9 = form["q9"];
                answers.Answer10 = form["q10"];
            }

            Session["PersonalityTestAnswers"] = answers;

            if (step == 2)
            {
                SaveAnswersToDatabase(answers);
                return RedirectToAction("ThankYou");
            }

            Session["Step"] = step + 1;

            return RedirectToAction("Index");
        }

        private void SaveAnswersToDatabase(PersonalityTestAnswer answers)
        {
            PersonalityTestManager.Instance.CreatePersonalityTest(answers);
        }

        public ActionResult ThankYou()
        {
            var answers = Session["PersonalityTestAnswers"] as PersonalityTestAnswer;
            //Ilyen dictionary-ben kell tárolni a vlaszokat, hogy a ThankYou oldalon könnyen lehessen kiiratni
            ViewBag.Answers = new Dictionary<string, string>
            {
                { "q1", answers?.Answer1 },
                { "q2", answers?.Answer2 },
                { "q3", answers?.Answer3 },
                { "q4", answers?.Answer4 },
                { "q5", answers?.Answer5 },
                { "q6", answers?.Answer6 },
                { "q7", answers?.Answer7 },
                { "q8", answers?.Answer8 },
                { "q9", answers?.Answer9 },
                { "q10", answers?.Answer10 }
            };
            Session["Step"] = null;
            Session["PersonalityTestAnswers"] = null;
            return View();
        }
    }
}
