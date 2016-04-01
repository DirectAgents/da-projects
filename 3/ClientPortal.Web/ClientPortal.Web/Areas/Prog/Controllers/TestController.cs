using System;
using System.Web.Mvc;
using ClientPortal.Web.Controllers;
using DirectAgents.Domain.Abstract;

namespace ClientPortal.Web.Areas.Prog.Controllers
{
    public class TestController : CPController
    {
        public TestController(ITDRepository datdRepository)
        {
            datdRepo = datdRepository;
        }

        public ActionResult Executive()
        {
            return View();
        }

        public ActionResult Weekly()
        {
            return View();
        }


        public JsonResult Data()
        {
            var advId = 18;
            var startDate = new DateTime(2016, 3, 1);
            var endDate = new DateTime(2016, 3, 23);
            var stats = datdRepo.DayOfWeekBasicStats(advId, startDate, endDate);
            //var stats = tdRepo.DailySummaryBasicStats(advId, startDate, endDate);
            var json = Json(stats, JsonRequestBehavior.AllowGet);
            //var json = Json(stats);
            return json;
        }
    }
}