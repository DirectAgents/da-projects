using ClientPortal.Data.Contracts;
using ClientPortal.Data.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;

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
            spreadsheet.SetReportDate(endDate);
            spreadsheet.SetClientName(searchProfile.SearchProfileName);

            bool useAnalytics = false; //TODO: get from searchProfile when implemented
            var monthlyStats = cpRepo.GetMonthStats(searchProfileId, numMonths, endDate, useAnalytics, searchProfile.ShowCalls);
            var weeklyStats = cpRepo.GetWeekStats(searchProfileId, numWeeks, (DayOfWeek)searchProfile.StartDayOfWeek, endDate, useAnalytics, searchProfile.ShowCalls);

            var propertyNames = new[] { "Title", "Clicks", "Impressions", "Orders", "Cost", "Revenue", "Calls" };
            spreadsheet.LoadMonthlyStats(monthlyStats, propertyNames);
            spreadsheet.LoadWeeklyStats(weeklyStats, propertyNames);

            // Prepare for weekly channel/campaign stats
            // Start with the week that includes endDate (could be a partial week)
            var periodStart = endDate;
            while (periodStart.DayOfWeek != (DayOfWeek)searchProfile.StartDayOfWeek)
                periodStart = periodStart.AddDays(-1);
            var periodEnd = periodStart.AddDays(6);
            spreadsheet.SetReportingPeriod(periodStart, periodEnd); // currently just used for Teacher Express template

            // TODO: determine from the report which stats are needed
            if (spreadsheet is SearchReport_ScholasticTeacherExpress)
            {
                // load the weekly campaign stats, starting with the most recent and going back the number of weeks specified
                for (int i = 0; i < numWeeks; i++)
                {
                    var weeklyCampaignStats = cpRepo.GetCampaignStats(searchProfileId, null, periodStart, periodEnd, false, useAnalytics, searchProfile.ShowCalls);
                    bool collapse = (i > 0);
                    ((SearchReport_ScholasticTeacherExpress)spreadsheet).LoadWeeklyCampaignStats(weeklyCampaignStats, propertyNames, periodStart, collapse);
                    if (i + 1 < numWeeks)
                    {   // not the last week
                        periodStart = periodStart.AddDays(-7);
                        periodEnd = periodEnd.AddDays(-7);
                    }
                    else
                    {
                        // after the last week was loaded... remove the template rows
                        ((SearchReport_ScholasticTeacherExpress)spreadsheet).RemoveWeeklyCampaignStatsTemplateRows();
                    }
                }

                var yesterday = DateTime.Now.AddDays(-1);
                var monthStart = new DateTime(yesterday.Year, yesterday.Month, 1);
                var monthEnd = monthStart.AddMonths(1).AddDays(-1);
                var latestMonthStats = cpRepo.GetCampaignStats(searchProfileId, null, monthStart, monthEnd, false, useAnalytics, searchProfile.ShowCalls);
                ((SearchReport_ScholasticTeacherExpress)spreadsheet).LoadLatestMonthCampaignStats(latestMonthStats, propertyNames, monthStart);
            }
            else
            {
                for (int i = 0; i < numWeeks; i++)
                {
                    var channelStatsDict = new Dictionary<string, IEnumerable<SearchStat>>();

                    int numChannels = searchProfile.SearchAccounts.Select(sa => sa.Channel).Distinct().Count();
                    if (numChannels == 1 && searchProfile.SearchAccounts.Count > 1)
                    { // if there is only one channel (e.g. Google) but multiple SearchAccounts, group campaigns by SearchAccount
                        foreach (var searchAccount in searchProfile.SearchAccounts)
                        {
                            var campaignStats = cpRepo.GetCampaignStats(searchAccount.SearchAccountId, periodStart, periodEnd, false, useAnalytics, searchProfile.ShowCalls);
                            if (campaignStats.Any())
                                channelStatsDict[searchAccount.Name] = campaignStats;
                        }
                    }
                    else
                    { // the "normal" way: group campaigns by channel (Google, Bing, etc)
                        var campaignStats = cpRepo.GetCampaignStats(searchProfileId, null, periodStart, periodEnd, false, useAnalytics, searchProfile.ShowCalls);
                        var channels = campaignStats.Select(s => s.Channel).Distinct();
                        foreach (string channel in channels)
                        {
                            channelStatsDict[channel] = campaignStats.Where(s => s.Channel == channel);
                        }                               // order of campaigns?
                    }

                    if (channelStatsDict.Keys.Any()) // TODO: if empty, somehow generate a row with zeros for this week
                    {
                        bool collapse = (i > 0);
                        spreadsheet.LoadWeeklyChannelRollupStats(channelStatsDict, propertyNames, periodStart, collapse);
                    }
                    if (i + 1 < numWeeks)
                    {   // not the last week
                        periodStart = periodStart.AddDays(-7);
                        periodEnd = periodEnd.AddDays(-7);
                    }
                }
                spreadsheet.RemoveWeeklyChannelRollupStatsTemplate();
            }
            return spreadsheet;
        }
        // DisposeResources when done with SearchReport?

    }
}
