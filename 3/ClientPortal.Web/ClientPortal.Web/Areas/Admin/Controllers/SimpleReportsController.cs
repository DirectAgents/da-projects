using CakeExtracter.Reports;
using ClientPortal.Data.Contexts;
using ClientPortal.Data.Contracts;
using ClientPortal.Web.Controllers;
using System;
using System.Configuration;
using System.Linq;
using System.Web.Mvc;
using WebMatrix.WebData;

namespace ClientPortal.Web.Areas.Admin.Controllers
{
    [Authorize(Users = "admin")]
    public class SimpleReportsController : CPController
    {
        public SimpleReportsController(IClientPortalRepository cpRepository)
        {
            cpRepo = cpRepository;
        }

        public ActionResult Index()
        {
            var simpleReports = cpRepo.SimpleReports.ToList().OrderBy(sr => sr.ParentName);
            return View(simpleReports);
        }

        public ActionResult IndexSearch(int? spId)
        {
            var simpleReportsQ = cpRepo.SearchSimpleReports;
            if (spId.HasValue)
                simpleReportsQ = simpleReportsQ.Where(sr => sr.SearchProfileId == spId.Value);
            var simpleReports = simpleReportsQ.OrderBy(sr => sr.SearchProfile.SearchProfileName).ToList();

            ViewBag.RefreshAction = "IndexSearch";
            return View("Index", simpleReports);
        }

        public ActionResult Edit(int id)
        {
            var simpleReport = cpRepo.GetSimpleReport(id);
            if (simpleReport == null)
                return HttpNotFound();

            if (simpleReport.IsSearchOnly)
                ViewBag.RedirectAction = "IndexSearch";
            return View(simpleReport);
        }

        [HttpPost]
        public ActionResult Edit(SimpleReport inReport, string redirectAction)
        {
            if (inReport.PeriodMonths < 0)
                inReport.PeriodMonths = 0;
            if (inReport.PeriodDays < 0)
                inReport.PeriodDays = 0;

            if (ModelState.IsValid)
            {
                var simpleReport = cpRepo.GetSimpleReport(inReport.SimpleReportId);
                if (simpleReport == null)
                    return HttpNotFound();

                simpleReport.SetEditableFieldsFrom(inReport);
                cpRepo.SaveChanges();

                if (!String.IsNullOrWhiteSpace(redirectAction))
                    return RedirectToAction(redirectAction);
                else
                    return RedirectToAction("Index");
            }
            cpRepo.FillExtended_SimpleReport(inReport);
            return View(inReport);
        }

        public ActionResult Test(int id, string redirectAction)
        {
            return GenerateTestView(id, "@directagents.com", redirectAction);
        }
        private ActionResult GenerateTestView(int id, string sendTo, string redirectAction)
        {
            var simpleReport = cpRepo.GetSimpleReport(id);
            if (simpleReport == null)
                return HttpNotFound();
            ViewBag.SendTo = sendTo;
            ViewBag.RedirectAction = redirectAction;
            return View("Test", simpleReport);
        }

        [HttpPost]
        public ActionResult Test(int id, string sendTo, string redirectAction)
        {
            if (String.IsNullOrWhiteSpace(sendTo) || sendTo.StartsWith("@") | !sendTo.Contains("@") | !sendTo.Contains("."))
                ModelState.AddModelError("", "Please enter a valid email address.");
            else
            {
                var simpleReport = cpRepo.GetSimpleReport(id);
                if (simpleReport == null)
                    return HttpNotFound();

                var gmailUsername = ConfigurationManager.AppSettings["GmailEmailer_Username"];
                var gmailPassword = ConfigurationManager.AppSettings["GmailEmailer_Password"];
                var reportManager = new SimpleReportManager(cpRepo, new GmailEmailer(new System.Net.NetworkCredential(gmailUsername, gmailPassword)));
                reportManager.SendReports(simpleReport, sendTo);

                if (!String.IsNullOrWhiteSpace(redirectAction))
                    return RedirectToAction(redirectAction);
                else
                    return RedirectToAction("Index");
            }
            return GenerateTestView(id, sendTo, redirectAction);
        }

        public ActionResult Initialize(int id)
        {
            bool success = cpRepo.InitializeSimpleReport(id, true);
            return Content(success.ToString());
        }
    }
}