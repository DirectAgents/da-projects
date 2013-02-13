using ClientPortal.Data.Contexts;
using ClientPortal.Data.Contracts;
using ClientPortal.Data.DTOs;
using ClientPortal.Data.Services;
using DirectAgents.Mvc.KendoGridBinder;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace ClientPortal.Web.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            ViewBag.Message = "Modify this template to jump-start your ASP.NET MVC application.";

            return View();
        }

        [HttpPost]
        public JsonResult OfferSummaryGrid(KendoGridRequest request)
        {
            List<OfferInfo> offers;
            using (var cakeContext = new CakeContext()) // TODO: DI
            {
                var offerRepository = new OfferRepository(cakeContext); // TODO: DI
                offers = offerRepository.GetOfferInfos(new DateTime(2013, 2, 1))//.Where(oi => oi.AdvertiserId == "278")
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
