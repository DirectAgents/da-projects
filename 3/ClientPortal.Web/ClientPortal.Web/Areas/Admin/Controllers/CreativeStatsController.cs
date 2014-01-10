using ClientPortal.Data.Contexts;
using ClientPortal.Data.Contracts;
using ClientPortal.Web.Controllers;
using System;
using System.Linq;
using System.Web.Mvc;

namespace ClientPortal.Web.Areas.Admin.Controllers
{
    [Authorize(Users = "admin")]
    public class CreativeStatsController : CPController
    {
        public CreativeStatsController(IClientPortalRepository cpRepository)
        {
            this.cpRepo = cpRepository;
        }

        public ActionResult Create(int campaigndropid)
        {
            var creativeStat = SetupForCreate(campaigndropid);
            if (creativeStat.CampaignDrop == null)
                return HttpNotFound();

            return View(creativeStat);
        }

        private CreativeStat SetupForCreate(int campaigndropid)
        {
            var creativeStat = new CreativeStat
            {
                CampaignDropId = campaigndropid
            };
            SetupForCreate(creativeStat);
            return creativeStat;
        }
        private void SetupForCreate(CreativeStat creativeStat)
        {
            creativeStat.CampaignDrop = cpRepo.GetCampaignDrop(creativeStat.CampaignDropId);
            if (creativeStat.CampaignDrop == null)
                return;

            var creativeIds = creativeStat.CampaignDrop.CreativeStats.Select(cs => cs.CreativeId).ToList();
            ViewData["Creatives"] = creativeStat.CampaignDrop.Campaign.Offer.Creatives
                                            .Where(c => !creativeIds.Contains(c.CreativeId))
                                            .OrderByDescending(c => c.DateCreated);
        }

        [HttpPost]
        public ActionResult Create(CreativeStat stat)
        {
            if (ModelState.IsValid)
            {
                if (cpRepo.AddCreativeStat(stat, true))
                    return RedirectToAction("Show", "CampaignDrops", new { id = stat.CampaignDropId });

                ModelState.AddModelError("", "CreativeStat could not be saved");
            }
            SetupForCreate(stat);
            return View(stat);
        }

        public ActionResult Delete(int id)
        {
            int? campaignDropId = cpRepo.DeleteCreativeStat(id, true);
            if (campaignDropId == null)
                return HttpNotFound();

            return RedirectToAction("Show", "CampaignDrops", new { id = campaignDropId.Value });
        }
    }
}
