using System;
using System.Web.Mvc;
using ClientPortal.Web.Areas.Prog.Models;
using ClientPortal.Web.Controllers;
using DirectAgents.Domain.Abstract;

namespace ClientPortal.Web.Areas.Prog.Controllers
{
    public class HomeController : CPController
    {
        public HomeController(ITDRepository progRepository, ClientPortal.Data.Contracts.IClientPortalRepository cpRepository)
        {
            this.progRepo = progRepository;
            this.cpRepo = cpRepository;
        }

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

            var model = new ExecSumVM
            {
                UserInfo = userInfo,
                MTDStat = progRepo.MTDBasicStat(userInfo.ProgAdvertiser.Id, yesterday),
                LastMonthStat = progRepo.MTDBasicStat(userInfo.ProgAdvertiser.Id, lastMonthEndDate)
            };
            return View(model);
        }
    }
}