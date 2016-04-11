using System;
using System.Web.Mvc;
using ClientPortal.Web.Controllers;
using DirectAgents.Domain.Abstract;

namespace ClientPortal.Web.Areas.Prog.Controllers
{
    public class TestController : CPController
    {
        public TestController(ITDRepository progRepository)
        {
            progRepo = progRepository;
        }

        public ActionResult Blank()
        {
            return View();
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
            var stats = progRepo.DayOfWeekBasicStats(advId, startDate, endDate);
            //var stats = tdRepo.DailySummaryBasicStats(advId, startDate, endDate);
            var json = Json(stats, JsonRequestBehavior.AllowGet);
            //var json = Json(stats);
            return json;
        }

        public JsonResult TestCalls()
        {
            var advId = 2;
            var startDate = new DateTime(2016, 2, 20);
            var endDate = new DateTime(2016, 3, 15);

            var stats = progRepo.DailySummaryBasicStats(advId); // for graphs (daily #s)
            //var stats = progRepo.DayOfWeekBasicStats(advId, startDate, endDate);
            //var stat = progRepo.MTDBasicStat(advId, endDate);
            //var stat = progRepo.DateRangeBasicStat(advId, startDate, endDate); // for campaign-to-date summary (won't include budget)

            var json = Json(stats, JsonRequestBehavior.AllowGet);
            //var json = Json(stats);
            return json;
        }

    }
}