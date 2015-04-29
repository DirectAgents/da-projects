using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AutoMapper;
using ClientPortal.Data.Contracts;
using ClientPortal.Data.DTOs;
using ClientPortal.Web.Models;
using KendoGridBinderEx;
using KendoGridBinderEx.ModelBinder.Mvc;
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
        public JsonResult WeekSumData(KendoGridMvcRequest request, int numweeks = 8)
        {
            var userInfo = GetUserInfo();
            request.AggregateObjects = request.AggregateObjects.Where(ao => ao.Aggregate != "agg");

            var titleSort = request.SortObjects.SingleOrDefault(so => so.Field == "Title");
            if (titleSort != null) titleSort.Field = "StartDate";

            var endDate = userInfo.Search_UseYesterdayAsLatest ? DateTime.Today.AddDays(-1) : DateTime.Today;
            var weekStats = cpRepo.GetWeekStats(userInfo.SearchProfile, numweeks, endDate);
            var kgrid = new KendoGridEx<SearchStat>(request, weekStats);

            return CreateJsonResult(kgrid);
        }

        public FileResult WeekSumExport(int numweeks = 8)
        {
            var userInfo = GetUserInfo();
            var endDate = userInfo.Search_UseYesterdayAsLatest ? DateTime.Today.AddDays(-1) : DateTime.Today;
            var weekStats = cpRepo.GetWeekStats(userInfo.SearchProfile, numweeks, endDate);

            string filename = "WeeklySummary" + ControllerHelpers.DateStamp() + ".csv";
            if (userInfo.SearchProfile.ShowRevenue)
            {
                var rows = Mapper.Map<IEnumerable<SearchStat>, IEnumerable<SearchStatExportRow_Retail>>(weekStats);
                return File(ControllerHelpers.CsvStream(rows), "application/CSV", filename);
            }
            else
            {
                var rows = Mapper.Map<IEnumerable<SearchStat>, IEnumerable<SearchStatExportRow_LeadGen>>(weekStats);
                return File(ControllerHelpers.CsvStream(rows), "application/CSV", filename);
            }
        }

        [HttpPost]
        public JsonResult MonthSumData(KendoGridMvcRequest request, int nummonths = 6)
        {
            var userInfo = GetUserInfo();
            request.AggregateObjects = request.AggregateObjects.Where(ao => ao.Aggregate != "agg");

            var titleSort = request.SortObjects.SingleOrDefault(so => so.Field == "Title");
            if (titleSort != null) titleSort.Field = "StartDate";

            var endDate = userInfo.Search_UseYesterdayAsLatest ? DateTime.Today.AddDays(-1) : DateTime.Today;
            var monthStats = cpRepo.GetMonthStats(userInfo.SearchProfile, nummonths, endDate);
            var kgrid = new KendoGridEx<SearchStat>(request, monthStats);

            return CreateJsonResult(kgrid);
        }

        public FileResult MonthSumExport(int nummonths = 6)
        {
            var userInfo = GetUserInfo();
            var endDate = userInfo.Search_UseYesterdayAsLatest ? DateTime.Today.AddDays(-1) : DateTime.Today;
            var monthStats = cpRepo.GetMonthStats(userInfo.SearchProfile, nummonths, endDate);

            string filename = "MonthlySummary" + ControllerHelpers.DateStamp() + ".csv";
            if (userInfo.SearchProfile.ShowRevenue)
            {
                var rows = Mapper.Map<IEnumerable<SearchStat>, IEnumerable<SearchStatExportRow_Retail>>(monthStats);
                return File(ControllerHelpers.CsvStream(rows), "application/CSV", filename);
            }
            else
            {
                var rows = Mapper.Map<IEnumerable<SearchStat>, IEnumerable<SearchStatExportRow_LeadGen>>(monthStats);
                return File(ControllerHelpers.CsvStream(rows), "application/CSV", filename);
            }
        }

        [HttpPost]
        public JsonResult ChannelPerfData(KendoGridMvcRequest request, int numweeks = 8) //, string startdate, string enddate)
        {
            request.AggregateObjects = request.AggregateObjects.Where(ao => ao.Aggregate != "agg");
            var userInfo = GetUserInfo();

            var channelStats = cpRepo.GetChannelStats(userInfo.SearchProfile, numweeks, !userInfo.Search_UseYesterdayAsLatest, true, userInfo.ShowSearchChannels);
            var kgrid = new KendoGridEx<SearchStat>(request, channelStats);

            return CreateJsonResult(kgrid);
        }

        public FileResult ChannelPerfExport(int numweeks = 8)
        {
            var userInfo = GetUserInfo();
            var stats = cpRepo.GetChannelStats(userInfo.SearchProfile, numweeks, !userInfo.Search_UseYesterdayAsLatest, true, userInfo.ShowSearchChannels);

            string filename = "ChannelPerformance" + ControllerHelpers.DateStamp() + ".csv";
            if (userInfo.SearchProfile.ShowRevenue)
            {
                var rows = Mapper.Map<IEnumerable<SearchStat>, IEnumerable<SearchStatExportRow_Retail>>(stats);
                return File(ControllerHelpers.CsvStream(rows), "application/CSV", filename);
            }
            else
            {
                var rows = Mapper.Map<IEnumerable<SearchStat>, IEnumerable<SearchStatExportRow_LeadGen>>(stats);
                return File(ControllerHelpers.CsvStream(rows), "application/CSV", filename);
            }
        }

        [HttpPost]
        public JsonResult DeviceData(KendoGridMvcRequest request, string startdate, string enddate)
        {
            request.AggregateObjects = request.AggregateObjects.Where(ao => ao.Aggregate != "agg");
            var userInfo = GetUserInfo();
            var cultureInfo = userInfo.CultureInfo;

            DateTime? start, end;
            if (!ControllerHelpers.ParseDates(startdate, enddate, cultureInfo, out start, out end))
                return Json(new { });
            if (!start.HasValue || !end.HasValue)
                return Json(new { });

            //TODO: useAnalytics argument; +includeCalls?
            var stats = cpRepo.GetDeviceStats(userInfo.SearchProfile, start.Value, end.Value);
            var kgrid = new KendoGridEx<SearchStat>(request, stats);

            return CreateJsonResult(kgrid);
        }

        [HttpPost]
        public JsonResult CampaignPerfData(KendoGridMvcRequest request, string startdate, string enddate, string channel, bool breakdown = false, bool? brandFilter = null)
        {
            request.AggregateObjects = request.AggregateObjects.Where(ao => ao.Aggregate != "agg");
            var userInfo = GetUserInfo();
            var cultureInfo = userInfo.CultureInfo;

            DateTime? start, end;
            if (!ControllerHelpers.ParseDates(startdate, enddate, cultureInfo, out start, out end))
                return Json(new { });

            if (!start.HasValue) start = userInfo.Search_Dates.FirstOfMonth;
            if (!end.HasValue) end = userInfo.Search_Dates.Latest;

            var stats = cpRepo.GetCampaignStats(userInfo.SearchProfile, channel, start, end, breakdown);
            stats = FilterBrand(stats, brandFilter);
            var kgrid = new KendoGridEx<SearchStat>(request, stats);

            return CreateJsonResult(kgrid);
        }

        // used for Megabus
        private IQueryable<SearchStat> FilterBrand(IQueryable<SearchStat> stats, bool? brandFilter)
        {
            if (brandFilter.HasValue)
            {
                if (brandFilter.Value)
                    return stats.Where(s => s.Title.StartsWith("DAG - Branded") || s.Title.StartsWith("DAB - Branded") || s.Title == "RAIS");
                else
                    return stats.Where(s => !s.Title.StartsWith("DAG - Branded") && !s.Title.ToLower().Contains("remarketing") && !s.Title.ToLower().Contains("rlsa")
                                        && !s.Title.StartsWith("DAB - Branded") && s.Title != "RAIS");
            }
            else
                return stats;
        }

        public FileResult CampaignPerfExport(string startdate, string enddate, string channel, bool breakdown = false, bool? brandFilter = null)
        {
            var userInfo = GetUserInfo();
            var cultureInfo = userInfo.CultureInfo;
            DateTime? start, end;
            if (!ControllerHelpers.ParseDates(startdate, enddate, cultureInfo, out start, out end))
                return File("Error parsing dates: " + startdate + " and " + enddate, "text/plain");

            if (!start.HasValue) start = userInfo.Search_Dates.FirstOfMonth;
            if (!end.HasValue) end = userInfo.Search_Dates.Latest;

            var stats = cpRepo.GetCampaignStats(userInfo.SearchProfile, channel, start, end, breakdown);
            stats = FilterBrand(stats, brandFilter)
                        .OrderBy(s => s.EndDate).ThenByDescending(s => s.Channel).ThenBy(s => s.Title);

            string filename = "CampaignPerformance" + ControllerHelpers.DateStamp() + ".csv";
            if (userInfo.SearchProfile.ShowRevenue)
            {
                var rows = Mapper.Map<IEnumerable<SearchStat>, IEnumerable<SearchStatExportRow_Retail>>(stats);
                return File(ControllerHelpers.CsvStream(rows), "application/CSV", filename);
            }
            else
            {
                var rows = Mapper.Map<IEnumerable<SearchStat>, IEnumerable<SearchStatExportRow_LeadGen>>(stats);
                return File(ControllerHelpers.CsvStream(rows), "application/CSV", filename);
            }
        }

        public JsonResult CampaignPerfWeeklyData(string startdate, string enddate)
        {
            var userInfo = GetUserInfo();
            var searchProfile = userInfo.SearchProfile;
            var cultureInfo = userInfo.CultureInfo;
            DateTime? start, end;

            if (!ControllerHelpers.ParseDates(startdate, enddate, cultureInfo, out start, out end))
                return Json(new { });

            if (!start.HasValue) start = userInfo.Search_Dates.FirstOfYear;
            if (!end.HasValue) end = userInfo.Search_Dates.Latest;

            // Get weekly search stats
            var rows = cpRepo.GetCampaignWeekStats2(searchProfile, start.Value, end.Value);

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
                var showing = searchProfile.ShowRevenue ? "ROAS(%)" : "CPL($)";
                var isActive = true; // TODO: make real

                dataRow.SetField("colChannel", group.Key.Channel);
                dataRow.SetField("colCampaign", group.Key.Campaign);
                dataRow.SetField("colIsActive", isActive);
                dataRow.SetField("colShowing", showing);

                foreach (var item in group)
                {
                    var columnName = strinfifyDates(item.StartDate, item.EndDate);
                    var columnValue = (showing == "ROAS(%)") ? item.ROAS : item.CPL;
                    dataRow.SetField(columnName, columnValue);
                }

                dataTable.Rows.Add(dataRow);
            }

            // Create result
            var json = new JsonNetResult { JsonRequestBehavior = JsonRequestBehavior.AllowGet };
            json.Data = new { data = dataTable };

            return json;
        }

        //[HttpPost]
        //public JsonResult AdgroupPerfData(KendoGridRequest request, string startdate, string enddate)
        //{
        //    var stats = cpRepo.GetAdgroupStats(); // note: this gives dummy data
        //    var kgrid = new KendoGrid<SearchStat>(request, stats);
        //    if (stats.Any())
        //        kgrid.aggregates = Aggregates(stats);

        //    var json = Json(kgrid);
        //    return json;
        //}

        // --- private methods ---

        private JsonResult CreateJsonResult(KendoGridEx<SearchStat> kgrid)
        {
            // Note: KendoGridEx<T> has a Groups property. For some reason, the grid on the client side doesn't work when the json returned
            //       has a null Groups property.  So we use a KG<T> which doesn't have a Groups property.
            var kg = new KG<SearchStat>();
            kg.data = kgrid.Data;
            kg.total = kgrid.Total;

            // Determine totalDays (The stats may or may not be for the same time period.)
            var periods = kgrid.Data.Select(s => new { StartDate = s.StartDate, EndDate = s.EndDate });
            var distinctPeriods = periods.Distinct();
            var totalDays = distinctPeriods.Sum(p => (p.EndDate - p.StartDate).Days + 1);
            kg.aggregates = Aggregates(kgrid, totalDays);

            var json = Json(kg);
            return json;

            //TRY THIS... includes "groups"
            //kgrid.Aggregates = kg.aggregates;
            //var json = Json(kgrid);
            //return json;
        }

        private object Aggregates(KendoGridEx<SearchStat> kgrid, int totalDays)
        {
            if (kgrid.Total == 0) return null;

            decimal sumRevenue = ((dynamic)kgrid.Aggregates)["Revenue"]["sum"];
            decimal sumCost = ((dynamic)kgrid.Aggregates)["Cost"]["sum"];
            int sumOrders = ((dynamic)kgrid.Aggregates)["Orders"]["sum"];
            int sumViewThrus = ((dynamic)kgrid.Aggregates)["ViewThrus"]["sum"];
            decimal sumViewThruRev = ((dynamic)kgrid.Aggregates)["ViewThruRev"]["sum"];
            int sumClicks = ((dynamic)kgrid.Aggregates)["Clicks"]["sum"];
            int sumImpressions = ((dynamic)kgrid.Aggregates)["Impressions"]["sum"];
            int sumCalls = ((dynamic)kgrid.Aggregates)["Calls"]["sum"];
            int sumTotalLeads = ((dynamic)kgrid.Aggregates)["TotalLeads"]["sum"];

            var aggregates = new
            {
                Revenue = new { sum = sumRevenue },
                Cost = new { sum = sumCost },
                ROAS = new { agg = sumCost == 0 ? 0 : (int)Math.Round(100 * sumRevenue / sumCost) },
                Margin = new { agg = sumRevenue - sumCost },
                Orders = new { sum = sumOrders },
                ViewThrus = new { sum = sumViewThrus },
                ViewThruRev = new { sum = sumViewThruRev },
                CPO = new { agg = sumOrders == 0 ? 0 : Math.Round(sumCost / sumOrders, 2) },
                OrderRate = new { agg = sumClicks == 0 ? 0 : Math.Round((decimal)100 * sumOrders / sumClicks, 2) },
                RevenuePerOrder = new { agg = sumOrders == 0 ? 0 : Math.Round(sumRevenue / sumOrders, 2) },
                CPC = new { agg = sumClicks == 0 ? 0 : Math.Round(sumCost / sumClicks, 2) },
                Clicks = new { sum = sumClicks },
                Impressions = new { sum = sumImpressions },
                CTR = new { agg = sumImpressions == 0 ? 0 : Math.Round((decimal)100 * sumClicks / sumImpressions, 2) },
                OrdersPerDay = new { agg = totalDays == 0 ? 0 : Math.Round((decimal)sumOrders / totalDays, 2) },

                Calls = new { sum = sumCalls },
                TotalLeads = new { sum = sumTotalLeads },
                ConvRate = new { agg = sumClicks == 0 ? 0 : Math.Round((decimal)100 * sumTotalLeads / sumClicks, 2) },
                CPL = new { agg = sumTotalLeads == 0 ? 0 : Math.Round(sumCost / sumTotalLeads, 2) },
            };
            return aggregates;
        }
    }

    class KG<T>
    {
        public IEnumerable<T> data { get; set; }
        public int total { get; set; }
        public object aggregates { get; set; }
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
