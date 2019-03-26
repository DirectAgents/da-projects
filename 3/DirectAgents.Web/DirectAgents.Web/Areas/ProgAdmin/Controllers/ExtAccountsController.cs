using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using CakeExtracter.Commands;
using DirectAgents.Domain.Abstract;
using DirectAgents.Domain.DTO;
using DirectAgents.Domain.Entities.CPProg;
using DirectAgents.Web.Areas.ProgAdmin.Models;
using DirectAgents.Web.Constants;


namespace DirectAgents.Web.Areas.ProgAdmin.Controllers
{
    public class ExtAccountsController : DirectAgents.Web.Controllers.ControllerBase
    {
        public ExtAccountsController(ICPProgRepository cpProgRepository, ITDRepository tdRepository)
        {
            this.cpProgRepo = cpProgRepository;
            this.tdRepo = tdRepository;
        }

        public ActionResult Index(string platform, int? campId)
        {
            var extAccounts = cpProgRepo.ExtAccounts(platformCode: platform, campId: campId)
                .OrderBy(a => a.Platform.Name).ThenBy(a => a.Disabled).ThenBy(a => a.Name);

            Session["platformCode"] = platform;
            Session["campId"] = campId.ToString();
            return View(extAccounts);
        }

        public ActionResult IndexGaugeSummary(bool extended = false)
        {
            var statsGauges = cpProgRepo.StatsGaugesByPlatform(extended: extended);
            var model = new StatsGaugeVM
            {
                StatsGauges = statsGauges.OrderBy(x => x.Platform.Name).ToList(),
                Extended = extended
            };
            return View("IndexGauge", model);
        }

        // For each account, shows a "gauge" of what stats are loaded
        public ActionResult IndexGauge(string platform, int? campId, bool recent = false, bool extended = false)
        {
            var platId = cpProgRepo.Platform(platform)?.Id;

            // "recent" means only include accounts with stats for this month
            var minDate = recent ? DateTime.Today.FirstDayOfMonth(-1) : (DateTime?) null;
            var statsGauges = cpProgRepo.StatsGaugesByAccount(platformId: platId, campId: campId, minDate: minDate, extended: extended);

            if (recent)
                statsGauges = statsGauges.Where(x => x.HasStats());

            //Group by platform
            var platformGauges = GetStatsGaugeListGroupedByPlatform(statsGauges);

            var model = new StatsGaugeVM
            {
                PlatformCode = platform,
                CampaignId = campId,
                StatsGauges = platformGauges,
                Extended = extended
            };
            return View(model);
        }

        public ActionResult CreateNew(string platform)
        {
            var plat = cpProgRepo.Platform(platform);
            if (plat != null)
            {
                var extAcct = new ExtAccount
                {
                    PlatformId = plat.Id,
                    Name = "zNew"
                };
                if (cpProgRepo.AddExtAccount(extAcct))
                    return RedirectToAction("Index", new { platform = Session["platformCode"], campId = Session["campId"] });
            }
            return Content("Error creating ExtAccount");
        }

        //Used for creating the group of 4 ExtAccounts for FB
        [HttpGet]
        public ActionResult CreateGroup(string platform)
        {
            var plat = cpProgRepo.Platform(platform);
            if (plat == null)
                return HttpNotFound();
            return View(plat);
        }
        [HttpPost]
        public ActionResult CreateGroup(ExtAccount extAcct)
        {
            CreateAddExtAccount(extAcct, " - Facebook", Network.ID_Facebook);
            CreateAddExtAccount(extAcct, " - Instagram", Network.ID_Instagram);
            CreateAddExtAccount(extAcct, " _AudNetwork", Network.ID_AudNetwork);
            CreateAddExtAccount(extAcct, " _Messenger", Network.ID_Messenger);

            var plat = cpProgRepo.Platform(extAcct.PlatformId);
            if (plat == null)
                return HttpNotFound();
            return RedirectToAction("Index", new { platform = plat.Code });
        }
        private void CreateAddExtAccount(ExtAccount baseAcct, string nameSuffix, int networkId)
        {
            var extAcct = new ExtAccount
            {
                PlatformId = baseAcct.PlatformId,
                ExternalId = baseAcct.ExternalId,
                Name = baseAcct.Name + nameSuffix,
                NetworkId = networkId
            };
            cpProgRepo.AddExtAccount(extAcct);
        }

        [HttpGet]
        public ActionResult Edit(int id)
        {
            var extAcct = cpProgRepo.ExtAccount(id);
            if (extAcct == null)
                return HttpNotFound();
            SetupForEdit();
            return View(extAcct);
        }
        [HttpPost]
        public ActionResult Edit(ExtAccount extAcct)
        {
            if (ModelState.IsValid)
            {
                if (cpProgRepo.SaveExtAccount(extAcct))
                    return RedirectToAction("Index", new { platform = Session["platformCode"], campId = Session["campId"] });
                ModelState.AddModelError("", "ExtAccount could not be saved.");
            }
            cpProgRepo.FillExtended(extAcct);
            SetupForEdit();
            return View(extAcct);
        }
        private void SetupForEdit()
        {
            ViewBag.Networks = cpProgRepo.Networks().OrderBy(n => n.Name);
        }

        /// <summary>
        /// The method returns a list of stats gauges grouped by platform, ordered by the value "Disabled" of ext account
        /// </summary>
        /// <param name="statsGauges">Not sorted, not grouped list of stats gauges</param>
        /// <returns>Sorted and grouped list of stats gauges</returns>
        private static IEnumerable<TDStatsGauge> GetStatsGaugeListGroupedByPlatform(IEnumerable<TDStatsGauge> statsGauges) 
        {
            var platformGroups = statsGauges.GroupBy(s => s.ExtAccount.Platform);
            return platformGroups.Select(GetStatsGauge).ToList();
        }

        /// <summary>
        /// The method returns a stats gauge for a platform
        /// </summary>
        /// <param name="platformGroup">A platform group</param>
        /// <returns>Item of a stats gauge for a platform</returns>
        private static TDStatsGauge GetStatsGauge(IGrouping<Platform, TDStatsGauge> platformGroup)
        {
            var pGauge = new TDStatsGauge
            {
                Platform = platformGroup.Key,
                Children = platformGroup
                    .OrderBy(gauge => gauge.ExtAccount.Disabled)
                    .ThenBy(gauge => gauge.ExtAccount.Name)
                    .ToList()
            };
            pGauge.SetFrom(platformGroup); // ?SetFrom(pGauge.Children) ...same thing?
            return pGauge;
        }

        #region Maintenance

        // json data for Maintenance Grid
        public JsonResult IndexData(string platform)
        {
            var extAccounts = cpProgRepo.ExtAccounts(platformCode: platform)
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
            var extAcct = cpProgRepo.ExtAccount(id);
            if (extAcct == null)
                return HttpNotFound();

            bool syncable = extAcct.Platform.Code == Platform.Code_Adform ||
                            extAcct.Platform.Code == Platform.Code_AdRoll ||
                            extAcct.Platform.Code == Platform.Code_FB ||
                            extAcct.Platform.Code == Platform.Code_YAM;

            var model = new AccountMaintenanceVM
            {
                ExtAccount = extAcct,
                Syncable = syncable,
                StatsGauge = cpProgRepo.GetStatsGaugeViaIds(acctId: extAcct.Id)
            };
            return PartialView(model);
        }

        // Called from links on ExtAccounts/MaintenanceDetail view - via Platforms/Maintenance json (SyncAccount)
        public JsonResult Sync(int id, DateTime? start, DateTime? latest, string level)
        {
            var extAcct = cpProgRepo.ExtAccount(id);
            if (extAcct != null)
                DoSync(start, latest, level, extAcct);
            return null;
        }

        // Maintenance/Detail form handler
        public ActionResult CustomSync(int id, DateTime start, string level)
        {
            var extAcct = cpProgRepo.ExtAccount(id);
            if (extAcct == null)
                return HttpNotFound();

            DoSync(start, null, level, extAcct);
            return RedirectToAction("Maintenance", "Platforms", new { id = extAcct.PlatformId });
        }

        // Called from the StatsGauge for a campaign
        public ActionResult StatsGaugeSyncClear(int id, int? campId, DateTime from, DateTime to, string level, bool clear, bool sync)
        {
            if (id < 0 && campId.HasValue) // (negative)id => platformId, clear/sync platform+campaign
            {
                int platformId = -id;
                var platform = cpProgRepo.Platform(platformId);
                if (platform == null)
                    return HttpNotFound();

                if (clear)
                    DoClear(from, to, level, campId: campId, platformId: platform.Id);
                if (sync)
                    DoSyncForCampaign(from, to, level, campId.Value, platform);
            }
            else // clear/sync one extAccount
            {
                var extAcct = cpProgRepo.ExtAccount(id);
                if (extAcct == null)
                    return HttpNotFound();

                if (clear)
                    DoClear(from, to, level, acctId: extAcct.Id);
                if (sync)
                    DoSyncForAccount(from, to, level, extAcct);
            }
            if (campId.HasValue)
                return RedirectToAction("IndexGauge", new { campId = campId.Value, super = 1 });
            else
                return Content("Done sync");
        }

        private void DoClear(DateTime? start, DateTime? end, string statsType, int? acctId = null, int? campId = null, int? platformId = null)
        {
            if (!start.HasValue || !end.HasValue || statsType == null)
                return;
            statsType = NormalizeStatsType(statsType);
            switch (statsType)
            {
                case "daily":
                    var dsums = cpProgRepo.DailySummaries(start, end, acctId: acctId, campId: campId, platformId: platformId);
                    cpProgRepo.DeleteDailySummaries(dsums);
                    break;
                case "strategy":
                    var ssums = cpProgRepo.StrategySummaries(start, end, acctId: acctId, campId: campId, platformId: platformId);
                    cpProgRepo.DeleteStrategySummaries(ssums);
                    break;
                case "adset":
                    var actions = cpProgRepo.AdSetActions(start, end, acctId: acctId, campId: campId, platformId: platformId);
                    cpProgRepo.DeleteAdSetActionStats(actions);
                    var asums = cpProgRepo.AdSetSummaries(start, end, acctId: acctId, campId: campId, platformId: platformId);
                    cpProgRepo.DeleteAdSetSummaries(asums);
                    break;
            }
        }

        private static void DoSync(DateTime? start, DateTime? latest, string level, ExtAccount extAcct)
        {
            if (!start.HasValue)
            {
                if (!latest.HasValue) // TODO: Go back to campaign's start date?
                    start = DateTime.Today.FirstDayOfQuarter().AddMonths(-3);
                else if (latest.Value.Day > 1)
                    start = new DateTime(latest.Value.Year, latest.Value.Month, 1);
                    // Go back to 1st of month - so as to refresh the month's stats
                else
                    start = latest.Value.AddMonths(-1); // if the latest stats are on the 1st
            }
            DoSyncForAccount(start, null, level, extAcct);
        }
        private static void DoSyncForAccount(DateTime? start, DateTime? end, string statsType, ExtAccount extAcct)
        {
            statsType = NormalizeStatsType(statsType);
            switch (extAcct.Platform.Code)
            {
                case Platform.Code_AdRoll:
                    string oneStatPer;
                    if (statsType == "daily")
                        oneStatPer = "advertisable";
                    else if (statsType == "strategy")
                        oneStatPer = "campaign";
                    else if (statsType == "creative")
                        oneStatPer = "ad";
                    else
                        oneStatPer = statsType;
                    DASynchAdrollStats.RunStatic(accountId: extAcct.Id, startDate: start, endDate: end, oneStatPer: oneStatPer);
                    break;
                case Platform.Code_Adform:
                    DASynchAdformStats.RunStatic(accountId: extAcct.Id, startDate: start, endDate: end, statsType: statsType);
                    break;
                case Platform.Code_Amazon:
                    DASynchAmazonStats.RunStatic(accountId: extAcct.Id, startDate: start, endDate: end, statsType: statsType, fromDatabase: true);
                    //Note: daily stats are just summed by day from strategy stats
                    break;
                case Platform.Code_Criteo:
                    DASynchCriteoStats.RunStatic(accountId: extAcct.Id, startDate: start, endDate: end, statsType: statsType);
                    //Note: daily stats are just summed by day from strategy stats
                    break;
                case Platform.Code_FB:
                    DASynchFacebookStats.RunStatic(accountId: extAcct.Id, startDate: start, endDate: end, statsType: statsType);
                    break;
                case Platform.Code_YAM:
                    DASynchYAMStats.RunStatic(accountId: extAcct.Id, startDate: start, endDate: end, statsType: statsType);
                    break;
            }
        }
        private static void DoSyncForCampaign(DateTime? start, DateTime? end, string statsType, int campaignId, Platform platform)
        {
            statsType = NormalizeStatsType(statsType);
            switch (platform.Code)
            {
                case Platform.Code_FB:
                    DASynchFacebookStats.RunStatic(campaignId: campaignId, startDate: start, endDate: end, statsType: statsType);
                    break;
            }
        }

        private static string NormalizeStatsType(string statsType)
        {
            if (statsType != null)
            {
                statsType = statsType.ToLower();
                if (statsType.StartsWith("adset"))
                    statsType = "adset";
            }
            return statsType;
        }

        #endregion

        #region Strats, AdSets, etc

        public ActionResult Strategies(int? id, int? strategyId, OrderBy sort = OrderBy.StrategyName)
        {
            var strategies = cpProgRepo.Strategies(acctId: id, id: strategyId);
            IOrderedQueryable<Strategy> orderedStrategies;
            switch (sort)
            {
                case OrderBy.StrategyType:
                    orderedStrategies = strategies.OrderBy(s => s.Type.Name).ThenBy(x => x.Name);
                    break;
                default:
                    orderedStrategies = strategies.OrderBy(s => s.Name);
                    break;
            }

            var strategyList = orderedStrategies.ThenBy(x => x.ExternalId).ToList();
            return View(strategyList);
        }

        public ActionResult AdSets(int? id, int? adSetId, OrderBy sort = OrderBy.AdSetName)
        {
            var adsets = cpProgRepo.AdSets(acctId: id, id: adSetId);
            IOrderedQueryable<AdSet> orderedAdSets;
            switch (sort)
            {
                case OrderBy.StrategyType:
                    orderedAdSets = adsets.OrderBy(s => s.Strategy.Type.Name).ThenBy(x => x.Strategy.Name).ThenBy(x => x.Name);
                    break;
                case OrderBy.StrategyName:
                    orderedAdSets = adsets.OrderBy(x => x.Strategy.Name).ThenBy(x => x.Name);
                    break;
                default:
                    orderedAdSets = adsets.OrderBy(x => x.Name);
                    break;
            }

            var adSetList = orderedAdSets.ThenBy(x => x.ExternalId).ToList();
            return View(adSetList);
        }

        public ActionResult Keywords(int? id, int? keywordId, OrderBy sort = OrderBy.KeywordName)
        {
            var keywords = cpProgRepo.Keywords(acctId: id, id: keywordId);
            IOrderedQueryable<Keyword> orderedKeywords;
            switch (sort)
            {
                case OrderBy.StrategyType:
                    orderedKeywords = keywords.OrderBy(s => s.Strategy.Type.Name).ThenBy(x => x.Strategy.Name).ThenBy(x => x.Name);
                    break;
                case OrderBy.StrategyName:
                    orderedKeywords = keywords.OrderBy(x => x.Strategy.Name).ThenBy(x => x.Name);
                    break;
                case OrderBy.AdSetName:
                    orderedKeywords = keywords.OrderBy(x => x.AdSet.Name).ThenBy(x => x.AdSet.Id).ThenBy(x => x.Name);
                    break;
                default:
                    orderedKeywords = keywords.OrderBy(x => x.Name);
                    break;
            }

            var keywordList = orderedKeywords.ThenBy(x => x.ExternalId).ToList();
            return View(keywordList);
        }

        public ActionResult SearchTerms(int? id, int? searchTermId, OrderBy sort = OrderBy.SearchTermName)
        {
            var searchTerms = cpProgRepo.SearchTerms(acctId: id, id: searchTermId);
            IOrderedQueryable<SearchTerm> orderedSearchTerms;
            switch (sort)
            {
                case OrderBy.KeywordName:
                    orderedSearchTerms = searchTerms.OrderBy(x => x.Keyword.Name).ThenBy(x => x.Name);
                    break;
                default:
                    orderedSearchTerms = searchTerms.OrderBy(x => x.Name);
                    break;
            }

            var termList = orderedSearchTerms.ToList();
            return View(termList);
        }

        #endregion
        
        #region Stats Uploading
        
        public ActionResult UploadStats(int id)
        {
            var extAcct = cpProgRepo.ExtAccount(id);
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

            var extAcct = cpProgRepo.ExtAccount(id);

            if (statsType.ToUpper() == "DAILYANDSTRATEGY")
            {
                using (var reader = new StreamReader(file.InputStream, Encoding.UTF8, true, 1024, true)) // leaveOpen: true
                {
                    DASynchTDDailySummaries.RunStatic(id, reader, "daily", statsDate: statsDateNullable);
                    reader.BaseStream.Position = 0;
                    DASynchTDDailySummaries.RunStatic(id, reader, "strategy", statsDate: statsDateNullable);
                }
                file.InputStream.Dispose();
            }
            else using (var reader = new StreamReader(file.InputStream))
            {
                if (statsType != null && statsType.ToUpper().StartsWith("CONV") && extAcct.Platform.Code == Platform.Code_AdRoll)
                    DASynchAdrollConvCsv.RunStatic(id, reader); // TODO: generic Conv syncher?
                else
                    DASynchTDDailySummaries.RunStatic(id, reader, statsType, statsDate: statsDateNullable);
            }

            return null;
        }

        #endregion
    }
}