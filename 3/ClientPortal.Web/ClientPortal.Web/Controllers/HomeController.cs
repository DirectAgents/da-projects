using ClientPortal.Data.Contracts;
using ClientPortal.Data.DTOs;
using ClientPortal.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using WebMatrix.WebData;

namespace ClientPortal.Web.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private ICakeRepository cakeRepo;

        public HomeController(ICakeRepository cakeRepository)
        {
            this.cakeRepo = cakeRepository;
        }

        public ActionResult Index()
        {
            return View();
        }

        public PartialViewResult Dashboard()
        {
            int? advertiserId = HomeController.GetAdvertiserId();
            if (advertiserId == null) return null;

            var now = DateTime.Now;
            var firstOfMonth = new DateTime(now.Year, now.Month, 1);
            var firstOfLastMonth = firstOfMonth.AddMonths(-1);
            var lastOfLastMonth = firstOfMonth.AddDays(-1);

            var oneMonthAgo = new DateTime(firstOfLastMonth.Year, firstOfLastMonth.Month, (now.Day < lastOfLastMonth.Day) ? now.Day : lastOfLastMonth.Day);
            // will be the last day of last month if today's "day" is greater than the number of days in last month

            var summaryMTD = cakeRepo.GetDateRangeSummary(firstOfMonth, now, advertiserId.Value);
            summaryMTD.Name = "Month-to-Date";
            var summaryLMTD = cakeRepo.GetDateRangeSummary(firstOfLastMonth, oneMonthAgo, advertiserId.Value);
            summaryLMTD.Name = "Last Month-to-Date";
            var summaryLM = cakeRepo.GetDateRangeSummary(firstOfLastMonth, lastOfLastMonth, advertiserId.Value);
            summaryLM.Name = "Last Month Total";

            var model = new DashboardModel
            {
                DateRangeSummaries = new List<DateRangeSummary> { summaryMTD, summaryLMTD, summaryLM }
            };
            return PartialView(model);
        }

        public static int? GetAdvertiserId()
        {
            int? advertiserId = null;

            if (WebSecurity.Initialized)
            {
                var userID = WebSecurity.CurrentUserId;
                using (var usersContext = new UsersContext())
                {
                    var userProfile = usersContext.UserProfiles.FirstOrDefault(c => c.UserId == userID);
                    if (userProfile != null)
                        advertiserId = userProfile.CakeAdvertiserId;
                }
            }
            return advertiserId;
        }

        // ---

        public ActionResult About()
        {
            ViewBag.Message = "Your app description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        public ActionResult Foundation()
        {
            return View();
        }
    }
}
