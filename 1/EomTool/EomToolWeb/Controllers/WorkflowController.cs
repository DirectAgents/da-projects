using EomTool.Domain.Abstract;
using EomTool.Domain.Entities;
using EomToolWeb.Models;
using System.Linq;
using System.Web.Mvc;

namespace EomToolWeb.Controllers
{
    public class WorkflowController : Controller
    {
        private IMainRepository mainRepo;
        private ISecurityRepository securityRepo;
        private IDAMain1Repository daMain1Repo;
        private IEomEntitiesConfig eomEntitiesConfig;

        public WorkflowController(IMainRepository mainRepository, ISecurityRepository securityRepository, IDAMain1Repository daMain1Repository, IEomEntitiesConfig eomEntitiesConfig)
        {
            this.mainRepo = mainRepository;
            this.securityRepo = securityRepository;
            this.daMain1Repo = daMain1Repository;
            this.eomEntitiesConfig = eomEntitiesConfig;
        }

        public ActionResult CampaignAmounts(int? am)
        {
            var campaignAmounts = mainRepo.CampaignAmounts(am, null)
                .OrderBy(ca => ca.AdvertiserName)
                .ThenBy(ca => ca.CampaignName);

            return View(campaignAmounts);
        }

        public ActionResult CampaignAffiliateAmounts(int advId)
        {
            var advertiser = mainRepo.GetAdvertiser(advId);
            var model = new CampaignAffiliateAmountsModel()
            {
                AdvertiserName = advertiser.name,
                CampaignAmounts = mainRepo.CampaignAmounts(null, advId, true)
            };
            return View(model);
        }

        [HttpGet]
        public ActionResult EditCampaign(int pid)
        {
            var campaign = mainRepo.GetCampaign(pid);
            if (campaign == null)
                return Content("Campaign not found");
            return View(campaign);
        }

        [HttpPost]
        public ActionResult EditCampaign(Campaign campaign)
        {
            if (mainRepo.SaveCampaign(campaign))
                return Content("Saved. You may now close this tab.");
            else
                return null;
        }

    }
}
