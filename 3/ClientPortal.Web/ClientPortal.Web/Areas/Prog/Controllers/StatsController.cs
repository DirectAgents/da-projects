using System;
using System.Configuration;
using System.Linq;
using System.Web.Mvc;
using ClientPortal.Data.Contracts;
using ClientPortal.Web.Controllers;
using DirectAgents.Domain.Abstract;
using DirectAgents.Domain.Entities.TD;

namespace ClientPortal.Web.Areas.Prog.Controllers
{
    [Authorize]
    public class StatsController : CPController
    {
        public StatsController(ICPProgRepository progRepository, IClientPortalRepository cpRepository)
        {
            this.progRepo = progRepository;
            this.cpRepo = cpRepository;
        }

        // --- Executive Summary ---

        public JsonResult ExecMTD()
        {
            var yesterday = DateTime.Today.AddDays(-1);
            return GetMonthStatsJson(yesterday);
        }
        public JsonResult ExecLastMonth()
        {
            DateTime endDate;
            var today = DateTime.Today; //Note: if it's the 1st, last month is still considered the current month
            if (today.Day == 1) // ...always go to last day of previous month, even if it has more days than the "current" month
                endDate = today.AddMonths(-1).AddDays(-1);
            else
                endDate = today.AddDays(-1).AddMonths(-1);

            return GetMonthStatsJson(endDate);
        }
        private JsonResult GetMonthStatsJson(DateTime endDate)
        {
            var userInfo = GetUserInfo();

            var basicStat = progRepo.MTDBasicStat(userInfo.ProgAdvertiser.Id, endDate);
            var array = new BasicStat[] { basicStat };

            var json = Json(array, JsonRequestBehavior.AllowGet); //TODO: don't allow get
            return json;
        }

        public JsonResult DailyCTD() // campaign-to-date dailies
        {
            var userInfo = GetUserInfo();
            var stats = progRepo.DailyBasicStats(userInfo.ProgAdvertiser.Id);

            var json = Json(stats, JsonRequestBehavior.AllowGet); //TODO: don't allow get
            return json;
        }

        // --- Strategy Summary ---

        public JsonResult StrategyMTD()
        {
            var userInfo = GetUserInfo();
            var yesterday = DateTime.Today.AddDays(-1);
            var stats = progRepo.MTDStrategyBasicStats(userInfo.ProgAdvertiser.Id, endDate: yesterday);

            var json = Json(stats, JsonRequestBehavior.AllowGet); //TODO: don't allow get
            return json;
        }

        // --- Weekly Summary ---

        public JsonResult DayOfWeek()
        {
            var userInfo = GetUserInfo();
            var stats = progRepo.DayOfWeekBasicStats(userInfo.ProgAdvertiser.Id, mondayFirst: false);

            var json = Json(stats, JsonRequestBehavior.AllowGet); //TODO: don't allow get
            return json;
        }

        public JsonResult Weekly()
        {
            var userInfo = GetUserInfo();
            var stats = progRepo.WeeklyBasicStats(userInfo.ProgAdvertiser.Id);

            var json = Json(stats, JsonRequestBehavior.AllowGet); //TODO: don't allow get
            return json;
        }

        public JsonResult StrategyByWeek(DateTime weekStart)
        {
            var userInfo = GetUserInfo();
            DateTime weekEnd = weekStart.AddDays(6);
            var stats = progRepo.StrategyBasicStats(userInfo.ProgAdvertiser.Id, weekStart, weekEnd);

            var json = Json(stats, JsonRequestBehavior.AllowGet); //TODO: don't allow get
            return json;
        }

        // --- Creative Performance ---

        public JsonResult Creative()
        {
            var userInfo = GetUserInfo();
            var stats = progRepo.CreativePerfBasicStats(userInfo.ProgAdvertiser.Id, includeInfo: true)
                .OrderBy(s => s.eCPA == 0).ThenBy(s => s.eCPA).ThenBy(s => s.MediaSpend);

            string progDemoAdvs = ConfigurationManager.AppSettings["ProgDemoAdvs"] ?? "";
            var demoAdvs = progDemoAdvs.Split(new char[] { ',' }); // e.g.  "sees", "seph", "sony", "winv"
            var advPrefix = userInfo.ProgAdvertiser.Name.Substring(0, 4).ToLower();
            if (demoAdvs.Contains(advPrefix))
            {
                var baseUrl = Url.Content("~/Images/Demo/");
                int j = 0;
                foreach (var stat in stats)
                {
                    var creativeImage = advPrefix + "1.jpg";
                    if (j % 4 > 0)
                    {
                        creativeImage = (j % 4 == 1) ? advPrefix + "2.jpg" : ((j % 4 == 2) ? advPrefix + "3.jpg" : advPrefix + "4.jpg");
                    }
                    stat.Url = baseUrl + creativeImage;
                    stat.AdWidth = 0;
                    stat.AdHeight = 0;
                    stat.AdBody = null;
                    stat.AdHeadline = null;
                    stat.AdMessage = null;
                    j++;
                }
            }

            var json = Json(stats, JsonRequestBehavior.AllowGet); //TODO: don't allow get
            return json;
        }
        public JsonResult Creative2()
        {
            var userInfo = GetUserInfo();
            var stats = progRepo.CreativePerfBasicStats2(userInfo.ProgAdvertiser.Id);

            var json = Json(stats, JsonRequestBehavior.AllowGet); //TODO: don't allow get
            return json;
        }
    }
}