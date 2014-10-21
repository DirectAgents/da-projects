﻿using ClientPortal.Data.Contexts;
using ClientPortal.Data.Contracts;
using ClientPortal.Web.Controllers;
using DAGenerators.Spreadsheets;
using System;
using System.Configuration;
using System.Linq;
using System.Web.Mvc;
using WebMatrix.WebData;

namespace ClientPortal.Web.Areas.Admin.Controllers
{
    [Authorize(Users = "admin")]
    public class SearchAdminController : CPController
    {
        public SearchAdminController(IClientPortalRepository cpRepository)
        {
            cpRepo = cpRepository;
        }

        public ActionResult SearchProfiles()
        {
            var searchProfiles = cpRepo.SearchProfiles();
            return View(searchProfiles.ToList());
        }

        public ActionResult CreateUserProfile(int spId)
        {
            var searchProfile = cpRepo.GetSearchProfile(spId);
            if (searchProfile == null)
                return HttpNotFound();
            return View(searchProfile);
        }

        [HttpPost]
        public ActionResult CreateUserProfile(int spId, string username, string password)
        {
            // TODO: validation
            var searchProfile = cpRepo.GetSearchProfile(spId);
            if (searchProfile != null)
            {
                WebSecurity.CreateUserAndAccount(
                    username, password,
                    new { SearchProfileId = searchProfile.SearchProfileId });
            }
            return RedirectToAction("SearchProfiles");
        }

        public ActionResult EditProfile(int spId)
        {
            var searchProfile = cpRepo.GetSearchProfile(spId);
            if (searchProfile == null)
                return HttpNotFound();
            return View(searchProfile);
        }

        [HttpPost]
        public ActionResult EditProfile(SearchProfile profile)
        {
            if (ModelState.IsValid)
            {
                cpRepo.SaveSearchProfile(profile);
                return RedirectToAction("SearchProfiles");
            }
            return View(profile);
        }


        public ActionResult EditSearchProfileContacts(int spId)
        {
            var searchProfile = cpRepo.GetSearchProfile(spId);
            if (searchProfile == null)
                return HttpNotFound();

            return View(searchProfile);
        }

        [HttpPost]
        public ActionResult EditSearchProfileContacts(int spId, string contactIds)
        {
            var contactIdsInt = contactIds.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries).Select(c => Convert.ToInt32(c));

            var searchProfile = cpRepo.GetSearchProfile(spId);
            searchProfile.SearchProfileContacts.Clear();
            cpRepo.SaveChanges();

            int order = 1;
            foreach (int contactId in contactIdsInt)
            {
                var contact = cpRepo.GetContact(contactId);
                if (contact != null)
                {
                    SearchProfileContact sc = new SearchProfileContact() { Contact = contact, Order = order++ };
                    searchProfile.SearchProfileContacts.Add(sc);
                }
            }
            cpRepo.SaveChanges();
            return View(searchProfile);
        }

        public ActionResult InitializeReport(int spId)
        {
            var success = cpRepo.InitializeSearchProfileSimpleReport(spId);
            return RedirectToAction("SearchProfiles");
        }

        public ActionResult TestReports(int spId)
        {
            var searchProfile = cpRepo.GetSearchProfile(spId);
            if (searchProfile == null)
                return HttpNotFound();

            return View(searchProfile);
        }

        public ActionResult GenerateSpreadsheet(int searchProfileId, int numWeeks = 8, int numMonths = 6)
        {
            var searchProfile = cpRepo.GetSearchProfile(searchProfileId);
            if (searchProfile == null)
                return HttpNotFound();

            string templateFolder = ConfigurationManager.AppSettings["PATH_Search"];
            var spreadsheet = new SearchReportPPC(templateFolder, searchProfile.SearchProfileName);
            var propertyNames = new[] { "Title", "Clicks", "Impressions", "Orders", "Cost", "Revenue" };

            bool useAnalytics = false; //TODO: get from searchProfile when implemented
            var weeklyStats = cpRepo.GetWeekStats(searchProfileId, numWeeks, (DayOfWeek)searchProfile.StartDayOfWeek, null, useAnalytics);
            bool includeToday = false;
            var monthlyStats = cpRepo.GetMonthStats(searchProfileId, numMonths, useAnalytics, includeToday);

            spreadsheet.LoadWeeklyStats(weeklyStats, propertyNames);
            spreadsheet.LoadMonthlyStats(monthlyStats, propertyNames);

            var fsr = new FileStreamResult(spreadsheet.GetAsMemoryStream(), SpreadsheetBase.ContentType);
            fsr.FileDownloadName = "TestReport.xlsx";
            return fsr;
            //spreadsheet.DisposeResources();
        }

    }
}
