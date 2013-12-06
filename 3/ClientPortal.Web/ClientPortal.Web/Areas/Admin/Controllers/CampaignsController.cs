using ClientPortal.Data.Contexts;
using ClientPortal.Data.Contracts;
using ClientPortal.Web.Controllers;
using System.Linq;
using System.Web.Mvc;

namespace ClientPortal.Web.Areas.Admin.Controllers
{
    [Authorize(Users = "admin")]
    public class CampaignsController : CPController
    {
        private ClientPortalContext db = new ClientPortalContext();

        public CampaignsController(IClientPortalRepository cpRepository)
        {
            this.cpRepo = cpRepository;
        }

        public ActionResult Index(int? offerId)
        {
            var campaigns = cpRepo.Campaigns(offerId, true)
                .OrderBy(c => c.OfferId)
                .ThenBy(c => c.CampaignName)
                .ThenBy(c => c.Affiliate.AffiliateName);

            return View(campaigns);
        }

        public ActionResult Show(int id)
        {
            var campaign = cpRepo.GetCampaign(id);

            return View(campaign);
        }
    }
}