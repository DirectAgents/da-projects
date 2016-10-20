using System;
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

            var model = new SearchStatsVM
            {
                SearchProfile = searchProfile
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
            //var weekStatsDictionaries = weekStats.Select(s => s.ToDictionary()).ToList();

            // todo? determine all convtypeids, then pass that in to FillIn...
            //cpRepo.FillInConvTypeStats(weekStatsDictionaries);
            //foreach (var dict in weekStatsDictionaries)
            //{
            //    dict["test"] = "1234abc";
            //}

            var json = Json(weekStatsDictionaries, JsonRequestBehavior.AllowGet);
            //var json = Json(dailyDTOs);
            return json;
        }
	}
}