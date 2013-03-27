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

            var summaryMTD = cakeRepo.GetDateRangeSummary(firstOfMonth, now, advertiserId.Value, null);
            summaryMTD.Name = "Month-to-Date";
            var summaryLMTD = cakeRepo.GetDateRangeSummary(firstOfLastMonth, oneMonthAgo, advertiserId.Value, null);
            summaryLMTD.Name = "Last Month-to-Date";
            var summaryLM = cakeRepo.GetDateRangeSummary(firstOfLastMonth, lastOfLastMonth, advertiserId.Value, null);
            summaryLM.Name = "Last Month-Total";

            var offers = cakeRepo.Offers(advertiserId);

            // Get Goals
            var goals = AccountRepository.GetGoals(advertiserId.Value, cakeRepo);
            var offerIds = goals.Where(g => g.OfferId.HasValue).Select(g => g.OfferId.Value).Distinct();
            List<OfferGoalSummary> offerGoalSummaries = new List<OfferGoalSummary>();
            foreach (var offerId in offerIds)
            {
                var offsumMTD = cakeRepo.GetDateRangeSummary(firstOfMonth, now, advertiserId.Value, offerId);
                offsumMTD.Name = "Month-to-Date";
                var offsumLMTD = cakeRepo.GetDateRangeSummary(firstOfLastMonth, oneMonthAgo, advertiserId.Value, offerId);
                offsumLMTD.Name = "Last Month-to-Date";
                var offsumLM = cakeRepo.GetDateRangeSummary(firstOfLastMonth, lastOfLastMonth, advertiserId.Value, offerId);
                offsumLM.Name = "Last Month-Total";

                var offer = offers.Where(o => o.Offer_Id == offerId).FirstOrDefault();
                var offerGoalSummary = new OfferGoalSummary()
                {
                    Offer = offer,
                    Goals = goals.Where(g => g.OfferId == offerId).ToList(),
                    DateRangeSummaries = new List<DateRangeSummary> { offsumMTD, offsumLMTD, offsumLM }
                };
                offerGoalSummaries.Add(offerGoalSummary);
            }

            var model = new DashboardModel
            {
                AdvertiserSummaries = new List<DateRangeSummary> { summaryMTD, summaryLMTD, summaryLM },
                OfferGoalSummaries = offerGoalSummaries
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
