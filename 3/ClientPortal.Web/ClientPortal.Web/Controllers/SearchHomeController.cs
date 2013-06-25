using ClientPortal.Data.Contracts;
using ClientPortal.Data.DTOs;
using ClientPortal.Data.Services;
using DirectAgents.Mvc.KendoGridBinder;
using System.Web.Mvc;
using System.Linq;
using System;

namespace ClientPortal.Web.Controllers
{
    public class SearchHomeController : CPController
    {
        public SearchHomeController(IClientPortalRepository cpRepository)
        {
            this.cpRepo = cpRepository;
        }

        public ActionResult Index()
        {
            return View();
        }

        public PartialViewResult Dashboard()
        {
            return PartialView();
        }

        [HttpPost]
        public JsonResult WeekSumData(KendoGridRequest request, string startdate, string enddate)
        {
            var sRepo = new SearchRepository();
            //var userInfo = GetUserInfo();
            //DateTime? start, end;
            //if (!ControllerHelpers.ParseDates(startdate, enddate, userInfo.CultureInfo, out start, out end))
            //    return Json(new { });

            //if (!start.HasValue) start = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);

            var weekStats = sRepo.GetWeekStats();
            var kgrid = new KendoGrid<SearchStat>(request, weekStats);
            if (weekStats.Any())
                kgrid.aggregates = Aggregates(weekStats);

            var json = Json(kgrid);
            return json;
        }

        private object Aggregates(IQueryable<SearchStat> stats)
        {
            var sumRevenue = stats.Sum(s => s.Revenue);
            var sumCost = stats.Sum(s => s.Cost);
            var sumOrders = stats.Sum(s => s.Orders);
            var aggregates = new
            {
                Revenue = new { sum = sumRevenue },
                Cost = new { sum = sumCost },
                ROAS = new { agg = (int)Math.Round(100 * sumRevenue / sumCost) },
                Orders = new { sum = sumOrders },
                CPO = new { agg = Math.Round( sumCost / sumOrders, 2) },
                Clicks = new { sum = stats.Sum(s => s.Clicks) },
                Impressions = new { sum = stats.Sum(s => s.Impressions) },
            };
            return aggregates;
        }

        [HttpPost]
        public JsonResult MonthSumData(KendoGridRequest request, string startdate, string enddate)
        {
            var sRepo = new SearchRepository();

            var monthStats = sRepo.GetMonthStats();
            var kgrid = new KendoGrid<SearchStat>(request, monthStats);
            if (monthStats.Any())
                kgrid.aggregates = Aggregates(monthStats);

            var json = Json(kgrid);
            return json;
        }

        [HttpPost]
        public JsonResult ChannelPerfData(KendoGridRequest request, string startdate, string enddate)
        {
            var sRepo = new SearchRepository();

            var channelStats = sRepo.GetChannelStats();
            var kgrid = new KendoGrid<SearchStat>(request, channelStats);
            if (channelStats.Any())
                kgrid.aggregates = Aggregates(channelStats);

            var json = Json(kgrid);
            return json;
        }

        [HttpPost]
        public JsonResult CampaignPerfData(KendoGridRequest request, string channel)
        {
            var sRepo = new SearchRepository();

            var stats = sRepo.GetCampaignStats(channel);
            var kgrid = new KendoGrid<SearchStat>(request, stats);
            if (stats.Any())
                kgrid.aggregates = Aggregates(stats);

            var json = Json(kgrid);
            return json;
        }
    }
}
