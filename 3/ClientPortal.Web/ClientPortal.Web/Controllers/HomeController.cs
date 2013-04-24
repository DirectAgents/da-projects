﻿using System;
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
            string advId = advertiserId.ToString();

            var dates = new Dates();

            var summaryWTD = cakeRepo.GetDateRangeSummary(dates.FirstOfWeek, dates.Now, advId, null);
            summaryWTD.Name = "Week-to-Date";
            var summaryMTD = cakeRepo.GetDateRangeSummary(dates.FirstOfMonth, dates.Now, advId, null);
            summaryMTD.Name = "Month-to-Date";
            var summaryLMTD = cakeRepo.GetDateRangeSummary(dates.FirstOfLastMonth, dates.OneMonthAgo, advId, null);
            summaryLMTD.Name = "Last Month-to-Date";
            var summaryLM = cakeRepo.GetDateRangeSummary(dates.FirstOfLastMonth, dates.LastOfLastMonth, advId, null);
            summaryLM.Name = "Last Month-Total";
//            var summaryYTD = cakeRepo.GetDateRangeSummary(dates.FirstOfYear, dates.Now, advId, null);
//            summaryYTD.Name = "Year-to-Date";

            var offerGoalSummaries = CreateOfferGoalSummaries(advertiserId.Value, dates);

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

        public PartialViewResult DashboardGoals()
        {
            int? advertiserId = GetAdvertiserId();
            if (advertiserId == null) return null;

            var dates = new Dates();
            var offerGoalSummaries = CreateOfferGoalSummaries(advertiserId.Value, dates);

            ViewBag.CreateGoalCharts = true;
            return PartialView(offerGoalSummaries);
        }

        public PartialViewResult OfferGoalsRow(int offerId, int? goalId)
        {
            int? advertiserId = GetAdvertiserId();
            if (advertiserId == null) return null;

            var offer = cakeRepo.Offers(advertiserId).Where(o => o.Offer_Id == offerId).FirstOrDefault();
            List<GoalVM> goals;
            if (goalId.HasValue)
            {
                var goal = AccountRepository.GetGoal(goalId.Value, OfferInfo.CurrencyToCulture(offer.Currency));
                goals = new List<GoalVM> { goal };
            }
            else
            {
                goals = AccountRepository.GetGoals(advertiserId.Value, offerId, false, cakeRepo);
            }
            var dates = new Dates();
            var offerGoalSummary = CreateOfferGoalSummary(offer, goals, dates);

            ViewBag.CreateGoalChart = true;
            return PartialView(offerGoalSummary);
        }

        public List<OfferGoalSummary> CreateOfferGoalSummaries(int advId, Dates dates)
        {
            var offers = cakeRepo.Offers(advId);
            var goals = AccountRepository.GetGoals(advId, null, false, cakeRepo);
            var offerIdsFromGoals = goals.Where(g => g.OfferId.HasValue).Select(g => g.OfferId.Value).Distinct().OrderBy(i => i);
            List<OfferGoalSummary> offerGoalSummaries = new List<OfferGoalSummary>();
            foreach (var offerId in offerIdsFromGoals)
            {
                var offer = offers.Where(o => o.Offer_Id == offerId).FirstOrDefault();
                var monthlyGoals = goals.Where(g => g.OfferId == offer.Offer_Id && g.IsMonthly).ToList();
                var customGoals = goals.Where(g => g.OfferId == offer.Offer_Id && !g.IsMonthly).ToList();
                if (monthlyGoals.Any())
                {
                    var offerGoalSummary = CreateOfferGoalSummary(offer, monthlyGoals, dates);
                    offerGoalSummaries.Add(offerGoalSummary);
                }
                if (customGoals.Any())
                {
                    var offerGoalSummary = CreateOfferGoalSummary(offer, customGoals, dates);
                    offerGoalSummaries.Add(offerGoalSummary);
                }
            }
            return offerGoalSummaries;
        }

        public OfferGoalSummary CreateOfferGoalSummary(CakeOffer offer, List<GoalVM> goals, Dates dates)
        {
            var offsumMTD = cakeRepo.GetDateRangeSummary(dates.FirstOfMonth, dates.Now, offer.Advertiser_Id, offer.Offer_Id);
            offsumMTD.Name = "Month-to-Date";
            var offsumLMTD = cakeRepo.GetDateRangeSummary(dates.FirstOfLastMonth, dates.OneMonthAgo, offer.Advertiser_Id, offer.Offer_Id);
            offsumLMTD.Name = "Last Month-to-Date";
            var offsumLM = cakeRepo.GetDateRangeSummary(dates.FirstOfLastMonth, dates.LastOfLastMonth, offer.Advertiser_Id, offer.Offer_Id);
            offsumLM.Name = "Last Month-Total";

            var offerGoalSummary = new OfferGoalSummary()
            {
                Offer = offer,
                Goals = goals,
                DateRangeSummaries = new List<DateRangeSummary> { offsumMTD, offsumLMTD, offsumLM }
            };
            return offerGoalSummary;
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

    public class Dates
    {
        public DateTime Now { get; set; }
        public DateTime FirstOfWeek { get; set; }
        public DateTime FirstOfMonth { get; set; }
        public DateTime FirstOfLastMonth { get; set; }
        public DateTime LastOfLastMonth { get; set; }
        public DateTime FirstOfYear { get; set; }

        public DateTime OneMonthAgo { get; set; }
        // will be the last day of last month if today's "day" is greater than the number of days in last month

        public Dates()
        {
            Now = DateTime.Now;

            FirstOfWeek = new DateTime(Now.Year, Now.Month, Now.Day);
            while (FirstOfWeek.DayOfWeek != DayOfWeek.Sunday)
            {
                FirstOfWeek = FirstOfWeek.AddDays(-1);
            }
            FirstOfMonth = new DateTime(Now.Year, Now.Month, 1);
            FirstOfLastMonth = FirstOfMonth.AddMonths(-1);
            LastOfLastMonth = FirstOfMonth.AddDays(-1);
            FirstOfYear = new DateTime(Now.Year, 1, 1);
            OneMonthAgo = new DateTime(FirstOfLastMonth.Year, FirstOfLastMonth.Month, (Now.Day < LastOfLastMonth.Day) ? Now.Day : LastOfLastMonth.Day);
        }
    }
}
