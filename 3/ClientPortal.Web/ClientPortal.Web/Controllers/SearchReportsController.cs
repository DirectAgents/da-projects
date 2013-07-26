using AutoMapper;
using ClientPortal.Data.Contracts;
using ClientPortal.Data.DTOs;
using ClientPortal.Data.Services;
using ClientPortal.Web.Models;
using DirectAgents.Mvc.KendoGridBinder;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ClientPortal.Web.Controllers
{
    [Authorize]
    public class SearchReportsController : CPController
    {
        public SearchReportsController(IClientPortalRepository cpRepository)
        {
            this.cpRepo = cpRepository;
        }

        [HttpPost]
        public JsonResult WeekSumData(KendoGridRequest request, int numweeks = 8)
        {
            var userInfo = GetUserInfo();

            var weekStats = cpRepo.GetWeekStats(userInfo.AdvertiserId, numweeks);
            var kgrid = new KendoGrid<SearchStat>(request, weekStats);
            if (weekStats.Any())
                kgrid.aggregates = Aggregates(weekStats);

            var json = Json(kgrid);
            return json;
        }

        public FileResult WeekSumExport(int numweeks = 8)
        {
            var userInfo = GetUserInfo();

            var weekStats = cpRepo.GetWeekStats(userInfo.AdvertiserId, numweeks);
            var rows = Mapper.Map<IEnumerable<SearchStat>, IEnumerable<SearchStatExportRow>>(weekStats);

            string filename = "WeeklySummary" + ControllerHelpers.DateStamp() + ".csv";
            return File(ControllerHelpers.CsvStream(rows), "application/CSV", filename);
        }

        [HttpPost]
        public JsonResult MonthSumData(KendoGridRequest request, int nummonths = 6)
        {
            var userInfo = GetUserInfo();

            var monthStats = cpRepo.GetMonthStats(userInfo.AdvertiserId, nummonths)
                .ToList()
                .OrderBy(s => s.StartDate)
                .AsQueryable();
            var kgrid = new KendoGrid<SearchStat>(request, monthStats);
            if (monthStats.Any())
                kgrid.aggregates = Aggregates(monthStats);

            var json = Json(kgrid);
            return json;
        }

        public FileResult MonthSumExport(int nummonths = 8)
        {
            var userInfo = GetUserInfo();

            var monthStats = cpRepo.GetMonthStats(userInfo.AdvertiserId, nummonths);
            var rows = Mapper.Map<IEnumerable<SearchStat>, IEnumerable<SearchStatExportRow>>(monthStats);

            string filename = "MonthlySummary" + ControllerHelpers.DateStamp() + ".csv";
            return File(ControllerHelpers.CsvStream(rows), "application/CSV", filename);
        }

        [HttpPost]
        public JsonResult ChannelPerfData(KendoGridRequest request, string startdate, string enddate)
        {
            var userInfo = GetUserInfo();

            var channelStats = cpRepo.GetChannelStats(userInfo.AdvertiserId);
            var kgrid = new KendoGrid<SearchStat>(request, channelStats);
            if (channelStats.Any())
                kgrid.aggregates = Aggregates(channelStats);

            var json = Json(kgrid);
            return json;
        }

        public FileResult ChannelPerfExport()
        {
            var userInfo = GetUserInfo();

            var stats = cpRepo.GetChannelStats(userInfo.AdvertiserId);
            var rows = Mapper.Map<IEnumerable<SearchStat>, IEnumerable<SearchStatExportRow>>(stats);

            string filename = "ChannelPerformance" + ControllerHelpers.DateStamp() + ".csv";
            return File(ControllerHelpers.CsvStream(rows), "application/CSV", filename);
        }

        [HttpPost]
        public JsonResult CampaignPerfData(KendoGridRequest request, string startdate, string enddate, string channel, bool breakdown = false)
        {
            var userInfo = GetUserInfo();
            var cultureInfo = userInfo.CultureInfo;
            DateTime? start, end;
            if (!ControllerHelpers.ParseDates(startdate, enddate, cultureInfo, out start, out end))
                return Json(new { });

            var stats = cpRepo.GetCampaignStats(userInfo.AdvertiserId, channel, start, end, breakdown)
                .ToList()
                .AsQueryable();

            var kgrid = new KendoGrid<SearchStat>(request, stats);
            if (stats.Any())
                kgrid.aggregates = Aggregates(stats);

            var json = Json(kgrid);
            return json;
        }

        public FileResult CampaignPerfExport(string startdate, string enddate, string channel, bool breakdown = false)
        {
            var userInfo = GetUserInfo();
            var cultureInfo = userInfo.CultureInfo;
            DateTime? start, end;
            if (!ControllerHelpers.ParseDates(startdate, enddate, cultureInfo, out start, out end))
                return File("Error parsing dates: " + startdate + " and " + enddate, "text/plain");

            var stats = cpRepo.GetCampaignStats(userInfo.AdvertiserId, channel, start, end, breakdown)
                .OrderByDescending(s => s.Channel).ThenBy(s => s.Title);
            var rows = Mapper.Map<IEnumerable<SearchStat>, IEnumerable<SearchStatExportRow>>(stats);

            string filename = "CampaignPerformance" + ControllerHelpers.DateStamp() + ".csv";
            return File(ControllerHelpers.CsvStream(rows), "application/CSV", filename);
        }

        [HttpPost]
        public JsonResult AdgroupPerfData(KendoGridRequest request, string startdate, string enddate)
        {
            var stats = cpRepo.GetAdgroupStats();
            var kgrid = new KendoGrid<SearchStat>(request, stats);
            if (stats.Any())
                kgrid.aggregates = Aggregates(stats);

            var json = Json(kgrid);
            return json;
        }

        // --- private methods ---

        private object Aggregates(IQueryable<SearchStat> stats)
        {
            var sumRevenue = stats.Sum(s => s.Revenue);
            var sumCost = stats.Sum(s => s.Cost);
            var sumOrders = stats.Sum(s => s.Orders);
            var sumClicks = stats.Sum(s => s.Clicks);
            var sumImpressions = stats.Sum(s => s.Impressions);

            // Determine totalDays (The stats may or may not be for the same time period.)
            var periods = stats.Select(s => new { StartDate = s.StartDate, EndDate = s.EndDate });
            var distinctPeriods = periods.Distinct();
            var totalDays = distinctPeriods.Sum(p => (p.EndDate - p.StartDate).Days + 1);
            var aggregates = new
            {
                Revenue = new { sum = sumRevenue },
                Cost = new { sum = sumCost },
                ROAS = new { agg = sumCost == 0 ? 0 : (int)Math.Round(100 * sumRevenue / sumCost) },
                Margin = new { agg = sumRevenue - sumCost },
                Orders = new { sum = sumOrders },
                CPO = new { agg = sumOrders == 0 ? 0 : Math.Round(sumCost / sumOrders, 2) },
                OrderRate = new { agg = sumClicks == 0 ? 0 : Math.Round((decimal)100 * sumOrders / sumClicks, 2) },
                RevenuePerOrder = new { agg = sumOrders == 0 ? 0 : Math.Round(sumRevenue / sumOrders, 2) },
                CPC = new { agg = sumClicks == 0 ? 0 : Math.Round(sumCost / sumClicks, 2) },
                Clicks = new { sum = sumClicks },
                Impressions = new { sum = sumImpressions },
                CTR = new { agg = sumImpressions == 0 ? 0 : Math.Round((decimal)100 * sumClicks / sumImpressions, 2) },
                OrdersPerDay = new { agg = totalDays == 0 ? 0 : Math.Round((decimal)sumOrders / totalDays, 2) },
            };
            return aggregates;
        }
    }
}
