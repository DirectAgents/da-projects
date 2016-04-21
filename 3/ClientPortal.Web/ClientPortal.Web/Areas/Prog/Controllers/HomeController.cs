using System;
using System.Web.Mvc;
using ClientPortal.Web.Areas.Prog.Models;
using ClientPortal.Web.Controllers;
using DirectAgents.Domain.Abstract;

namespace ClientPortal.Web.Areas.Prog.Controllers
{
    [Authorize]
    public class HomeController : CPController
    {
        public HomeController(ITDRepository progRepository, ClientPortal.Data.Contracts.IClientPortalRepository cpRepository)
        {
            this.progRepo = progRepository;
            this.cpRepo = cpRepository;
        }

        // "Executive Summary"
        public ActionResult Index()
        {
            var userInfo = GetUserInfo();

            var today = DateTime.Today;
            var yesterday = today.AddDays(-1);
            DateTime lastMonthEndDate; //Note: if it's the 1st, last month is still considered the current month
            if (today.Day == 1) // ...always go to last day of previous month, even if it has more days than the "current" month
                lastMonthEndDate = today.AddMonths(-1).AddDays(-1);
            else
                lastMonthEndDate = today.AddDays(-1).AddMonths(-1);

            int advId = userInfo.ProgAdvertiser.Id;
            DateTime earliestStatDate = progRepo.EarliestStatDate(advId: advId) ?? yesterday;

            var model = new ExecSumVM
            {
                UserInfo = userInfo,
                MTDStat = progRepo.MTDBasicStat(advId, yesterday),
                LastMonthStat = progRepo.MTDBasicStat(advId, lastMonthEndDate),
                CTDStat = progRepo.DateRangeBasicStat(advId, earliestStatDate, yesterday)
            };
            return View(model);
        }

        public ActionResult Strategy()
        {
            var userInfo = GetUserInfo();
            int advId = userInfo.ProgAdvertiser.Id;

            var yesterday = DateTime.Today.AddDays(-1);
            var model = new StratSumVM
            {
                UserInfo = userInfo,
                MTDStrategyStats = progRepo.MTDStrategyBasicStats(advId, endDate: yesterday)
            };
            return View(model);
        }

        public ActionResult Weekly()
        {
            var userInfo = GetUserInfo();

            var model = new WeekSumVM
            {
                UserInfo = userInfo
            };
            return View(model);
        }

        public ActionResult Creative()
        {
            var userInfo = GetUserInfo();
            int advId = userInfo.ProgAdvertiser.Id;
            DateTime yesterday = DateTime.Today.AddDays(-1);
            DateTime campaignStart = progRepo.EarliestStatDate(advId) ?? yesterday;

            var model = new CreatPerfVM
            {
                UserInfo = userInfo,
                StartDate = campaignStart,
                EndDate = yesterday
            };
            return View(model);
        }
    }
}