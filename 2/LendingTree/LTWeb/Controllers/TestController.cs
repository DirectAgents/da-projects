﻿using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using System.Xml.Linq;

namespace LTWeb.Controllers
{
    public class TestController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Test()
        {
            return View();
        }

        public ActionResult Setup()
        {
            var ltModel = LTWeb.Session.LTModel;
            HomeController.PrepareLendingTreeModelForNewSession(ltModel, null, null);

            ltModel.LoanType = "REFINANCE";
            ltModel.PropertyState = "CA";
            ltModel.PropertyZip = "12345";
            ltModel.PropertyApproximateValue = 150000;
            ltModel.EstimatedMortgageBalance = 100001;
            ltModel.CashOut = 15000;
            ltModel.MonthlyPayment = 950;
            ltModel.FirstName = "test";
            ltModel.LastName = "test";
            ltModel.Address = "10 main st.";
            ltModel.Email = "a@a.com";
            ltModel.HomePhone = "1231231234";
            ltModel.WorkPhone = "2342342345";
            ltModel.SSN = "011-22-3456";
            ltModel.DOB = "10/10/1960";

            return RedirectToAction("Show", "Questions", new { test = true });
        }

        public ActionResult LeadService()
        {
            string path = System.Web.HttpContext.Current.Server.MapPath("~/App_Data/questions2.xml");
            var questions = XDocument.Load(path)
                                        .Descendants("question")
                                        .Distinct(new QuestionComparer())
                                        .Where(c => c.Attribute("key").Value != "Complete");
            return View(questions);
        }

        class QuestionComparer : IEqualityComparer<XElement>
        {
            public bool Equals(XElement x, XElement y)
            {
                return x.Attribute("key").Value == y.Attribute("key").Value;
            }

            public int GetHashCode(XElement obj)
            {
                return obj.Attribute("key").Value.GetHashCode();
            }
        }
    }
}
