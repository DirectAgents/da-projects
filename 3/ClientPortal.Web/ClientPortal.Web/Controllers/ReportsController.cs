using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web.Mvc;
using ClientPortal.Data.Contracts;
using ClientPortal.Data.DTOs;
using ClientPortal.Web.Models;
using DirectAgents.Mvc.KendoGridBinder;

namespace ClientPortal.Web.Controllers
{
    [Authorize]
    public class ReportsController : Controller
    {
        private ICakeRepository cakeRepo;
        private IClientPortalRepository cpRepo;

        public ReportsController(ICakeRepository cakeRepository, IClientPortalRepository cpRepository)
        {
            this.cakeRepo = cakeRepository;
            this.cpRepo = cpRepository;
        }

        public PartialViewResult OfferSummaryPartial()
        {
            var userProfile = HomeController.GetUserProfile();

            //var today = DateTime.Now;
            var today = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1).AddDays(-1);
            ViewBag.today = today.ToString("d", userProfile.CultureInfo);
            return PartialView("_OfferSummaryPartial");
        }

        [HttpPost]
        public JsonResult OfferSummaryData(KendoGridRequest request, string startdate, string enddate)
        {
            var userProfile = HomeController.GetUserProfile();
            DateTime? start, end;
            if (!ParseDates(startdate, enddate, userProfile.CultureInfo, out start, out end))
                return Json(new { });

            if (!start.HasValue) start = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);

            var offerInfos = cakeRepo.GetOfferInfos(start, end, userProfile.CakeAdvertiserId);
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
            var userProfile = HomeController.GetUserProfile();

            var now = DateTime.Now;
            ViewBag.firstOfMonth = new DateTime(now.Year, now.Month, 1).ToString("d", userProfile.CultureInfo);
            ViewBag.today = DateTime.Now.ToString("d", userProfile.CultureInfo);
            return PartialView("_DailySummaryPartial");
        }

        [HttpPost]
        public JsonResult DailySummaryData(KendoGridRequest request, string startdate, string enddate, bool cumulative = false, bool projection = false)
        {
            var userProfile = HomeController.GetUserProfile();
            DateTime? start, end;
            if (!ParseDates(startdate, enddate, userProfile.CultureInfo, out start, out end))
                return Json(new { });

            if (!start.HasValue) start = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);

            var dailyInfos = cakeRepo.GetDailyInfos(start, end, userProfile.CakeAdvertiserId);

            if (cumulative)
                dailyInfos = cakeRepo.MakeCumulative(dailyInfos);
            if (projection)
                dailyInfos = cakeRepo.AddProjection(dailyInfos);

            var kgrid = new KendoGrid<DailyInfo>(request, dailyInfos);

            if (dailyInfos.Any())
            {
                if (cumulative)
                {
                    DateTime maxDate = dailyInfos.Max(i => i.Date);
                    int maxClicks = dailyInfos.Max(i => i.Clicks);
                    int maxConversions = dailyInfos.Max(i => i.Conversions);
                    decimal maxRevenue = dailyInfos.Max(i => i.Revenue);
                    kgrid.aggregates = new
                    {
                        Date = new { max = maxDate },
                        Clicks = new { max = maxClicks },
                        Conversions = new { max = maxConversions },
                        Revenue = new { max = maxRevenue }
                    };
                }
                else
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
            }
            var json = Json(kgrid);
            return json;
        }

        [HttpPost]
        public JsonResult MonthlySummaryData(KendoGridRequest request, string startdate, string enddate)
        {
            var userProfile = HomeController.GetUserProfile();
            DateTime? start, end;
            if (!ParseDates(startdate, enddate, userProfile.CultureInfo, out start, out end))
                return Json(new { });

            if (!start.HasValue) start = new DateTime(DateTime.Now.Year, 1, 1);

            var monthlyInfos = cakeRepo.GetMonthlyInfosFromDaily(start, end, userProfile.CakeAdvertiserId.Value, null);
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
            var userProfile = HomeController.GetUserProfile();

            //var today = DateTime.Now;
            var today = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1).AddDays(-1);
            ViewBag.today = today.ToString("d", userProfile.CultureInfo);
            return PartialView("_ConversionReportPartial");
        }

        public PartialViewResult AffiliateReportPartial()
        {
            var userProfile = HomeController.GetUserProfile();

            //var today = DateTime.Now;
            var today = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1).AddDays(-1);
            ViewBag.today = today.ToString("d", userProfile.CultureInfo);
            return PartialView("_AffiliateReportPartial");
        }

        [HttpPost]
        public JsonResult ConversionReportData(KendoGridRequest request, string startdate, string enddate, int? offerid)
        {
            var userProfile = HomeController.GetUserProfile();
            DateTime? start, end;
            if (!ParseDates(startdate, enddate, userProfile.CultureInfo, out start, out end))
                return Json(new { });

            if (!start.HasValue) start = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);

            var conversionInfos = cpRepo.GetConversionInfos(start, end, userProfile.CakeAdvertiserId, offerid);

            var kgrid = new KendoGrid<ConversionInfo>(request, conversionInfos);
            if (conversionInfos.Any())
            {
                kgrid.aggregates = new
                {
                    PriceReceived = new { sum = conversionInfos.Sum(c => c.PriceReceived) },
                    ConvRev = new { sum = conversionInfos.Sum(c => c.ConvRev) }
                };
            }
            var json = Json(kgrid);
            return json;
        }

        [HttpPost]
        public JsonResult AffiliateReportData(KendoGridRequest request, string startdate, string enddate, int? offerid)
        {
            var userProfile = HomeController.GetUserProfile();
            DateTime? start, end;
            if (!ParseDates(startdate, enddate, userProfile.CultureInfo, out start, out end))
                return Json(new { });

            if (!start.HasValue) start = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);

            var conversionInfos = cakeRepo.GetAffiliateInfos(start, end, userProfile.CakeAdvertiserId, offerid);

            var kgrid = new KendoGrid<AffiliateSummary>(request, conversionInfos);
            if (conversionInfos.Any())
            {
                kgrid.aggregates = new
                {
                    PriceReceived = new { sum = conversionInfos.Sum(c => c.PriceReceived) },
                    Count = new { sum = conversionInfos.Sum(c => c.Count) }
                };
            }
            var json = Json(kgrid);
            return json;
        }

        [HttpPost]
        public JsonResult ConversionSummaryData(KendoGridRequest request, string startdate, string enddate, int? offerid)
        {
            var userProfile = HomeController.GetUserProfile();
            DateTime? start, end;
            if (!ParseDates(startdate, enddate, userProfile.CultureInfo, out start, out end))
                return Json(new { });

            if (!start.HasValue) start = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);

            var conversionSummaries = cakeRepo.GetConversionSummaries(start, end, userProfile.CakeAdvertiserId, offerid);

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
            using (var db = new UsersContext())
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
                var group = query.GroupBy(c => c.Region);
                foreach (var grouping in group)
                {
                    results.Add(new object[]
                    { 
                        "US-" + grouping.Key.ToUpper(), 
                        grouping.Sum(c => c.Conversions) 
                    });
                }
                results.Sort(new Comparer());
                results.Insert(0, new[] { "State", "Conversions" });
                json.Data = results;
                return json;
            }
        }

        private class Comparer : IComparer<object[]>
        {
            public int Compare(object[] x, object[] y)
            {
                int a = (int)x[1];
                int b = (int)y[1];
                return (a < b) ? -1 : (a == b) ? 0 : 1;
            }
        }

        // --- helper methods ---

        public static DateTime? ParseDate(string dateString, CultureInfo cultureInfo)
        {
            DateTime? date;
            ParseDate(dateString, cultureInfo, out date);
            return date;
        }

        // returns true iff parsed; a null or whitespace dateString qualifies as parsed, resulting in the out parameter being null
        public static bool ParseDate(string dateString, CultureInfo cultureInfo, out DateTime? date)
        {
            date = null;
            if (String.IsNullOrWhiteSpace(dateString)) return true;

            DateTime parseDate;
            bool parsed = DateTime.TryParse(dateString, cultureInfo, DateTimeStyles.None, out parseDate);
            if (parsed)
                date = parseDate;

            return parsed;
        }

        // returns false if either of the dates couldn't be parsed
        public static bool ParseDates(string startdate, string enddate, CultureInfo cultureInfo, out DateTime? start, out DateTime? end)
        {
            bool startParsed = ParseDate(startdate, cultureInfo, out start);
            bool endParsed = ParseDate(enddate, cultureInfo, out end);
            return (startParsed && endParsed);
        }
    }
}
