using ClientPortal.Data.Contexts;
using ClientPortal.Data.DTOs;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;

namespace ClientPortal.Data.Services
{
    public partial class ClientPortalRepository
    {
        public IQueryable<SearchProfile> SearchProfiles()
        {
            var searchProfiles = context.SearchProfiles;
            return searchProfiles;
        }

        public SearchProfile GetSearchProfile(int searchProfileId)
        {
            var searchProfile = context.SearchProfiles.Find(searchProfileId);
            return searchProfile;
        }
        public void SaveSearchProfile(SearchProfile searchProfile)
        {
            var entry = context.Entry(searchProfile);
            entry.State = EntityState.Modified;
            context.SaveChanges();
        }

        // by default, report goes to first contact
        public bool InitializeSearchProfileSimpleReport(int searchProfileId, string email = null)
        {
            bool success = false;
            var searchProfile = GetSearchProfile(searchProfileId);
            if (searchProfile != null && !searchProfile.SimpleReports.Any())
            {
                if (String.IsNullOrWhiteSpace(email))
                {
                    var spContacts = searchProfile.SearchProfileContactsOrdered;
                    if (spContacts.Any())
                        email = spContacts.First().Contact.Email;
                }
                if (!String.IsNullOrWhiteSpace(email))
                {
                    var simpleReport = new SimpleReport
                    {
                        Email = email,
                        PeriodDays = 7,
                        Enabled = false
                    };
                    searchProfile.SimpleReports.Add(simpleReport);
                    context.SaveChanges();
                    success = true;
                }
            }
            return success;
        }

        // --- Search Stats ---

        public SearchStat GetSearchStats(int searchProfileId, DateTime? start, DateTime? end, bool includeToday = true)
        {
            var summaries = GetSearchDailySummaries(null, searchProfileId, null, null, null, null, start, end, includeToday);
            bool any = summaries.Any();
            var searchStat = new SearchStat
            {
                EndDate = end.Value,
                CustomByStartDate = start.Value,
                Impressions = !any ? 0 : summaries.Sum(s => s.Impressions),
                Clicks = !any ? 0 : summaries.Sum(s => s.Clicks),
                Orders = !any ? 0 : summaries.Sum(s => s.Orders),
                Revenue = !any ? 0 : summaries.Sum(s => s.Revenue),
                Cost = !any ? 0 : summaries.Sum(s => s.Cost)
            };

            return searchStat;
        }

        private IQueryable<SearchCampaign> GetSearchCampaigns(int? advertiserId, int? searchProfileId, string channel, int? searchAccountId, string channelPrefix)
        {
            var searchCampaigns = context.SearchCampaigns.AsQueryable();

            if (advertiserId.HasValue)
                searchCampaigns = searchCampaigns.Where(c => c.SearchAccount.AdvertiserId == advertiserId.Value);
            if (searchProfileId.HasValue)
                searchCampaigns = searchCampaigns.Where(c => c.SearchAccount.SearchProfileId == searchProfileId.Value);
            if (channel != null)
                searchCampaigns = searchCampaigns.Where(c => c.SearchAccount.Channel == channel);
            if (searchAccountId.HasValue)
            {
                searchCampaigns = searchCampaigns.Where(c =>
                                    (!c.AltSearchAccountId.HasValue && c.SearchAccountId.HasValue && c.SearchAccountId.Value == searchAccountId.Value)
                                 || (c.AltSearchAccountId.HasValue && c.AltSearchAccountId.Value == searchAccountId.Value));
                // (if an AltSearchAccountId is specified, use that instead of the campaign's SearchAccountId)
            }
            if (!String.IsNullOrWhiteSpace(channelPrefix))
                searchCampaigns = searchCampaigns.Where(c => c.SearchCampaignName.StartsWith(channelPrefix));

            return searchCampaigns;
        }

        private IQueryable<SearchDailySummary2> GetSearchDailySummaries(int? advertiserId, int? searchProfileId, string channel, int? searchAccountId, string channelPrefix, string device, DateTime? start, DateTime? end, bool includeToday)
        {
            var searchCampaigns = GetSearchCampaigns(advertiserId, searchProfileId, channel, searchAccountId, channelPrefix);
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

        private IQueryable<GoogleAnalyticsSummary> GetGoogleAnalyticsSummaries(int? advertiserId, int? searchProfileId, string channel, int? searchAccountId, string channelPrefix, DateTime? start, DateTime? end, bool includeToday)
        {
            var searchCampaigns = GetSearchCampaigns(advertiserId, searchProfileId, channel, searchAccountId, channelPrefix);
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

        // if endDate is null, goes up to the latest complete week
        public IQueryable<SearchStat> GetWeekStats(int searchProfileId, int numWeeks, DayOfWeek startDayOfWeek, DateTime? endDate, bool useAnalytics)
        {
            return GetWeekStats(null, searchProfileId, null, null, null, null, numWeeks, startDayOfWeek, endDate, useAnalytics);
        }

        private IQueryable<SearchStat> GetWeekStats(int? advertiserId, int? searchProfileId, string channel, int? searchAccountId, string channelPrefix, string device, int numWeeks, DayOfWeek startDayOfWeek, DateTime? endDate, bool useAnalytics)
        {
            if (!String.IsNullOrWhiteSpace(device) && useAnalytics)
                throw new Exception("specifying a device and useAnalytics not supported");

            if (!endDate.HasValue)
            {
                // Start with yesterday and go back until it's the end of the latest full week
                endDate = DateTime.Today.AddDays(-1);
                while (endDate.Value.AddDays(1).DayOfWeek != startDayOfWeek)
                    endDate = endDate.Value.AddDays(-1);
            }

            // Go back 7 weeks from endDate, then forward 1 day, so it's the right number of weeks, inclusive of start and endDate
            DateTime startDate = endDate.Value.AddDays(-7 * numWeeks + 1);

            // Now move start date back to the closest startDayOfWeek (there may be a partial week now, at the end)
            // (Will only apply if endDate was set to a day other than at the end of the week)
            while (startDate.DayOfWeek != startDayOfWeek)
                startDate = startDate.AddDays(-1);

            var daySums =
                GetSearchDailySummaries(advertiserId, searchProfileId, channel, searchAccountId, channelPrefix, device, startDate, endDate, true)

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
                var gaSums = GetGoogleAnalyticsSummaries(advertiserId, searchProfileId, channel, searchAccountId, channelPrefix, startDate, endDate, true)
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

            var adjuster = new YearWeekAdjuster
            {
                StartDayOfWeek = startDayOfWeek,
                CalendarWeekRule = CalendarWeekRule.FirstFullWeek
            };

            var stats = daySums
                    // Reproject with members that break date up into Year and Week
                    .Select(s => new
                    {
                        Date = s.Date,
                        Year = adjuster.GetYearAdjustedByWeek(s.Date),
                        Week = CultureInfo.InvariantCulture.Calendar.GetWeekOfYear(s.Date, adjuster.CalendarWeekRule, startDayOfWeek),
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
                        WeekStartDay = startDayOfWeek,
                        FillToLatest = endDate, // Acts as a boolean except for the most recent week
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
        public IQueryable<WeeklySearchStat> GetCampaignWeekStats2(int searchProfileId, int numWeeks, bool includeToday, DayOfWeek startDayOfWeek, bool useAnalytics)
        {
            DateTime start = DateTime.Today.AddDays(-7 * numWeeks + 6); // Set start date to numWeeks weeks ago, plus 6
            // Now move start date back to the closest startDayOfWeek
            while (start.DayOfWeek != startDayOfWeek)
                start = start.AddDays(-1);

            DateTime end = DateTime.Today;
            if (!includeToday)
                end = end.AddDays(-1);

            return GetCampaignWeekStats2(searchProfileId, start, end, startDayOfWeek, useAnalytics);
        }
        public IQueryable<WeeklySearchStat> GetCampaignWeekStats2(int searchProfileId, DateTime startDate, DateTime endDate, DayOfWeek startDayOfWeek, bool useAnalytics)
        {
            var weeks = CalenderWeek.Generate(startDate, endDate, startDayOfWeek);
            var sums = GetSearchDailySummaries(null, searchProfileId, null, null, null, null, startDate, endDate, true)
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
                var gaStats = GetGoogleAnalyticsSummaries(null, searchProfileId, null, null, null, startDate, endDate, true)
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

        public IQueryable<SearchStat> GetMonthStats(int searchProfileId, int? numMonths, bool useAnalytics, bool includeToday)
        {
            DateTime start;
            if (numMonths.HasValue)
                start = DateTime.Today.AddMonths((numMonths.Value - 1) * -1);
            else
                start = DateTime.Today.AddYears(-1);

            start = new DateTime(start.Year, start.Month, 1);

            var stats = GetSearchDailySummaries(null, searchProfileId, null, null, null, null, start, null, includeToday)
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
                var gaStats = GetGoogleAnalyticsSummaries(null, searchProfileId, null, null, null, start, null, includeToday)
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

        // Get a SearchStat summary for each week for each channel (Google/Bing/etc)... and, if includeAccountBreakdown, each SearchAccount
        public IQueryable<SearchStat> GetChannelStats(int searchProfileId, int numWeeks, DayOfWeek startDayOfWeek, bool useAnalytics, bool includeToday, bool includeAccountBreakdown, bool includeSearchChannels)
        {
            var searchProfile = GetSearchProfile(searchProfileId);
            DateTime endDate = includeToday ? DateTime.Today : DateTime.Today.AddDays(-1);

            bool includeMainChannels = true; // (e.g. Google/Bing/etc)
            //if (includeAccountBreakdown)
            //{ // Don't include main channels if we're including a breakdown by account and there is only one main channel
            //    var mainChannels = searchProfile.SearchAccounts.Select(sa => sa.Channel).Distinct();
            //    if (mainChannels.Count() <= 1)
            //        includeMainChannels = false;
            //}
            IQueryable<SearchStat> stats = new List<SearchStat>().AsQueryable();
            if (includeMainChannels)
            {
                var googleStats = GetWeekStats(null, searchProfileId, "Google", null, null, null, numWeeks, startDayOfWeek, endDate, useAnalytics);
                var bingStats = GetWeekStats(null, searchProfileId, "Bing", null, null, null, numWeeks, startDayOfWeek, endDate, useAnalytics);
                var criteoStats = GetWeekStats(null, searchProfileId, "Criteo", null, null, null, numWeeks, startDayOfWeek, endDate, useAnalytics);
                stats = googleStats.Concat(bingStats).Concat(criteoStats).AsQueryable();
            }
            if (includeAccountBreakdown)
            {
                var channels = new string[] { "Google", "Bing", "Criteo" };
                foreach (var channel in channels)
                {
                    var searchAccounts = searchProfile.SearchAccounts.Where(sa => sa.Channel == channel);
                    if (searchAccounts.Count() > 1)
                    {
                        foreach (var searchAccount in searchAccounts)
                        {
                            var accountStats = GetWeekStats(null, searchProfileId, channel, searchAccount.SearchAccountId, null, null, numWeeks, startDayOfWeek, endDate, useAnalytics);
                            stats = stats.Concat(accountStats);
                        }
                    }
                }
            }
            if (includeSearchChannels)
            {
                foreach (var searchChannel in this.SearchChannels)
                {
                    if (!String.IsNullOrWhiteSpace(searchChannel.Device) && useAnalytics)
                        continue; // specifying device and useAnalytics not supported; analytics summaries are not broken down by device

                    var channelStats = GetWeekStats(null, searchProfileId, null, null, searchChannel.Prefix, searchChannel.Device, numWeeks, startDayOfWeek, endDate, useAnalytics);
                    if (channelStats.Count() > 0)
                        stats = stats.Concat(channelStats);
                }
            }
            return stats;
        }

        // Get a SearchStat summary for each individual campaign (or if breakdown, one for each Network/Device/ClickType combo)
        public IQueryable<SearchStat> GetCampaignStats(int searchProfileId, string channel, DateTime? start, DateTime? end, bool breakdown, bool useAnalytics)
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

            var summaries = GetSearchDailySummaries(null, searchProfileId, channel, searchAccountId, channelPrefix, device, start, end, true).ToList();
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
                    var gaSums = GetGoogleAnalyticsSummaries(null, searchProfileId, channel, searchAccountId, channelPrefix, start, end, true)
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

        //public IQueryable<SearchStat> GetAdgroupStats()
        //{
        //    var stats = new List<SearchStat>
        //    {
        //        new SearchStat(true, 2013, 6, 2, 1281, 34, 2, 230m, 31.49m, "Apple Computer Ram"),
        //        new SearchStat(true, 2013, 6, 2, 1002, 23, 0, 0m, 14.13m, "Apple Memory"),
        //        new SearchStat(true, 2013, 6, 2, 1819, 20, 1, 80m, 15.96m, "Apple Memory Module"),
        //        new SearchStat(true, 2013, 6, 2, 1295, 11, 0, 0m, 17.31m, "Apple RAM"),
        //    };
        //    return stats.AsQueryable();
        //}

        // --- Private methods ---

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

    }

    class YearWeekAdjuster
    {
        public DayOfWeek StartDayOfWeek { get; set; }
        public CalendarWeekRule CalendarWeekRule { get; set; }

        public int GetYearAdjustedByWeek(DateTime date)
        {
            int year = date.Year;
            if (date.Month == 1) // adjustment only needed in January
            {
                int week = CultureInfo.InvariantCulture.Calendar.GetWeekOfYear(date, this.CalendarWeekRule, this.StartDayOfWeek);
                if (week >= 52)
                    year = year - 1;
            }
            return year;
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
