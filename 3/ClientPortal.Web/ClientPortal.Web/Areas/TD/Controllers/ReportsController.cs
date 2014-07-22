using ClientPortal.Data.Contracts;
using ClientPortal.Data.DTOs.TD;
using ClientPortal.Data.Entities.TD.DBM;
using ClientPortal.Web.Areas.TD.Models;
using ClientPortal.Web.Controllers;
using DirectAgents.Mvc.KendoGridBinder;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace ClientPortal.Web.Areas.TD.Controllers
{
    [Authorize]
    public class ReportsController : CPController
    {
        public ReportsController(ITDRepository tdRepository, IClientPortalRepository cpRepository)
        {
            tdRepo = tdRepository;
            cpRepo = cpRepository;
        }

        public ActionResult Summary(string metric1, string metric2)
        {
            var userInfo = GetUserInfo();
            var model = new TDReportModel(userInfo, metric1, metric2);

            if (model.MetricsToGraph.Length == 0)
            {
                return PartialView("ChartSetup", model);
            }

            return PartialView(model);
        }

        public JsonResult SummaryData(KendoGridRequest request)
        {
            var userInfo = GetUserInfo();
            if (!userInfo.InsertionOrderID.HasValue)
                return Json(new { });

            var summaries = tdRepo.GetDailyStatsSummaries(null, null, userInfo.InsertionOrderID.Value);
            var kgrid = new KendoGrid<StatsSummary>(request, summaries);
            if (summaries.Any())
            {
                int impressions = summaries.Sum(s => s.Impressions);
                int clicks = summaries.Sum(s => s.Clicks);
                int conversions = summaries.Sum(s => s.Conversions);
                decimal spend = summaries.Sum(s => s.Spend);
                decimal cpm = (impressions == 0) ? 0 : 1000 * spend / impressions;
                decimal cpc = (clicks == 0) ? 0 : spend / clicks;
                decimal cpa = (conversions == 0) ? 0 : spend / conversions;
                kgrid.aggregates = new
                {
                    Impressions = new { sum = impressions, cpm = cpm },
                    Clicks = new { sum = clicks, cpc = cpc, ctr = Math.Round((double)clicks / impressions, 4) },
                    Conversions = new { sum = conversions, cpa = cpa, convrate = Math.Round((double)conversions / clicks, 4) },
                    Spend = new { sum = spend }
                };
            }
            var json = Json(kgrid, JsonRequestBehavior.AllowGet);
            return json;
        }

        public ActionResult Sample()
        {
            return PartialView();
        }

        public JsonResult SampleData(KendoGridRequest request)
        {
            int insertionOrderID = 1286935; // Betterment

            var summaries = tdRepo.GetDailyStatsSummaries(null, null, insertionOrderID);
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

        public ActionResult Creative()
        {
            var userInfo = GetUserInfo();
            var model = new TDReportModel(userInfo);
            return PartialView(model);
        }

        public JsonResult CreativeData(KendoGridRequest request)
        {
            var userInfo = GetUserInfo();
            if (!userInfo.InsertionOrderID.HasValue)
                return Json(new { });

            var summaries = tdRepo.GetCreativeSummaries(null, null, userInfo.InsertionOrderID.Value);
            var kgrid = new KendoGrid<CreativeSummary>(request, summaries);
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

    }
}
