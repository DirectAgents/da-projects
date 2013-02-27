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
        public ActionResult Index()
        {
            ViewBag.Message = "Modify this template to jump-start your ASP.NET MVC application.";

            return View();
        }

        [HttpPost]
        public JsonResult OfferSummaryGrid(KendoGridRequest request, DateTime? startdate, DateTime? enddate)
        {
            Func<OfferInfo, bool> filter = (oi => true);
            int? advertiserId = GetAdvertiserId();
            if (advertiserId.HasValue) filter = (oi => oi.AdvertiserId == advertiserId.Value.ToString());

            List<OfferInfo> offerInfos;
            using (var cakeContext = new CakeContext()) // TODO: DI
            {
                var offerRepository = new OfferRepository(cakeContext); // TODO: DI

                offerInfos = offerRepository
                            .GetOfferInfos(startdate, enddate)
                            .Where(filter)
                            .OrderByDescending(oi => oi.Revenue).ToList();
            }
            var kgrid = new KendoGrid<OfferInfo>(request, offerInfos);
            kgrid.aggregates = new
            {
                Clicks = new { sum = offerInfos.Select(i => i.Clicks).Sum() },
                Conversions = new { sum = offerInfos.Select(i => i.Conversions).Sum() },
                Revenue = new { sum = offerInfos.Select(i => i.Revenue).Sum() }
            };
            var json = Json(kgrid);
            return json;
        }

        [HttpPost]
        public JsonResult DailySummaryGrid(KendoGridRequest request, DateTime? startdate, DateTime? enddate)
        {
            var now = DateTime.Now;
            if (!startdate.HasValue) startdate = new DateTime(now.Year, now.Month, 1);
            if (!enddate.HasValue) enddate = now;

            int? advertiserId = GetAdvertiserId();
            if (advertiserId == null) return null;

            List<DailyInfo> dailyInfos;
            using (var cakeContext = new CakeContext()) // TODO: DI
            {
                var offerRepository = new OfferRepository(cakeContext); // TODO: DI

                dailyInfos = offerRepository
                            .GetDailyInfos(startdate, enddate, advertiserId.Value)
                            .OrderBy(di => di.Date).ToList();
            }

            int totalImpressions = dailyInfos.Select(i => i.Impressions).Sum();
            int totalClicks = dailyInfos.Select(i => i.Clicks).Sum();
            int totalConversions = dailyInfos.Select(i => i.Conversions).Sum();
            float totalConversionPct = (totalClicks == 0) ? 0 : (float)Math.Round((double)totalConversions / totalClicks, 3);
            decimal totalRevenue = dailyInfos.Select(i => i.Revenue).Sum();
            decimal totalEPC = (totalClicks == 0) ? 0 : Math.Round(totalRevenue / totalClicks, 2);

            var kgrid = new KendoGrid<DailyInfo>(request, dailyInfos);
            kgrid.aggregates = new
            {
                Impressions = new { sum = totalImpressions },
                Clicks = new { sum = totalClicks },
                Conversions = new { sum = totalConversions },
                ConversionPct = new { agg = totalConversionPct },
                Revenue = new { sum = totalRevenue },
                EPC = new { agg = totalEPC }
            };
            var json = Json(kgrid);
            return json;
        }

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
