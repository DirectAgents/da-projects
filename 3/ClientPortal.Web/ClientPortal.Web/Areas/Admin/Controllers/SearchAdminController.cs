using System;
using System.Configuration;
using System.Linq;
using System.Web.Mvc;
using ClientPortal.Data.Contexts;
using ClientPortal.Data.Contracts;
using ClientPortal.Web.Controllers;
using DAGenerators.Spreadsheets;
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
            return RedirectToAction("SearchProfiles");
        }

        public ActionResult CreateSearchAccount(int spId, string channel)
        {
            var searchAccount = new SearchAccount
            {
                SearchProfileId = spId,
                Channel = channel,
                Name = "New"
            };
            cpRepo.CreateSearchAccount(searchAccount);
            return RedirectToAction("EditProfile", new { spId = spId });
        }
        public ActionResult EditSearchAccount(int id)
        {
            var searchAccount = cpRepo.GetSearchAccount(id);
            if (searchAccount == null)
                return HttpNotFound();
            return View(searchAccount);
        }
        [HttpPost]
        public ActionResult EditSearchAccount(SearchAccount searchAccount)
        {
            if (ModelState.IsValid)
            {
                if (cpRepo.SaveSearchAccount(searchAccount))
                    return RedirectToAction("EditProfile", new { spId = searchAccount.SearchProfileId });
                ModelState.AddModelError("", "SearchAccount could not be saved.");
            }
            return View(searchAccount);
        }

        // ---

        public ActionResult InitializeReport(int spId)
        {
            var success = cpRepo.InitializeSearchProfileSimpleReport(spId);
            return RedirectToAction("SearchProfiles");
        }

        [HttpGet]
        public ActionResult GenerateSpreadsheet(int spId)
        {
            var searchProfile = cpRepo.GetSearchProfile(spId);
            if (searchProfile == null)
                return HttpNotFound();

            return View(searchProfile);
        }

        [HttpPost]
        public ActionResult GenerateSpreadsheet(int searchProfileId, DateTime? endDate, int numWeeks = 0, int numMonths = 0, string filename = "report.xlsx", bool groupBySearchAccount = false, string campaignInclude = null, string campaignExclude = null)
        {
            string templateFolder = ConfigurationManager.AppSettings["PATH_Search"];
            if (!endDate.HasValue)
                endDate = DateTime.Today.AddDays(-1); // if not specified; (user can always set endDate to today if desired)
                //endDate = (UserSettings.Search_UseYesterdayAsLatest ? DateTime.Today.AddDays(-1) : DateTime.Today);

            var spreadsheet = DAGenerators.Spreadsheets.GeneratorCP.GenerateSearchReport(cpRepo, templateFolder, searchProfileId, numWeeks, numMonths, endDate.Value, groupBySearchAccount, campaignInclude, campaignExclude);
            if (spreadsheet == null)
                return HttpNotFound();

            var fsr = new FileStreamResult(spreadsheet.GetAsMemoryStream(), SpreadsheetBase.ContentType);
            fsr.FileDownloadName = filename;
            return fsr;
            //spreadsheet.DisposeResources();
        }

        public ActionResult Components(int spId) // (Report Components, that is)
        {
            var searchProfile = cpRepo.GetSearchProfile(spId);
            if (searchProfile == null)
                return HttpNotFound();

            return View(searchProfile);
        }

        // --- SearchConvTypes ---

        public ActionResult ConvTypes(int spId)
        {
            var searchProfile = cpRepo.GetSearchProfile(spId);

            return View(searchProfile);
        }
        //[HttpPost]
        public JsonResult ConvTypesData(int spId)
        {
            var searchConvTypes = cpRepo.GetSearchConvTypes(spId).OrderBy(ct => ct.Alias);

            var kg = new KG<SearchConvType>();
            kg.data = searchConvTypes;
            kg.total = searchConvTypes.Count();

            var json = Json(kg, JsonRequestBehavior.AllowGet);
            //var json = Json(kg);
            return json;
        }
        [HttpPost]
        public ActionResult ConvTypesUpdate(SearchConvType[] models)
        {
            //TODO: try/catch/return errors
            foreach (var row in models)
            {
                var searchConvType = cpRepo.GetSearchConvType(row.SearchConvTypeId);
                searchConvType.Alias = row.Alias;
            }
            cpRepo.SaveChanges();

            return Json(models);
        }

    }
}
