using System.Linq;
using System.Web.Mvc;
using DirectAgents.Domain.Abstract;
using DirectAgents.Domain.Entities.TD;

namespace DirectAgents.Web.Areas.TD.Controllers
{
    public class CampaignsController : DirectAgents.Web.Controllers.ControllerBase
    {
        public CampaignsController(ITDRepository tdRepository)
        {
            this.tdRepo = tdRepository;
        }

        public ActionResult Index(int? advId)
        {
            var campaigns = tdRepo.Campaigns(advId: advId)
                .OrderBy(c => c.Advertiser.Name).ThenBy(c => c.Name);

            Session["advId"] = advId.ToString();
            return View(campaigns);
        }

        public ActionResult CreateNew(int advId)
        {
            var campaign = new Campaign
            {
                AdvertiserId = advId,
                Name = "zNew",
                DefaultBudgetInfo = new BudgetInfoVals
                {
                    MgmtFeePct = 15,
                    MarginPct = 30
                }
            };
            if (tdRepo.AddCampaign(campaign))
                return RedirectToAction("Index", new { advId = Session["advId"] });
            else
                return Content("Error creating Campaign");
        }
        public ActionResult Delete(int id)
        {
            tdRepo.DeleteCampaign(id);
            return RedirectToAction("Index", new { advId = Session["advId"] });
        }

        [HttpGet]
        public ActionResult Edit(int id)
        {
            var campaign = tdRepo.Campaign(id);
            if (campaign == null)
                return HttpNotFound();
            SetupForEdit(id);
            return View(campaign);
        }
        [HttpPost]
        public ActionResult Edit(Campaign camp)
        {
            if (ModelState.IsValid)
            {
                if (tdRepo.SaveCampaign(camp))
                    return RedirectToAction("Edit", new { id = camp.Id });
                    //return RedirectToAction("Index", new { advId = Session["advId"] });
                ModelState.AddModelError("", "Campaign could not be saved.");
            }
            tdRepo.FillExtended(camp);
            SetupForEdit(camp.Id);
            return View(camp);
        }
        private void SetupForEdit(int campId)
        {
            ViewBag.ExtAccounts = tdRepo.ExtAccountsNotInCampaign(campId)
                .OrderBy(a => a.Platform.Name).ThenBy(a => a.Name).ThenBy(a => a.ExternalId);
        }

        public ActionResult AddAccount(int id, int acctId)
        {
            tdRepo.AddExtAccountToCampaign(id, acctId);
            return RedirectToAction("Edit", new { id = id });
        }
        public ActionResult RemoveAccount(int id, int acctId)
        {
            tdRepo.RemoveExtAccountFromCampaign(id, acctId);
            return RedirectToAction("Edit", new { id = id });
        }
    }
}