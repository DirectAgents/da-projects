using CakeExtracter.Commands;
using ClientPortal.Data.Contracts;
using ClientPortal.Data.Entities.TD;
using ClientPortal.Web.Controllers;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebMatrix.WebData;

namespace ClientPortal.Web.Areas.Admin.Controllers
{
    [Authorize(Users = "admin")]
    public class TDAdminController : CPController
    {
        public TDAdminController(ITDRepository tdRepository, IClientPortalRepository cpRepository)
        {
            tdRepo = tdRepository;
            cpRepo = cpRepository;
        }

        public ActionResult InsertionOrders()
        {
            var insertionOrders = tdRepo.InsertionOrders();
            //var insertionOrders = tdRepo.InsertionOrders().ToList();
            //var userProfiles = cpRepo.UserProfiles().Where(up => up.InsertionOrderId.HasValue).ToList();
            //foreach (var io in insertionOrders)
            //{
            //    io.UserProfiles = userProfiles.Where(up => up.InsertionOrderId == io.InsertionOrderID);
            //}

            return View(insertionOrders);
        }

        public ActionResult CreateAccount(int ioID)
        {
            tdRepo.CreateAccountForInsertionOrder(ioID);
            return RedirectToAction("InsertionOrders");
        }

        public ActionResult TradingDeskAccounts()
        {
            var tdAccounts = tdRepo.TradingDeskAccounts().ToList();
            var userProfiles = cpRepo.UserProfiles().Where(up => up.TradingDeskAccountId.HasValue).ToList();
            foreach (var tdAccount in tdAccounts)
            {
                tdAccount.UserProfiles = userProfiles.Where(up => up.TradingDeskAccountId == tdAccount.TradingDeskAccountId);
                var advIds = tdAccount.AdvertiserIds();
                tdAccount.Advertisers = cpRepo.Advertisers.Where(a => advIds.Contains(a.AdvertiserId)).ToList();
            }
            return View(tdAccounts);
        }

        public ActionResult EditAccount(int tdaId)
        {
            var tdAccount = tdRepo.GetTradingDeskAccount(tdaId);
            if (tdAccount == null)
                return HttpNotFound();
            return Do_EditAccount(tdAccount);
        }

        private ActionResult Do_EditAccount(TradingDeskAccount tdAccount)
        {
            tdAccount.UserProfiles = cpRepo.UserProfiles().Where(up => up.TradingDeskAccountId == tdAccount.TradingDeskAccountId).ToList();
            var advIds = tdAccount.AdvertiserIds();
            tdAccount.Advertisers = cpRepo.Advertisers.Where(a => advIds.Contains(a.AdvertiserId)).ToList();

            ViewData["FixedMetricItems"] = FixedMetricItems();
            return View(tdAccount);
        }
        private List<SelectListItem> FixedMetricItems()
        {
            List<SelectListItem> items = new List<SelectListItem>();
            items.AddRange(new[]{
                new SelectListItem() {Text="(none)", Value=""},
                new SelectListItem() {Text="Spend Multiplier", Value="SpendMult"},
                new SelectListItem() {Text="Fixed CPM", Value="CPM"},
                new SelectListItem() {Text="Fixed CPC", Value="CPC"},
            });
            return items;
        }

        [HttpPost]
        public ActionResult EditAccount(TradingDeskAccount tdAccount)
        {
            if (ModelState.IsValid)
            {
                if (string.IsNullOrWhiteSpace(tdAccount.FixedMetricName))
                    tdAccount.FixedMetricValue = null;
                tdRepo.SaveTradingDeskAccount(tdAccount);
                return RedirectToAction("TradingDeskAccounts");
            }
            return Do_EditAccount(tdAccount);
        }

        public ActionResult CreateUserProfile(int tdaId)
        {
            var tdAccount = tdRepo.GetTradingDeskAccount(tdaId);
            if (tdAccount == null)
                return HttpNotFound();
            return View(tdAccount);
        }

        [HttpPost]
        public ActionResult CreateUserProfile(int tdaId, string username, string password) //, bool sendemail, string email)
        {
            var tdAccount = tdRepo.GetTradingDeskAccount(tdaId);
            if (tdAccount != null)
            {
                WebSecurity.CreateUserAndAccount(
                    username, password,
                    new { TradingDeskAccountId = tdAccount.TradingDeskAccountId });

                //if (sendemail && !String.IsNullOrWhiteSpace(email))
                //    Helpers.SendUserProfileEmail(username, password, email);
            }
            return RedirectToAction("TradingDeskAccounts");
        }

        [HttpGet]
        public ActionResult Upload()
        {
            var adrollProfiles = tdRepo.AdRollProfiles();
            return View(adrollProfiles);
        }
        [HttpPost]
        public ActionResult Upload(HttpPostedFileBase file, int profileId)
        {
            string status;
            using (var reader = new StreamReader(file.InputStream))
            {
                TDSynchAdDailySummariesAdrollCsv.RunStatic(reader, profileId, out status);
            }
            return Json(new { status = status.Replace("\n", "<br/>") }, "text/plain");
        }

        public ActionResult StatsRollup(int profileId)
        {
            var statsRollup = tdRepo.AdRollStatsRollup(profileId);
            return View(statsRollup);
        }
    }
}
