using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CakeExtracter.Commands;
using ClientPortal.Data.Contracts;
using ClientPortal.Data.Entities.TD;
using ClientPortal.Data.Entities.TD.AdRoll;
using ClientPortal.Data.Entities.TD.DBM;
using ClientPortal.Web.Controllers;
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

        [HttpGet]
        public ActionResult NewAccount()
        {
            int maxId = tdRepo.MaxTradingDeskAccountId();
            var tda = new TradingDeskAccount
            {
                TradingDeskAccountId = maxId + 1
            };
            tdRepo.CreateTradingDeskAccount(tda);

            return RedirectToAction("TradingDeskAccounts");
        }

        [HttpGet]
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
        //TODO: check if username is already taken

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

        // --- Insertion Orders and AdRoll Profiles

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

        [HttpGet]
        public ActionResult CreateInsertionOrder(int tdaId)
        {
            var io = new InsertionOrder
            {
                TradingDeskAccountId = tdaId
            };
            return View(io);
        }
        [HttpPost]
        public ActionResult CreateInsertionOrder(InsertionOrder io)
        {
            var existingIO = tdRepo.GetInsertionOrder(io.InsertionOrderID);
            if (existingIO != null)
            {
                ModelState.AddModelError("", "Insertion Order (with that Id) already added to TradingDeskAccount " + existingIO.TradingDeskAccountId);
            }
            if (ModelState.IsValid)
            {
                tdRepo.SaveInsertionOrder(io);
                return RedirectToAction("EditAccount", new { tdaId = io.TradingDeskAccountId });
            }
            return View(io);
        }

        [HttpGet]
        public ActionResult EditInsertionOrder(int id)
        {
            var io = tdRepo.GetInsertionOrder(id);
            if (io == null)
                return HttpNotFound();
            return View(io);
        }
        [HttpPost]
        public ActionResult EditInsertionOrder(InsertionOrder io)
        {
            if (ModelState.IsValid)
            {
                tdRepo.SaveInsertionOrder(io);
                return RedirectToAction("EditAccount", new { tdaId = io.TradingDeskAccountId });
            }
            return View(io);
        }

        public ActionResult AdRollProfiles()
        {
            var arps = tdRepo.AdRollProfiles();
            return View(arps);
        }

        public ActionResult CreateNextAdRollProfile(int tdaId)
        {
            int maxId = tdRepo.MaxAdRollProfileId();
            var arProfile = new AdRollProfile
            {
                Id = maxId + 1,
                TradingDeskAccountId = tdaId,
                Name = "New Profile"
            };
            tdRepo.SaveAdRollProfile(arProfile);
            return RedirectToAction("EditAccount", new { tdaId = tdaId });
        }

        [HttpGet]
        public ActionResult EditAdRollProfile(int id)
        {
            var arp = tdRepo.GetAdRollProfile(id);
            if (arp == null)
                return HttpNotFound();
            return View(arp);
        }
        [HttpPost]
        public ActionResult EditAdRollProfile(AdRollProfile arProfile)
        {
            if (ModelState.IsValid)
            {
                tdRepo.SaveAdRollProfile(arProfile);
                return RedirectToAction("EditAccount", new { tdaId = arProfile.TradingDeskAccountId });
            }
            return View(arProfile);
        }

        // --- Etc ---

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
