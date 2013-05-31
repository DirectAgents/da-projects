using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using AutoMapper;
using ClientPortal.Data.Contracts;
using ClientPortal.Data.DTOs;
using ClientPortal.Web.Models;
using CsvHelper;
using DirectAgents.Mvc.KendoGridBinder;

namespace ClientPortal.Web.Controllers
{
    [Authorize]
    public class ReportsController : CPController
    {
        private ICakeRepository cakeRepo;

        public ReportsController(ICakeRepository cakeRepository, IClientPortalRepository cpRepository)
        {
            this.cakeRepo = cakeRepository;
            this.cpRepo = cpRepository;
        }

        public PartialViewResult OfferSummaryPartial()
        {
            var userInfo = GetUserInfo();

            var now = DateTime.Now;
            var model = new ReportModel()
            {
                StartDate = new DateTime(now.Year, now.Month, 1).ToString("d", userInfo.CultureInfo),
                EndDate = now.ToString("d", userInfo.CultureInfo),
                ShowConVal = userInfo.ShowConversionData,
                ConValName = userInfo.ConversionValueName,
                ConValIsNum = userInfo.ConversionValueIsNumber
            };
            return PartialView("_OfferSummaryPartial", model);
        }

        [HttpPost]
        public JsonResult OfferSummaryData(KendoGridRequest request, string startdate, string enddate)
        {
            var userInfo = GetUserInfo();
            DateTime? start, end;
            if (!ControllerHelpers.ParseDates(startdate, enddate, userInfo.CultureInfo, out start, out end))
                return Json(new { });

            if (!start.HasValue) start = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);

            var offerInfos = cakeRepo.GetOfferInfos(start, end, userInfo.AdvertiserId);
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
            var userInfo = GetUserInfo();

            var now = DateTime.Now;
            var model = new ReportModel()
            {
                StartDate = new DateTime(now.Year, now.Month, 1).ToString("d", userInfo.CultureInfo),
                EndDate = now.ToString("d", userInfo.CultureInfo)
            };
            return PartialView("_DailySummaryPartial", model);
        }

        [HttpPost]
        public JsonResult DailySummaryData(KendoGridRequest request, string startdate, string enddate, bool cumulative = false, bool projection = false)
        {
            var userInfo = GetUserInfo();
            DateTime? start, end;
            if (!ControllerHelpers.ParseDates(startdate, enddate, userInfo.CultureInfo, out start, out end))
                return Json(new { });

            if (!start.HasValue) start = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);

            var dailyInfos = cakeRepo.GetDailyInfos(start, end, userInfo.AdvertiserId);

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
            var userInfo = GetUserInfo();
            DateTime? start, end;
            if (!ControllerHelpers.ParseDates(startdate, enddate, userInfo.CultureInfo, out start, out end))
                return Json(new { });

            if (!start.HasValue) start = new DateTime(DateTime.Now.Year, 1, 1);

            var monthlyInfos = cakeRepo.GetMonthlyInfosFromDaily(start, end, userInfo.AdvertiserId.Value, null);
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
            var userInfo = GetUserInfo();

            var now = DateTime.Now;
            var model = new ReportModel()
            {
                StartDate = now.ToString("d", userInfo.CultureInfo),
                EndDate = now.ToString("d", userInfo.CultureInfo),
                ShowConVal = userInfo.ShowConversionData,
                ConValName = userInfo.ConversionValueName,
                ConValIsNum = userInfo.ConversionValueIsNumber
            };

            return PartialView("_ConversionReportPartial", model);
        }

        [HttpPost]
        public JsonResult ConversionReportData(KendoGridRequest request, string startdate, string enddate, int? offerid)
        {
            var userInfo = GetUserInfo();
            DateTime? start, end;
            if (!ControllerHelpers.ParseDates(startdate, enddate, userInfo.CultureInfo, out start, out end))
                return Json(new { });

            if (!start.HasValue) start = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);

            var conversionInfos = cpRepo.GetConversionInfos(start, end, userInfo.AdvertiserId, offerid);

            var kgrid = new KendoGrid<ConversionInfo>(request, conversionInfos);
            if (conversionInfos.Any())
            {
                kgrid.aggregates = new
                {
                    PriceReceived = new { sum = conversionInfos.Sum(c => c.PriceReceived) },
                    ConVal = new { sum = conversionInfos.Sum(c => c.ConVal) }
                };
            }
            var json = Json(kgrid);
            return json;
        }

        [HttpPost]
        public JsonResult ConversionSummaryData(KendoGridRequest request, string startdate, string enddate, int? offerid)
        {
            var userInfo = GetUserInfo();
            DateTime? start, end;
            if (!ControllerHelpers.ParseDates(startdate, enddate, userInfo.CultureInfo, out start, out end))
                return Json(new { });

            if (!start.HasValue) start = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);

            var conversionSummaries = cakeRepo.GetConversionSummaries(start, end, userInfo.AdvertiserId, offerid);

            var kgrid = new KendoGrid<ConversionSummary>(request, conversionSummaries);
            // todo: aggregates?

            var json = Json(kgrid);
            return json;
        }

        public PartialViewResult CPMSummaryPartial()
        {
            var now = DateTime.Now;
            var firstOfMonth = new DateTime(now.Year, now.Month, 1);
            var model = new ReportModel()
            {
                StartDate = firstOfMonth.AddMonths(-3).ToString("MM/yyyy"),
                EndDate = firstOfMonth.AddMonths(-1).ToString("MM/yyyy")
            };
            return PartialView("_CPMSummaryPartial", model);
        }

        [HttpPost]
        public JsonResult CPMSummaryData(KendoGridRequest request, DateTime? startdate, DateTime? enddate)
        {
            int? advertiserId = GetAdvertiserId();

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

        public PartialViewResult AffiliateReportPartial()
        {
            var userInfo = GetUserInfo();

            var now = DateTime.Now;
            var model = new ReportModel()
            {
                StartDate = now.ToString("d", userInfo.CultureInfo),
                EndDate = now.ToString("d", userInfo.CultureInfo)
            };
            return PartialView("_AffiliateReportPartial", model);
        }

        [HttpPost]
        public JsonResult AffiliateReportData(KendoGridRequest request, string startdate, string enddate, int? offerid)
        {
            var userInfo = GetUserInfo();
            DateTime? start, end;
            if (!ControllerHelpers.ParseDates(startdate, enddate, userInfo.CultureInfo, out start, out end))
                return Json(new { });

            if (!start.HasValue) start = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);

            var affiliateSummaries = cakeRepo.GetAffiliateSummaries(start, end, userInfo.AdvertiserId, offerid);

            var kgrid = new KendoGrid<AffiliateSummary>(request, affiliateSummaries);
            if (affiliateSummaries.Any())
            {
                kgrid.aggregates = new
                {
                    PriceReceived = new { sum = affiliateSummaries.Sum(c => c.PriceReceived) },
                    Count = new { sum = affiliateSummaries.Sum(c => c.Count) }
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
            var fromDate = new DateTime(2013, 5, 1);
            var toDate = new DateTime(2013, 5, 30);
            int advertiserId = GetAdvertiserId() ?? 0;
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

        public JsonResult MobileDevicesData(DateTime? startDate, DateTime? endDate, int take)
        {
            var clicksByDevice = this.cpRepo.GetClicksByDeviceName(
                                                    start: startDate,
                                                    end: endDate ?? startDate,
                                                    advertiserId: GetAdvertiserId(),
                                                    offerId: null);

            var data = clicksByDevice.Where(c => c.DeviceName != "Other");

            decimal totalClicks = data.Sum(c => c.ClickCount);

            var chartData = data.Take(take).Select(c => new
            {
                category = TruncateDeviceNameForChartLegendLabel(c.DeviceName),
                value = c.ClickCount / totalClicks
            });

            var json = new JsonResult
            {
                JsonRequestBehavior = JsonRequestBehavior.AllowGet,
                Data = new
                {
                    data = data,
                    chart = chartData
                }
            };
            return json;
        }

        private string TruncateDeviceNameForChartLegendLabel(string deviceName)
        {
            return deviceName.Length > 20 ? deviceName.Substring(0, 20) + ".." : deviceName;
        }
    }
}
