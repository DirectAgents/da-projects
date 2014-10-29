using ClientPortal.Data.Contexts;
using ClientPortal.Data.Contracts;
using ClientPortal.Web.Controllers;
using DAGenerators.Spreadsheets;
using System;
using System.Configuration;
using System.Linq;
using System.Reflection;
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

        // ---

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

        // ---

        public ActionResult SearchProfiles()
        {
            var searchProfiles = cpRepo.SearchProfiles();
            return View(searchProfiles.ToList());
        }

        public ActionResult CreateSearchProfile()
        {
            var profile = new SearchProfile
            {
                SearchProfileId = cpRepo.MaxSearchProfileId() + 1
            };
            return View(profile);
        }
        [HttpPost]
        public ActionResult CreateSearchProfile(SearchProfile profile)
        {
            if (ModelState.IsValid)
            {
                if (cpRepo.CreateSearchProfile(profile))
                    return RedirectToAction("SearchProfiles");
                ModelState.AddModelError("", "Profile could not be saved.");
            }
            return View(profile);
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
                if (cpRepo.SaveSearchProfile(profile))
                    return RedirectToAction("SearchProfiles");
                ModelState.AddModelError("", "Profile could not be saved.");
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

        // ---

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

        public ActionResult GenerateSpreadsheet(int searchProfileId, int numWeeks = 8, int numMonths = 6, string filename = "report.xlsx")
        {
            var searchProfile = cpRepo.GetSearchProfile(searchProfileId);
            if (searchProfile == null)
                return HttpNotFound();

            string templateFolder = ConfigurationManager.AppSettings["PATH_Search"];
            var profileAbbrev = searchProfile.SearchProfileName.Replace(" ", "");
            var className = "SearchReport_" + profileAbbrev;

            SearchReportPPC spreadsheet;
            //var assembly = Assembly.GetAssembly(typeof(SearchReportPPC));
            //var type = assembly.GetType("SearchReport_" + profileAbbrev, false);
            var baseType = typeof(SearchReportPPC);
            var type = baseType.Assembly.GetTypes().FirstOrDefault(t => t.IsSubclassOf(baseType) && t.Name == className);

            if (type == null)
                spreadsheet = new SearchReportPPC();
            else
                spreadsheet = (SearchReportPPC)Activator.CreateInstance(type);

            spreadsheet.Setup(templateFolder);
            spreadsheet.SetClientName(searchProfile.SearchProfileName);

            bool useAnalytics = false; //TODO: get from searchProfile when implemented
            bool includeToday = false;
            var monthlyStats = cpRepo.GetMonthStats(searchProfileId, numMonths, useAnalytics, includeToday);
            var weeklyStats = cpRepo.GetWeekStats(searchProfileId, numWeeks, (DayOfWeek)searchProfile.StartDayOfWeek, null, useAnalytics);

            var propertyNames = new[] { "Title", "Clicks", "Impressions", "Orders", "Cost", "Revenue" };
            spreadsheet.LoadMonthlyStats(monthlyStats, propertyNames);
            spreadsheet.LoadWeeklyStats(weeklyStats, propertyNames);

            // TODO: determine from the report which stats are needed
            if (spreadsheet is SearchReport_ScholasticTeacherExpress)
            {
                var yesterday = DateTime.Now.AddDays(-1);
                var monthStart = new DateTime(yesterday.Year, yesterday.Month, 1);
                var monthEnd = monthStart.AddMonths(1).AddDays(-1);
                var latestMonthStats = cpRepo.GetCampaignStats(searchProfileId, null, monthStart, monthEnd, false, useAnalytics);
                ((SearchReport_ScholasticTeacherExpress)spreadsheet).LoadLatestMonthCampaignStats(latestMonthStats, propertyNames);
            }

            var fsr = new FileStreamResult(spreadsheet.GetAsMemoryStream(), SpreadsheetBase.ContentType);
            fsr.FileDownloadName = filename;
            return fsr;
            //spreadsheet.DisposeResources();
        }

    }
}
