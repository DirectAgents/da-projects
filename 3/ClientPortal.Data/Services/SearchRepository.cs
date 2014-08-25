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
        public SearchStat GetSearchStats(int? advertiserId, DateTime? start, DateTime? end, bool includeToday = true)
        {
            var summaries = GetSearchDailySummaries(advertiserId, null, null, null, null, start, end, includeToday);
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

        private IQueryable<SearchCampaign> GetSearchCampaigns(int? advertiserId, string channel, int? searchAccountId, string channelPrefix)
        {
            var searchCampaigns = context.SearchCampaigns.AsQueryable();

            if (advertiserId.HasValue)
                searchCampaigns = searchCampaigns.Where(c => c.SearchAccount.AdvertiserId == advertiserId.Value);
            if (channel != null)
                searchCampaigns = searchCampaigns.Where(c => c.SearchAccount.Channel == channel);
            if (searchAccountId.HasValue)
                searchCampaigns = searchCampaigns.Where(c => c.SearchAccount.SearchAccountId == searchAccountId.Value);
            if (!String.IsNullOrWhiteSpace(channelPrefix))
                searchCampaigns = searchCampaigns.Where(c => c.SearchCampaignName.StartsWith(channelPrefix));

            return searchCampaigns;
        }

        private IQueryable<SearchDailySummary2> GetSearchDailySummaries(int? advertiserId, string channel, int? searchAccountId, string channelPrefix, string device, DateTime? start, DateTime? end, bool includeToday)
        {
            var searchCampaigns = GetSearchCampaigns(advertiserId, channel, searchAccountId, channelPrefix);
            var summaries = searchCampaigns.SelectMany(c => c.SearchDailySummaries2);

            if (!String.IsNullOrEmpty(device))
                summaries = summaries.Where(s => s.Device == device);

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

        private IQueryable<GoogleAnalyticsSummary> GetGoogleAnalyticsSummaries(int? advertiserId, string channel, int? searchAccountId, string channelPrefix, DateTime? start, DateTime? end, bool includeToday)
        {
            var searchCampaigns = GetSearchCampaigns(advertiserId, channel, searchAccountId, channelPrefix);
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
        public IQueryable<SearchStat> GetWeekStats(int? advertiserId, string channel, int? searchAccountId, string channelPrefix, string device, int? numWeeks, DayOfWeek startDayOfWeek, bool useAnalytics, bool includeToday)
        {
            if (!String.IsNullOrWhiteSpace(device) && useAnalytics)
                throw new Exception("specifying a device and useAnalytics not supported");

            DateTime today = DateTime.Today;
            DateTime start;
            if (numWeeks.HasValue) // If the number of weeks is present
                start = today.AddDays(-7 * numWeeks.Value + 6); // Then set start date to numWeeks weeks ago, plus 6 (works with leap year?)
            //else if (today.Month == 12)
            //    start = new DateTime(today.Year, 1, 1);
            else
                start = today.AddMonths(-11); // Otherwise set the start date to 11 months ago

            // Now move start date back to the closest startDayOfWeek
            while (start.DayOfWeek != startDayOfWeek)
                start = start.AddDays(-1);

            var latestDate = includeToday ? today : today.AddDays(-1);

            var daySums =
                GetSearchDailySummaries(advertiserId, channel, searchAccountId, channelPrefix, device, start, null, includeToday)

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
                var gaSums = GetGoogleAnalyticsSummaries(advertiserId, channel, searchAccountId, channelPrefix, start, null, includeToday)
                    .GroupBy(s => s.Date)
                    .Select(g => new AnalyticsSummary
                    {
                        Date = g.Key,
                        Transactions = g.Sum(x => x.Transactions),
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
                               Orders = (gaSum == null) ? 0 : gaSum.Transactions,
                               Revenue = (gaSum == null) ? 0 : gaSum.Revenue,
                               Cost = daySum.Cost
                           }).ToList();
            }

            var title = channel;
            if (searchAccountId.HasValue)
            {
                var searchAccount = context.SearchAccounts.Find(searchAccountId.Value);
                if (searchAccount != null)
                {
                    title = (title == null) ? "" : title + " - ";
                    title += searchAccount.Name;
                }
            }
            if (!String.IsNullOrWhiteSpace(channelPrefix) || !String.IsNullOrWhiteSpace(device))
            {
                var searchChannel = GetSearchChannel(channelPrefix, device);
                if (title == channel) // no "channel" or searchAccount was specified
                    title = "";       // (don't include "channel" because the searchChannel's name will have it)
                else
                    title += " - "; // just in case both a searchAccount _and_ a searchChannel are specified
                title += (searchChannel != null) ? searchChannel.Name : (channelPrefix ?? ".") + "/" + (device ?? ".");
            }

            var stats = daySums
                    // Reproject with members that break date up into Year and Week
                    .Select(s => new
                    {
                        Date = s.Date,
                        Year = s.Date.Year,
                        Week = CultureInfo.InvariantCulture.Calendar.GetWeekOfYear(s.Date, CalendarWeekRule.FirstFourDayWeek, startDayOfWeek),
                        Impressions = s.Impressions,
                        Clicks = s.Clicks,
                        Orders = s.Orders,
                        Revenue = s.Revenue,
                        Cost = s.Cost
                    })

                    // Now group by Year and Week
                    .GroupBy(x => new { x.Year, x.Week })
                    // NOTE / TODO: There's an issue when a week is part in one year and part in another
                    // After constructing a SearchStat, we could group by Range (or StartDate,EndDate)
                    // but maybe there's another way, like a projection where Year=Year+1 and Week=1 when Week=53 ?

                    // Order by Year then Week
                    .OrderBy(g => g.Key.Year)
                    .ThenBy(g => g.Key.Week)

                    // Finally select new SearchStat objects
                    .Select((g, i) => new SearchStat
                    {
                        WeekStartDay = startDayOfWeek,
                        FillToLatest = latestDate,
                        WeekByMaxDate = g.Max(s => s.Date), // Supply the latest date in the group of dates (which are all part of a week)
                        TitleIfNotNull = title, // (if null, Title is "Range")

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
        public IQueryable<WeeklySearchStat> GetCampaignWeekStats2(int? advertiserId, DateTime startDate, DateTime endDate, DayOfWeek startDayOfWeek, bool useAnalytics)
        {
            var weeks = CalenderWeek.Generate(startDate, endDate, startDayOfWeek);
            var sums = GetSearchDailySummaries(advertiserId, null, null, null, null, startDate, endDate, true)
                .Select(s => new
                {
                    s.Date,
                    s.SearchCampaign.SearchAccount.Channel,
                    s.SearchCampaignId,
                    s.SearchCampaign.SearchCampaignName,
                    s.Orders,
                    s.Revenue,
                    s.Cost
                })
                .AsEnumerable()
                .GroupBy(s => new
                {
                    Week = weeks.First(w => w.EndDate >= s.Date),
                    s.Channel,
                    s.SearchCampaignId,
                    s.SearchCampaignName
                })
                .Select(g => new SearchSummary
                {
                    Week = g.Key.Week,
                    Channel = g.Key.Channel,
                    CampaignId = g.Key.SearchCampaignId,
                    CampaignName = g.Key.SearchCampaignName,
                    Orders = g.Sum(s => s.Orders),
                    Revenue = g.Sum(s => s.Revenue),
                    Cost = g.Sum(s => s.Cost)
                });

            if (useAnalytics)
            {
                var gaStats = GetGoogleAnalyticsSummaries(advertiserId, null, null, null, startDate, endDate, true)
                    .AsEnumerable()
                    .GroupBy(s => new
                    {
                        Week = weeks.First(w => w.EndDate >= s.Date),
                        s.SearchCampaignId
                    })
                    .Select(g => new AnalyticsSummary
                    {
                        CampaignId = g.Key.SearchCampaignId,
                        Week = g.Key.Week,
                        Transactions = g.Sum(s => s.Transactions),
                        Revenue = g.Sum(s => s.Revenue)
                    });
                sums = (from sum in sums
                         join ga in gaStats on new { sum.CampaignId, sum.Week.StartDate } equals new { ga.CampaignId, ga.Week.StartDate } into gj_stats
                         from gaStat in gj_stats.DefaultIfEmpty() // left join to gaStats
                         select new SearchSummary
                         {
                             Week = sum.Week,
                             Channel = sum.Channel,
                             CampaignId = sum.CampaignId,
                             CampaignName = sum.CampaignName,
                             Orders = (gaStat == null) ? 0 : gaStat.Transactions,
                             Revenue = (gaStat == null) ? 0 : gaStat.Revenue,
                             Cost = sum.Cost
                         });
            }

            var stats = sums
                .OrderBy(s => s.Week.StartDate)
                .ThenBy(s => s.CampaignName)
                .Select(s => new WeeklySearchStat
                {
                    StartDate = s.Week.StartDate,
                    EndDate = s.Week.EndDate,
                    Channel = s.Channel,
                    Campaign = s.CampaignName,
                    ROAS = s.Cost == 0 ? 0 : (int)Math.Round(100 * s.Revenue / s.Cost),
                    CPO = s.Orders == 0 ? 0 : Math.Round(s.Cost / s.Orders, 2)
                });
            return stats.AsQueryable();
        }

        public IQueryable<SearchStat> GetMonthStats(int? advertiserId, int? numMonths, bool useAnalytics, bool includeToday)
        {
            DateTime start;
            if (numMonths.HasValue)
                start = DateTime.Today.AddMonths((numMonths.Value - 1) * -1);
            else
                start = DateTime.Today.AddYears(-1);

            start = new DateTime(start.Year, start.Month, 1);

            var stats = GetSearchDailySummaries(advertiserId, null, null, null, null, start, null, includeToday)
                .GroupBy(s => new { s.Date.Year, s.Date.Month })
                .Select(g =>
                new SearchStat
                {
                    MonthByMaxDate = g.Max(s => s.Date),
                    Impressions = g.Sum(s => s.Impressions),
                    Clicks = g.Sum(s => s.Clicks),
                    Orders = g.Sum(s => s.Orders),
                    Revenue = g.Sum(s => s.Revenue),
                    Cost = g.Sum(s => s.Cost)
                })
                .ToList().AsQueryable();

            if (useAnalytics)
            {
                var gaStats = GetGoogleAnalyticsSummaries(advertiserId, null, null, null, start, null, includeToday)
                    .GroupBy(s => new { s.Date.Year, s.Date.Month })
                    .ToList()
                    .Select(g => new SearchSummary
                    {
                        Date = new DateTime(g.Key.Year, g.Key.Month, 1),
                        Orders = g.Sum(s => s.Transactions),
                        Revenue = g.Sum(s => s.Revenue)
                    });

                stats = (from stat in stats
                         join ga in gaStats on stat.StartDate equals ga.Date into gj_stats
                         from gaStat in gj_stats.DefaultIfEmpty() // left join to gaStats
                         select new SearchStat
                         {
                             MonthByMaxDate = stat.EndDate,
                             Impressions = stat.Impressions,
                             Clicks = stat.Clicks,
                             Orders = (gaStat == null) ? 0 : gaStat.Orders,
                             Revenue = (gaStat == null) ? 0 : gaStat.Revenue,
                             Cost = stat.Cost
                         });
            }
            return stats.OrderBy(s => s.StartDate);
        }

        public IQueryable<SearchStat> GetChannelStats(int? advertiserId, DayOfWeek startDayOfWeek, bool useAnalytics, bool includeToday, bool includeAccountBreakdown, bool includeSearchChannels)
        {
            var googleStats = GetWeekStats(advertiserId, "Google", null, null, null, null, startDayOfWeek, useAnalytics, includeToday);
            var bingStats = GetWeekStats(advertiserId, "Bing", null, null, null, null, startDayOfWeek, useAnalytics, includeToday);
            var stats = googleStats.Concat(bingStats).AsQueryable();

            if (advertiserId.HasValue)
            {
                var advertiser = context.Advertisers.SingleOrDefault(a => a.AdvertiserId == advertiserId.Value);
                if (advertiser != null && includeAccountBreakdown)
                {
                    var channels = new string[] { "Google", "Bing" };
                    foreach (var channel in channels)
                    {
                        var searchAccounts = advertiser.SearchAccounts.Where(sa => sa.Channel == channel);
                        if (searchAccounts.Count() > 1)
                        {
                            foreach (var searchAccount in searchAccounts)
                            {
                                var accountStats = GetWeekStats(advertiserId, channel, searchAccount.SearchAccountId, null, null, null, startDayOfWeek, useAnalytics, includeToday);
                                stats = stats.Concat(accountStats);
                            }
                        }
                    }
                }
                if (advertiser != null && includeSearchChannels)
                {
                    foreach (var searchChannel in this.SearchChannels)
                    {
                        if (!String.IsNullOrWhiteSpace(searchChannel.Device) && useAnalytics)
                            continue; // specifying device and useAnalytics not supported; analytics summaries are not broken down by device

                        var channelStats = GetWeekStats(advertiserId, null, null, searchChannel.Prefix, searchChannel.Device, null, startDayOfWeek, useAnalytics, includeToday);
                        if (channelStats.Count() > 0)
                            stats = stats.Concat(channelStats);
                    }
                }
            }
            return stats;
        }

        public IQueryable<SearchStat> GetCampaignStats(int? advertiserId, string channel, DateTime? start, DateTime? end, bool breakdown, bool useAnalytics)
        {
            if (!start.HasValue)
                start = new DateTime(DateTime.Today.Year, 1, 1);

            string channelPrefix = null;
            string device = null;
            int? searchAccountId = null;
            if (channel != null)
            {
                if (channel.StartsWith("~")) // for a specific SearchChannel
                {
                    var searchChannel = GetSearchChannelByName(channel);
                    channel = null;
                    if (searchChannel != null)
                    {
                        channelPrefix = searchChannel.Prefix;
                        device = searchChannel.Device;
                        if (channelPrefix == null)
                        {
                            if (searchChannel.Name.Contains("Google") && !searchChannel.Name.Contains("Bing"))
                                channel = "Google";
                            if (!searchChannel.Name.Contains("Google") && searchChannel.Name.Contains("Bing"))
                                channel = "Bing";
                        }
                    }
                    else
                    {
                        throw new Exception("SearchChannel not found");
                    }
                }
                else if (channel.Contains(" - ")) // for a specific SearchAccount
                {
                    int i = channel.IndexOf(" - ");
                    string accountName = channel.Substring(i + 3);
                    channel = channel.Substring(0, i);
                    var searchAccount = context.SearchAccounts.SingleOrDefault(sa => sa.Channel == channel && sa.Name == accountName);
                    if (searchAccount != null)
                        searchAccountId = searchAccount.SearchAccountId;
                }
            }

            var summaries = GetSearchDailySummaries(advertiserId, channel, searchAccountId, channelPrefix, device, start, end, true).ToList();
            IQueryable<SearchStat> stats;
            if (breakdown)
            {
                // TODO: figure out how to join to gaStats if useAnalytics==true

                stats = summaries.GroupBy(s => new { s.SearchCampaign.SearchAccount.Channel, s.SearchCampaign.SearchCampaignName, s.Network, s.Device, s.ClickType })
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
                    }).AsQueryable();
            }
            else
            {
                var sums = summaries.GroupBy(s => new { s.SearchCampaign.SearchAccount.Channel, s.SearchCampaignId, s.SearchCampaign.SearchCampaignName })
                    .Select(g => new SearchSummary
                    {
                        Channel = g.Key.Channel,
                        CampaignId = g.Key.SearchCampaignId,
                        CampaignName = g.Key.SearchCampaignName,
                        Impressions = g.Sum(s => s.Impressions),
                        Clicks = g.Sum(s => s.Clicks),
                        Orders = g.Sum(s => s.Orders),
                        Revenue = g.Sum(s => s.Revenue),
                        Cost = g.Sum(s => s.Cost)
                    });
                if (!useAnalytics)
                {
                    stats = sums//.OrderBy(s => s.Channel).ThenBy(s => s.CampaignName)
                        .Select(s => new SearchStat
                        {
                            EndDate = end.Value,
                            CustomByStartDate = start.Value,
                            Channel = s.Channel,
                            Title = s.CampaignName,
                            Impressions = s.Impressions,
                            Clicks = s.Clicks,
                            Orders = s.Orders,
                            Revenue = s.Revenue,
                            Cost = s.Cost
                        }).AsQueryable();
                }
                else // using Analytics...
                {
                    var gaSums = GetGoogleAnalyticsSummaries(advertiserId, channel, searchAccountId, channelPrefix, start, end, true)
                        .GroupBy(s => s.SearchCampaignId)
                        .Select(g => new AnalyticsSummary
                        {
                            CampaignId = g.Key,
                            Transactions = g.Sum(s => s.Transactions),
                            Revenue = g.Sum(s => s.Revenue)
                        }).ToList();
                    stats = (from sum in sums
                             join ga in gaSums on sum.CampaignId equals ga.CampaignId into gj_sums
                             from gaSum in gj_sums.DefaultIfEmpty() // left join to gaSums
                             select new SearchStat
                             {
                                 EndDate = end.Value,
                                 CustomByStartDate = start.Value,
                                 Channel = sum.Channel,
                                 Title = sum.CampaignName,
                                 Impressions = sum.Impressions,
                                 Clicks = sum.Clicks,
                                 Orders = (gaSum == null) ? 0 : gaSum.Transactions,
                                 Revenue = (gaSum == null) ? 0 : gaSum.Revenue,
                                 Cost = sum.Cost
                             }).AsQueryable();
                }
            }
            return stats.OrderByDescending(s => s.Revenue);
        }

        private List<SearchChannel> _searchChannels;
        private List<SearchChannel> SearchChannels
        {
            get
            {
                if (_searchChannels == null)
                    _searchChannels = context.SearchChannels.ToList();
                return _searchChannels;
            }
        }
        private SearchChannel GetSearchChannel(string prefix, string device)
        {
            var searchChannel = this.SearchChannels.SingleOrDefault(sc => sc.Prefix == prefix && sc.Device == device);
            return searchChannel;
        }
        private SearchChannel GetSearchChannelByName(string name)
        {
            var searchChannel = this.SearchChannels.SingleOrDefault(sc => sc.Name == name);
            return searchChannel;
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

    class AnalyticsSummary
    {
        public int CampaignId { get; set; }
        public DateTime Date { get; set; }
        public CalenderWeek Week { get; set; }
        public int Transactions { get; set; }
        public decimal Revenue { get; set; }
    }

    class SearchSummary
    {
        public string Channel { get; set; }
        public int CampaignId { get; set; }
        public string CampaignName { get; set; }
        public DateTime Date { get; set; }
        public CalenderWeek Week { get; set; }
        public int Impressions { get; set; }
        public int Clicks { get; set; }
        public int Orders { get; set; }
        public decimal Revenue { get; set; }
        public decimal Cost { get; set; }
    }

    //class SummaryPair
    //{
    //    public SearchDailySummary2 Summary { get; set; }
    //    public GoogleAnalyticsSummary GaSummary { get; set; }
    //}
}
