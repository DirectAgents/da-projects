using ClientPortal.Data.Contexts;
using ClientPortal.Data.Contracts;
using ClientPortal.Data.DTOs;
using ClientPortal.Data.Services;
using ClientPortal.Web.Models;
using DirectAgents.Mvc.KendoGridBinder;
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
        private IOfferRepository offerRepo;

        public HomeController(IOfferRepository offerRepository)
        {
            this.offerRepo = offerRepository;
        }

        public ActionResult Index()
        {
            ViewBag.Message = "Modify this template to jump-start your ASP.NET MVC application.";

            return View();
        }

        public PartialViewResult OfferSummaryPartial()
        {
            var test = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1).AddDays(-1);
            ViewBag.today = test.ToShortDateString();
//            ViewBag.today = DateTime.Now.ToShortDateString();
            return PartialView("_OfferSummaryPartial");
        }

        [HttpPost]
        public JsonResult OfferSummaryData(KendoGridRequest request, DateTime? startdate, DateTime? enddate)
        {
            int? advertiserId = GetAdvertiserId();

            var offerInfos = offerRepo.GetOfferInfos(startdate, enddate, advertiserId);
            var kgrid = new KendoGrid<OfferInfo>(request, offerInfos);
            if (offerInfos.Any())
            {
                kgrid.aggregates = new
                {
                    Clicks = new { sum = offerInfos.Sum(i => i.Clicks) },
                    Conversions = new { sum = offerInfos.Sum(i => i.Conversions) },
                    Revenue = new { sum = offerInfos.Sum(i => i.Revenue) }
                };
            }
            var json = Json(kgrid);
            return json;
        }

        public PartialViewResult DailySummaryPartial()
        {
            var now = DateTime.Now;
            ViewBag.firstOfMonth = new DateTime(now.Year, now.Month, 1).ToShortDateString(); ;
            ViewBag.today = DateTime.Now.ToShortDateString();
            return PartialView("_DailySummaryPartial");
        }

        [HttpPost]
        public JsonResult DailySummaryData(KendoGridRequest request, DateTime? startdate, DateTime? enddate)
        {
            var now = DateTime.Now;
            if (!startdate.HasValue) startdate = new DateTime(now.Year, now.Month, 1);
            if (!enddate.HasValue) enddate = now;

            int? advertiserId = GetAdvertiserId();
            var dailyInfos = offerRepo.GetDailyInfos(startdate, enddate, advertiserId);
            var kgrid = new KendoGrid<DailyInfo>(request, dailyInfos);

            if (dailyInfos.Any())
            {
                int totalImpressions = dailyInfos.Sum(i => i.Impressions);
                int totalClicks = dailyInfos.Sum(i => i.Clicks);
                int totalConversions = dailyInfos.Sum(i => i.Conversions);
                float totalConversionPct = (totalClicks == 0) ? 0 : (float)Math.Round((double)totalConversions / totalClicks, 3);
                decimal totalRevenue = dailyInfos.Sum(i => i.Revenue);
                decimal totalEPC = (totalClicks == 0) ? 0 : Math.Round(totalRevenue / totalClicks, 2);
                kgrid.aggregates = new
                {
                    Impressions = new { sum = totalImpressions },
                    Clicks = new { sum = totalClicks },
                    Conversions = new { sum = totalConversions },
                    ConversionPct = new { agg = totalConversionPct },
                    Revenue = new { sum = totalRevenue },
                    EPC = new { agg = totalEPC }
                };
            }
            var json = Json(kgrid);
            return json;
        }

        public PartialViewResult CPMSummaryPartial()
        {
            var now = DateTime.Now;
            var firstOfMonth = new DateTime(now.Year, now.Month, 1);
            ViewBag.start = firstOfMonth.AddMonths(-3).ToString("MM/yyyy");
            ViewBag.end = firstOfMonth.AddMonths(-1).ToString("MM/yyyy");
            return PartialView("_CPMSummaryPartial");
        }

        [HttpPost]
        public JsonResult CPMSummaryData(KendoGridRequest request, DateTime? startdate, DateTime? enddate)
        {
            int? advertiserId = GetAdvertiserId();

            var monthlyInfos = offerRepo
                .GetMonthlyInfos("CPM", startdate, enddate, advertiserId)
                .Where(i => i.CampaignStatusId == CampaignStatus.Verified); // TODO: filter by AccountingStatus (or combine into one row)

            var kgrid = new KendoGrid<MonthlyInfo>(request, monthlyInfos);
            if (monthlyInfos.Any())
            {
                kgrid.aggregates = new
                {
                    Revenue = new { sum = monthlyInfos.Sum(i => i.Revenue) }
                };
            }
            var json = Json(kgrid);
            return json;
        }

        public PartialViewResult ConversionReportPartial()
        {
            var test = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1).AddDays(-1);
            ViewBag.today = test.ToShortDateString();
            //ViewBag.today = DateTime.Now.ToShortDateString();
            return PartialView("_ConversionReportPartial");
        }

        [HttpPost]
        public JsonResult ConversionReportData(KendoGridRequest request, DateTime? startdate, DateTime? enddate, int? offerid)
        {
            int? advertiserId = GetAdvertiserId();

            var conversions = offerRepo.GetConversions(startdate, enddate, advertiserId);
            if (offerid.HasValue)
                conversions = conversions.Where(c => c.OfferId == offerid.Value);

            var kgrid = new KendoGrid<ConversionInfo>(request, conversions);
            if (conversions.Any())
            {
                kgrid.aggregates = new
                {
                    PriceReceived = new { sum = conversions.Sum(c => c.PriceReceived) }
                };
            }
            var json = Json(kgrid);
            return json;
        }

        // ---

        private int? GetAdvertiserId()
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
