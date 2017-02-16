using System;
using System.Web.Mvc;
using DirectAgents.Domain.Abstract;
using DirectAgents.Domain.Entities.AB;

namespace DirectAgents.Web.Areas.AB.Controllers
{
    public class CampaignsController : DirectAgents.Web.Controllers.ControllerBase
    {
        public CampaignsController(IABRepository abRepository)
        {
            this.abRepo = abRepository;
        }

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult BucketList(int acctId)
        {
            var clientAccount = abRepo.ClientAccount(acctId);
            if (clientAccount == null)
                return HttpNotFound();

            var campaigns = abRepo.Campaigns(clientId: clientAccount.ClientId);
            ViewBag.Campaigns = campaigns;

            return View(clientAccount);
        }
	}
}