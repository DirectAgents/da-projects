using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CakeExtracter.Commands;
using DirectAgents.Domain.Abstract;
using DirectAgents.Domain.Entities.TD;
using DirectAgents.Web.Areas.TD.Models;

namespace DirectAgents.Web.Areas.TD.Controllers
{
    public class ExtAccountsController : DirectAgents.Web.Controllers.ControllerBase
    {
        public ExtAccountsController(ITDRepository tdRepository)
        {
            this.tdRepo = tdRepository;
        }

        public ActionResult Index(string platform, int? campId)
        {
            var extAccounts = tdRepo.ExtAccounts(platformCode: platform, campId: campId)
                .OrderBy(a => a.Platform.Name).ThenBy(a => a.Name);

            Session["platformCode"] = platform;
            Session["campId"] = campId.ToString();
            return View(extAccounts);
        }

        // For each account, shows a "gauge" of what stats are loaded
        public ActionResult IndexGauge(string platform, int? campId)
        {
            var extAccounts = tdRepo.ExtAccounts(platformCode: platform, campId: campId)
                .OrderBy(a => a.Platform.Name).ThenBy(a => a.Name);

            List<StatsGaugeVM> statsGaugeVMs = new List<StatsGaugeVM>();
            foreach (var extAcct in extAccounts)
            {
                var statsGaugeVM = new StatsGaugeVM
                {
                    Platform = extAcct.Platform,
                    ExtAccount = extAcct,
                    Gauge = tdRepo.GetStatsGauge(extAcct.Id)
                };
                statsGaugeVMs.Add(statsGaugeVM);
            }
            return View(statsGaugeVMs);
        }

        public ActionResult CreateNew(string platform)
        {
            var plat = tdRepo.Platform(platform);
            if (plat != null)
            {
                var extAcct = new ExtAccount
                {
                    PlatformId = plat.Id,
                    Name = "zNew"
                };
                if (tdRepo.AddExtAccount(extAcct))
                    return RedirectToAction("Index", new { platform = Session["platformCode"], campId = Session["campId"] });
            }
            return Content("Error creating ExtAccount");
        }

        [HttpGet]
        public ActionResult Edit(int id)
        {
            var extAcct = tdRepo.ExtAccount(id);
            if (extAcct == null)
                return HttpNotFound();
            return View(extAcct);
        }
        [HttpPost]
        public ActionResult Edit(ExtAccount extAcct)
        {
            if (ModelState.IsValid)
            {
                if (tdRepo.SaveExtAccount(extAcct))
                    return RedirectToAction("Index", new { platform = Session["platformCode"], campId = Session["campId"] });
                ModelState.AddModelError("", "ExtAccount could not be saved.");
            }
            tdRepo.FillExtended(extAcct);
            return View(extAcct);
        }

        // --- Maintenance ---

        // json data for Maintenance Grid
        public JsonResult IndexData(string platform)
        {
            var extAccounts = tdRepo.ExtAccounts(platformCode: platform)
                .Select(a => new
                {
                    a.Id,
                    a.ExternalId,
                    a.Name,
                    Platform = a.Platform.Name
                });
            var json = Json(new { data = extAccounts, total = extAccounts.Count() });
            return json;
        }

        public ActionResult MaintenanceDetail(int id)
        {
            var extAcct = tdRepo.ExtAccount(id);
            if (extAcct == null)
                return HttpNotFound();

            bool syncable = extAcct.Platform.Code == Platform.Code_AdRoll ||
                            extAcct.Platform.Code == Platform.Code_FB;
            if (extAcct.Platform.Code == Platform.Code_DBM)
            {
                int ioID;
                if (int.TryParse(extAcct.ExternalId, out ioID))
                {
                    var io = tdRepo.InsertionOrder(ioID);
                    if (io != null)
                        syncable = !string.IsNullOrWhiteSpace(io.Bucket);
                }
            }
            var model = new AccountMaintenanceVM
            {
                ExtAccount = extAcct,
                Syncable = syncable,
                StatsGauge = tdRepo.GetStatsGauge(extAcct.Id)
            };
            return PartialView(model);
        }

        public JsonResult Sync(int id, DateTime? start, string level)
        {
            var extAcct = tdRepo.ExtAccount(id);
            if (extAcct == null)
                return null;

            // TODO: Go back to campaign's start date
            if (!start.HasValue)
                start = Common.FirstOfMonth().AddMonths(-6);
                //start = Common.FirstOfYear().AddYears(-1);
            else if (start.Value.Day > 1)
                start = new DateTime(start.Value.Year, start.Value.Month, 1);
            // Go back to 1st of month - so as to refresh the stats

            switch (extAcct.Platform.Code)
            {
                case Platform.Code_AdRoll:
                    string oneStatPer;
                    if (level == "account")
                        oneStatPer = "advertisable";
                    else if (level == "strategy")
                        oneStatPer = "campaign";
                    else if (level == "creative")
                        oneStatPer = "ad";
                    else
                        oneStatPer = level;
                    DASynchAdrollStats.RunStatic(extAcct.ExternalId, startDate: start, oneStatPer: oneStatPer);
                    break;
                case Platform.Code_DBM:
                    int ioID;
                    if (int.TryParse(extAcct.ExternalId, out ioID))
                        DASynchDBMStatsOld.RunStatic(insertionOrderID: ioID); // gets report with stats up to yesterday (and back ?30? days)
                    break;
                case Platform.Code_FB:
                    DASynchFacebookStats.RunStatic(accountId: extAcct.Id, startDate: start);
                    break;
            }
            return null;
        }

        // --- Strats, TDads, etc

        public ActionResult Strategies(int? id)
        {
            var strategies = tdRepo.Strategies(acctId: id).OrderBy(s => s.Name);
            return View(strategies);
        }
        public ActionResult TDads(int? id)
        {
            var ads = tdRepo.TDads(acctId: id).OrderBy(a => a.Name).ThenBy(a => a.Id);
            return View(ads);
        }

        // --- Stats Uploading ---

        public ActionResult UploadStats(int id)
        {
            var extAcct = tdRepo.ExtAccount(id);
            if (extAcct == null)
                return HttpNotFound();

            return View(extAcct);
        }
        public ActionResult UploadFile(int id, HttpPostedFileBase file, string statsType)
        {
            using (var reader = new StreamReader(file.InputStream))
            {
                DASynchTDDailySummaries.RunStatic(id, reader, statsType);
            }
            return null;
        }
	}
}