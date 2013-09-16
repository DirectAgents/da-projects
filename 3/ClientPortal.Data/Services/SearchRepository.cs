using ClientPortal.Data.Contexts;
using ClientPortal.Data.DTOs;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace ClientPortal.Data.Services
{
    public partial class ClientPortalRepository
    {
        public IQueryable<SearchDailySummary2> GetSearchDailySummaries(int? advertiserId, string channel, DateTime? start, DateTime? end, bool includeToday = false)
        {
            // All SearchDailySummary2 rows
            var summaries = context.SearchDailySummary2.AsQueryable();

            // Filter to advertiser, if present
            if (advertiserId.HasValue)
                summaries = summaries.Where(s => s.SearchCampaign.AdvertiserId == advertiserId.Value);

            // Filter to channel, if present
            if (channel != null)
                summaries = summaries.Where(s => s.SearchCampaign.Channel == channel);

            // Filter to start date, if present
            if (start.HasValue)
                summaries = summaries.Where(s => s.Date >= start);

            // When specifying, should the current day be included or just up until and including yesterday?
            // By default, we only go up to yesterday.
            if (!includeToday)
            {
                var yesterday = DateTime.Today.AddDays(-1);

                if (!end.HasValue || yesterday < end.Value) // if no end date is present OR end date is at least the current date
                    end = yesterday; // then set end date to yesterday's date
            }

            // Filter to end date, if present or includeToday logic sets it
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

            if (numWeeks.HasValue) // If the number of weeks is present
                start = DateTime.Today.AddDays(-7 * numWeeks.Value + 6); // Then set set start date to numWeeks weeks ago, plus 6 (works with leap year?)
            else
                start = DateTime.Today.AddYears(-1); // Otherwise set the start date to a year ago

            // Now move start date back to the closest Monday
            while (start.DayOfWeek != DayOfWeek.Monday)
                start = start.AddDays(-1);

            var stats =
                GetSearchDailySummaries(advertiserId, channel, start, null)

                    // Group by the Date
                    .GroupBy(s => s.Date)

                    // Aggregate counts
                    .Select(g => new
                    {
                        Date = g.Key,
                        Impressions = g.Sum(x => x.Impressions),
                        Clicks = g.Sum(x => x.Clicks),
                        Orders = g.Sum(x => x.Orders),
                        Revenue = g.Sum(x => x.Revenue),
                        Cost = g.Sum(x => x.Cost)
                    })

                    // Force execution (leave Linq to Entities)
                    .ToList()

                    // Reproject with members that break date up into Year and Week
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

                    // Now group by Year and Week
                    .GroupBy(x => new { x.Year, x.Week })

                    // Order by Year then Week
                    .OrderBy(g => g.Key.Year)
                    .ThenBy(g => g.Key.Week)

                    // Finally select new SearchStat objects
                    .Select((g, i) => new SearchStat
                    {
                        WeekByMaxDate = g.Max(s => s.Date), // Supply the latest date in the group of dates (which are all part of a week)
                        TitleIfNotNull = channel, // (if null, Title is "Range")

                        // Re-aggregate since we had to re-group
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

        public IQueryable<WeeklySearchStat> GetCampaignWeekStats2(int? advertiserId, DateTime startDate, DateTime endDate, DayOfWeek startDayOfWeek)
        {
            var weeks = CalenderWeek.Generate(startDate, endDate, startDayOfWeek);
            var stats = GetSearchDailySummaries(90001, null, startDate, endDate)
                            .Select(c => new
                            {
                                c.Date,
                                c.SearchCampaign.Channel,
                                c.SearchCampaign.SearchCampaignName,
                                c.Orders,
                                c.Revenue,
                                c.Cost
                            })
                            .AsEnumerable()
                            .GroupBy(c => new
                            {
                                Week = weeks.First(w => w.EndDate >= c.Date), 
                                c.Channel,
                                c.SearchCampaignName
                            })
                            .OrderBy(c => c.Key.Week.StartDate)
                            .ThenBy(c => c.Key.SearchCampaignName)
                            .Select(c => new
                            {
                                c.Key,
                                TotalOrders = c.Sum(o => o.Orders),
                                TotalRevenue = c.Sum(r => r.Revenue),
                                TotalCost = c.Sum(co => co.Cost)
                            })
                            .Select(c => new WeeklySearchStat
                            {
                                StartDate = c.Key.Week.StartDate,
                                EndDate = c.Key.Week.EndDate,
                                Channel = c.Key.Channel,
                                Campaign = c.Key.SearchCampaignName,
                                ROAS = c.TotalCost == 0 ? 0 : (int)Math.Round(100 * c.TotalRevenue / c.TotalCost),
                                CPO = c.TotalOrders == 0 ? 0 : Math.Round(c.TotalCost / c.TotalOrders, 2)
                            });
            return stats.AsQueryable();
        }

        // Same as GetWeekStats except gropued by channel and campaign as well
        // TODO: merge implementations - GetWeekStats can call this and then regroup, eliminating channel and campaign
        //                               alternately, both can call a private helper "GetStatsByWeek" and then return the approprate grouping
        public IQueryable<SearchStat> GetCampaignWeekStats(int? advertiserId, DateTime start, DateTime end, string channel = null)
        {
            var stats =
                GetSearchDailySummaries(advertiserId, channel, start, end)

                    // Group by the Date
                    .GroupBy(s => new { s.Date, s.SearchCampaign.Channel, s.SearchCampaign.SearchCampaignName })

                    // Aggregate counts
                    .Select(g => new
                    {
                        Date = g.Key.Date,
                        Channel = g.Key.Channel,
                        Campaign = g.Key.SearchCampaignName,
                        Impressions = g.Sum(x => x.Impressions),
                        Clicks = g.Sum(x => x.Clicks),
                        Orders = g.Sum(x => x.Orders),
                        Revenue = g.Sum(x => x.Revenue),
                        Cost = g.Sum(x => x.Cost)
                    })

                    // Force execution (leave Linq to Entities)
                    .ToList()

                    // Reproject with members that break date up into Year and Week
                    .Select(s => new
                    {
                        Date = s.Date,
                        Channel = s.Channel,
                        Campaign = s.Campaign,
                        Year = s.Date.Year,
                        Week = CultureInfo.InvariantCulture.Calendar.GetWeekOfYear(s.Date, CalendarWeekRule.FirstFourDayWeek, DayOfWeek.Monday),
                        Impressions = s.Impressions,
                        Clicks = s.Clicks,
                        Orders = s.Orders,
                        Revenue = s.Revenue,
                        Cost = s.Cost
                    })

                    // Now group by Year and Week
                    .GroupBy(x => new { x.Channel, x.Campaign, x.Year, x.Week })

                    // Order by Year then Week
                    .OrderBy(g => g.Key.Year)
                    .ThenBy(g => g.Key.Week)

                    // Finally select new SearchStat objects
                    .Select((g, i) => new SearchStat
                    {
                        Channel = g.Key.Channel,
                        Campaign = g.Key.Campaign,

                        WeekByMaxDate = g.Max(s => s.Date), // Supply the latest date in the group of dates (which are all part of a week)
                        TitleIfNotNull = channel, // (if null, Title is "Range")

                        // Re-aggregate since we had to re-group
                        Impressions = g.Sum(s => s.Impressions),
                        Clicks = g.Sum(s => s.Clicks),
                        Orders = g.Sum(s => s.Orders),
                        Revenue = g.Sum(s => s.Revenue),
                        Cost = g.Sum(s => s.Cost)
                    });

            return stats.AsQueryable();
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

        public IQueryable<SearchStat> GetCampaignStats(int? advertiserId, string channel, DateTime? start, DateTime? end, bool breakdown)
        {
            if (!start.HasValue)
                start = new DateTime(DateTime.Today.Year, 1, 1);
            if (!end.HasValue)
                end = DateTime.Today.AddDays(-1);

            var summaries = GetSearchDailySummaries(advertiserId, channel, start, end);
            IQueryable<SearchStat> stats;
            if (breakdown)
            {
                stats = summaries.GroupBy(s => new { s.SearchCampaign.Channel, s.SearchCampaign.SearchCampaignName, s.Network, s.Device, s.ClickType })
                    .OrderBy(g => g.Key.Channel).ThenBy(g => g.Key.SearchCampaignName)
                    .Select(g => new SearchStat
                    {
                        EndDate = end.Value,
                        CustomByStartDate = start.Value,
                        Channel = g.Key.Channel,
                        Title = g.Key.SearchCampaignName,
                        Network = g.Key.Network,
                        Device = g.Key.Device,
                        ClickType = g.Key.ClickType,
                        Impressions = g.Sum(s => s.Impressions),
                        Clicks = g.Sum(s => s.Clicks),
                        Orders = g.Sum(s => s.Orders),
                        Revenue = g.Sum(s => s.Revenue),
                        Cost = g.Sum(s => s.Cost)
                    });
            }
            else
            {
                stats = summaries.GroupBy(s => new { s.SearchCampaign.Channel, s.SearchCampaign.SearchCampaignName })
                    .OrderBy(g => g.Key.Channel).ThenBy(g => g.Key.SearchCampaignName)
                    .Select(g => new SearchStat
                    {
                        EndDate = end.Value,
                        CustomByStartDate = start.Value,
                        Channel = g.Key.Channel,
                        Title = g.Key.SearchCampaignName,
                        Impressions = g.Sum(s => s.Impressions),
                        Clicks = g.Sum(s => s.Clicks),
                        Orders = g.Sum(s => s.Orders),
                        Revenue = g.Sum(s => s.Revenue),
                        Cost = g.Sum(s => s.Cost)
                    });
            }
            return stats;
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
