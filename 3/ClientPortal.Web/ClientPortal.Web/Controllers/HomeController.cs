using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using MoreLinq;
using ClientPortal.Data.Contexts;
using ClientPortal.Data.Contracts;
using ClientPortal.Data.DTOs;
using ClientPortal.Web.Models;
using WebMatrix.WebData;
using StackExchange.Profiling;
using System.Web.Helpers;

namespace ClientPortal.Web.Controllers
{
    [Authorize]
    public class HomeController : CPController
    {
        public HomeController(IClientPortalRepository cpRepository)
        {
            this.cpRepo = cpRepository;
        }

        public ActionResult Index()
        {
            var userInfo = GetUserInfo();
            if (!userInfo.HasUserProfile)
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
                    CultureInfo = userInfo.CultureInfo,
                    HasLogo = (userInfo.Logo != null),
                    ShowCPMRep = userInfo.ShowCPMRep
                };
                return View(model);
            }
        }

        public FileResult Logo()
        {
            var userInfo = GetUserInfo();
            if (userInfo.Logo == null)
                return null;

            WebImage logo = new WebImage(userInfo.Logo);
            return File(logo.GetBytes(), "image/" + logo.ImageFormat, logo.FileName);
        }

        public JsonResult MainContactsData()
        {
            IEnumerable<Contact> contacts = new List<Contact>();

            var advertiser = GetAdvertiser();
            if (advertiser != null)
            {
                contacts = advertiser.AdvertiserContacts.OrderBy(ac => ac.Order).Take(2).Select(ac => ac.Contact);
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
            var userInfo = GetUserInfo();
            Session["DashboardDateRangeType"] = type;

            DateTime? start, end;
            if (ControllerHelpers.ParseDate(startdate, userInfo.CultureInfo, out start))
                Session["DashboardStart"] = start;
            if (ControllerHelpers.ParseDate(enddate, userInfo.CultureInfo, out end))
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
                var userInfo = GetUserInfo();
                if (userInfo.AdvertiserId == null)
                    return null;
                
                int? advId = userInfo.AdvertiserId;
                bool showConversionData = userInfo.ShowConversionData;

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
                    offerGoalSummaries = CreateOfferGoalSummaries(advId.Value, dates, showConversionData);
                }

                var model = new DashboardModel
                {
                    AdvertiserSummaries = new List<DateRangeSummary> { summaryWTD, summaryMTD, summaryLMTD, summaryLM },
                    OfferGoalSummaries = offerGoalSummaries,
                    DateRangeType = GetDashboardDateRangeType(),
                    Start = GetDashboardDateRangeStart(),
                    End = GetDashboardDateRangeEnd(),
                    ShowConVal = showConversionData,
                    ConValName = userInfo.ConversionValueName,
                    ConValIsNum = userInfo.ConversionValueIsNumber
                };
                return PartialView(model);
            }
        }

        public PartialViewResult DashboardGoals()
        {
            var userInfo = GetUserInfo();
            if (userInfo.AdvertiserId == null)
                return null;

            var dates = new Dates();
            var offerGoalSummaries = CreateOfferGoalSummaries(userInfo.AdvertiserId.Value, dates, userInfo.ShowConversionData);

            ViewBag.CreateGoalCharts = true;
            return PartialView(offerGoalSummaries);
        }

        public PartialViewResult OfferGoalsRow(int offerId, int? goalId)
        {
            var userInfo = GetUserInfo();
            if (userInfo.AdvertiserId == null)
                return null;

            var offer = cpRepo.Offers(userInfo.AdvertiserId).Where(o => o.Offer_Id == offerId).FirstOrDefault();
            List<Goal> goals;
            if (goalId.HasValue)
                goals = offer.Goals.Where(g => g.Id == goalId).ToList();
            else
                goals = offer.Goals.ToList();

            var dates = new Dates();
            var offerGoalSummary = CreateOfferGoalSummary(userInfo.AdvertiserId, offer, goals, dates, userInfo.ShowConversionData);

            ViewBag.CreateGoalChart = true;
            return PartialView(offerGoalSummary);
        }

        private List<OfferGoalSummary> CreateOfferGoalSummaries(int advId, Dates dates, bool includeConversionData)
        {
            var goals = cpRepo.GetGoals(advId);
            var offers = goals.Where(g => g.Offer != null).Select(g => g.Offer).DistinctBy(o => o.Offer_Id).OrderBy(o => o.OfferName);
            List<OfferGoalSummary> offerGoalSummaries = new List<OfferGoalSummary>();
            foreach (var offer in offers)
            {
                var monthlyGoals = offer.Goals.Where(g => g.IsMonthly).ToList();
                var customGoals = offer.Goals.Where(g => !g.IsMonthly).ToList();
                if (monthlyGoals.Any())
                {
                    var offerGoalSummary = CreateOfferGoalSummary(advId, offer, monthlyGoals, dates, includeConversionData);
                    offerGoalSummaries.Add(offerGoalSummary);
                }
                if (customGoals.Any())
                {
                    foreach (var goalGroup1 in customGoals.GroupBy(g => g.StartDate))
                    {
                        foreach (var goalGroup2 in goalGroup1.GroupBy(g => g.EndDate))
                        {
                            var offerGoalSummary = CreateOfferGoalSummary(advId, offer, goalGroup2.ToList(), dates, includeConversionData);
                            offerGoalSummaries.Add(offerGoalSummary);
                        }
                    }
                }
            }
            return offerGoalSummaries;
        }

        private OfferGoalSummary CreateOfferGoalSummary(int? advId, Offer offer, List<Goal> goals, Dates dates, bool includeConversionData)
        {
            List<DateRangeSummary> summaries;
            if (!goals.Any() || goals[0].IsMonthly) // assume all goals are the same type
            {
                var offsumMTD = cpRepo.GetDateRangeSummary(dates.FirstOfMonth, dates.Now, advId, offer.Offer_Id, includeConversionData);
                offsumMTD.Name = "Month-to-Date";
                var offsumLMTD = cpRepo.GetDateRangeSummary(dates.FirstOfLastMonth, dates.OneMonthAgo, advId, offer.Offer_Id, includeConversionData);
                offsumLMTD.Name = "Last MTD";
                var offsumLM = cpRepo.GetDateRangeSummary(dates.FirstOfLastMonth, dates.LastOfLastMonth, advId, offer.Offer_Id, includeConversionData);
                offsumLM.Name = "Last Month";
                summaries = new List<DateRangeSummary> { offsumMTD, offsumLMTD, offsumLM };
            }
            else
            {
                var goal0 = goals[0];
                var sumActual = cpRepo.GetDateRangeSummary(goal0.StartDate, goal0.EndDate, advId, offer.Offer_Id, includeConversionData);
                sumActual.Name = (dates.Now.Date > goal0.EndDate) ? "Results" : "Results to-Date";
                var sumGoal = new DateRangeSummary() { Name = "Goal", Culture = OfferInfo.CurrencyToCulture(offer.Currency) };
                foreach (var goal in goals)
                {
                    switch (goal.MetricId)
                    {
                        case Metric.Clicks:
                            sumGoal.Clicks = (int)goal.Target;
                            break;
                        case Metric.Leads:
                            sumGoal.Conversions = (int)goal.Target;
                            break;
                        case Metric.Spend:
                            sumGoal.Revenue = goal.Target;
                            break;
                    }
                }
                summaries = new List<DateRangeSummary> { sumActual, sumGoal };
            }

            var offerGoalSummary = new OfferGoalSummary()
            {
                Offer = offer,
                Goals = goals.Select(g => new GoalVM(g)).ToList(),
                DateRangeSummaries = summaries
            };
            return offerGoalSummary;
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
