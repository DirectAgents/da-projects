using System;
using System.Configuration;
using System.Linq;
using System.Web.Mvc;
using CakeExtracter.Reports;
using ClientPortal.Data.Contexts;
using ClientPortal.Data.Contracts;
using ClientPortal.Web.Controllers;

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
            var simpleReports = cpRepo.SimpleReports.ToList().OrderBy(sr => !sr.Enabled).ThenBy(sr => sr.NextSend).ThenBy(sr => sr.ParentName);
            return View(simpleReports);
        }

        public ActionResult IndexSearch(int? spId)
        {
            var simpleReportsQ = cpRepo.SearchSimpleReports;
            if (spId.HasValue)
                simpleReportsQ = simpleReportsQ.Where(sr => sr.SearchProfileId == spId.Value);
            var simpleReports = simpleReportsQ.OrderBy(sr => !sr.Enabled).ThenBy(sr => sr.NextSend).ThenBy(sr => sr.SearchProfile.SearchProfileName).ToList();

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
            return GenerateTestView(id, "@directagents.com", true, SimpleReport.DefaultNumWeeks, SimpleReport.DefaultNumMonths, null, redirectAction);
        }
        private ActionResult GenerateTestView(int id, string sendTo, bool includeSpreadsheet, int? numWeeks, int? numMonths, string filename, string redirectAction)
        {
            var simpleReport = cpRepo.GetSimpleReport(id);
            if (simpleReport == null)
                return HttpNotFound();
            ViewBag.SendTo = sendTo;
            ViewBag.IncludeSpreadsheet = includeSpreadsheet;
            ViewBag.NumWeeks = numWeeks;
            ViewBag.NumMonths = numMonths;
            ViewBag.Filename = filename;
            ViewBag.RedirectAction = redirectAction;
            return View("Test", simpleReport);
        }

        [HttpPost]
        public ActionResult Test(int id, string sendTo, string redirectAction, bool includeSpreadsheet, int? numWeeks, int? numMonths, string filename)
        {
            if (String.IsNullOrWhiteSpace(sendTo) || sendTo.StartsWith("@") | !sendTo.Contains("@") | !sendTo.Contains("."))
                ModelState.AddModelError("", "Please enter a valid email address.");
            if (includeSpreadsheet)
            {
                if (!numWeeks.HasValue || numWeeks.Value < 0)
                    ModelState.AddModelError("", "Please specify a valid number of Weeks.");
                if (!numMonths.HasValue || numMonths.Value < 0)
                    ModelState.AddModelError("", "Please specify a valid number of months.");
                if (String.IsNullOrWhiteSpace(filename))
                    ModelState.AddModelError("", "Please specify a valid filename.");
            }
            if (ModelState.IsValid)
            {
                var simpleReport = cpRepo.GetSimpleReport(id);
                if (simpleReport == null)
                    return HttpNotFound();

                if (includeSpreadsheet)
                {
                    simpleReport.IncludeAttachment = true;
                    simpleReport.Attachment_NumWeeks = numWeeks.Value;
                    simpleReport.Attachment_NumMonths = numMonths.Value;
                    simpleReport.Attachment_Filename = filename;
                }
                var gmailUsername = ConfigurationManager.AppSettings["GmailEmailer_Username"];
                var gmailPassword = ConfigurationManager.AppSettings["GmailEmailer_Password"];
                var reportManager = new SimpleReportManager(cpRepo, new GmailEmailer(new System.Net.NetworkCredential(gmailUsername, gmailPassword)));
                reportManager.SendReports(simpleReport, sendTo);

                if (!String.IsNullOrWhiteSpace(redirectAction))
                    return RedirectToAction(redirectAction);
                else
                    return RedirectToAction("Index");
            }
            return GenerateTestView(id, sendTo, includeSpreadsheet, numWeeks, numMonths, filename, redirectAction);
        }

        public ActionResult Initialize(int id)
        {
            bool success = cpRepo.InitializeSimpleReport(id, true);
            return Content(success.ToString());
        }
    }
}