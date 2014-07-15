using ClientPortal.Data.Contracts;
using ClientPortal.Data.DTOs.TD;
using ClientPortal.Data.Entities.TD.DBM;
using ClientPortal.Web.Areas.TD.Models;
using DirectAgents.Mvc.KendoGridBinder;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ClientPortal.Web.Areas.TD.Controllers
{
    public class ReportsController : Controller
    {
        ITDRepository tdRepo;

        int insertionOrderID = 1286935; // Betterment

        public ReportsController(ITDRepository tdRepository)
        {
            tdRepo = tdRepository;
        }

        public ActionResult Sample()
        {
            return PartialView();
        }

        public ActionResult Summary(string metric1, string metric2)
        {
            if (String.IsNullOrWhiteSpace(metric1))
            {
                return PartialView("ChartSetup");
            }

            var metricsList = new List<string> { metric1 };
            if (!String.IsNullOrWhiteSpace(metric2))
                metricsList.Add(metric2);
            var model = new ReportModel
            {
                Metrics = metricsList.ToArray()
            };
            return PartialView(model);
        }

        public JsonResult SampleData(KendoGridRequest request)
        {
            var summaries = tdRepo.GetDailySummaries(null, null, insertionOrderID);
            var kgrid = new KendoGrid<DailySummary>(request, summaries);
            if (summaries.Any())
            {
                kgrid.aggregates = new
                {
                    Impressions = new { sum = summaries.Sum(s => s.Impressions) },
                    Clicks = new { sum = summaries.Sum(s => s.Clicks) },
                    Conversions = new { sum = summaries.Sum(s => s.Conversions) },
                    Revenue = new { sum = summaries.Sum(s => s.Revenue) }
                };
            }
            var json = Json(kgrid, JsonRequestBehavior.AllowGet);
            return json;
        }

        public ActionResult Creative()
        {
            return PartialView();
        }

        public JsonResult CreativeData(KendoGridRequest request)
        {
            var summaries = tdRepo.GetCreativeSummaries(null, null, insertionOrderID);
            var kgrid = new KendoGrid<CreativeSummary>(request, summaries);
            if (summaries.Any())
            {
                kgrid.aggregates = new
                {
                    Impressions = new { sum = summaries.Sum(s => s.Impressions) },
                    Clicks = new { sum = summaries.Sum(s => s.Clicks) },
                    Conversions = new { sum = summaries.Sum(s => s.Conversions) },
                    Revenue = new { sum = summaries.Sum(s => s.Revenue) }
                };
            }
            var json = Json(kgrid, JsonRequestBehavior.AllowGet);
            return json;
        }

        // ---

        protected override void Dispose(bool disposing)
        {
            tdRepo.Dispose();
            base.Dispose(disposing);
        }
    }
}
