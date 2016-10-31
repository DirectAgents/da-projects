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

    }
}