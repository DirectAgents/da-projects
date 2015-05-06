using System;
using System.Collections.Generic;
using System.Linq;
using ClientPortal.Data.Contracts;
using ClientPortal.Data.DTOs;

namespace DAGenerators.Spreadsheets
{
    public class Generator
    {
        public static SearchReportPPC GenerateSearchReport(IClientPortalRepository cpRepo, string templateFolder, int searchProfileId, int numWeeks, int numMonths, DateTime endDate)
        {
            var searchProfile = cpRepo.GetSearchProfile(searchProfileId);
            if (searchProfile == null)
                return null;

            var profileAbbrev = searchProfile.SearchProfileName.Replace(" ", "");
            var className = "SearchReport_" + profileAbbrev;

            SearchReportPPC spreadsheet;
            var baseType = typeof(SearchReportPPC);
            var type = baseType.Assembly.GetTypes().FirstOrDefault(t => t.IsSubclassOf(baseType) && t.Name == className);

            if (type == null)
            {
                if (searchProfile.ShowRevenue) // eCom/Retail
                {
                    spreadsheet = new SearchReportPPC();
                }
                else // LeadGen
                {
                    if (searchProfile.ShowCalls)
                        spreadsheet = new SearchReportLeadGenWithCalls();
                    else
                        spreadsheet = new SearchReportLeadGen();
                }
            }
            else // custom
            {
                spreadsheet = (SearchReportPPC)Activator.CreateInstance(type);
            }

            spreadsheet.Setup(templateFolder);
            if (!searchProfile.ShowViewThrus)
            {
                spreadsheet.MakeColumnHidden(spreadsheet.Metric_ViewThrus);
                spreadsheet.MakeColumnHidden(spreadsheet.Metric_ViewThruRev);
            }
            if (!searchProfile.ShowCassConvs)
            {
                spreadsheet.MakeColumnHidden(spreadsheet.Metric_CassConvs);
                spreadsheet.MakeColumnHidden(spreadsheet.Metric_CassConVal);
            }
            spreadsheet.SetReportDate(endDate);
            spreadsheet.SetClientName(searchProfile.SearchProfileName);

            var monthlyStats = cpRepo.GetMonthStats(searchProfile, numMonths, endDate);
            var weeklyStats = cpRepo.GetWeekStats(searchProfile, numWeeks, endDate);

            var propertyNames = new[] { "Title", "Clicks", "Impressions", "Orders", "Cost", "Revenue", "Calls", "ViewThrus", "CassConvs", "CassConVal" };
            spreadsheet.LoadMonthlyStats(monthlyStats, propertyNames);
            spreadsheet.LoadWeeklyStats(weeklyStats, propertyNames);

            // Year-Over-Year - for the most recent completed month
            DateTime today = DateTime.Today;
            DateTime monthStart = new DateTime(today.Year, today.Month, 1);
            DateTime monthEnd = monthStart.AddDays(-1);
            monthStart = monthStart.AddMonths(-1);

            var monthStats = cpRepo.GetSearchStats(searchProfile, monthStart, monthEnd, false);
            monthStats.Title = monthStart.ToString("MMM-yy");

            monthStart = monthStart.AddYears(-1);
            monthEnd = monthStart.AddMonths(1).AddDays(-1);
            var monthStatsLastYear = cpRepo.GetSearchStats(searchProfile, monthStart, monthEnd, false);
            monthStatsLastYear.Title = monthStart.ToString("MMM-yy");

            var yoyStats = new[] { monthStatsLastYear, monthStats };
            spreadsheet.LoadYearOverYearStats(yoyStats, propertyNames);

            // Prepare for weekly channel/campaign stats
            // Start with the week that includes endDate (could be a partial week)
            var periodStart = endDate;
            while (periodStart.DayOfWeek != (DayOfWeek)searchProfile.StartDayOfWeek)
                periodStart = periodStart.AddDays(-1);
            var periodEnd = periodStart.AddDays(6);
            spreadsheet.SetReportingPeriod(periodStart, periodEnd); // currently just used for Teacher Express template

            // load the weekly campaign stats, starting with the most recent and going back the number of weeks specified
            for (int i = 0; i < numWeeks; i++)
            {
                var channelStatsDict = new Dictionary<string, IEnumerable<SearchStat>>();

                int numChannels = searchProfile.SearchAccounts.Select(sa => sa.Channel).Distinct().Count();
                if (numChannels == 1 && searchProfile.SearchAccounts.Count > 1)
                { // if there is only one channel (e.g. Google) but multiple SearchAccounts, group campaigns by SearchAccount
                    foreach (var searchAccount in searchProfile.SearchAccounts)
                    {
                        var campaignStats = cpRepo.GetCampaignStats(searchProfile, searchAccount.SearchAccountId, periodStart, periodEnd, false);
                        if (campaignStats.Any())
                            channelStatsDict[searchAccount.Name] = campaignStats;
                    }
                }
                else
                { // the "normal" way: group campaigns by channel (Google, Bing, etc)
                    var campaignStats = cpRepo.GetCampaignStats(searchProfile, null, periodStart, periodEnd, false);
                    var channels = campaignStats.Select(s => s.Channel).Distinct();
                    foreach (string channel in channels)
                    {
                        channelStatsDict[channel] = campaignStats.Where(s => s.Channel == channel);
                    }                               // order of campaigns?
                }

                if (channelStatsDict.Keys.Any()) // TODO: if empty, somehow generate a row with zeros for this week
                {
                    bool collapse = (i > 0);
                    spreadsheet.LoadWeeklyCampaignPerfStats(channelStatsDict, propertyNames, periodStart, collapse);
                }
                if (i + 1 < numWeeks)
                {   // not the last week
                    periodStart = periodStart.AddDays(-7);
                    periodEnd = periodEnd.AddDays(-7);
                }
            }
            spreadsheet.RemoveWeeklyChannelRollupStatsTemplate();

            return spreadsheet;
        }
        // DisposeResources when done with SearchReport?

    }
}
