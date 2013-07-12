﻿using ClientPortal.Data.Contexts;
using ClientPortal.Data.DTOs;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace ClientPortal.Data.Services
{
    public partial class ClientPortalRepository
    {
        public IQueryable<SearchDailySummary> GetSearchDailySummaries(int? advertiserId, string channel, DateTime? start, DateTime? end, bool includeToday = false)
        {
            var summaries = context.SearchDailySummaries.AsQueryable();

            if (advertiserId.HasValue)
                summaries = summaries.Where(s => s.SearchCampaign.AdvertiserId == advertiserId.Value);

            if (channel != null)
                summaries = summaries.Where(s => s.SearchCampaign.Channel == channel);

            if (start.HasValue)
                summaries = summaries.Where(s => s.Date >= start);

            if (!includeToday)
            {
                var yesterday = DateTime.Today.AddDays(-1);
                if (!end.HasValue || yesterday < end.Value)
                    end = yesterday;
            }
            if (end.HasValue)
                summaries = summaries.Where(s => s.Date <= end);
            return summaries;
        }

        // this does sunday through saturday. would have to SET DATEFIRST 1 in sql server to get it to work correctly
        //public IQueryable<SearchStat> GetWeekStatsX()
        //{   // need to group by year also
        //    var stats = context.SearchDailySummaries.GroupBy(s => SqlFunctions.DatePart("week", s.Date)).Select(g =>
        //        new SearchStat
        //        {
        //            Week = g.Key ?? 0,
        //            Impressions = g.Sum(s => s.Impressions),
        //            Clicks = g.Sum(s => s.Clicks),
        //            Orders = g.Sum(s => s.Orders),
        //            Revenue = g.Sum(s => s.Revenue),
        //            Cost = g.Sum(s => s.Cost)
        //        });
        //    return stats;
        //}
        public IQueryable<SearchStat> GetWeekStats(int? advertiserId, int? numWeeks, string channel = null)
        {
            DateTime start;
            if (numWeeks.HasValue)
                start = DateTime.Today.AddDays(-7 * numWeeks.Value + 6);
            else
                start = DateTime.Today.AddYears(-1);

            while (start.DayOfWeek != DayOfWeek.Monday)
                start = start.AddDays(-1);

            var stats = GetSearchDailySummaries(advertiserId, channel, start, null).GroupBy(s => s.Date).Select(g => new
            {
                Date = g.Key,
                Impressions = g.Sum(x => x.Impressions),
                Clicks = g.Sum(x => x.Clicks),
                Orders = g.Sum(x => x.Orders),
                Revenue = g.Sum(x => x.Revenue),
                Cost = g.Sum(x => x.Cost)
            })
            .ToList()
            .Select(s => new
            {
                Date = s.Date,
                Year = s.Date.Year,
                Week = CultureInfo.InvariantCulture.Calendar.GetWeekOfYear(s.Date, CalendarWeekRule.FirstFourDayWeek, DayOfWeek.Monday),
                Impressions = s.Impressions,
                Clicks = s.Clicks,
                Orders = s.Orders,
                Revenue = s.Revenue,
                Cost = s.Cost
            })
            .GroupBy(x => new { x.Year, x.Week })
            .OrderBy(g => g.Key.Year).ThenBy(g => g.Key.Week)
            .Select((g, i) => new SearchStat
            {
                WeekByMaxDate = g.Max(s => s.Date),
                TitleIfNotNull = channel, // (if null, Title is "Range")
                Impressions = g.Sum(s => s.Impressions),
                Clicks = g.Sum(s => s.Clicks),
                Orders = g.Sum(s => s.Orders),
                Revenue = g.Sum(s => s.Revenue),
                Cost = g.Sum(s => s.Cost)
            });
            return stats.AsQueryable();
            //{
            //    WeekGroup = g,
            //    WeekNum = i + 1,
            //    Year = g.Key.Year,
            //    CalendarWeek = g.Key.Week
            //}
        }

        public IQueryable<SearchStat> GetMonthStats(int? advertiserId, int? numMonths)
        {
            DateTime start;
            if (numMonths.HasValue)
                start = DateTime.Today.AddMonths((numMonths.Value - 1) * -1);
            else
                start = DateTime.Today.AddYears(-1);

            start = new DateTime(start.Year, start.Month, 1);

            var stats = GetSearchDailySummaries(advertiserId, null, start, null)
                .GroupBy(s => new { s.Date.Year, s.Date.Month })
                .OrderBy(g => g.Key.Year).ThenBy(g => g.Key.Month)
                .Select(g =>
                new SearchStat
                {
                    MonthByMaxDate = g.Max(s => s.Date),
                    Impressions = g.Sum(s => s.Impressions),
                    Clicks = g.Sum(s => s.Clicks),
                    Orders = g.Sum(s => s.Orders),
                    Revenue = g.Sum(s => s.Revenue),
                    Cost = g.Sum(s => s.Cost)
                });
            return stats;
        }

        public IQueryable<SearchStat> GetChannelStats(int? advertiserId)
        {
            var googleStats = GetWeekStats(advertiserId, null, "Google");
            var bingStats = GetWeekStats(advertiserId, null, "Bing");
            return googleStats.Concat(bingStats).AsQueryable();
        }

        public IQueryable<SearchStat> GetCampaignStats(int? advertiserId, string channel, DateTime? start, DateTime? end)
        {
            if (!start.HasValue)
                start = new DateTime(DateTime.Today.Year, 1, 1);
            if (!end.HasValue)
                end = DateTime.Today.AddDays(-1);

            var stats = GetSearchDailySummaries(advertiserId, channel, start, end)
                .GroupBy(s => s.SearchCampaign.SearchCampaignName)
                .OrderBy(g => g.Key)
                .Select(g => new SearchStat
                {
                    EndDate = end.Value,
                    CustomByStartDate = start.Value,
                    Title = g.Key,
                    Impressions = g.Sum(s => s.Impressions),
                    Clicks = g.Sum(s => s.Clicks),
                    Orders = g.Sum(s => s.Orders),
                    Revenue = g.Sum(s => s.Revenue),
                    Cost = g.Sum(s => s.Cost)
                });
            return stats;
        }

        // temp; for dummy data
        public IQueryable<SearchStat> GetCampaignStats(string channel)
        {
            List<SearchStat> stats = new List<SearchStat>();
            if (channel == null || channel.Contains("Google"))
                stats.AddRange(new List<SearchStat> {
                    new SearchStat(true, 6, 2, 6025, 118, 4, 339.95m, 121.62m, "DA - \"Apple\" Memory - Keywords"),
                    new SearchStat(true, 6, 2, 6562, 293, 13, 2802.85m, 320.46m, "DA - \"Mac\" Memory - Keywords"),
                });
            if (channel == null || channel.Contains("Bing"))
                stats.AddRange(new List<SearchStat> {
                    new SearchStat(true, 6, 2, 1562, 67, 3, 599.97m, 71.86m, "DA - BING - \"Mac\" Memory - Keywords"),
                    new SearchStat(true, 6, 2, 1224, 116, 4, 419.96m, 74.01m, "DA - BING - iMac Memory"),
                });
            return stats.AsQueryable();
        }

        public IQueryable<SearchStat> GetAdgroupStats()
        {
            var stats = new List<SearchStat>
            {
                new SearchStat(true, 6, 2, 1281, 34, 2, 230m, 31.49m, "Apple Computer Ram"),
                new SearchStat(true, 6, 2, 1002, 23, 0, 0m, 14.13m, "Apple Memory"),
                new SearchStat(true, 6, 2, 1819, 20, 1, 80m, 15.96m, "Apple Memory Module"),
                new SearchStat(true, 6, 2, 1295, 11, 0, 0m, 17.31m, "Apple RAM"),
            };
            return stats.AsQueryable();
        }
    }
}
