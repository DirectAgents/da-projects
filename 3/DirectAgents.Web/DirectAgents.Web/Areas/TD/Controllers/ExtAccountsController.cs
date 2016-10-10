﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CakeExtracter.Commands;
using DirectAgents.Domain.Abstract;
using DirectAgents.Domain.DTO;
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
        public ActionResult IndexGauge(string platform, int? campId, bool recent = false)
        {
            var extAccounts = tdRepo.ExtAccounts(platformCode: platform, campId: campId)
                .OrderBy(a => a.Platform.Name).ThenBy(a => a.Name);

            var recentDate = DateTime.Today.FirstDayOfMonth(-1); // for comparison, if recent==true
            List<TDStatsGauge> statsGauges = new List<TDStatsGauge>();
            foreach (var extAcct in extAccounts)
            {
                var statsGauge = tdRepo.GetStatsGauge(extAccount: extAcct);
                if (recent == false || (statsGauge.Daily.Latest.HasValue && statsGauge.Daily.Latest.Value >= recentDate))
                    statsGauges.Add(statsGauge);
            }

            //Group by platform
            var platformGroups = statsGauges.GroupBy(s => s.ExtAccount.Platform).OrderBy(g => g.Key.Name);
            List<TDStatsGauge> platformGauges = new List<TDStatsGauge>();
            foreach (var platGroup in platformGroups)
            {
                var pGauge = new TDStatsGauge
                {
                    Platform = platGroup.Key,
                    Children = platGroup.ToList()
                };
                pGauge.Daily.Earliest = platGroup.Min(p => p.Daily.Earliest);
                pGauge.Daily.Latest = platGroup.Max(p => p.Daily.Latest);
                pGauge.Strategy.Earliest = platGroup.Min(p => p.Strategy.Earliest);
                pGauge.Strategy.Latest = platGroup.Max(p => p.Strategy.Latest);
                pGauge.Creative.Earliest = platGroup.Min(p => p.Creative.Earliest);
                pGauge.Creative.Latest = platGroup.Max(p => p.Creative.Latest);
                pGauge.Site.Earliest = platGroup.Min(p => p.Site.Earliest);
                pGauge.Site.Latest = platGroup.Max(p => p.Site.Latest);
                pGauge.Conv.Earliest = platGroup.Min(p => p.Conv.Earliest);
                pGauge.Conv.Latest = platGroup.Max(p => p.Conv.Latest);

                platformGauges.Add(pGauge);
            }

            var model = new StatsGaugeVM
            {
                PlatformCode = platform,
                CampaignId = campId,
                StatsGauges = platformGauges
            };
            return View(model);
        }
        public ActionResult IndexGaugeSummary()
        {
            var platforms = tdRepo.Platforms().OrderBy(p => p.Name);
            List<TDStatsGauge> statsGauges = new List<TDStatsGauge>();
            foreach (var platform in platforms)
            {
                var statsGauge = tdRepo.GetStatsGauge(platform: platform);
                statsGauges.Add(statsGauge);
            }
            var model = new StatsGaugeVM
            {
                StatsGauges = statsGauges
            };
            return View("IndexGauge", model);
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
                StatsGauge = tdRepo.GetStatsGaugeViaIds(acctId: extAcct.Id)
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
                    DASynchAdrollStats.RunStatic(accountId: extAcct.Id, startDate: start, oneStatPer: oneStatPer);
                    break;
                case Platform.Code_DBM: //TODO: remove/replace this
                    int ioID;
                    if (int.TryParse(extAcct.ExternalId, out ioID))
                        DASynchDBMStatsOld.RunStatic(insertionOrderID: ioID); // gets report with stats up to yesterday (and back ?30? days)
                    break;
                case Platform.Code_FB: //TODO: pass in statsType
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

        // --- Stats Uploading ---

        public ActionResult UploadStats(int id)
        {
            var extAcct = tdRepo.ExtAccount(id);
            if (extAcct == null)
                return HttpNotFound();

            //TODO: remember where came from - for Back button

            return View(extAcct);
        }
        public ActionResult UploadFile(int id, HttpPostedFileBase file, string statsType, string statsDate)
        {
            DateTime? statsDateNullable = null;
            DateTime parseDate;
            if (DateTime.TryParse(statsDate, out parseDate))
                statsDateNullable = parseDate;
            else
                statsDateNullable = null;

            using (var reader = new StreamReader(file.InputStream))
            {
                //if (statsType != null && statsType.ToUpper().StartsWith("CONV"))
                //    DASynchAdrollConvCsv.RunStatic(id, reader); // TODO: generic Conv syncher?
                //else
                DASynchTDDailySummaries.RunStatic(id, reader, statsType, statsDate: statsDateNullable);
            }
            return null;
        }
	}
}