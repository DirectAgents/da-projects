using ClientPortal.Data.Contracts;
using ClientPortal.Web.Models;
using System;
using System.Linq;
using System.Web.Mvc;
using WebMatrix.WebData;

namespace ClientPortal.Web.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private IOfferRepository offerRepo;

        public HomeController(IOfferRepository offerRepository)
        {
            this.offerRepo = offerRepository;
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

            var summaryMTD = offerRepo.GetAdvertiserSummary(firstOfMonth, now, advertiserId.Value);
            var summaryLMTD = offerRepo.GetAdvertiserSummary(firstOfLastMonth, oneMonthAgo, advertiserId.Value);
            var summaryLM = offerRepo.GetAdvertiserSummary(firstOfLastMonth, lastOfLastMonth, advertiserId.Value);

            var model = new DashboardModel
            {
                SummaryMTD = summaryMTD,
                SummaryLMTD = summaryLMTD,
                SummaryLM = summaryLM
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
