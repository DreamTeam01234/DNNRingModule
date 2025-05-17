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

using Hotcakes.Commerce;
using Hotcakes.Commerce.Catalog;

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
            ViewBag.TopTypes = Session["TopTypes"] as string[] ?? new[] { "A", "B" };

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
                    q10 = answers?.Answer10,
                    q11 = answers?.Answer11,
                    q12 = answers?.Answer12,
                    q13 = answers?.Answer13,
                    q14 = answers?.Answer14,
                    q15 = answers?.Answer15
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

            if (step == 1)
            {
                answers.Answer1 = form["q1"];
                answers.Answer2 = form["q2"];
                answers.Answer3 = form["q3"];
                answers.Answer4 = form["q4"];
                answers.Answer5 = form["q5"];
                answers.Answer6 = form["q6"];
                answers.Answer7 = form["q7"];
                answers.Answer8 = form["q8"];
                answers.Answer9 = form["q9"];
                answers.Answer10 = form["q10"];

                var types = new[] {
                    answers.Answer1, answers.Answer2, answers.Answer3, answers.Answer4, answers.Answer5,
                    answers.Answer6, answers.Answer7, answers.Answer8, answers.Answer9, answers.Answer10
                };
                var topTypes = types
                    .Where(x => !string.IsNullOrEmpty(x))
                    .GroupBy(x => x)
                    .OrderByDescending(g => g.Count())
                    .Take(2)
                    .Select(g => g.Key)
                    .ToArray();
                
                if (topTypes.Length == 1)
                {
                    var fallback = new[] { "A", "B", "C", "D" }
                        .Where(t => t != topTypes[0])
                        .OrderBy(_ => Guid.NewGuid())
                        .First();
                    topTypes = new[] { topTypes[0], fallback };
                    // Véletlenszerűen választunk egy plusz kategóriát, ha minden kérdésre ugyanaz a kategória a válasz
                }
                Session["TopTypes"] = topTypes;
            }
            else if (step == 2)
            {
                answers.Answer11 = form["q11"];
                answers.Answer12 = form["q12"];
                answers.Answer13 = form["q13"];
                answers.Answer14 = form["q14"];
                answers.Answer15 = form["q15"];
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

        public ActionResult PingSession()
        {
            return Content(""); //Ahhoz, hogy ne törlődjön a session
        }


        //INNENTŐL A THANKYOU-VAL FOGLALKOZUNK
        public ActionResult ThankYou()
        {
            var answers = Session["PersonalityTestAnswers"] as PersonalityTestAnswer;

            var all = new[] {
                answers?.Answer11, answers?.Answer12, answers?.Answer13, answers?.Answer14, answers?.Answer15,
            };

            var letter = all.Where(x => !string.IsNullOrEmpty(x))
                            .GroupBy(x => x)
                            .OrderByDescending(g => g.Count())
                            .Select(g => g.Key)
                            .FirstOrDefault() ?? "D";

            var rewriteUrls = GetRewriteUrlsFromAnswer(letter);
            UpdateKivalCategoryByRewriteUrls(rewriteUrls);

            ViewBag.CategoryLetter = letter;
            ViewBag.Answers = Enumerable.Range(1, 15)
                .ToDictionary(i => $"q{i}", i => (string)answers?.GetType().GetProperty($"Answer{i}")?.GetValue(answers));

            Session["Step"] = null;
            Session["PersonalityTestAnswers"] = null;

            return View();
        }

        private List<string> GetRewriteUrlsFromAnswer(string letter)
        {
            var map = new Dictionary<string, List<string>>
            {
                { "A", new List<string> { "sport" } },
                { "B", new List<string> { "okosgyc5b1rc5b1" } },
                { "C", new List<string> { "fantc3a1zia" } },
                { "D", new List<string> { "ezc3bcst", "arany" } } // ezüst + arany
            };
            return map.TryGetValue(letter, out var urls) ? urls : new List<string> { "ezc3bcst" };
        }

        private void UpdateKivalCategoryByRewriteUrls(List<string> rewriteUrls)
        {
            var app = HotcakesApplication.Current;
            var kival = app.CatalogServices.Categories.FindBySlug("kival");
            if (kival == null) return;

            app.CatalogServices.CategoriesXProducts.DeleteAllForCategory(kival.Bvin);

            var products = new List<Product>();

            foreach (var url in rewriteUrls)
            {
                var category = app.CatalogServices.Categories.FindBySlug(url);
                if (category == null) continue;

                var items = app.CatalogServices.CategoriesXProducts
                    .FindForCategory(category.Bvin, 1, int.MaxValue)
                    .Select(x => app.CatalogServices.Products.Find(x.ProductId))
                    .Where(p => p != null && p.Status == ProductStatus.Active);

                products.AddRange(items);
            }

            products = products.OrderBy(_ => Guid.NewGuid()).Take(3).ToList();
            products.ForEach(p =>
            {
                app.CatalogServices.CategoriesXProducts.AddProductToCategory(p.Bvin, kival.Bvin);
            });
        }
    }
}