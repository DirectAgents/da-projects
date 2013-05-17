﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using ClientPortal.Data.Contexts;
using ClientPortal.Data.Contracts;
using ClientPortal.Data.DTOs;
using ClientPortal.Web.Models;
using WebMatrix.WebData;
using StackExchange.Profiling;

namespace ClientPortal.Web.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private ICakeRepository cakeRepo;
        private IClientPortalRepository cpRepo;

        public HomeController(ICakeRepository cakeRepository, IClientPortalRepository cpRepository)
        {
            this.cakeRepo = cakeRepository;
            this.cpRepo = cpRepository;
        }

        public ActionResult Index()
        {
            var userProfile = GetUserProfile();
            if (userProfile == null)
            {
                try
                {
                    WebSecurity.Logout();
                }
                catch
                {
                }
                return RedirectToAction("Login", "Account");
            }
            var profiler = MiniProfiler.Current;
            using (profiler.Step("Index"))
            {
                var model = new IndexModel()
                {
                    CultureInfo = userProfile.CultureInfo,
                    ShowCPMRep = userProfile.ShowCPMRep
                };

                if (userProfile.CakeAdvertiserId.HasValue)
                {
                    int advId = userProfile.CakeAdvertiserId.Value;
                    model.Advertiser = cakeRepo.Advertiser(advId);

                    var advertiser = cpRepo.GetAdvertiser(advId);
                    if (advertiser != null)
                        model.LogoImage = advertiser.LogoFilename;
                }

                return View(model);
            }
        }

        public JsonResult MainContactsData()
        {
            IEnumerable<Contact> contacts = new List<Contact>();

            var advertiser = GetAdvertiser();
            if (advertiser != null)
            {
                contacts = advertiser.AdvertiserContacts.OrderBy(ac => ac.Order).Select(ac => ac.Contact);
            }
            var result = new JsonResult
            {
                JsonRequestBehavior = JsonRequestBehavior.AllowGet,
                Data = contacts
            };
            return result;
        }

        public ActionResult Contact()
        {
            var advertiser = GetAdvertiser();
            IEnumerable<Contact> contacts = new List<Contact>();
            if (advertiser != null)
                contacts = advertiser.AdvertiserContacts.OrderBy(ac => ac.Order).Select(ac => ac.Contact);

            return PartialView(contacts);
        }

        public ActionResult SetDashboardDateRange(string type, string startdate, string enddate)
        {
            var userProfile = GetUserProfile();
            Session["DashboardDateRangeType"] = type;

            DateTime? start, end;
            if (ControllerHelpers.ParseDate(startdate, userProfile.CultureInfo, out start))
                Session["DashboardStart"] = start;
            if (ControllerHelpers.ParseDate(enddate, userProfile.CultureInfo, out end))
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
            switch (type)
            {
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
            var profiler = MiniProfiler.Current;
            using (profiler.Step("Dashboard"))
            {
                var userProfile = GetUserProfile();
                if (userProfile == null || userProfile.CakeAdvertiserId == null)
                    return null;
                
                string advId = userProfile.CakeAdvertiserId.ToString();
                bool showConversionData = userProfile.ShowConversionData;

                var dates = new Dates();

                DateRangeSummary summaryWTD = null;
                DateRangeSummary summaryMTD = null;
                DateRangeSummary summaryLMTD = null;
                DateRangeSummary summaryLM = null;

                using (profiler.Step("summaryWTD"))
                {
                    summaryWTD = cpRepo.GetDateRangeSummary(dates.FirstOfWeek, dates.Now, advId, null, showConversionData);
                    summaryWTD.Name = "Week-to-Date";
                    summaryWTD.Link = "javascript: jumpToOffSumRep('wtd')";
                }

                using (profiler.Step("summaryLMTD"))
                {
                    summaryLMTD = cpRepo.GetDateRangeSummary(dates.FirstOfLastMonth, dates.OneMonthAgo, advId, null, showConversionData);
                    summaryLMTD.Name = "Last MTD";
                    summaryLMTD.Link = "javascript: jumpToOffSumRep('lmtd')";
                }

                using (profiler.Step("summaryMTD"))
                {
                    summaryMTD = cpRepo.GetDateRangeSummary(dates.FirstOfMonth, dates.Now, advId, null, showConversionData);
                    summaryMTD.Name = "Month-to-Date";
                    summaryMTD.Link = "javascript: jumpToOffSumRep('mtd')";
                    summaryMTD.PctChg_Clicks = DateRangeSummary.ComputePercentChange(summaryLMTD.Clicks, summaryMTD.Clicks);
                    summaryMTD.PctChg_Conversions = DateRangeSummary.ComputePercentChange(summaryLMTD.Conversions, summaryMTD.Conversions);
                    summaryMTD.Chg_ConversionRate = DateRangeSummary.ComputeChange(summaryLMTD.ConversionRate, summaryMTD.ConversionRate);
                    summaryMTD.PctChg_Revenue = DateRangeSummary.ComputePercentChange(summaryLMTD.Revenue, summaryMTD.Revenue);
                    summaryMTD.PctChg_ConVal = DateRangeSummary.ComputePercentChange(summaryLMTD.ConVal, summaryMTD.ConVal);
                }

                using (profiler.Step("summaryLM"))
                {
                    summaryLM = cpRepo.GetDateRangeSummary(dates.FirstOfLastMonth, dates.LastOfLastMonth, advId, null, showConversionData);
                    summaryLM.Name = "Last Month";
                    summaryLM.Link = "javascript: jumpToOffSumRep('lmt')";
                }
                //            var summaryYTD = cpRepo.GetDateRangeSummary(dates.FirstOfYear, dates.Now, advId, null);
                //            summaryYTD.Name = "Year-to-Date";

                List<OfferGoalSummary> offerGoalSummaries;
                using (profiler.Step("offerGoalSummaries"))
                {
                    offerGoalSummaries = CreateOfferGoalSummaries(userProfile.CakeAdvertiserId.Value, dates, showConversionData);
                }

                var model = new DashboardModel
                {
                    AdvertiserSummaries = new List<DateRangeSummary> { summaryWTD, summaryMTD, summaryLMTD, summaryLM },
                    OfferGoalSummaries = offerGoalSummaries,
                    DateRangeType = GetDashboardDateRangeType(),
                    Start = GetDashboardDateRangeStart(),
                    End = GetDashboardDateRangeEnd(),
                    ShowConVal = userProfile.ShowConversionData,
                    ConValName = userProfile.ConversionValueName,
                    ConValIsNum = userProfile.ConversionValueIsNumber
                };
                return PartialView(model);
            }
        }

        public PartialViewResult DashboardGoals()
        {
            var userProfile = GetUserProfile();
            if (userProfile == null || userProfile.CakeAdvertiserId == null)
                return null;

            var dates = new Dates();
            var offerGoalSummaries = CreateOfferGoalSummaries(userProfile.CakeAdvertiserId.Value, dates, userProfile.ShowConversionData);

            ViewBag.CreateGoalCharts = true;
            return PartialView(offerGoalSummaries);
        }

        public PartialViewResult OfferGoalsRow(int offerId, int? goalId)
        {
            var userProfile = GetUserProfile();
            if (userProfile == null || userProfile.CakeAdvertiserId == null)
                return null;

            var offer = cakeRepo.Offers(userProfile.CakeAdvertiserId).Where(o => o.Offer_Id == offerId).FirstOrDefault();
            List<GoalVM> goalVMs;
            if (goalId.HasValue)
            {
                GoalVM goalVM = null;
                var goal = cpRepo.GetGoal(goalId.Value);
                if (goal != null)
                    goalVM = new GoalVM(goal, null, OfferInfo.CurrencyToCulture(offer.Currency));
                goalVMs = new List<GoalVM> { goalVM };
            }
            else
            {
                var goals = cpRepo.GetGoals(userProfile.CakeAdvertiserId.Value);
                goalVMs = AccountRepository.GetGoalVMs(goals.ToList(), new[] { offer }, false);
            }
            var dates = new Dates();
            var offerGoalSummary = CreateOfferGoalSummary(offer, goalVMs, dates, userProfile.ShowConversionData);

            ViewBag.CreateGoalChart = true;
            return PartialView(offerGoalSummary);
        }

        public List<OfferGoalSummary> CreateOfferGoalSummaries(int advId, Dates dates, bool includeConversionData)
        {
            var offers = cakeRepo.Offers(advId);
            var goals = cpRepo.GetGoals(advId);
            var goalVMs = AccountRepository.GetGoalVMs(goals.ToList(), offers.ToList(), false);
            var offerIdsFromGoals = goalVMs.Where(g => g.OfferId.HasValue).Select(g => g.OfferId.Value).Distinct().OrderBy(i => i);
            List<OfferGoalSummary> offerGoalSummaries = new List<OfferGoalSummary>();
            foreach (var offerId in offerIdsFromGoals)
            {
                var offer = offers.Where(o => o.Offer_Id == offerId).FirstOrDefault();
                var monthlyGoals = goalVMs.Where(g => g.OfferId == offer.Offer_Id && g.IsMonthly).ToList();
                var customGoals = goalVMs.Where(g => g.OfferId == offer.Offer_Id && !g.IsMonthly).ToList();
                if (monthlyGoals.Any())
                {
                    var offerGoalSummary = CreateOfferGoalSummary(offer, monthlyGoals, dates, includeConversionData);
                    offerGoalSummaries.Add(offerGoalSummary);
                }
                if (customGoals.Any())
                {
                    foreach (var goalGroup1 in customGoals.GroupBy(g => g.StartDateParsed))
                    {
                        foreach (var goalGroup2 in goalGroup1.GroupBy(g => g.EndDateParsed))
                        {
                            var offerGoalSummary = CreateOfferGoalSummary(offer, goalGroup2.ToList(), dates, includeConversionData);
                            offerGoalSummaries.Add(offerGoalSummary);
                        }
                    }
                }
            }
            return offerGoalSummaries;
        }

        public OfferGoalSummary CreateOfferGoalSummary(CakeOffer offer, List<GoalVM> goals, Dates dates, bool includeConversionData)
        {
            List<DateRangeSummary> summaries;
            if (!goals.Any() || goals[0].IsMonthly) // assume all goals are the same type
            {
                var offsumMTD = cpRepo.GetDateRangeSummary(dates.FirstOfMonth, dates.Now, offer.Advertiser_Id, offer.Offer_Id, includeConversionData);
                offsumMTD.Name = "Month-to-Date";
                var offsumLMTD = cpRepo.GetDateRangeSummary(dates.FirstOfLastMonth, dates.OneMonthAgo, offer.Advertiser_Id, offer.Offer_Id, includeConversionData);
                offsumLMTD.Name = "Last MTD";
                var offsumLM = cpRepo.GetDateRangeSummary(dates.FirstOfLastMonth, dates.LastOfLastMonth, offer.Advertiser_Id, offer.Offer_Id, includeConversionData);
                offsumLM.Name = "Last Month";
                summaries = new List<DateRangeSummary> { offsumMTD, offsumLMTD, offsumLM };
            }
            else
            {
                var goal0 = goals[0];
                var sumActual = cpRepo.GetDateRangeSummary(goal0.StartDateParsed.Value, goal0.EndDateParsed.Value, offer.Advertiser_Id, offer.Offer_Id, includeConversionData);
                sumActual.Name = (dates.Now.Date > goal0.EndDateParsed) ? "Results" : "Results to-Date";
                var sumGoal = new DateRangeSummary() { Name = "Goal", Culture = goal0.Culture };
                foreach (var goal in goals)
                {
                    switch (goal.MetricId)
                    {
                        case MetricEnum.Clicks:
                            sumGoal.Clicks = (int)goal.Target;
                            break;
                        case MetricEnum.Leads:
                            sumGoal.Conversions = (int)goal.Target;
                            break;
                        case MetricEnum.Spend:
                            sumGoal.Revenue = goal.Target;
                            break;
                    }
                }
                summaries = new List<DateRangeSummary> { sumActual, sumGoal };
            }

            var offerGoalSummary = new OfferGoalSummary()
            {
                Offer = offer,
                Goals = goals,
                DateRangeSummaries = summaries
            };
            return offerGoalSummary;
        }

        public Advertiser GetAdvertiser()
        {
            int? advId = GetAdvertiserId();
            if (advId.HasValue)
                return cpRepo.GetAdvertiser(advId.Value);
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
