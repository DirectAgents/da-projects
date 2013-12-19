using CakeExtracter.Commands;
using ClientPortal.Data.Contexts;
using ClientPortal.Data.Contracts;
using ClientPortal.Web.Controllers;
using System.Linq;
using System.Web.Mvc;

namespace ClientPortal.Web.Areas.Admin.Controllers
{
    [Authorize(Users = "admin")]
    public class OffersController : CPController
    {
        public OffersController(IClientPortalRepository cpRepository)
        {
            this.cpRepo = cpRepository;
        }

        public ActionResult Index(int? am, int? advertiserid, int? mincampaigns)
        {
            var offers = cpRepo.Offers(am, advertiserid, true, mincampaigns);

            offers = offers
                .OrderBy(o => o.Advertiser.AdvertiserName)
                .ThenBy(o => o.OfferName);

            return View(offers);
        }

        public ActionResult Show(int id)
        {
            var offer = cpRepo.GetOffer(id);

            return View(offer);
        }

        public ActionResult SynchCampaigns(int offerid)
        {
            var cmd = new SynchCampaignsCommand
            {
                OfferId = offerid
            };
            cmd.Run(null);

            return Content("okay");
        }

        public ActionResult SynchCreatives(int offerid)
        {
            var cmd = new SynchCreativesCommand
            {
                OfferId = offerid
            };
            cmd.Run(null);

            return Content("okay");
        }

    }
}
