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

            List<OfferInfo> offers;
            using (var cakeContext = new CakeContext()) // TODO: DI
            {
                var offerRepository = new OfferRepository(cakeContext); // TODO: DI

                offers = offerRepository
                            .GetOfferInfos(startdate, enddate)
                            .Where(filter)
                            .OrderByDescending(oi => oi.Revenue).ToList();
            }
            var kgrid = new KendoGrid<OfferInfo>(request, offers);
            var json = Json(kgrid);
            return json;
        }

        [HttpPost]
        public JsonResult DailySummaryGrid(KendoGridRequest request, DateTime? startdate, DateTime? enddate)
        {
            if (!startdate.HasValue) startdate = new DateTime(2013, 2, 1); // for testing!

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
            var kgrid = new KendoGrid<DailyInfo>(request, dailyInfos);
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
