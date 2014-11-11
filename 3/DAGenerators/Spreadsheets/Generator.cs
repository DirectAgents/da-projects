﻿using ClientPortal.Data.Contracts;
using ClientPortal.Data.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DAGenerators.Spreadsheets
{
    public class Generator
    {
        public static SearchReportPPC GenerateSearchReport(IClientPortalRepository cpRepo, string templateFolder, int searchProfileId, int numWeeks, int numMonths)
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
                spreadsheet = new SearchReportPPC();
            else
                spreadsheet = (SearchReportPPC)Activator.CreateInstance(type);

            spreadsheet.Setup(templateFolder);
            spreadsheet.SetClientName(searchProfile.SearchProfileName);

            bool useAnalytics = false; //TODO: get from searchProfile when implemented
            bool includeToday = false;
            var monthlyStats = cpRepo.GetMonthStats(searchProfileId, numMonths, useAnalytics, includeToday);
            var weeklyStats = cpRepo.GetWeekStats(searchProfileId, numWeeks, (DayOfWeek)searchProfile.StartDayOfWeek, null, useAnalytics);

            var propertyNames = new[] { "Title", "Clicks", "Impressions", "Orders", "Cost", "Revenue" };
            spreadsheet.LoadMonthlyStats(monthlyStats, propertyNames);
            spreadsheet.LoadWeeklyStats(weeklyStats, propertyNames);

            var periodStart = DateTime.Today.AddDays(-7);
            while (periodStart.DayOfWeek != (DayOfWeek)searchProfile.StartDayOfWeek)
                periodStart = periodStart.AddDays(-1);
            var periodEnd = periodStart.AddDays(6);
            spreadsheet.SetReportingPeriod(periodStart, periodEnd);

            // TODO: determine from the report which stats are needed
            if (spreadsheet is SearchReport_ScholasticTeacherExpress)
            {
                // load the weekly campaign stats, starting with the most recent and going back the number of weeks specified
                for (int i = 0; i < numWeeks; i++)
                {
                    var weeklyCampaignStats = cpRepo.GetCampaignStats(searchProfileId, null, periodStart, periodEnd, false, useAnalytics);
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
                var latestMonthStats = cpRepo.GetCampaignStats(searchProfileId, null, monthStart, monthEnd, false, useAnalytics);
                ((SearchReport_ScholasticTeacherExpress)spreadsheet).LoadLatestMonthCampaignStats(latestMonthStats, propertyNames, monthStart);
            }
            else
            {
                for (int i = 0; i < numWeeks; i++)
                {
                    var channelStatsDict = new Dictionary<string, IEnumerable<SearchStat>>();
                    bool collapse = (i > 0);
                    var campaignStats = cpRepo.GetCampaignStats(searchProfileId, null, periodStart, periodEnd, false, useAnalytics);
                    //var totalStats = 
                    var channels = campaignStats.Select(s => s.Channel).Distinct();
                    foreach (string channel in channels)
                    {
                        channelStatsDict[channel] = campaignStats.Where(s => s.Channel == channel);
                    }                               // order of campaigns?

                    spreadsheet.LoadWeeklyChannelRollupStats(channelStatsDict, propertyNames, periodStart, collapse);

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
