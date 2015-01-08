using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using ClientPortal.Data.Contracts;
using ClientPortal.Data.DTOs;
using ClientPortal.Web.Models;
using DirectAgents.Mvc.KendoGridBinder;
using StackExchange.Profiling;

namespace ClientPortal.Web.Controllers
{
    [Authorize]
    public class ReportsController : CPController
    {
        public ReportsController(IClientPortalRepository cpRepository)
        {
            this.cpRepo = cpRepository;
        }

        public PartialViewResult OfferSummaryPartial()
        {
            var model = new ReportModel(GetUserInfo());
            return PartialView("_OfferSummaryPartial", model);
        }

        [HttpPost]
        public JsonResult OfferSummaryData(KendoGridRequest request, string startdate, string enddate)
        {
            var userInfo = GetUserInfo();
            DateTime? start, end;
            if (!ControllerHelpers.ParseDates(startdate, enddate, userInfo.CultureInfo, out start, out end))
                return Json(new { });

            if (!start.HasValue) start = userInfo.Dates.FirstOfMonth;

            var offerInfos = cpRepo.GetOfferInfos(start, end, userInfo.AdvertiserId);
            var kgrid = new KendoGrid<OfferInfo>(request, offerInfos);
            if (offerInfos.Any())
            {
                var totalClicks = offerInfos.Sum(i => i.Clicks);
                var totalConversions = offerInfos.Sum(i => i.Conversions);
                float totalConversionRate = (totalClicks == 0) ? 0 : (float)Math.Round((double)totalConversions / totalClicks, 3);
                kgrid.aggregates = new
                {
                    Clicks = new { sum = totalClicks },
                    Conversions = new { sum = totalConversions },
                    ConvRate = new { agg = totalConversionRate },
                    Revenue = new { sum = offerInfos.Sum(i => i.Revenue) }
                };
            }
            else
            {
                kgrid.aggregates = new
                {
                    Clicks = new { sum = 0 },
                    Conversions = new { sum = 0 },
                    ConvRate = new { agg = 0 },
                    Revenue = new { sum = 0 }
                };
            }
            var json = Json(kgrid);
            return json;
        }

        [HttpPost]
        public JsonResult Offers(string startdate, string enddate)
        {
            var userInfo = GetUserInfo();
            DateTime? start, end;
            ControllerHelpers.ParseDates(startdate, enddate, userInfo.CultureInfo, out start, out end);

            var offers = cpRepo.Offers(userInfo.AdvertiserId, start, end);
            var offerVMs = offers.ToList().Select(o => new IdNameVM { Id = o.OfferId, Name = o.DisplayName })
                .OrderBy(o => o.Name);

            var json = Json(offerVMs);
            return json;
        }

        public PartialViewResult DailySummaryPartial()
        {
            var model = new ReportModel(GetUserInfo());
            return PartialView("_DailySummaryPartial", model);
        }

        [HttpPost]
        public JsonResult DailySummaryData(KendoGridRequest request, string startdate, string enddate, int? offerid, bool cumulative = false, bool projection = false)
        {
            var userInfo = GetUserInfo();
            DateTime? start, end;
            if (!ControllerHelpers.ParseDates(startdate, enddate, userInfo.CultureInfo, out start, out end))
                return Json(new { });

            if (!start.HasValue) start = userInfo.Dates.FirstOfMonth;

            var dailyInfos = cpRepo.GetDailyInfos(start, end, userInfo.AdvertiserId, offerid);
            bool anyInfos = dailyInfos.Any();

            DailyInfo totals = new DailyInfo()
            {   // Computed before making cumulative or projecting:
                Impressions = anyInfos ? dailyInfos.Sum(i => i.Impressions) : 0,
                Clicks = anyInfos ? dailyInfos.Sum(i => i.Clicks) : 0,
                Conversions = anyInfos ? dailyInfos.Sum(i => i.Conversions) : 0,
                Revenue = anyInfos ? dailyInfos.Sum(i => i.Revenue) : 0
            };
            DailyInfo projectionInfo = null;

            if (cumulative)
                dailyInfos = DailyInfo.MakeCumulative(dailyInfos);
            if (projection)
                dailyInfos = DailyInfo.AddProjection(dailyInfos, out projectionInfo);

            var kgrid = new KendoGrid<DailyInfo>(request, dailyInfos);

            if (anyInfos)
            {
                if (cumulative)
                {
                    DateTime maxDate = dailyInfos.Max(i => i.Date);
                    int maxClicks = dailyInfos.Max(i => i.Clicks);
                    int maxConversions = dailyInfos.Max(i => i.Conversions);
                    decimal maxRevenue = dailyInfos.Max(i => i.Revenue);
                    kgrid.aggregates = new
                    {
                        Date = new { max = maxDate, proj = (projectionInfo == null ? maxDate : projectionInfo.Date) },
                        Clicks = new { max = maxClicks, sum = totals.Clicks },
                        Conversions = new { max = maxConversions, sum = totals.Conversions, proj = (projectionInfo == null ? -1 : projectionInfo.Conversions) },
                        Revenue = new { max = maxRevenue, sum = totals.Revenue }
                    };
                }
                else
                {
                    float totalConversionPct = (totals.Clicks == 0) ? 0 : (float)Math.Round((double)totals.Conversions / totals.Clicks, 3);
                    decimal totalEPC = (totals.Clicks == 0) ? 0 : Math.Round(totals.Revenue / totals.Clicks, 2);
                    kgrid.aggregates = new
                    {
                        Impressions = new { sum = totals.Impressions },
                        Clicks = new { sum = totals.Clicks },
                        Conversions = new { sum = totals.Conversions },
                        ConversionPct = new { agg = totalConversionPct },
                        Revenue = new { sum = totals.Revenue },
                        EPC = new { agg = totalEPC }
                    };
                }
            }
            var json = Json(kgrid);
            return json;
        }

        public PartialViewResult ConversionReportPartial()
        {
            var model = new ReportModel(GetUserInfo());
            return PartialView("_ConversionReportPartial", model);
        }

        [HttpPost]
        public JsonResult ConversionReportData(KendoGridRequest request, string startdate, string enddate, int? offerid)
        {
            var userInfo = GetUserInfo();
            DateTime? start, end;
            if (!ControllerHelpers.ParseDates(startdate, enddate, userInfo.CultureInfo, out start, out end))
                return Json(new { });

            if (!start.HasValue) start = userInfo.Dates.FirstOfMonth;

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

            if (!start.HasValue) start = userInfo.Dates.FirstOfMonth;

            var conversionSummaries = cpRepo.GetConversionSummaries(start, end, userInfo.AdvertiserId, offerid);

            var kgrid = new KendoGrid<ConversionSummary>(request, conversionSummaries);
            // todo: aggregates?

            var json = Json(kgrid);
            return json;
        }

        public PartialViewResult CPMSummaryPartial()
        {
            var userInfo = GetUserInfo();
            var firstOfLatest = userInfo.Dates.FirstOfLatestCompleteMonth;
            var model = new ReportModel(
                startDate: firstOfLatest.AddMonths(-2).ToString("MM/yyyy"),
                endDate: firstOfLatest.ToString("MM/yyyy")
                );
            return PartialView("_CPMSummaryPartial", model);
        }

        [HttpPost]
        public JsonResult CPMSummaryData(KendoGridRequest request, DateTime? startdate, DateTime? enddate)
        {
            var userInfo = GetUserInfo();

            // TODO: ParseDates ?? ...& if (!start.HasValue) start = userInfo.Dates.FirstOfYear

            var monthlyInfos = cpRepo
                .GetMonthlyInfos("CPM", startdate, enddate, userInfo.AdvertiserId)
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
            var model = new ReportModel(GetUserInfo());
            return PartialView("_AffiliateReportPartial", model);
        }

        [HttpPost]
        public JsonResult AffiliateReportData(KendoGridRequest request, string startdate, string enddate, int? offerid)
        {
            var userInfo = GetUserInfo();
            DateTime? start, end;
            if (!ControllerHelpers.ParseDates(startdate, enddate, userInfo.CultureInfo, out start, out end))
                return Json(new { });

            if (!start.HasValue) start = userInfo.Dates.FirstOfMonth;

            var affSums = cpRepo.GetAffiliateSummaries(start, end, userInfo.AdvertiserId, offerid);

            var kgrid = new KendoGrid<AffiliateSummary>(request, affSums);
            if (affSums.Any())
            {
                var totalClicks = affSums.Sum(a => a.Clicks);
                var totalConversions = affSums.Sum(a => a.Convs);
                float totalConversionRate = (totalClicks == 0) ? 0 : (float)Math.Round((double)totalConversions / totalClicks, 3);
                kgrid.aggregates = new
                {
                    Clicks = new { sum = totalClicks },
                    Convs = new { sum = totalConversions },
                    ConvRate = new { agg = totalConversionRate },
                    Price = new { sum = affSums.Sum(c => c.Price) }
                };
            }
            var json = Json(kgrid);
            return json;
        }

        public PartialViewResult HeatMap()
        {
            return PartialView("_HeatMap");
        }

        public JsonResult HeatMapData(string startdate, string enddate)
        {
            var userInfo = GetUserInfo();
            DateTime? start, end;
            if (!ControllerHelpers.ParseDates(startdate, enddate, userInfo.CultureInfo, out start, out end))
                return ControllerHelpers.CreateJson();

            if (!start.HasValue) start = userInfo.Dates.FirstOfMonth;
            if (!end.HasValue) end = userInfo.Dates.Latest;

            var advertiserId = GetAdvertiserId();
            var profiler = MiniProfiler.Current;
            using (profiler.Step("HeatMapData"))
            {
                var results = cpRepo.GetConversionCountsByRegion(start.Value, end.Value, advertiserId.Value)
                    .Select(c => new object[]
                {
                    "US-" + c.RegionCode,
                    c.ClickCount // TODO: ClickCount is named wrong, should be ConversionCount
                }).ToList();

                results.Insert(0, new[] { "State", "Conversions" });

                var json = ControllerHelpers.CreateJson(results);
                return json;
            }
        }

        public JsonResult MobileDevicesData(string startdate, string enddate, int take)
        {
            var profiler = MiniProfiler.Current;
            using (profiler.Step("MobileDevicesData"))
            {
                var userInfo = GetUserInfo();
                DateTime? start, end;
                if (!ControllerHelpers.ParseDates(startdate, enddate, userInfo.CultureInfo, out start, out end))
                    return ControllerHelpers.CreateJson();

                if (!start.HasValue) start = userInfo.Dates.FirstOfMonth;

                var clicksByDevice = this.cpRepo.GetClicksByDeviceName(
                                                        start: start,
                                                        end: end ?? start,
                                                        advertiserId: GetAdvertiserId(),
                                                        offerId: null);

                var data = clicksByDevice.Where(c => c.DeviceName != "unknown");

                decimal totalClicks = data.Sum(c => c.ClickCount);

                var chartData = data.Take(take).Select(c => new
                {
                    category = TruncateDeviceNameForChartLegendLabel(c.DeviceName),
                    value = c.ClickCount / totalClicks
                });

                var jsonData = new {
                    data = data,
                    chart = chartData
                };
                var json = ControllerHelpers.CreateJson(jsonData);
                return json;
            }
        }

        private string TruncateDeviceNameForChartLegendLabel(string deviceName)
        {
            return deviceName.Length > 20 ? deviceName.Substring(0, 20) + ".." : deviceName;
        }
    }
}
