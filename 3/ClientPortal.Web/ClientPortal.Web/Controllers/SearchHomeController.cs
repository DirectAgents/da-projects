using System;
using System.Web.Mvc;
using ClientPortal.Data.Contracts;
using ClientPortal.Web.Models;

namespace ClientPortal.Web.Controllers
{
    [Authorize]
    public class SearchHomeController : CPController
    {
        public SearchHomeController(IClientPortalRepository cpRepository)
        {
            this.cpRepo = cpRepository;
        }

        public ActionResult Index()
        {
            var userInfo = GetUserInfo();
            var result = CheckLogout(userInfo);
            if (result != null) return result;

            if (!userInfo.HasSearch)
                return RedirectToAction("Index", "Home");

            var model = new IndexModel(userInfo, true);
            return View(model);
        }

        public PartialViewResult Dashboard()
        {
            return PartialView();
        }

        public PartialViewResult Monthly()
        {
            return PartialView();
        }

        public PartialViewResult ChannelPerf()
        {
            return PartialView();
        }

        public PartialViewResult CampaignPerf()
        {
            var userInfo = GetUserInfo();

            var today = DateTime.Today;
            var start = new DateTime(today.Year, today.Month, 1);
            var end = today;
            var model = new SearchReportModel()
            {
                StartDate = start.ToString("d", userInfo.CultureInfo),
                EndDate = end.ToString("d", userInfo.CultureInfo)
            };
            return PartialView(model);
        }

        // (add a report tab) 4. Add action method for partial view to generate tab content
        public PartialViewResult CampaignWeekly()
        {
            var userInfo = GetUserInfo();

            return PartialView(new SearchReportModel
            {
                StartDate = new DateTime(DateTime.Today.Year, DateTime.Today.AddMonths(-2).Month, 1).ToString("d", userInfo.CultureInfo),
                EndDate = DateTime.Today.ToString("d", userInfo.CultureInfo)
            });
        }

        public PartialViewResult AdgroupPerf()
        {
            var userInfo = GetUserInfo();

            var start = new DateTime(2013, 6, 17);
            var end = new DateTime(2013, 6, 23);
            var model = new SearchReportModel()
            {
                StartDate = start.ToString("d", userInfo.CultureInfo),
                EndDate = end.ToString("d", userInfo.CultureInfo)
            };
            return PartialView(model);
        }

    }
}
