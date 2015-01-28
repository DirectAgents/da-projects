using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using AutoMapper;
using ClientPortal.Data.Contracts;
using ClientPortal.Data.DTOs.TD;
using ClientPortal.Data.Entities.TD;
using ClientPortal.Data.Entities.TD.AdRoll;
using ClientPortal.Data.Entities.TD.DBM;
using ClientPortal.Web.Areas.TD.Models;
using ClientPortal.Web.Controllers;
using ClientPortal.Web.Models;
using DirectAgents.Mvc.KendoGridBinder;
using DirectAgents.Mvc.KendoGridBinder.Containers;

namespace ClientPortal.Web.Areas.TD.Controllers
{
    [Authorize]
    public class StatsController : CPController
    {
        public StatsController(ITDRepository tdRepository, IClientPortalRepository cpRepository)
        {
            tdRepo = tdRepository;
            cpRepo = cpRepository;
        }

        public JsonResult SummaryData(KendoGridRequest request, string startdate, string enddate)
        {
            DateTime? start, end;
            var userInfo = GetUserInfo();
            if (!ControllerHelpers.ParseDates(startdate, enddate, userInfo.CultureInfo, out start, out end) || userInfo.TDAccount == null)
                return Json(new { });

            var summaries = tdRepo.GetDailyStatsSummaries(start, end, userInfo.TDAccount);

            var kgrid = new KendoGrid<StatsSummary>(request, summaries);
            kgrid.aggregates = GetAggregates(summaries, userInfo);

            var json = Json(kgrid, JsonRequestBehavior.AllowGet);
            return json;
        }

        public JsonResult SampleData(KendoGridRequest request)
        {
            //int insertionOrderID = 1286935; // Betterment
            int adrollProfileId = 1; // AHS
            var tda = new TradingDeskAccount
            {
                InsertionOrders = new InsertionOrder[] { },
                AdRollProfiles = new[] { new AdRollProfile { Id = adrollProfileId } }
            };
            var start = new DateTime(2014, 12, 1);
            var end = new DateTime(2014, 12, 31);

            var summaries = tdRepo.GetDailyStatsSummaries(start, end, tda);
            var kgrid = new KendoGrid<StatsSummary>(request, summaries);
            if (summaries.Any())
            {
                kgrid.aggregates = new
                {
                    Impressions = new { sum = summaries.Sum(s => s.Impressions) },
                    Clicks = new { sum = summaries.Sum(s => s.Clicks) },
                    Conversions = new { sum = summaries.Sum(s => s.Conversions) },
                    Spend = new { sum = summaries.Sum(s => s.Spend) }
                };
            }
            var json = Json(kgrid, JsonRequestBehavior.AllowGet);
            return json;
        }

        public JsonResult CreativeData(KendoGridRequest request, string startdate, string enddate)
        {
            DateTime? start, end;
            var userInfo = GetUserInfo();
            if (!ControllerHelpers.ParseDates(startdate, enddate, userInfo.CultureInfo, out start, out end) || userInfo.TDAccount == null)
                return Json(new { });

            var costPerFields = new[] { "CPM", "CPC", "CPA" };
            if (request.SortObjects.Any(so => costPerFields.Contains(so.Field)))
            {
                List<SortObject> sortObjects = new List<SortObject>();
                foreach (var sortObject in request.SortObjects)
                {
                    // when sorting on eCPA, make 'N/A' rows high
                    if (sortObject.Field == "CPA")
                    {
                        sortObjects.Add(new SortObject("Conv", sortObject.Direction == "asc" ? "desc" : "asc"));
                    }
                    sortObjects.Add(sortObject);
                    if (costPerFields.Contains(sortObject.Field))
                    {   // for rows with identical sort values (CPM/CPC/CPA), sort secondarily by Spend
                        sortObjects.Add(new SortObject("Spend", sortObject.Direction));
                    }
                }
                request.SortObjects = sortObjects;
            }

            var summaries = tdRepo.GetCreativeStatsSummaries(start, end, userInfo.TDAccount);

            var kgrid = new KendoGrid<CreativeStatsSummary>(request, summaries);
            kgrid.aggregates = GetAggregates(summaries, userInfo);

            var json = Json(kgrid, JsonRequestBehavior.AllowGet);
            return json;
        }

        [HttpPost]
        public JsonResult WeeklyData(KendoGridRequest request, int numweeks = 8)
        {
            var userInfo = GetUserInfo();

            var endDate = DateTime.Today.AddDays(-1); // TODO: TD_UseYesterdayAsLatest ?
            var weekStats = tdRepo.GetWeekStats(userInfo.TDAccount, numweeks, endDate);
            var kgrid = new KendoGrid<RangeStat>(request, weekStats);
            if (weekStats.Any())
                kgrid.aggregates = GetAggregates(weekStats, userInfo);

            var json = Json(kgrid);
            return json;
        }

        public FileResult WeeklyExport(int numweeks = 8)
        {
            var userInfo = GetUserInfo();

            var endDate = DateTime.Today.AddDays(-1); // TODO: TD_UseYesterdayAsLatest ?
            var weekStats = tdRepo.GetWeekStats(userInfo.TDAccount, numweeks, endDate);
            var rows = Mapper.Map<IEnumerable<RangeStat>, IEnumerable<RangeStatExportRow>>(weekStats);

            string filename = "Weekly" + ControllerHelpers.DateStamp() + ".csv";
            return File(ControllerHelpers.CsvStream(rows), "application/CSV", filename);
        }

        // --- private methods ---

        private object GetAggregates(IEnumerable<StatsSummary> summaries, UserInfo userInfo)
        {
            int impressions = 0, clicks = 0, conversions = 0;
            decimal spend = 0;
            if (summaries.Any())
            {
                impressions = summaries.Sum(s => s.Impressions);
                clicks = summaries.Sum(s => s.Clicks);
                conversions = summaries.Sum(s => s.Conversions);
                spend = summaries.Sum(s => s.Spend);
            }
            return Aggregates(impressions, clicks, conversions, spend, userInfo.TDAccount.ManagementFeePct);
        }

        private object GetAggregates(IEnumerable<CreativeStatsSummary> summaries, UserInfo userInfo)
        {
            int impressions = 0, clicks = 0, conversions = 0;
            decimal spend = 0;
            if (summaries.Any())
            {
                impressions = summaries.Sum(s => s.Impressions);
                clicks = summaries.Sum(s => s.Clicks);
                conversions = summaries.Sum(s => s.Conversions);
                spend = summaries.Sum(s => s.Spend);
            }
            return Aggregates(impressions, clicks, conversions, spend, userInfo.TDAccount.ManagementFeePct);
        }

        private object GetAggregates(IEnumerable<RangeStat> stats, UserInfo userInfo)
        {
            int impressions = 0, clicks = 0, conversions = 0;
            decimal spend = 0;
            if (stats.Any())
            {
                impressions = stats.Sum(s => s.Impressions);
                clicks = stats.Sum(s => s.Clicks);
                conversions = stats.Sum(s => s.Conversions);
                spend = stats.Sum(s => s.Spend);
            }
            return Aggregates(impressions, clicks, conversions, spend, userInfo.TDAccount.ManagementFeePct);
        }

        private object Aggregates(int impressions, int clicks, int conversions, decimal spend, decimal? managementFeePct)
        {
            decimal fee = 0;
            if (managementFeePct.HasValue)
                fee = spend * managementFeePct.Value / 100;

            var aggregates = new
            {
                Impressions = new { sum = impressions },
                Clicks = new { sum = clicks },
                CTR = new { agg = (impressions == 0) ? 0 : Math.Round((double)clicks / impressions, 4) },
                Conversions = new { sum = conversions },
                ConvRate = new { agg = (clicks == 0) ? 0 : Math.Round((double)conversions / clicks, 4) },
                Spend = new { sum = spend, fee = fee, total = spend + fee },
                CPM = new { agg = (impressions == 0) ? 0 : 1000 * spend / impressions },
                CPC = new { agg = (clicks == 0) ? 0 : spend / clicks },
                CPA = new { agg = (conversions == 0) ? 0 : spend / conversions }
            };
            return aggregates;
        }
    }
}
