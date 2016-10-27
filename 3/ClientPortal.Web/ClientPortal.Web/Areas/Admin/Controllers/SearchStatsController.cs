using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using ClientPortal.Data.Contexts;
using ClientPortal.Data.Contracts;
using ClientPortal.Data.DTOs;
using ClientPortal.Web.Areas.Admin.Models;
using ClientPortal.Web.Controllers;

namespace ClientPortal.Web.Areas.Admin.Controllers
{
    [Authorize(Users = "admin")]
    public class SearchStatsController : CPController
    {
        public SearchStatsController(IClientPortalRepository cpRepository)
        {
            cpRepo = cpRepository;
        }

        //public ActionResult Weekly(int spId)
        //{
        //    var searchProfile = cpRepo.GetSearchProfile(spId);
        //    if (searchProfile == null)
        //        return HttpNotFound();

        //    // Determine what the conversion types are & set up ColumnConfigs for them.
        //    int numweeks = 16;
        //    DateTime endDate = DateTime.Today.AddDays(-1);
        //    var convTypes = cpRepo.GetConversionTypesForWeekStats(searchProfile, numweeks, null, endDate)
        //                        .OrderBy(ct => ct.Name).ToList();
        //    var columnConfigs = CreateColumnConfigs("Week", convTypes);

        //    var model = new SearchStatsVM
        //    {
        //        SearchProfile = searchProfile,
        //        ColumnConfigs = columnConfigs,
        //        StatsType = "Weekly"
        //    };
        //    return View("Generic", model);
        //}
        //public ActionResult Monthly(int spId)
        //{
        //    var searchProfile = cpRepo.GetSearchProfile(spId);
        //    if (searchProfile == null)
        //        return HttpNotFound();

        //    // Determine what the conversion types are & set up ColumnConfigs for them.
        //    int nummonths = 16;
        //    DateTime endDate = DateTime.Today.AddDays(-1);
        //    var convTypes = cpRepo.GetConversionTypesForMonthStats(searchProfile, nummonths, null, endDate)
        //                        .OrderBy(ct => ct.Name).ToList();
        //    var columnConfigs = CreateColumnConfigs("Month", convTypes);

        //    var model = new SearchStatsVM
        //    {
        //        SearchProfile = searchProfile,
        //        ColumnConfigs = columnConfigs,
        //        StatsType = "Monthly"
        //    };
        //    return View("Generic", model);
        //}
        public ActionResult WeeklyMonthly(int spId, int numRows = 16, bool monthlyNotWeekly = false)
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
        public JsonResult WeeklyData(int spId, int numRows = 16)
        {
            return WeeklyMonthlyData(spId, numRows, monthlyNotWeekly: false);
        }
        //[HttpPost]
        public JsonResult MonthlyData(int spId, int numRows = 16)
        {
            return WeeklyMonthlyData(spId, numRows, monthlyNotWeekly: true);
        }

        //[HttpPost]
        public JsonResult WeeklyMonthlyData(int spId, int numRows = 16, bool monthlyNotWeekly = false)
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

        ////[HttpPost]
        //public JsonResult WeeklyData(int spId)
        //{
        //    var searchProfile = cpRepo.GetSearchProfile(spId);
        //    if (searchProfile == null)
        //        return null; //TODO: return empty json?

        //    int numweeks = 16;
        //    DateTime endDate = DateTime.Today.AddDays(-1);
        //    var weekStats = cpRepo.GetWeekStats(searchProfile, numweeks, null, endDate);

        //    var weekStatsDictionaries = cpRepo.FillInConversionTypeStats(searchProfile.SearchProfileId, weekStats);

        //    var json = Json(weekStatsDictionaries, JsonRequestBehavior.AllowGet);
        //    //var json = Json(weekStatsDictionaries);
        //    return json;
        //}

        ////[HttpPost]
        //public JsonResult MonthlyData(int spId)
        //{
        //    var searchProfile = cpRepo.GetSearchProfile(spId);
        //    if (searchProfile == null)
        //        return null; //TODO: return empty json?

        //    int nummonths = 16;
        //    DateTime endDate = DateTime.Today.AddDays(-1);
        //    var monthStats = cpRepo.GetMonthStats(searchProfile, nummonths, null, endDate);

        //    var weekStatsDictionaries = cpRepo.FillInConversionTypeStats(searchProfile.SearchProfileId, monthStats);

        //    var json = Json(weekStatsDictionaries, JsonRequestBehavior.AllowGet);
        //    //var json = Json(weekStatsDictionaries);
        //    return json;
        //}
    }
}