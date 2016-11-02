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
        private const int DEFAULT_NUMROWS = 16;

        public SearchStatsController(IClientPortalRepository cpRepository)
        {
            cpRepo = cpRepository;
        }

        public ActionResult Generic(int spId, string statsType, string interval, int numRows = DEFAULT_NUMROWS)
        {
            statsType = statsType.ToLower();
            interval = interval.ToLower();

            if (statsType == "overall")
            {
                return WeeklyMonthly(spId, (interval == "monthly"), numRows);
            }
            else if (statsType == "campaign")
            {
                return WeeklyMonthlyBreakdown(spId, (interval == "monthly"), numRows);
            }
            return HttpNotFound();
        }

        public ActionResult WeeklyMonthly(int spId, bool monthlyNotWeekly, int numRows = DEFAULT_NUMROWS)
        {
            var searchProfile = cpRepo.GetSearchProfile(spId);
            if (searchProfile == null)
                return HttpNotFound();

            // Determine what the conversion types are & set up ColumnConfigs for them.
            DateTime endDate = DateTime.Today.AddDays(-1); //TODO? determine end of last complete week, if weekly?
            IEnumerable<SearchConvType> convTypes;
            if (monthlyNotWeekly)
                convTypes = cpRepo.GetConversionTypesForMonthStats(searchProfile, numRows, null, endDate)
                                    .OrderBy(ct => ct.Name).ToList();
            else
                convTypes = cpRepo.GetConversionTypesForWeekStats(searchProfile, numRows, null, endDate)
                                    .OrderBy(ct => ct.Name).ToList();

            string intervalName = (monthlyNotWeekly ? "Month" : "Week");
            var model = new SearchStatsVM
            {
                SearchProfile = searchProfile,
                ColumnConfigs = CreateColumnConfigs(intervalName, convTypes),
                StatsType = intervalName + "ly",
                NumRows = numRows
            };
            return View("Generic", model);
        }
        public ActionResult WeeklyMonthlyBreakdown(int spId, bool monthlyNotWeekly, int numRows = DEFAULT_NUMROWS)
        {
            //TODO: these as arguments?
            //bool groupBySearchAccount = false;
            //string campaignNameInclude = null;

            var searchProfile = cpRepo.GetSearchProfile(spId);
            if (searchProfile == null)
                return HttpNotFound();

            // Determine what the conversion types are & set up ColumnConfigs for them.
            DateTime endDate = DateTime.Today.AddDays(-1); //TODO? determine end of last complete week, if weekly?
            IEnumerable<SearchConvType> convTypes;
            if (monthlyNotWeekly)
                convTypes = cpRepo.GetConversionTypesForMonthStats(searchProfile, numRows, null, endDate)
                                    .OrderBy(ct => ct.Name).ToList();
            else
                convTypes = cpRepo.GetConversionTypesForWeekStats(searchProfile, numRows, null, endDate)
                                    .OrderBy(ct => ct.Name).ToList();

            string intervalName = (monthlyNotWeekly ? "Month" : "Week");
            var model = new SearchStatsVM
            {
                SearchProfile = searchProfile,
                ColumnConfigs = CreateColumnConfigs("Campaign", convTypes),
                StatsType = intervalName + "lyBreakdown",
                NumRows = numRows
            };
            return View("Generic", model);
        }

        private IEnumerable<ColumnConfig> CreateColumnConfigs(string titleColumnText, IEnumerable<SearchConvType> convTypes)
        {
            int letter = 0;
            var columnConfigs = new List<ColumnConfig>() {
                new ColumnConfig("Title", titleColumnText, ColumnConfig.Alphabet[letter++], null, kendoType: "string")
            };
            foreach (var convType in convTypes)
            {
                int id = convType.SearchConvTypeId;
                columnConfigs.Add(new ColumnConfig("conv" + id, convType.Name, ColumnConfig.Alphabet[letter++], null));
                columnConfigs.Add(new ColumnConfig("cval" + id, "ConVal", ColumnConfig.Alphabet[letter++], ColumnConfig.Format_Max2Dec));
            }
            return columnConfigs;
        }

        //[HttpPost]
        public JsonResult WeeklyData(int spId, int numRows = DEFAULT_NUMROWS)
        {
            return WeeklyMonthlyData(spId, numRows, monthlyNotWeekly: false);
        }
        //[HttpPost]
        public JsonResult MonthlyData(int spId, int numRows = DEFAULT_NUMROWS)
        {
            return WeeklyMonthlyData(spId, numRows, monthlyNotWeekly: true);
        }
        //[HttpPost]
        public JsonResult WeeklyMonthlyData(int spId, int numRows = DEFAULT_NUMROWS, bool monthlyNotWeekly = false)
        {
            var searchProfile = cpRepo.GetSearchProfile(spId);
            if (searchProfile == null)
                return null; //TODO: return empty json?

            DateTime endDate = DateTime.Today.AddDays(-1);
            IQueryable<SearchStat> stats;
            if (monthlyNotWeekly)
                stats = cpRepo.GetMonthStats(searchProfile, numRows, null, endDate);
            else
                stats = cpRepo.GetWeekStats(searchProfile, numRows, null, endDate);

            var statsDictionaries = cpRepo.FillInConversionTypeStats(searchProfile.SearchProfileId, stats);

            var json = Json(statsDictionaries, JsonRequestBehavior.AllowGet);
            //var json = Json(statsDictionaries);
            return json;
        }

        //[HttpPost]
        public JsonResult WeeklyBreakdownData(int spId, int numRows = DEFAULT_NUMROWS)
        {
            return WeeklyMonthlyBreakdownData(spId, numRows, monthlyNotWeekly: false);
        }
        //[HttpPost]
        public JsonResult MonthlyBreakdownData(int spId, int numRows = DEFAULT_NUMROWS)
        {
            return WeeklyMonthlyBreakdownData(spId, numRows, monthlyNotWeekly: true);
        }
        //[HttpPost]
        public JsonResult WeeklyMonthlyBreakdownData(int spId, int numPeriods = DEFAULT_NUMROWS, bool monthlyNotWeekly = false)
        {
            //TODO: these as arguments?
            bool groupBySearchAccount = false;
            string campaignNameInclude = null;

            var searchProfile = cpRepo.GetSearchProfile(spId);
            if (searchProfile == null)
                return null; //TODO: return empty json?

            List<Dictionary<string, IEnumerable<SearchStat>>> periodDictStats = new List<Dictionary<string, IEnumerable<SearchStat>>>();

            //Start with most recent week/month and loop backwards, getting campaign stats for each period
            DateTime endDate = DateTime.Today.AddDays(-1); //TODO? determine end of last complete week, if weekly?
            DateTime periodStart, periodEnd;
            if (monthlyNotWeekly)
            {
                periodStart = new DateTime(endDate.Year, endDate.Month, 1); // (the first could be a partial month)
                periodEnd = endDate;
            }
            else //weekly
            {
                periodStart = endDate; // Start with the week that includes endDate (could be a partial week)
                while (periodStart.DayOfWeek != (DayOfWeek)searchProfile.StartDayOfWeek)
                    periodStart = periodStart.AddDays(-1);
                periodEnd = endDate;
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
                    var groupStatDicts = cpRepo.FillInConversionTypeStats(searchProfile.SearchProfileId, campaignStatsDict[groupKey]);
                    finalStatsDicts.AddRange(groupStatDicts);

                    var groupSummary = new Dictionary<string, object>();
                    groupSummary["Title"] = "(End: " + groupKey + ")";
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