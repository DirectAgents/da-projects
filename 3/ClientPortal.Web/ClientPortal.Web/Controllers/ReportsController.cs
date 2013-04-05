using ClientPortal.Data.Contracts;
using ClientPortal.Data.DTOs;
using DirectAgents.Mvc.KendoGridBinder;
using System;
using System.Linq;
using System.Web.Mvc;
using CakeMarketing;
using System.Data.SqlClient;
using System.Collections.Generic;

namespace ClientPortal.Web.Controllers
{
    [Authorize]
    public class ReportsController : Controller
    {
        private ICakeRepository cakeRepo;

        public ReportsController(ICakeRepository cakeRepository)
        {
            this.cakeRepo = cakeRepository;
        }

        public PartialViewResult OfferSummaryPartial()
        {
            var test = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1).AddDays(-1);
            ViewBag.today = test.ToShortDateString();
            //            ViewBag.today = DateTime.Now.ToShortDateString();
            return PartialView("_OfferSummaryPartial");
        }

        [HttpPost]
        public JsonResult OfferSummaryData(KendoGridRequest request, DateTime? startdate, DateTime? enddate)
        {
            int? advertiserId = HomeController.GetAdvertiserId();

            var offerInfos = cakeRepo.GetOfferInfos(startdate, enddate, advertiserId);
            var kgrid = new KendoGrid<OfferInfo>(request, offerInfos);
            if (offerInfos.Any())
            {
                kgrid.aggregates = new
                {
                    Clicks = new { sum = offerInfos.Sum(i => i.Clicks) },
                    Conversions = new { sum = offerInfos.Sum(i => i.Conversions) },
                    Revenue = new { sum = offerInfos.Sum(i => i.Revenue) }
                };
            }
            var json = Json(kgrid);
            return json;
        }

        public PartialViewResult DailySummaryPartial()
        {
            var now = DateTime.Now;
            ViewBag.firstOfMonth = new DateTime(now.Year, now.Month, 1).ToShortDateString(); ;
            ViewBag.today = DateTime.Now.ToShortDateString();
            return PartialView("_DailySummaryPartial");
        }

        [HttpPost]
        public JsonResult DailySummaryData(KendoGridRequest request, DateTime? startdate, DateTime? enddate)
        {
            var now = DateTime.Now;
            if (!startdate.HasValue) startdate = new DateTime(now.Year, now.Month, 1);
            if (!enddate.HasValue) enddate = now;

            int? advertiserId = HomeController.GetAdvertiserId();
            var dailyInfos = cakeRepo.GetDailyInfos(startdate, enddate, advertiserId);
            var kgrid = new KendoGrid<DailyInfo>(request, dailyInfos);

            if (dailyInfos.Any())
            {
                int totalImpressions = dailyInfos.Sum(i => i.Impressions);
                int totalClicks = dailyInfos.Sum(i => i.Clicks);
                int totalConversions = dailyInfos.Sum(i => i.Conversions);
                float totalConversionPct = (totalClicks == 0) ? 0 : (float)Math.Round((double)totalConversions / totalClicks, 3);
                decimal totalRevenue = dailyInfos.Sum(i => i.Revenue);
                decimal totalEPC = (totalClicks == 0) ? 0 : Math.Round(totalRevenue / totalClicks, 2);
                kgrid.aggregates = new
                {
                    Impressions = new { sum = totalImpressions },
                    Clicks = new { sum = totalClicks },
                    Conversions = new { sum = totalConversions },
                    ConversionPct = new { agg = totalConversionPct },
                    Revenue = new { sum = totalRevenue },
                    EPC = new { agg = totalEPC }
                };
            }
            var json = Json(kgrid);
            return json;
        }

        [HttpPost]
        public JsonResult MonthlySummaryData(KendoGridRequest request, DateTime? startdate, DateTime? enddate)
        {
            var now = DateTime.Now;
            if (!startdate.HasValue) startdate = new DateTime(now.Year, now.Month, 1);
            if (!enddate.HasValue) enddate = now;

            int? advertiserId = HomeController.GetAdvertiserId();
            if (advertiserId == null) return null;

            var monthlyInfos = cakeRepo.GetMonthlyInfosFromDaily(startdate, enddate, advertiserId.Value, null);
            var kgrid = new KendoGrid<MonthlyInfo>(request, monthlyInfos);

            if (monthlyInfos.Any())
            {
                decimal totalRevenue = monthlyInfos.Sum(i => i.Revenue);
                kgrid.aggregates = new
                {
                    Revenue = new { sum = totalRevenue }
                };
            }
            var json = Json(kgrid);
            return json;
        }

        public PartialViewResult ConversionReportPartial()
        {
            var test = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1).AddDays(-1);
            ViewBag.today = test.ToShortDateString();
            //ViewBag.today = DateTime.Now.ToShortDateString();
            return PartialView("_ConversionReportPartial");
        }

        [HttpPost]
        public JsonResult ConversionReportData(KendoGridRequest request, DateTime? startdate, DateTime? enddate, int? offerid)
        {
            int? advertiserId = HomeController.GetAdvertiserId();
            var conversionInfos = cakeRepo.GetConversionInfos(startdate, enddate, advertiserId, offerid);

            var kgrid = new KendoGrid<ConversionInfo>(request, conversionInfos);
            if (conversionInfos.Any())
            {
                kgrid.aggregates = new
                {
                    PriceReceived = new { sum = conversionInfos.Sum(c => c.PriceReceived) }
                };
            }
            var json = Json(kgrid);
            return json;
        }

        [HttpPost]
        public JsonResult ConversionSummaryData(KendoGridRequest request, DateTime? startdate, DateTime? enddate, int? offerid)
        {
            int? advertiserId = HomeController.GetAdvertiserId();
            var conversionSummaries = cakeRepo.GetConversionSummaries(startdate, enddate, advertiserId, offerid);

            var kgrid = new KendoGrid<ConversionSummary>(request, conversionSummaries);
            // todo: aggregates?

            var json = Json(kgrid);
            return json;
        }

        public PartialViewResult CPMSummaryPartial()
        {
            var now = DateTime.Now;
            var firstOfMonth = new DateTime(now.Year, now.Month, 1);
            ViewBag.start = firstOfMonth.AddMonths(-3).ToString("MM/yyyy");
            ViewBag.end = firstOfMonth.AddMonths(-1).ToString("MM/yyyy");
            return PartialView("_CPMSummaryPartial");
        }

        [HttpPost]
        public JsonResult CPMSummaryData(KendoGridRequest request, DateTime? startdate, DateTime? enddate)
        {
            int? advertiserId = HomeController.GetAdvertiserId();

            var monthlyInfos = cakeRepo
                .GetMonthlyInfos("CPM", startdate, enddate, advertiserId)
                .Where(i => i.CampaignStatusId == CampaignStatus.Verified); // TODO: filter by AccountingStatus (or combine into one row)

            var kgrid = new KendoGrid<MonthlyInfo>(request, monthlyInfos);
            if (monthlyInfos.Any())
            {
                kgrid.aggregates = new
                {
                    Revenue = new { sum = monthlyInfos.Sum(i => i.Revenue) }
                };
            }
            var json = Json(kgrid);
            return json;
        }

        public PartialViewResult HeatMap()
        {
            return PartialView("_HeatMap");
        }

        public JsonResult HeatMapData()
        {
            var fromDate = new DateTime(2013, 4, 1);
            var toDate = new DateTime(2013, 4, 5);
            int advertiserId = HomeController.GetAdvertiserId() ?? 0;
            var json = new JsonResult { JsonRequestBehavior = JsonRequestBehavior.AllowGet };
            using (var db = new CakeExtracterContext())
            {
                var clicks = db.Clicks.Where(c => c.advertiser.advertiser_id == advertiserId);
                var conversions = db.Conversions.Where(
                                        c => c.advertiser.advertiser_id == advertiserId &&
                                             c.conversion_date >= fromDate &&
                                             c.conversion_date < toDate);
                var query = from a in clicks
                            from b in conversions
                            where a.click_id == b.click_id
                            select new
                            {
                                Region = a.region.region_code,
                                Conversions = 1
                            };
                var results = new List<object[]>();
                results.Insert(0, new[] { "State", "Conversions" });
                var group = query.GroupBy(c => c.Region);
                foreach (var grouping in group)
                {
                    results.Add(new object[]
                    { 
                        "US-" + grouping.Key.ToUpper(), 
                        grouping.Sum(c => c.Conversions) 
                    });
                }
                json.Data = results;
                return json;
            }
        }
    }
}
