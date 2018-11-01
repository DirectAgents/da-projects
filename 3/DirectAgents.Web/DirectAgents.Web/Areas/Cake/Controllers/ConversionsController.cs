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

        public ActionResult Index(int? offId, int? affId, DateTime? start, DateTime? end)
        {
            var today = DateTime.Today;
            start = start ?? new DateTime(today.Year, today.Month, 1);

            var eventConvs = daRepo.GetEventConversions(offerId: offId, affiliateId: affId, startDate: start, endDate: end);
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

        public ActionResult AffSum(int? offId, DateTime? start, DateTime? end)
        {
            var today = DateTime.Today;
            start = start ?? new DateTime(today.Year, today.Month, 1);

            var eventConvs = daRepo.GetEventConversions(offerId: offId, startDate: start, endDate: end);
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