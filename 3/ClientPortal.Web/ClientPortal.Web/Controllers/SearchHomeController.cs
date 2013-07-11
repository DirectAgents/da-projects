using ClientPortal.Data.Contracts;
using ClientPortal.Data.DTOs;
using ClientPortal.Data.Services;
using DirectAgents.Mvc.KendoGridBinder;
using System.Web.Mvc;
using System.Linq;
using System;
using ClientPortal.Web.Models;
using System.Globalization;

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

            var model = new IndexModel(userInfo);
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

            var start = new DateTime(2013, 5, 27);
            var end = new DateTime(2013, 6, 2);
            var model = new SearchReportModel()
            {
                StartDate = start.ToString("d", userInfo.CultureInfo),
                EndDate = end.ToString("d", userInfo.CultureInfo)
            };
            return PartialView(model);
        }

        public PartialViewResult AdgroupPerf()
        {
            var userInfo = GetUserInfo();

            var start = new DateTime(2013, 5, 27);
            var end = new DateTime(2013, 6, 2);
            var model = new SearchReportModel()
            {
                StartDate = start.ToString("d", userInfo.CultureInfo),
                EndDate = end.ToString("d", userInfo.CultureInfo)
            };
            return PartialView(model);
        }

    }
}
