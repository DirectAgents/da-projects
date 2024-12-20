﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using AutoMapper;
using ClientPortal.Data.Contracts;
using ClientPortal.Data.DTOs.TD;
using ClientPortal.Web.Areas.TD.Models;
using ClientPortal.Web.Controllers;
using ClientPortal.Web.Models;
using DirectAgents.Mvc.KendoGridBinder;
using DirectAgents.Mvc.KendoGridBinder.Containers;

//TODO: switch to KendoGridBinderEx

namespace ClientPortal.Web.Areas.TD.Controllers
{
    [Authorize]
    public class StatsController : CPController
    {
        public StatsController(ITDRepository cptdRepository, IClientPortalRepository cpRepository)
        {
            cptdRepo = cptdRepository;
            cpRepo = cpRepository;
        }

        public JsonResult SummaryData(KendoGridRequest request, string startdate, string enddate)
        {
            DateTime? start, end;
            var userInfo = GetUserInfo();
            if (!ControllerHelpers.ParseDates(startdate, enddate, userInfo.CultureInfo, out start, out end) || userInfo.TDAccount == null)
                return Json(new { });

            var summaries = cptdRepo.GetDailyStatsSummaries(start, end, userInfo.TDAccount);

            var kgrid = new KendoGrid<StatsSummary>(request, summaries);
            var aggregates = GetAggregates(summaries, userInfo);
            aggregates.Info = new
            {
                Start = (start == null) ? "" : start.Value.ToString("d", userInfo.CultureInfo),
                End = (end == null) ? "" : end.Value.ToString("d", userInfo.CultureInfo)
            };
            kgrid.aggregates = aggregates;

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
            List<SortObject> sortObjects = new List<SortObject>();

            // make a new SortObjects list...
            foreach (var sortObject in request.SortObjects)
            {
                // Pre-SortObjects
                if (sortObject.Field == "CPA")
                {   // when sorting on eCPA, make 'N/A' rows high
                    sortObjects.Add(new SortObject("Conv", sortObject.Direction == "asc" ? "desc" : "asc"));
                }

                // The original SortObject (direction reversed, except for CreativeName and CPA)
                if (sortObject.Field == "CreativeName" || sortObject.Field == "CPA")
                    sortObjects.Add(sortObject);
                else
                    sortObjects.Add(new SortObject(sortObject.Field, sortObject.Direction == "asc" ? "desc" : "asc"));

                // Post-SortObjects
                if (costPerFields.Contains(sortObject.Field))
                {   // for rows with identical sort values (CPM/CPC/CPA), sort secondarily by Spend
                    sortObjects.Add(new SortObject("Spend", sortObject.Direction));
                }
            }
            request.SortObjects = sortObjects;

            var summaries = cptdRepo.GetCreativeStatsSummaries(start, end, userInfo.TDAccount);

            var kgrid = new KendoGrid<CreativeStatsSummary>(request, summaries);
            kgrid.aggregates = GetAggregates(summaries, userInfo);

            var json = Json(kgrid, JsonRequestBehavior.AllowGet);
            return json;
        }

        [HttpPost]
        public JsonResult WeeklyData(KendoGridRequest request, int numweeks = 8)
        {
            var userInfo = GetUserInfo();

            var endDate = userInfo.TD_UseYesterdayAsLatest ? DateTime.Today.AddDays(-1) : DateTime.Today;
            var weekStats = cptdRepo.GetWeekStats(userInfo.TDAccount, numweeks, endDate);
            var kgrid = new KendoGrid<RangeStat>(request, weekStats);
            if (weekStats.Any())
                kgrid.aggregates = GetAggregates(weekStats, userInfo);

            var json = Json(kgrid);
            return json;
        }
        public FileResult WeeklyExport(int numweeks = 8)
        {
            var userInfo = GetUserInfo();

            var endDate = userInfo.TD_UseYesterdayAsLatest ? DateTime.Today.AddDays(-1) : DateTime.Today;
            var weekStats = cptdRepo.GetWeekStats(userInfo.TDAccount, numweeks, endDate);
            var rows = Mapper.Map<IEnumerable<RangeStat>, IEnumerable<RangeStatExportRow>>(weekStats);

            string filename = "Weekly" + ControllerHelpers.DateStamp() + ".csv";
            return File(ControllerHelpers.CsvStream(rows), "application/CSV", filename);
        }

        [HttpPost]
        public JsonResult MonthlyData(KendoGridRequest request, int nummonths = 6)
        {
            var userInfo = GetUserInfo();

            var endDate = userInfo.TD_UseYesterdayAsLatest ? DateTime.Today.AddDays(-1) : DateTime.Today;
            var monthStats = cptdRepo.GetMonthStats(userInfo.TDAccount, nummonths, endDate);
            var kgrid = new KendoGrid<RangeStat>(request, monthStats);
            if (monthStats.Any())
                kgrid.aggregates = GetAggregates(monthStats, userInfo);

            var json = Json(kgrid);
            return json;
        }
        public FileResult MonthlyExport(int nummonths = 8)
        {
            var userInfo = GetUserInfo();

            var endDate = userInfo.TD_UseYesterdayAsLatest ? DateTime.Today.AddDays(-1) : DateTime.Today;
            var monthStats = cptdRepo.GetMonthStats(userInfo.TDAccount, nummonths, endDate);
            var rows = Mapper.Map<IEnumerable<RangeStat>, IEnumerable<RangeStatExportRow>>(monthStats);

            string filename = "Monthly" + ControllerHelpers.DateStamp() + ".csv";
            return File(ControllerHelpers.CsvStream(rows), "application/CSV", filename);
        }

        // --- private methods ---

        private Aggregates GetAggregates(IEnumerable<StatsSummary> summaries, UserInfo userInfo)
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

        private Aggregates GetAggregates(IEnumerable<CreativeStatsSummary> summaries, UserInfo userInfo)
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

        private Aggregates GetAggregates(IEnumerable<RangeStat> stats, UserInfo userInfo)
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

        private Aggregates Aggregates(int impressions, int clicks, int conversions, decimal spend, decimal? managementFeePct)
        {
            //decimal fee = 0;
            //if (managementFeePct.HasValue)
            //    fee = spend * managementFeePct.Value / 100;

            var aggregates = new Aggregates
            {
                Impressions = new { sum = impressions },
                Clicks = new { sum = clicks },
                CTR = new { agg = (impressions == 0) ? 0 : Math.Round((double)clicks / impressions, 5) },
                Conversions = new { sum = conversions },
                ConvRate = new { agg = (clicks == 0) ? 0 : Math.Round((double)conversions / clicks, 4) },
                //Spend = new { sum = spend, fee = fee, total = spend + fee },
                Spend = new { sum = spend },
                CPM = new { agg = (impressions == 0) ? 0 : 1000 * spend / impressions },
                CPC = new { agg = (clicks == 0) ? 0 : spend / clicks },
                CPA = new { agg = (conversions == 0) ? 0 : spend / conversions }
            };
            return aggregates;
        }
    }

    public class Aggregates
    {
        public object Impressions;
        public object Clicks;
        public object CTR;
        public object Conversions;
        public object ConvRate;
        public object Spend;
        public object CPM;
        public object CPC;
        public object CPA;

        public object Info;
    }
}
