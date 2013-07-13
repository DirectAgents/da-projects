using ClientPortal.Data.Contracts;
using ClientPortal.Data.DTOs;
using ClientPortal.Data.Services;
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
        public JsonResult WeekSumData(KendoGridRequest request, int numWeeks = 8)
        {
            var userInfo = GetUserInfo();

            var weekStats = cpRepo.GetWeekStats(userInfo.AdvertiserId, numWeeks);
            var kgrid = new KendoGrid<SearchStat>(request, weekStats);
            if (weekStats.Any())
                kgrid.aggregates = Aggregates(weekStats);

            var json = Json(kgrid);
            return json;
        }

        [HttpPost]
        public JsonResult MonthSumData(KendoGridRequest request, int numMonths = 6)
        {
            var userInfo = GetUserInfo();

            var monthStats = cpRepo.GetMonthStats(userInfo.AdvertiserId, numMonths);
            var kgrid = new KendoGrid<SearchStat>(request, monthStats);
            if (monthStats.Any())
                kgrid.aggregates = Aggregates(monthStats);

            var json = Json(kgrid);
            return json;
        }

        [HttpPost]
        public JsonResult CampaignPerfData(KendoGridRequest request, string startdate, string enddate, string channel)
        {
            var userInfo = GetUserInfo();
            var cultureInfo = userInfo.CultureInfo;
            DateTime? start, end;
            if (!ControllerHelpers.ParseDates(startdate, enddate, cultureInfo, out start, out end))
                return Json(new { });

            var stats = cpRepo.GetCampaignStats(userInfo.AdvertiserId, channel, start, end);

            var kgrid = new KendoGrid<SearchStat>(request, stats);
            if (stats.Any())
                kgrid.aggregates = Aggregates(stats);

            var json = Json(kgrid);
            return json;
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
            var aggregates = new
            {
                Revenue = new { sum = sumRevenue },
                Cost = new { sum = sumCost },
                ROAS = new { agg = sumCost == 0 ? 0 : (int)Math.Round(100 * sumRevenue / sumCost) },
                Orders = new { sum = sumOrders },
                CPO = new { agg = sumOrders == 0 ? 0 : Math.Round(sumCost / sumOrders, 2) },
                Clicks = new { sum = stats.Sum(s => s.Clicks) },
                Impressions = new { sum = stats.Sum(s => s.Impressions) },
            };
            return aggregates;
        }
    }
}
