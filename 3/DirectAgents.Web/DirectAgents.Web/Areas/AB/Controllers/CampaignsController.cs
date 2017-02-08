using System.Linq;
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

        public ActionResult Index(int? clientId, int? accountId)
        {
            var campaigns = abRepo.Campaigns(clientId: clientId, clientAccountId: accountId);
            return View(campaigns);
        }

        public ActionResult New(int accountId)
        {
            var clientAccount = abRepo.ClientAccount(accountId);
            if (clientAccount == null)
                return HttpNotFound();

            var camp = new ProtoCampaign
            {
                Name = "zNew"
            };
            clientAccount.ProtoCampaigns.Add(camp);
            abRepo.SaveChanges();

            return RedirectToAction("Index", new { accountId = accountId });
        }

        [HttpGet]
        public ActionResult Edit(int id)
        {
            var camp = abRepo.Campaign(id);
            if (camp == null)
                return HttpNotFound();
            return View(camp);
        }
        [HttpPost]
        public ActionResult Edit(ProtoCampaign camp)
        {
            if (ModelState.IsValid)
            {
                if (abRepo.SaveCampaign(camp))
                    return RedirectToAction("Index", new { accountId = camp.ClientAccountId });
                ModelState.AddModelError("", "Campaign could not be saved.");
            }
            abRepo.FillExtended(camp);
            return View(camp);
        }
	}
}