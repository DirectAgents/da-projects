using ClientPortal.Data.Contracts;
using ClientPortal.Web.Models;
using System;
using System.Linq;
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

        public PartialViewResult Dashboard() // Weekly
        {
            var userInfo = GetUserInfo();
            var model = new SearchReportModel { SearchProfile = userInfo.SearchProfile };
            return PartialView(model);
        }

        public PartialViewResult Monthly()
        {
            var userInfo = GetUserInfo();
            var model = new SearchReportModel { SearchProfile = userInfo.SearchProfile };
            return PartialView(model);
        }

        public PartialViewResult ChannelPerf()
        {
            var userInfo = GetUserInfo();
            var model = new SearchReportModel { SearchProfile = userInfo.SearchProfile };
            return PartialView(model);
        }

        public PartialViewResult CampaignPerf()
        {
            var userInfo = GetUserInfo();

            var model = new SearchReportModel
            {
                SearchProfile = userInfo.SearchProfile,
                StartDate = userInfo.Search_Dates.FirstOfMonth.ToString("d", userInfo.CultureInfo),
                EndDate = userInfo.Search_Dates.Latest.ToString("d", userInfo.CultureInfo)
            };
            return PartialView(model);
        }

        public PartialViewResult CampaignWeekly()
        {
            var userInfo = GetUserInfo();

            int numWeeks = 8;
            DateTime start = DateTime.Today.AddDays(-7 * numWeeks + 6);
            while (start.DayOfWeek != userInfo.Search_StartDayOfWeek)
                start = start.AddDays(-1);

            var model = new SearchReportModel
            {
                StartDate = start.ToString("d", userInfo.CultureInfo),
                EndDate = userInfo.Search_Dates.Latest.ToString("d", userInfo.CultureInfo)
            };
            return PartialView(model);
        }

        //public PartialViewResult AdgroupPerf()
        //{
        //    var userInfo = GetUserInfo();

        //    var start = new DateTime(2013, 6, 17);
        //    var end = new DateTime(2013, 6, 23);
        //    var model = new SearchReportModel()
        //    {
        //        StartDate = start.ToString("d", userInfo.CultureInfo),
        //        EndDate = end.ToString("d", userInfo.CultureInfo)
        //    };
        //    return PartialView(model);
        //}

        public ActionResult Contact(bool showtitle = true)
        {
            var userInfo = GetUserInfo();
            var contacts = userInfo.SearchProfile.SearchProfileContactsOrdered.Select(spc => spc.Contact);
            ViewBag.ShowTitle = showtitle;
            return PartialView("_Contact", contacts);
        }
    }
}
