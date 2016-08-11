using System;
using System.Web.Helpers;
using System.Web.Mvc;
using ClientPortal.Data.Contracts;
using ClientPortal.Web.Models;

namespace ClientPortal.Web.Controllers
{
    public class CombinedController : CPController
    {
        public CombinedController(IClientPortalRepository cpRepository, DirectAgents.Domain.Abstract.ITDRepository progRepository)
        {
            this.cpRepo = cpRepository;
            this.progRepo = progRepository;
        }

        public FileResult Logo()
        {
            var userInfo = GetUserInfo();
            if (userInfo.ClientLogo == null)
                return null;
            WebImage logo = new WebImage(userInfo.ClientLogo);
            return File(logo.GetBytes(), "image/" + logo.ImageFormat, logo.FileName);
        }

        public ActionResult Index() // Combined Dashboard
        {
            var userInfo = GetUserInfo();
            if (!userInfo.HasSearch || !userInfo.HasProgrammatic())
                return RedirectToAction("Go", "Home");

            var today = DateTime.Today;
            var yesterday = today.AddDays(-1);
            var monthStart = new DateTime(yesterday.Year, yesterday.Month, 1);
            var weekStart = yesterday;
            while (weekStart.DayOfWeek != DayOfWeek.Monday) //TODO: use client's start-day-of-week
            {
                weekStart = weekStart.AddDays(-1);
            }
            // Get search stats
            var mtdSearch = cpRepo.GetSearchStats(userInfo.SearchProfile, monthStart, yesterday, false);
            var wtdSearch = cpRepo.GetSearchStats(userInfo.SearchProfile, weekStart, yesterday, false);

            // Get programmatic stats
            int progAdvId = userInfo.ProgAdvertiser.Id;
            var mtdProg = progRepo.MTDBasicStat(progAdvId, yesterday);
            var wtdProg = progRepo.DateRangeBasicStat(progAdvId, weekStart, yesterday);

            // Combine them
            var mtdStat = new StatVM(monthStart, yesterday);
            mtdStat.Add(mtdSearch);
            mtdStat.Add(mtdProg);

            var wtdStat = new StatVM(weekStart, yesterday);
            wtdStat.Add(wtdSearch);
            wtdStat.Add(wtdProg);

            var model = new CombinedVM
            {
                UserInfo = userInfo,
                MTDStat = mtdStat,
                WTDStat = wtdStat
            };

            return View(model);
        }
	}
}