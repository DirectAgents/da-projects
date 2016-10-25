using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using ClientPortal.Data.Contracts;
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

        public ActionResult Weekly(int spId)
        {
            var searchProfile = cpRepo.GetSearchProfile(spId);
            if (searchProfile == null)
                return HttpNotFound();

            int letter = 0;
            var columnConfigs = new List<ColumnConfig>() {
                new ColumnConfig("Title", "Week", ColumnConfig.Alphabet[letter++], null, kendoType: "string")
            };

            int numweeks = 16;
            DateTime endDate = DateTime.Today.AddDays(-1);

            // Determine what the conversion types are & set up ColumnConfigs for them.
            var convTypes = cpRepo.GetConversionTypesForWeekStats(searchProfile, numweeks, null, endDate)
                                .OrderBy(ct => ct.Name).ToList();
            foreach (var convType in convTypes)
            {
                int id = convType.SearchConvTypeId;
                columnConfigs.Add(new ColumnConfig("conv" + id, convType.Name, ColumnConfig.Alphabet[letter++], null));
                columnConfigs.Add(new ColumnConfig("cval" + id, "ConVal", ColumnConfig.Alphabet[letter++], ColumnConfig.Format_Max2Dec));
            }

            var model = new SearchStatsVM
            {
                SearchProfile = searchProfile,
                ColumnConfigs = columnConfigs
            };

            return View(model);
        }

        //[HttpPost]
        public JsonResult WeeklyData(int spId)
        {
            var searchProfile = cpRepo.GetSearchProfile(spId);
            if (searchProfile == null)
                return null; //TODO: return empty json?

            int numweeks = 16;
            DateTime endDate = DateTime.Today.AddDays(-1);
            var weekStats = cpRepo.GetWeekStats(searchProfile, numweeks, null, endDate);

            var weekStatsDictionaries = cpRepo.FillInConversionTypeStats(searchProfile.SearchProfileId, weekStats);

            var json = Json(weekStatsDictionaries, JsonRequestBehavior.AllowGet);
            //var json = Json(weekStatsDictionaries);
            return json;
        }
	}
}