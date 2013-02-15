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

            // TODO: encapsulate this logic
            if (WebSecurity.Initialized) 
            {
                var userID = WebSecurity.CurrentUserId;
                using (var usersContext = new UsersContext())
                {
                    var userProfile = usersContext.UserProfiles.FirstOrDefault(c => c.UserId == userID);
                    if (userProfile != null && userProfile.CakeAdvertiserId != null)
                        filter = (io => io.AdvertiserId == userProfile.CakeAdvertiserId.Value.ToString()); // TODO: advertiser id shouldn't be a string
                }
            }

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
