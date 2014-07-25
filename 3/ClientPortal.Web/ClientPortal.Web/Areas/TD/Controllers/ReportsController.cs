using ClientPortal.Data.Contracts;
using ClientPortal.Data.DTOs.TD;
using ClientPortal.Data.Entities.TD.DBM;
using ClientPortal.Web.Areas.TD.Models;
using ClientPortal.Web.Controllers;
using DirectAgents.Mvc.KendoGridBinder;
using DirectAgents.Mvc.KendoGridBinder.Containers;
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

                kgrid.aggregates = Aggregates(impressions, clicks, conversions, spend);
            }
            var json = Json(kgrid, JsonRequestBehavior.AllowGet);
            return json;
        }

        private object Aggregates(int impressions, int clicks, int conversions, decimal spend)
        {
            var aggregates = new
            {
                Impressions = new { sum = impressions },
                Clicks = new { sum = clicks },
                CTR = new { agg = Math.Round((double)clicks / impressions, 4) },
                Conversions = new { sum = conversions },
                ConvRate = new { agg = Math.Round((double)conversions / clicks, 4) },
                Spend = new { sum = spend },
                CPM = new { agg = (impressions == 0) ? 0 : 1000 * spend / impressions },
                CPC = new { agg = (clicks == 0) ? 0 : spend / clicks },
                CPA = new { agg = (conversions == 0) ? 0 : spend / conversions }
            };
            return aggregates;
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

            if (request.SortObjects.Any(so => so.Field == "CPA"))
            {
                // When sorting on eCPA, make 'N/A' rows high and within those, sort by Spend.
                List<SortObject> sortObjects = new List<SortObject>();
                foreach (var sortObject in request.SortObjects)
                {
                    if (sortObject.Field == "CPA")
                    {
                        sortObjects.Add(new SortObject("Conv", sortObject.Direction == "asc" ? "desc" : "asc"));
                        sortObjects.Add(new SortObject("Spend", sortObject.Direction));
                    }
                    sortObjects.Add(sortObject);
                }
                request.SortObjects = sortObjects;
            }
            var kgrid = new KendoGrid<CreativeSummary>(request, summaries, true);
            if (summaries.Any())
            {
                int impressions = summaries.Sum(s => s.Impressions);
                int clicks = summaries.Sum(s => s.Clicks);
                int conversions = summaries.Sum(s => s.Conversions);
                decimal spend = summaries.Sum(s => s.Spend);

                kgrid.aggregates = Aggregates(impressions, clicks, conversions, spend);
            }
            var json = Json(kgrid, JsonRequestBehavior.AllowGet);
            return json;
        }

    }
}
