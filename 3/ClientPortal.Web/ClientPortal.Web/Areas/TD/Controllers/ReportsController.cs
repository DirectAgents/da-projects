using ClientPortal.Data.Contracts;
using ClientPortal.Data.DTOs.TD;
using ClientPortal.Data.Entities.TD;
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
            if (userInfo.TDAccount == null)
                return Json(new { });

            var tda = userInfo.TDAccount;
            var summaries = tdRepo.GetDailyStatsSummaries(null, null, tda);
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
            var tda = new TradingDeskAccount
            {
                InsertionOrders = new[] { new InsertionOrder { InsertionOrderID = insertionOrderID } }
            };
            var start = new DateTime(2014, 5, 27);
            var end = new DateTime(2014, 6, 25);

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

        public ActionResult Creative()
        {
            var userInfo = GetUserInfo();
            var model = new TDReportModel(userInfo);
            return PartialView(model);
        }

        public JsonResult CreativeData(KendoGridRequest request)
        {
            var userInfo = GetUserInfo();
            if (userInfo.TDAccount == null)
                return Json(new { });

            var summaries = tdRepo.GetCreativeStatsSummaries(null, null, userInfo.TDAccount);

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
            var kgrid = new KendoGrid<CreativeStatsSummary>(request, summaries);
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
