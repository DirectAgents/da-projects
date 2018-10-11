using System;
using System.Linq;
using System.Web.Mvc;
using DirectAgents.Domain.Abstract;

namespace DirectAgents.Web.Areas.Cake.Controllers
{
    public class ConversionsController : DirectAgents.Web.Controllers.ControllerBase
    {
        public ConversionsController(IMainRepository mainRepository)
        {
            this.daRepo = mainRepository;
        }

        public ActionResult Index(int? offId, int? affId)
        {
            var eventConvs = daRepo.GetEventConversions(offerId: offId, affiliateId: affId);
            if (eventConvs.Any())
            {
                var firstConv = eventConvs.First();
                if (offId.HasValue)
                    ViewBag.Offer = firstConv.Offer;
                if (affId.HasValue)
                    ViewBag.Affiliate = firstConv.Affiliate;
            }
            return View(eventConvs.OrderBy(x => x.Id));
        }

        public ActionResult AffSum(int? offId)
        {
            var eventConvs = daRepo.GetEventConversions(offerId: offId);
            if (eventConvs.Any())
            {
                var firstConv = eventConvs.First();
                if (offId.HasValue)
                    ViewBag.Offer = firstConv.Offer;
            }
            return View(eventConvs.GroupBy(x => x.Affiliate).OrderBy(x => x.Key.AffiliateName));
        }
    }
}