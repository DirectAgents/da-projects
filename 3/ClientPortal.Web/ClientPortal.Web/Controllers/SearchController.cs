using System;
using System.Web.Mvc;
using ClientPortal.Data.Contracts;
using ClientPortal.Web.Models;

namespace ClientPortal.Web.Controllers
{
    [Authorize]
    public class SearchController : CPController
    {
        public SearchController(IClientPortalRepository cpRepository, DirectAgents.Domain.Abstract.ITDRepository progRepository)
        {
            this.cpRepo = cpRepository;
            this.progRepo = progRepository; // used in GetUserInfo()
        }

        public ActionResult Index()
        {
            return RedirectToAction("Weekly");
        }

        private ActionResult SetupView()
        {
            var userInfo = GetUserInfo();
            //CheckLogout...
            if (!userInfo.HasSearch)
                return RedirectToAction("Index", "Home");

            ViewBag.HasSearch = true;
            ViewBag.HasProg = userInfo.HasProgrammatic();

            var model = new SearchVM(userInfo);
            return View(model);
        }

        public ActionResult Weekly()
        {
            return SetupView();
        }
        public ActionResult Monthly()
        {
            return SetupView();
        }
        public ActionResult ChannelPerf()
        {
            return SetupView();
        }

        public ActionResult CampaignPerf()
        {
            var userInfo = GetUserInfo();
            if (!userInfo.HasSearch)
                return RedirectToAction("Index", "Home");

            ViewBag.HasSearch = true;
            ViewBag.HasProg = userInfo.HasProgrammatic();

            var model = new SearchVM(userInfo);
            model.StartDate = model.Dates.FirstOfMonth;
            model.EndDate = model.Dates.Latest;
            return View(model);
        }

        public ActionResult CampaignWeekly()
        {
            var userInfo = GetUserInfo();
            if (!userInfo.HasSearch)
                return RedirectToAction("Index", "Home");

            ViewBag.HasSearch = true;
            ViewBag.HasProg = userInfo.HasProgrammatic();

            int numWeeks = 8;
            DateTime start = DateTime.Today.AddDays(-7 * numWeeks + 6);
            while (start.DayOfWeek != userInfo.Search_StartDayOfWeek)
                start = start.AddDays(-1);

            var model = new SearchVM(userInfo);
            model.StartDate = model.Dates.DateString(start);
            model.EndDate = model.Dates.Latest;
            return View(model);
        }

        public ActionResult Contact()
        {
            return SetupView();
        }

        public ActionResult Test()
        {
            var userInfo = GetUserInfo();
            var model = new SearchVM(userInfo);
            model.StartDate = model.Dates.FirstOfMonth;
            model.EndDate = model.Dates.Latest;
            return View(model);
        }

	}
}