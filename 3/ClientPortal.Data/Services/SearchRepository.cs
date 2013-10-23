using ClientPortal.Data.Contexts;
using ClientPortal.Data.DTOs;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace ClientPortal.Data.Services
{
    class SummaryPair
    {
        public SearchDailySummary2 Summary { get; set; }
        public GoogleAnalyticsSummary GaSummary { get; set; }
    }

    class SearchSummary
    {
        public DateTime Date { get; set; }
        public int Impressions { get; set; }
        public int Clicks { get; set; }
        public int Orders { get; set; }
        public decimal Revenue { get; set; }
        public decimal Cost { get; set; }
    }

    public partial class ClientPortalRepository
    {
        public SearchStat GetSearchStats(int? advertiserId, DateTime? start, DateTime? end, bool includeToday = false)
        {
            if (!start.HasValue)
                start = new DateTime(DateTime.Today.Year, 1, 1);
            if (!end.HasValue)
                end = DateTime.Today.AddDays(-1);

            var summaries = GetSearchDailySummaries(advertiserId, null, start, end, includeToday);
            var searchStat = new SearchStat
            {
                EndDate = end.Value,
                CustomByStartDate = start.Value,
                Impressions = summaries.Sum(s => s.Impressions),
                Clicks = summaries.Sum(s => s.Clicks),
                Orders = summaries.Sum(s => s.Orders),
                Revenue = summaries.Sum(s => s.Revenue),
                Cost = summaries.Sum(s => s.Cost)
            };

            return searchStat;
        }

        public IQueryable<SearchCampaign> GetSearchCampaigns(int? advertiserId, string channel)
        {
            var searchCampaigns = context.SearchCampaigns.AsQueryable();

            if (advertiserId.HasValue)
                searchCampaigns = searchCampaigns.Where(c => c.AdvertiserId == advertiserId.Value);
            if (channel != null)
                searchCampaigns = searchCampaigns.Where(c => c.Channel == channel);

            return searchCampaigns;
        }

        private IQueryable<SearchDailySummary2> GetSearchDailySummaries(int? advertiserId, string channel, DateTime? start, DateTime? end, bool includeToday)
        {
            var searchCampaigns = GetSearchCampaigns(advertiserId, channel);
            var summaries = searchCampaigns.SelectMany(c => c.SearchDailySummaries2);

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
/*
            if (useAnalytics) // use Google Analytics for transactions (orders) and revenue
            {
                var gaSummaries = searchCampaigns.SelectMany(c => c.GoogleAnalyticsSummaries);

                if (start.HasValue)
                    gaSummaries = gaSummaries.Where(s => s.Date >= start);
                if (end.HasValue)
                    gaSummaries = gaSummaries.Where(s => s.Date <= end);

                var pairs = (from sum in summaries
                            join ga in gaSummaries on new { sum.SearchCampaignId, sum.Date } equals new { ga.SearchCampaignId, ga.Date } into gj_sums
                            from gaSum in gj_sums.DefaultIfEmpty() // left join to gaSums
                            select new SummaryPair() { Summary = sum, GaSummary = gaSum })
                            .ToList();
                summaries = pairs
                            .Select(sp => new SearchDailySummary2()
                            {
                                SearchCampaignId = sp.Summary.SearchCampaignId,
                                Date = sp.Summary.Date,
                                Revenue = (sp.GaSummary == null) ? 0 : sp.GaSummary.Revenue,
                                Cost = sp.Summary.Cost,
                                Orders = (sp.GaSummary == null) ? 0 : sp.GaSummary.Transactions,
                                Clicks = sp.Summary.Clicks,
                                Impressions = sp.Summary.Impressions,
                                CurrencyId = sp.Summary.CurrencyId,
                                Network = sp.Summary.Network,
                                Device = sp.Summary.Device,
                                ClickType = sp.Summary.ClickType
                            }).AsQueryable();
                //note that if using analytics, the summaries collection has been ToList'ed
            }
*/
            return summaries;
        }

        private IQueryable<GoogleAnalyticsSummary> GetGoogleAnalyticsSummaries(int? advertiserId, string channel, DateTime? start, DateTime? end, bool includeToday)
        {
            var searchCampaigns = GetSearchCampaigns(advertiserId, channel);
            var summaries = searchCampaigns.SelectMany(c => c.GoogleAnalyticsSummaries);

            if (start.HasValue) summaries = summaries.Where(s => s.Date >= start);

            if (!includeToday)
            {
                var yesterday = DateTime.Today.AddDays(-1);
                if (!end.HasValue || yesterday < end.Value) end = yesterday;
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
        public IQueryable<SearchStat> GetWeekStats(int? advertiserId, string channel, int? numWeeks, bool useAnalytics = false)
        {
            DateTime start;

            if (numWeeks.HasValue) // If the number of weeks is present
                start = DateTime.Today.AddDays(-7 * numWeeks.Value + 6); // Then set start date to numWeeks weeks ago, plus 6 (works with leap year?)
            else
                start = DateTime.Today.AddYears(-1); // Otherwise set the start date to a year ago

            // Now move start date back to the closest Monday
            while (start.DayOfWeek != DayOfWeek.Monday)
                start = start.AddDays(-1);

            var daySums =
                GetSearchDailySummaries(advertiserId, channel, start, null, false)

                    // Group by the Date
                    .GroupBy(s => s.Date)

                    // Aggregate counts
                    .Select(g => new SearchSummary
                    {
                        Date = g.Key,
                        Impressions = g.Sum(x => x.Impressions),
                        Clicks = g.Sum(x => x.Clicks),
                        Orders = g.Sum(x => x.Orders),
                        Revenue = g.Sum(x => x.Revenue),
                        Cost = g.Sum(x => x.Cost)
                    })

                    // Force execution (leave Linq to Entities)
                    .ToList();

            if (useAnalytics)
            {
                var gaSums = GetGoogleAnalyticsSummaries(advertiserId, channel, start, null, false)
                    .GroupBy(s => s.Date)
                    .Select(g => new SearchSummary
                    {
                        Date = g.Key,
                        Orders = g.Sum(x => x.Transactions),
                        Revenue = g.Sum(x => x.Revenue)
                    }).ToList();
                // note: can we do this without ToList'ing?

                daySums = (from daySum in daySums
                           join ga in gaSums on daySum.Date equals ga.Date into gj_sums
                           from gaSum in gj_sums.DefaultIfEmpty() // left join to gaSums
                           select new SearchSummary
                           {
                               Date = daySum.Date,
                               Impressions = daySum.Impressions,
                               Clicks = daySum.Clicks,
                               Orders = (gaSum == null) ? 0 : gaSum.Orders,
                               Revenue = (gaSum == null) ? 0 : gaSum.Revenue,
                               Cost = daySum.Cost
                           }).ToList();
            }

            var stats = daySums
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

        // (used for Campaign Weekly report)
        public IQueryable<WeeklySearchStat> GetCampaignWeekStats2(int? advertiserId, DateTime startDate, DateTime endDate, DayOfWeek startDayOfWeek)
        {
            var weeks = CalenderWeek.Generate(startDate, endDate, startDayOfWeek);
            var stats = GetSearchDailySummaries(advertiserId, null, startDate, endDate, false)
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

        public IQueryable<SearchStat> GetMonthStats(int? advertiserId, int? numMonths)
        {
            DateTime start;
            if (numMonths.HasValue)
                start = DateTime.Today.AddMonths((numMonths.Value - 1) * -1);
            else
                start = DateTime.Today.AddYears(-1);

            start = new DateTime(start.Year, start.Month, 1);

            var stats = GetSearchDailySummaries(advertiserId, null, start, null, false)
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
            var googleStats = GetWeekStats(advertiserId, "google", null);
            var bingStats = GetWeekStats(advertiserId, "bing", null);
            return googleStats.Concat(bingStats).AsQueryable();
        }

        public IQueryable<SearchStat> GetCampaignStats(int? advertiserId, string channel, DateTime? start, DateTime? end, bool breakdown)
        {
            if (!start.HasValue)
                start = new DateTime(DateTime.Today.Year, 1, 1);
            if (!end.HasValue)
                end = DateTime.Today.AddDays(-1);

            var summaries = GetSearchDailySummaries(advertiserId, channel, start, end, false);
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
                new SearchStat(true, 2013, 6, 2, 1281, 34, 2, 230m, 31.49m, "Apple Computer Ram"),
                new SearchStat(true, 2013, 6, 2, 1002, 23, 0, 0m, 14.13m, "Apple Memory"),
                new SearchStat(true, 2013, 6, 2, 1819, 20, 1, 80m, 15.96m, "Apple Memory Module"),
                new SearchStat(true, 2013, 6, 2, 1295, 11, 0, 0m, 17.31m, "Apple RAM"),
            };
            return stats.AsQueryable();
        }
    }
}
