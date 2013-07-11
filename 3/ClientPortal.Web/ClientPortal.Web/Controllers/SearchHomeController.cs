using ClientPortal.Data.Contracts;
using ClientPortal.Data.DTOs;
using ClientPortal.Data.Services;
using DirectAgents.Mvc.KendoGridBinder;
using System.Web.Mvc;
using System.Linq;
using System;
using ClientPortal.Web.Models;
using System.Globalization;

namespace ClientPortal.Web.Controllers
{
    [Authorize]
    public class SearchHomeController : CPController
    {
        public SearchHomeController(IClientPortalRepository cpRepository)
        {
            this.cpRepo = cpRepository;
        }

        public ActionResult Index()
        {
            var userInfo = GetUserInfo();
            var result = CheckLogout(userInfo);
            if (result != null) return result;

            if (!userInfo.HasSearch)
                return RedirectToAction("Index", "Home");

            var model = new IndexModel(userInfo);
            return View(model);
        }

        public PartialViewResult Dashboard()
        {
            return PartialView();
        }

        public PartialViewResult ChannelPerf()
        {
            return PartialView();
        }

        public PartialViewResult CampaignPerf()
        {
            var userInfo = GetUserInfo();

            var start = new DateTime(2013, 5, 27);
            var end = new DateTime(2013, 6, 2);
            var model = new SearchReportModel()
            {
                StartDate = start.ToString("d", userInfo.CultureInfo),
                EndDate = end.ToString("d", userInfo.CultureInfo)
            };
            return PartialView(model);
        }

        public PartialViewResult AdgroupPerf()
        {
            var userInfo = GetUserInfo();

            var start = new DateTime(2013, 5, 27);
            var end = new DateTime(2013, 6, 2);
            var model = new SearchReportModel()
            {
                StartDate = start.ToString("d", userInfo.CultureInfo),
                EndDate = end.ToString("d", userInfo.CultureInfo)
            };
            return PartialView(model);
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
        public JsonResult ChannelPerfData(KendoGridRequest request, string startdate, string enddate)
        {
            var channelStats = cpRepo.GetChannelStats();
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
    }
}
