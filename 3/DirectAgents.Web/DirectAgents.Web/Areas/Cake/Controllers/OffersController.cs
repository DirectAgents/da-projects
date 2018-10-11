using System;
using System.Linq;
using System.Web.Mvc;
using DirectAgents.Domain.Abstract;

namespace DirectAgents.Web.Areas.Cake.Controllers
{
    public class OffersController : DirectAgents.Web.Controllers.ControllerBase
    {
        public OffersController(IMainRepository mainRepository)
        {
            this.daRepo = mainRepository;
        }

        public ActionResult Index(int? advId)
        {
            var offers = daRepo.GetOffers(advertiserId: advId);
            if (offers.Any())
            {
                var firstOffer = offers.First();
                if (advId.HasValue)
                    ViewBag.Advertiser = firstOffer.Advertiser;
            }
            return View(offers);
        }
    }
}