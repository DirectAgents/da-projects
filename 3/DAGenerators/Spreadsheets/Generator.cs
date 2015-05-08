using System;
using System.Collections.Generic;
using System.Linq;
using ClientPortal.Data.Contexts;
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

            bool partialMonth = (endDate.AddDays(1).Day > 1); // it's a full month if the day after endDate is the 1st

            var propertyNames = new[] { "Title", "Clicks", "Impressions", "Orders", "Cost", "Revenue", "Calls", "ViewThrus", "CassConvs", "CassConVal" };
            var weeklyStats = cpRepo.GetWeekStats(searchProfile, numWeeks, endDate);
            var monthlyStats = cpRepo.GetMonthStats(searchProfile, numMonths, endDate);
            spreadsheet.LoadWeeklyStats(weeklyStats, propertyNames);
            spreadsheet.LoadMonthlyStats(monthlyStats, propertyNames);

            if (weeklyStats.Count() > 0)
                spreadsheet.CreateCharts(true);
            else if (monthlyStats.Count() > 0)
                spreadsheet.CreateCharts(false);

            // Year-Over-Year - for the most recent completed month
            DateTime monthStart = new DateTime(endDate.Year, endDate.Month, 1); // temp value
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

            // Monthly campaign performance stats...
            // start with the most recent and go back the number of months specified
            var periodStart = new DateTime(endDate.Year, endDate.Month, 1); // (the first could be a partial month)
            var periodEnd = endDate;
            for (int i = 0; i < numMonths; i++)
            {
                var campaignStatsDict = GetOneCampaignStatsDict(cpRepo, searchProfile, periodStart, periodEnd);

                if (campaignStatsDict.Keys.Any()) // TODO: if empty, somehow generate a row with zeros for this week
                {
                    bool collapse = (i > 0);
                    spreadsheet.LoadMonthlyCampaignPerfStats(campaignStatsDict, propertyNames, collapse, periodStart, periodEnd);
                }
                periodEnd = periodStart.AddDays(-1);
                periodStart = periodStart.AddMonths(-1);
            }
            spreadsheet.CampaignPerfStatsCleanup(true);

            // Weekly campaign performance stats...
            periodStart = endDate; // Start with the week that includes endDate (could be a partial week)
            while (periodStart.DayOfWeek != (DayOfWeek)searchProfile.StartDayOfWeek)
                periodStart = periodStart.AddDays(-1);
            periodEnd = endDate;

            spreadsheet.SetReportingPeriod(periodStart, periodEnd); // currently just used for Teacher Express template
            // TODO: implement showing reporting period as latest week or latest month

            // Load the weekly campaign stats, starting with the most recent and going back the number of weeks specified
            for (int i = 0; i < numWeeks; i++)
            {
                var campaignStatsDict = GetOneCampaignStatsDict(cpRepo, searchProfile, periodStart, periodEnd);

                if (campaignStatsDict.Keys.Any()) // TODO: if empty, somehow generate a row with zeros for this week
                {
                    bool collapse = (i > 0);
                    spreadsheet.LoadWeeklyCampaignPerfStats(campaignStatsDict, propertyNames, collapse, periodStart, periodEnd);
                }
                periodEnd = periodStart.AddDays(-1);
                periodStart = periodStart.AddDays(-7);
            }
            spreadsheet.CampaignPerfStatsCleanup(false);

            return spreadsheet;
        }

        // Get stats for one week/month/etc - grouped by channel or searchAccount
        private static Dictionary<string, IEnumerable<SearchStat>> GetOneCampaignStatsDict(IClientPortalRepository cpRepo, SearchProfile searchProfile, DateTime periodStart, DateTime periodEnd)
        {
            var campaignStatsDict = new Dictionary<string, IEnumerable<SearchStat>>();

            int numChannels = searchProfile.SearchAccounts.Select(sa => sa.Channel).Distinct().Count();
            if (numChannels == 1 && searchProfile.SearchAccounts.Count > 1)
            { // if there is only one channel (e.g. Google) but multiple SearchAccounts, group campaigns by SearchAccount
                foreach (var searchAccount in searchProfile.SearchAccounts)
                {
                    var campaignStats = cpRepo.GetCampaignStats(searchProfile, searchAccount.SearchAccountId, periodStart, periodEnd, false, searchProfile.ShowCassConvs);
                    if (campaignStats.Any())
                        campaignStatsDict[searchAccount.Name] = SortCampaignStats(searchProfile, campaignStats);
                }
            }
            else
            { // the "normal" way: group campaigns by channel (Google, Bing, etc)
                var campaignStats = cpRepo.GetCampaignStats(searchProfile, null, periodStart, periodEnd, false, searchProfile.ShowCassConvs);
                var channels = campaignStats.Select(s => s.Channel).Distinct();
                foreach (string channel in channels)
                {
                    campaignStatsDict[channel] = SortCampaignStats(searchProfile, campaignStats.Where(s => s.Channel == channel));
                }
            }
            return campaignStatsDict;
        }

        private static IQueryable<SearchStat> SortCampaignStats(SearchProfile searchProfile, IQueryable<SearchStat> campaignStats)
        {
            if (searchProfile.ShowRevenue)
                return campaignStats.OrderByDescending(s => s.Revenue).ThenByDescending(s => s.Cost).ThenByDescending(s => s.Impressions);
            else
                return campaignStats; // already ordered by title (campaign name)
        }

        // DisposeResources when done with SearchReport?
    }
}
