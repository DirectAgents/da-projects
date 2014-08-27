using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AutoMapper;
using ClientPortal.Data.Contexts;
using ClientPortal.Data.Contracts;
using ClientPortal.Data.DTOs;
using ClientPortal.Data.Services;
using ClientPortal.Web.Models;
using DirectAgents.Mvc.KendoGridBinder;
using Newtonsoft.Json;

namespace ClientPortal.Web.Controllers
{
    [Authorize]
    public class SearchReportsController : CPController
    {
        public SearchReportsController(IClientPortalRepository cpRepository)
        {
            this.cpRepo = cpRepository;
        }

        [HttpPost]
        public JsonResult WeekSumData(KendoGridRequest request, int numweeks = 8)
        {
            var userInfo = GetUserInfo();

            var weekStats = cpRepo.GetWeekStats(userInfo.SearchProfile.SearchProfileId, numweeks, userInfo.SearchStartDayOfWeek, userInfo.UseAnalytics, !userInfo.UseYesterdayAsLatest);
            var kgrid = new KendoGrid<SearchStat>(request, weekStats);
            if (weekStats.Any())
                kgrid.aggregates = Aggregates(weekStats);

            var json = Json(kgrid);
            return json;
        }

        public FileResult WeekSumExport(int numweeks = 8)
        {
            var userInfo = GetUserInfo();

            var weekStats = cpRepo.GetWeekStats(userInfo.SearchProfile.SearchProfileId, numweeks, userInfo.SearchStartDayOfWeek, userInfo.UseAnalytics, !userInfo.UseYesterdayAsLatest);
            var rows = Mapper.Map<IEnumerable<SearchStat>, IEnumerable<SearchStatExportRow>>(weekStats);

            string filename = "WeeklySummary" + ControllerHelpers.DateStamp() + ".csv";
            return File(ControllerHelpers.CsvStream(rows), "application/CSV", filename);
        }

        [HttpPost]
        public JsonResult MonthSumData(KendoGridRequest request, int nummonths = 6)
        {
            var userInfo = GetUserInfo();

            var monthStats = cpRepo.GetMonthStats(userInfo.SearchProfile.SearchProfileId, nummonths, userInfo.UseAnalytics, !userInfo.UseYesterdayAsLatest)
                .ToList()
                .OrderBy(s => s.StartDate)
                .AsQueryable();
            var kgrid = new KendoGrid<SearchStat>(request, monthStats);
            if (monthStats.Any())
                kgrid.aggregates = Aggregates(monthStats);

            var json = Json(kgrid);
            return json;
        }

        public FileResult MonthSumExport(int nummonths = 6)
        {
            var userInfo = GetUserInfo();

            var monthStats = cpRepo.GetMonthStats(userInfo.SearchProfile.SearchProfileId, nummonths, userInfo.UseAnalytics, !userInfo.UseYesterdayAsLatest);
            var rows = Mapper.Map<IEnumerable<SearchStat>, IEnumerable<SearchStatExportRow>>(monthStats);

            string filename = "MonthlySummary" + ControllerHelpers.DateStamp() + ".csv";
            return File(ControllerHelpers.CsvStream(rows), "application/CSV", filename);
        }

        [HttpPost]
        public JsonResult ChannelPerfData(KendoGridRequest request) //, string startdate, string enddate)
        {
            var userInfo = GetUserInfo();

            var channelStats = cpRepo.GetChannelStats(userInfo.SearchProfile.SearchProfileId, userInfo.SearchStartDayOfWeek, userInfo.UseAnalytics, !userInfo.UseYesterdayAsLatest, true, userInfo.ShowSearchChannels);
            var kgrid = new KendoGrid<SearchStat>(request, channelStats);
            if (channelStats.Any())
                kgrid.aggregates = Aggregates(channelStats);

            var json = Json(kgrid);
            return json;
        }

        public FileResult ChannelPerfExport()
        {
            var userInfo = GetUserInfo();

            var stats = cpRepo.GetChannelStats(userInfo.SearchProfile.SearchProfileId, userInfo.SearchStartDayOfWeek, userInfo.UseAnalytics, !userInfo.UseYesterdayAsLatest, true, userInfo.ShowSearchChannels);
            var rows = Mapper.Map<IEnumerable<SearchStat>, IEnumerable<SearchStatExportRow>>(stats);

            string filename = "ChannelPerformance" + ControllerHelpers.DateStamp() + ".csv";
            return File(ControllerHelpers.CsvStream(rows), "application/CSV", filename);
        }

        [HttpPost]
        public JsonResult CampaignPerfData(KendoGridRequest request, string startdate, string enddate, string channel, bool breakdown = false)
        {
            var userInfo = GetUserInfo();
            var cultureInfo = userInfo.CultureInfo;
            DateTime? start, end;
            if (!ControllerHelpers.ParseDates(startdate, enddate, cultureInfo, out start, out end))
                return Json(new { });

            if (!start.HasValue) start = userInfo.DatesForSearch().FirstOfMonth;

            var stats = cpRepo.GetCampaignStats(userInfo.SearchProfile.SearchProfileId, channel, start, end, breakdown, userInfo.UseAnalytics);

            var kgrid = new KendoGrid<SearchStat>(request, stats);
            if (stats.Any())
                kgrid.aggregates = Aggregates(stats);

            var json = Json(kgrid);
            return json;
        }

        public FileResult CampaignPerfExport(string startdate, string enddate, string channel, bool breakdown = false)
        {
            var userInfo = GetUserInfo();
            var cultureInfo = userInfo.CultureInfo;
            DateTime? start, end;
            if (!ControllerHelpers.ParseDates(startdate, enddate, cultureInfo, out start, out end))
                return File("Error parsing dates: " + startdate + " and " + enddate, "text/plain");

            if (!start.HasValue) start = userInfo.DatesForSearch().FirstOfMonth;

            var stats = cpRepo.GetCampaignStats(userInfo.SearchProfile.SearchProfileId, channel, start, end, breakdown, userInfo.UseAnalytics)
                .OrderBy(s => s.EndDate).ThenByDescending(s => s.Channel).ThenBy(s => s.Title);
            var rows = Mapper.Map<IEnumerable<SearchStat>, IEnumerable<SearchStatExportRow>>(stats);

            string filename = "CampaignPerformance" + ControllerHelpers.DateStamp() + ".csv";
            return File(ControllerHelpers.CsvStream(rows), "application/CSV", filename);
        }

        public JsonResult CampaignPerfWeeklyData(string startdate, string enddate)
        {
            var userInfo = GetUserInfo();
            var cultureInfo = userInfo.CultureInfo;
            DateTime? start, end;

            if (!ControllerHelpers.ParseDates(startdate, enddate, cultureInfo, out start, out end))
                return Json(new { });

            if (!start.HasValue) start = userInfo.DatesForSearch().FirstOfYear;

            // Get weekly search stats
            var rows = cpRepo.GetCampaignWeekStats2(userInfo.SearchProfile.SearchProfileId, start.Value, end.Value, userInfo.SearchStartDayOfWeek, userInfo.UseAnalytics);

            // Create DataTable
            var dataTable = new DataTable("data");

            // Add regular columns
            dataTable.Columns.AddRange(new[]
            {
                new DataColumn("colChannel"),
                new DataColumn("colCampaign"),
                new DataColumn("colIsActive"),
                new DataColumn("colShowing"),
            });

            // Local func to convert two dates into a column name
            Func<DateTime, DateTime, string> strinfifyDates = 
                (dt1, dt2) =>
                    "col" + dt1.ToString("MM/dd").Replace("/", "_slash_") + "_space__dash__space_" + dt2.ToString("MM/dd").Replace("/", "_slash_");

            // Select week column headers
            var weekColumnHeaders = rows
                                    .Select(c => new { c.StartDate, c.EndDate })
                                    .Distinct()
                                    .Select(c => strinfifyDates(c.StartDate, c.EndDate));

            // Add week columns
            foreach (var week in weekColumnHeaders)
            {
                dataTable.Columns.Add(week);
            }

            // Group by channel and campaign to process rows
            var groups = rows.GroupBy(c => new { c.Channel, c.Campaign });

            // Add a row for each group
            foreach (var group in groups)
            {
                var dataRow = dataTable.NewRow();
                var showing = group.All(c => c.ROAS == 0) ? "CPO" : "ROAS";
                var isActive = true; // TODO: make real

                dataRow.SetField("colChannel", group.Key.Channel);
                dataRow.SetField("colCampaign", group.Key.Campaign);
                dataRow.SetField("colIsActive", isActive);
                dataRow.SetField("colShowing", showing);

                foreach (var item in group)
                {
                    var columnName = strinfifyDates(item.StartDate, item.EndDate);
                    var columnValue = (showing == "ROAS") ? item.ROAS: item.CPO;
                    dataRow.SetField(columnName, columnValue);
                }

                dataTable.Rows.Add(dataRow);
            }

            // Create result
            var json = new JsonNetResult { JsonRequestBehavior = JsonRequestBehavior.AllowGet };
            json.Data = new { data = dataTable };

            return json;
        }

        // ? unused ?
        [HttpPost]
        public JsonResult AdgroupPerfData(KendoGridRequest request, string startdate, string enddate)
        {
            var stats = cpRepo.GetAdgroupStats(); // note: this gives dummy data
            var kgrid = new KendoGrid<SearchStat>(request, stats);
            if (stats.Any())
                kgrid.aggregates = Aggregates(stats);

            var json = Json(kgrid);
            return json;
        }

        // --- private methods ---

        private object Aggregates(IQueryable<SearchStat> stats)
        {
            var sumRevenue = stats.Sum(s => s.Revenue);
            var sumCost = stats.Sum(s => s.Cost);
            var sumOrders = stats.Sum(s => s.Orders);
            var sumClicks = stats.Sum(s => s.Clicks);
            var sumImpressions = stats.Sum(s => s.Impressions);

            // Determine totalDays (The stats may or may not be for the same time period.)
            var periods = stats.Select(s => new { StartDate = s.StartDate, EndDate = s.EndDate });
            var distinctPeriods = periods.Distinct();
            var totalDays = distinctPeriods.Sum(p => (p.EndDate - p.StartDate).Days + 1);
            var aggregates = new
            {
                Revenue = new { sum = sumRevenue },
                Cost = new { sum = sumCost },
                ROAS = new { agg = sumCost == 0 ? 0 : (int)Math.Round(100 * sumRevenue / sumCost) },
                Margin = new { agg = sumRevenue - sumCost },
                Orders = new { sum = sumOrders },
                CPO = new { agg = sumOrders == 0 ? 0 : Math.Round(sumCost / sumOrders, 2) },
                OrderRate = new { agg = sumClicks == 0 ? 0 : Math.Round((decimal)100 * sumOrders / sumClicks, 2) },
                RevenuePerOrder = new { agg = sumOrders == 0 ? 0 : Math.Round(sumRevenue / sumOrders, 2) },
                CPC = new { agg = sumClicks == 0 ? 0 : Math.Round(sumCost / sumClicks, 2) },
                Clicks = new { sum = sumClicks },
                Impressions = new { sum = sumImpressions },
                CTR = new { agg = sumImpressions == 0 ? 0 : Math.Round((decimal)100 * sumClicks / sumImpressions, 2) },
                OrdersPerDay = new { agg = totalDays == 0 ? 0 : Math.Round((decimal)sumOrders / totalDays, 2) },
            };
            return aggregates;
        }
    }

    public class JsonNetResult : JsonResult
    {
        //        public JsonRequestBehavior JsonRequestBehavior { get; set; }
        //        public Encoding ContentEncoding { get; set; }
        //        public string ContentType { get; set; }
        //        public object Data { get; set; }

        public JsonSerializerSettings SerializerSettings { get; set; }
        public Formatting Formatting { get; set; }

        public JsonNetResult()
        {
            Formatting = Formatting.None;
            SerializerSettings = new JsonSerializerSettings();
            JsonRequestBehavior = JsonRequestBehavior.DenyGet;
        }

        public override void ExecuteResult(ControllerContext context)
        {
            if (context == null)
                throw new ArgumentNullException("context");

            if (JsonRequestBehavior == JsonRequestBehavior.DenyGet
                && String.Equals(context.HttpContext.Request.HttpMethod, "GET", StringComparison.OrdinalIgnoreCase))
            {
                throw new InvalidOperationException("This request has been blocked because sensitive information could be disclosed to third party web sites when this is used in a GET request. To allow GET requests, set JsonRequestBehavior to AllowGet.");
            }

            HttpResponseBase response = context.HttpContext.Response;

            response.ContentType = !string.IsNullOrEmpty(ContentType)
                                        ? ContentType
                                        : "application/json";

            if (ContentEncoding != null)
                response.ContentEncoding = ContentEncoding;

            if (Data != null)
            {
                var writer = new JsonTextWriter(response.Output) { Formatting = Formatting };

                var serializer = JsonSerializer.Create(SerializerSettings);
                serializer.Serialize(writer, Data);

                writer.Flush();
            }
        }
    }
}
