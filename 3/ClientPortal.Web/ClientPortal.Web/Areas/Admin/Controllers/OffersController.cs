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
        private ClientPortalContext db = new ClientPortalContext();

        public OffersController(IClientPortalRepository cpRepository)
        {
            this.cpRepo = cpRepository;
        }

        public ActionResult Index(int? mincampaigns)
        {
            var offers = cpRepo.Offers(true);

            if (mincampaigns.HasValue)
                offers = offers.Where(o => o.Campaigns.Count >= mincampaigns.Value);

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

    }
}
