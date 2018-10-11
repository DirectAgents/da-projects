using System;
using System.Linq;
using System.Web.Mvc;
using DirectAgents.Domain.Abstract;

namespace DirectAgents.Web.Areas.Cake.Controllers
{
    public class CampsController : DirectAgents.Web.Controllers.ControllerBase
    {
        public CampsController(IMainRepository mainRepository)
        {
            this.daRepo = mainRepository;
        }

        public ActionResult Index(int? offId)
        {
            var camps = daRepo.GetCamps(offerId: offId);
            if (camps.Any())
            {
                var firstCamp = camps.First();
                if (offId.HasValue)
                    ViewBag.Offer = firstCamp.Offer;
            }
            return View(camps.OrderBy(x => x.Affiliate.AffiliateName).ThenBy(x => x.CampaignId));
        }
    }
}