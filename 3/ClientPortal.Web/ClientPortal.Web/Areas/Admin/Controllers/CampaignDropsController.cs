using ClientPortal.Data.Contexts;
using ClientPortal.Data.Contracts;
using ClientPortal.Web.Controllers;
using System.Linq;
using System.Web.Mvc;

namespace ClientPortal.Web.Areas.Admin.Controllers
{
    [Authorize(Users = "admin")]
    public class CampaignDropsController : CPController
    {
        public CampaignDropsController(IClientPortalRepository cpRepository)
        {
            this.cpRepo = cpRepository;
        }

        public ActionResult Index(int? offerid, int? campaignid)
        {
            var drops = cpRepo.CampaignDrops(offerid, campaignid);
            return View(drops.OrderBy(d => d.Date));
        }

        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Create(CampaignDrop drop)
        {
            if (ModelState.IsValid)
            {
                //db.Advertisers.Add(advertiser);
                //db.SaveChanges();
                //return RedirectToAction("Index");
            }

            return View(drop);
        }

        public ActionResult Show(int id)
        {
            var campaignDrop = cpRepo.GetCampaignDrop(id);

            if (campaignDrop == null)
                return HttpNotFound();

            return View(campaignDrop);
        }

        public ActionResult Edit(int id)
        {
            var campaignDrop = cpRepo.GetCampaignDrop(id);

            if (campaignDrop == null)
                return HttpNotFound();

            return View(campaignDrop);
        }

        [HttpPost]
        public ActionResult Edit(CampaignDrop drop)
        {
            if (ModelState.IsValid)
            {
                bool success = cpRepo.SaveCampaignDrop(drop, true);
                if (success)
                    return RedirectToAction("Show", "CampaignDrops", new { id = drop.CampaignDropId });
            }
            cpRepo.FillExtended_CampaignDrop(drop); // for campaign/affiliate info
            return View(drop);
        }
    }
}
