using ClientPortal.Data.Contracts;
using ClientPortal.Web.Models;
using System;
using System.Web.Mvc;

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

            var model = new SearchIndexModel(userInfo);
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

            var model = new SearchReportModel()
            {
                StartDate = userInfo.Search_Dates.FirstOfMonth.ToString("d", userInfo.CultureInfo),
                EndDate = userInfo.Search_Dates.Latest.ToString("d", userInfo.CultureInfo)
            };
            return PartialView(model);
        }

        public PartialViewResult CampaignWeekly()
        {
            var userInfo = GetUserInfo();

            return PartialView(new SearchReportModel
            {
                StartDate = userInfo.Search_Dates.FirstOfMonth.AddMonths(-2).ToString("d", userInfo.CultureInfo),
                EndDate = userInfo.Search_Dates.Latest.ToString("d", userInfo.CultureInfo)
            });
        }

        // UNDER CONSTRUCTION (TODO:)
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
