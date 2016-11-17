using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using ClientPortal.Data.Contexts;
using ClientPortal.Data.Contracts;
using ClientPortal.Data.DTOs;
using ClientPortal.Web.Areas.Admin.Models;
using ClientPortal.Web.Controllers;
using DAGenerators.Spreadsheets;

namespace ClientPortal.Web.Areas.Admin.Controllers
{
    [Authorize(Users = "admin")]
    public class SearchStatsController : CPController
    {
        private const int DEFAULT_NUMPERIODS = 16;
        private const int DEFAULT_NUMROWS = 1000;

        public SearchStatsController(IClientPortalRepository cpRepository)
        {
            cpRepo = cpRepository;
        }

        public ActionResult Generic(int spId, DateTime? endDate, string statsType, string interval, int numPeriods = DEFAULT_NUMPERIODS, bool includeConVals = false, bool groupBySearchAccount = false, int numRows = DEFAULT_NUMROWS)
        {
            if (!endDate.HasValue)
                endDate = DateTime.Today.AddDays(-1); // if not specified; (user can always set endDate to today if desired)
            //endDate = (UserSettings.Search_UseYesterdayAsLatest ? DateTime.Today.AddDays(-1) : DateTime.Today);
            statsType = statsType.ToLower();
            interval = interval.ToLower();

            if (statsType == "overall")
            {
                return WeeklyMonthly(spId, (interval == "monthly"), endDate, numPeriods, includeConVals, numRows);
            }
            else if (statsType == "campaign")
            {
                return WeeklyMonthlyBreakdown(spId, (interval == "monthly"), endDate, numPeriods, includeConVals, groupBySearchAccount, numRows);
            }
            return HttpNotFound();
        }

        public ActionResult WeeklyMonthly(int spId, bool monthlyNotWeekly, DateTime? endDate, int numPeriods = DEFAULT_NUMPERIODS, bool includeConVals = false, int numRows = DEFAULT_NUMROWS)
        {
            var searchProfile = cpRepo.GetSearchProfile(spId);
            if (searchProfile == null)
                return HttpNotFound();

            // Determine what the conversion types are & set up ColumnConfigs for them.
            IQueryable<SearchConvType> convTypes;
            if (monthlyNotWeekly)
                convTypes = cpRepo.GetConversionTypesForMonthStats(searchProfile, numPeriods, null, endDate);
            else
                convTypes = cpRepo.GetConversionTypesForWeekStats(searchProfile, numPeriods, null, endDate);

            string intervalName = (monthlyNotWeekly ? "Month" : "Week");
            var model = new SearchStatsVM
            {
                SearchProfile = searchProfile,
                ColumnConfigs = CreateColumnConfigs(intervalName, convTypes, includeConVals),
                EndDate = endDate,
                StatsType = intervalName + "ly",
                NumPeriods = numPeriods,
                NumRows = numRows
            };
            return View("Generic", model);
        }
        public ActionResult WeeklyMonthlyBreakdown(int spId, bool monthlyNotWeekly, DateTime? endDate, int numPeriods = DEFAULT_NUMPERIODS, bool includeConVals = false, bool groupBySearchAccount = false, int numRows = DEFAULT_NUMROWS)
        {
            //TODO: these as arguments?
            //bool groupBySearchAccount = false;
            //string campaignNameInclude = null;

            var searchProfile = cpRepo.GetSearchProfile(spId);
            if (searchProfile == null)
                return HttpNotFound();

            // Determine what the conversion types are & set up ColumnConfigs for them.
            IQueryable<SearchConvType> convTypes;
            if (monthlyNotWeekly)
                convTypes = cpRepo.GetConversionTypesForMonthStats(searchProfile, numPeriods, null, endDate);
            else
                convTypes = cpRepo.GetConversionTypesForWeekStats(searchProfile, numPeriods, null, endDate);

            string intervalName = (monthlyNotWeekly ? "Month" : "Week");
            var model = new SearchStatsVM
            {
                SearchProfile = searchProfile,
                ColumnConfigs = CreateColumnConfigs("Campaign", convTypes, includeConVals),
                EndDate = endDate,
                StatsType = intervalName + "lyBreakdown",
                NumPeriods = numPeriods,
                GroupBySearchAccount = groupBySearchAccount,
                NumRows = numRows
            };
            return View("Generic", model);
        }

        private IEnumerable<ColumnConfig> CreateColumnConfigs(string titleColumnText, IQueryable<SearchConvType> convTypes, bool includeConVals)
        {
            var minIdLookupByAlias = cpRepo.MinConvTypeIdLookupByAlias();
            int letter = 0;
            var columnConfigs = new List<ColumnConfig>() {
                new ColumnConfig("Title", titleColumnText, ColumnConfig.Alphabet[letter++], null, kendoType: "string")
            };
            var aliasGroups = convTypes.GroupBy(ct => ct.Alias).OrderBy(g => g.Key);
            foreach (var aliasGroup in aliasGroups)
            {
                int id = minIdLookupByAlias[aliasGroup.Key];
                columnConfigs.Add(new ColumnConfig("conv" + id, aliasGroup.Key, ColumnConfig.Alphabet[letter++], null));
                if (includeConVals)
                    columnConfigs.Add(new ColumnConfig("cval" + id, "ConVal", ColumnConfig.Alphabet[letter++], ColumnConfig.Format_Max2Dec));
            }
            return columnConfigs;
        }

        //[HttpPost]
        public JsonResult WeeklyData(int spId, DateTime? endDate, int numPeriods = DEFAULT_NUMPERIODS)
        {
            return WeeklyMonthlyData(spId, endDate, numPeriods, monthlyNotWeekly: false);
        }
        //[HttpPost]
        public JsonResult MonthlyData(int spId, DateTime? endDate, int numPeriods = DEFAULT_NUMPERIODS)
        {
            return WeeklyMonthlyData(spId, endDate, numPeriods, monthlyNotWeekly: true);
        }
        //[HttpPost]
        public JsonResult WeeklyMonthlyData(int spId, DateTime? endDate, int numPeriods = DEFAULT_NUMPERIODS, bool monthlyNotWeekly = false)
        {
            var searchProfile = cpRepo.GetSearchProfile(spId);
            if (searchProfile == null)
                return null; //TODO: return empty json?

            IQueryable<SearchStat> stats;
            if (monthlyNotWeekly)
                stats = cpRepo.GetMonthStats(searchProfile, numPeriods, null, endDate);
            else
                stats = cpRepo.GetWeekStats(searchProfile, numPeriods, null, endDate);

            var statsDictionaries = cpRepo.FillInConversionTypeStats(searchProfile.SearchProfileId, stats);

            var json = Json(statsDictionaries, JsonRequestBehavior.AllowGet);
            //var json = Json(statsDictionaries);
            return json;
        }

        //[HttpPost]
        public JsonResult WeeklyBreakdownData(int spId, DateTime? endDate, int numPeriods = DEFAULT_NUMPERIODS, bool groupBySearchAccount = false)
        {
            return WeeklyMonthlyBreakdownData(spId, endDate, numPeriods, groupBySearchAccount, monthlyNotWeekly: false);
        }
        //[HttpPost]
        public JsonResult MonthlyBreakdownData(int spId, DateTime? endDate, int numPeriods = DEFAULT_NUMPERIODS, bool groupBySearchAccount = false)
        {
            return WeeklyMonthlyBreakdownData(spId, endDate, numPeriods, groupBySearchAccount, monthlyNotWeekly: true);
        }
        //[HttpPost]
        public JsonResult WeeklyMonthlyBreakdownData(int spId, DateTime? endDate, int numPeriods = DEFAULT_NUMPERIODS, bool groupBySearchAccount = false, bool monthlyNotWeekly = false)
        {
            //TODO: these as arguments?
            //bool groupBySearchAccount = false;
            string campaignNameInclude = null;

            var searchProfile = cpRepo.GetSearchProfile(spId);
            if (searchProfile == null)
                return null; //TODO: return empty json?

            if (!endDate.HasValue)
                endDate = DateTime.Today.AddDays(-1);

            List<Dictionary<string, IEnumerable<SearchStat>>> periodDictStats = new List<Dictionary<string, IEnumerable<SearchStat>>>();

            //Start with most recent week/month and loop backwards, getting campaign stats for each period
            DateTime periodStart, periodEnd;
            if (monthlyNotWeekly)
            {
                periodStart = new DateTime(endDate.Value.Year, endDate.Value.Month, 1); // (the first could be a partial month)
                periodEnd = endDate.Value;
            }
            else //weekly
            {
                periodStart = endDate.Value; // Start with the week that includes endDate (could be a partial week)
                while (periodStart.DayOfWeek != (DayOfWeek)searchProfile.StartDayOfWeek)
                    periodStart = periodStart.AddDays(-1);
                periodEnd = endDate.Value;
            }
            for (int i = 0; i < numPeriods; i++) // loop through each time period
            {
                //groupBySearchAccount, campaignNameInclude???
                var campaignStatsDict = GeneratorCP.GetOneCampaignStatsDict(cpRepo, searchProfile, periodStart, periodEnd, groupBySearchAccount, campaignNameInclude);

                // if (campaignStatsDict.Keys.Any()) // TODO: if empty, somehow generate a row with zeros for this week/month

                periodDictStats.Insert(0, campaignStatsDict);

                if (monthlyNotWeekly)
                {
                    periodEnd = periodStart.AddDays(-1);
                    periodStart = periodStart.AddMonths(-1);
                }
                else
                {
                    periodEnd = periodStart.AddDays(-1);
                    periodStart = periodStart.AddDays(-7);
                }
            }

            var minConvTypeIdLookupByAlias = cpRepo.MinConvTypeIdLookupByAlias();

            var finalStatsDicts = new List<IDictionary<string, object>>();
            foreach (var campaignStatsDict in periodDictStats) // loop through each time period
            {
                DateTime? start = null, end = null;
                foreach (var groupKey in campaignStatsDict.Keys) // loop through each group (channel / searchaccount)
                {
                    if (!start.HasValue)
                    {   // Remember the start/end dates for the current time period
                        var firstStat = campaignStatsDict[groupKey].FirstOrDefault();
                        if (firstStat != null)
                        {
                            start = firstStat.StartDate;
                            end = firstStat.EndDate;
                        }
                    }
                    var groupStatDicts = cpRepo.FillInConversionTypeStats(searchProfile.SearchProfileId, campaignStatsDict[groupKey], aliasToIdLookup: minConvTypeIdLookupByAlias);
                    finalStatsDicts.AddRange(groupStatDicts);

                    var groupSummary = new Dictionary<string, object>();
                    groupSummary["Title"] = "*End: " + groupKey + "*";
                    finalStatsDicts.Add(groupSummary);
                }
                if (start.HasValue)
                {
                    var timePeriodSummary = new Dictionary<string, object>();
                    timePeriodSummary["Title"] = String.Format("**END: {0:d} - {1:d}**", start, end);
                    finalStatsDicts.Add(timePeriodSummary);
                }
            }

            var json = Json(finalStatsDicts, JsonRequestBehavior.AllowGet);
            //var json = Json(statsDictionaries);
            return json;
        }

    }
}