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
            var start = yesterday;
            while (start.DayOfWeek != DayOfWeek.Monday) //TODO: use client's start-day-of-week
            {
                start = start.AddDays(-1);
            }
            int progAdvId = userInfo.ProgAdvertiser.Id; // temp

            var model = new CombinedVM
            {
                UserInfo = userInfo,
                MTDStat = progRepo.MTDBasicStat(progAdvId, yesterday), // temp
                WTDStat = progRepo.DateRangeBasicStat(progAdvId, start, yesterday) // temp
            };

            return View(model);
        }
	}
}