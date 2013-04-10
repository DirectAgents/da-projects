using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using ClientPortal.Data.Contexts;
using ClientPortal.Data.Contracts;
using ClientPortal.Data.DTOs;
using ClientPortal.Web.Models;
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
            var userProfile = GetUserProfile();
            CakeAdvertiser advertiser = userProfile.CakeAdvertiserId.HasValue ? cakeRepo.Advertiser(userProfile.CakeAdvertiserId.Value) : null;

            var model = new IndexModel()
            {
                CultureInfo = userProfile.CultureInfo,
                Advertiser = advertiser
            };
            return View(model);
        }

        public ActionResult Contact()
        {
            var model = GetAdvertiser();
            return PartialView(model);
        }

        public ActionResult SetDashboardDateRange(string type, string startdate, string enddate)
        {
            var userProfile = GetUserProfile();
            Session["DashboardDateRangeType"] = type;

            DateTime? start, end;
            if (ReportsController.ParseDate(startdate, userProfile.CultureInfo, out start))
                Session["DashboardStart"] = start;
            if (ReportsController.ParseDate(enddate, userProfile.CultureInfo, out end))
                Session["DashboardEnd"] = end;

            return null;
        }
        private string GetDashboardDateRangeType()
        {
            return (string)Session["DashboardDateRangeType"] ?? "mtd";
        }

        private DateTime? GetDashboardDateRangeStart()
        {
            DateTime now = DateTime.Now;
            string type = GetDashboardDateRangeType();
            switch (type) {
                case "ytd":
                    return new DateTime(now.Year, 1, 1);
                case "mtd":
                    return new DateTime(now.Year, now.Month, 1);
                default:
                    return (DateTime?)Session["DashboardStart"];
            }
        }
        private DateTime? GetDashboardDateRangeEnd()
        {
            DateTime now = DateTime.Now;
            string type = GetDashboardDateRangeType();
            switch (type)
            {
                case "ytd":
                case "mtd":
                    return new DateTime(now.Year, now.Month, now.Day);
                default:
                    return (DateTime?)Session["DashboardEnd"];
            }
        }

        public PartialViewResult Dashboard()
        {
            int? advertiserId = GetAdvertiserId();
            if (advertiserId == null) return null;

            var now = DateTime.Now;

            var firstOfWeek = new DateTime(now.Year, now.Month, now.Day);
            while (firstOfWeek.DayOfWeek != DayOfWeek.Sunday)
            {
                firstOfWeek = firstOfWeek.AddDays(-1);
            }
            var firstOfMonth = new DateTime(now.Year, now.Month, 1);
            var firstOfLastMonth = firstOfMonth.AddMonths(-1);
            var lastOfLastMonth = firstOfMonth.AddDays(-1);
            var firstOfYear = new DateTime(now.Year, 1, 1);

            var oneMonthAgo = new DateTime(firstOfLastMonth.Year, firstOfLastMonth.Month, (now.Day < lastOfLastMonth.Day) ? now.Day : lastOfLastMonth.Day);
            // will be the last day of last month if today's "day" is greater than the number of days in last month

            var summaryWTD = cakeRepo.GetDateRangeSummary(firstOfWeek, now, advertiserId.Value, null);
            summaryWTD.Name = "Week-to-Date";
            var summaryMTD = cakeRepo.GetDateRangeSummary(firstOfMonth, now, advertiserId.Value, null);
            summaryMTD.Name = "Month-to-Date";
            var summaryLMTD = cakeRepo.GetDateRangeSummary(firstOfLastMonth, oneMonthAgo, advertiserId.Value, null);
            summaryLMTD.Name = "Last Month-to-Date";
            var summaryLM = cakeRepo.GetDateRangeSummary(firstOfLastMonth, lastOfLastMonth, advertiserId.Value, null);
            summaryLM.Name = "Last Month-Total";
//            var summaryYTD = cakeRepo.GetDateRangeSummary(firstOfYear, now, advertiserId.Value, null);
//            summaryYTD.Name = "Year-to-Date";

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
                AdvertiserSummaries = new List<DateRangeSummary> { summaryWTD, summaryMTD, summaryLMTD, summaryLM },
                OfferGoalSummaries = offerGoalSummaries,
                DateRangeType = GetDashboardDateRangeType(),
                Start = GetDashboardDateRangeStart(),
                End = GetDashboardDateRangeEnd()
            };
            return PartialView(model);
        }

        public CakeAdvertiser GetAdvertiser()
        {
            int? advId = GetAdvertiserId();
            if (advId.HasValue)
                return cakeRepo.Advertiser(advId.Value);
            else
                return null;
        }

        public static int? GetAdvertiserId()
        {
            int? advertiserId = null;

            var userProfile = GetUserProfile();
            if (userProfile != null)
                advertiserId = userProfile.CakeAdvertiserId;

            return advertiserId;
        }

        public static UserProfile GetUserProfile()
        {
            UserProfile userProfile = null;

            if (WebSecurity.Initialized)
            {
                var userID = WebSecurity.CurrentUserId;
                using (var usersContext = new UsersContext())
                {
                    userProfile = usersContext.UserProfiles.FirstOrDefault(c => c.UserId == userID);
                }
            }
            return userProfile;
        }

        // ---

        public ActionResult Foundation()
        {
            return View();
        }
    }
}
