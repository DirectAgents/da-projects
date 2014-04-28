using CakeExtracter.Commands;
using ClientPortal.Data.Contexts;
using ClientPortal.Data.Contracts;
using ClientPortal.Web.Controllers;
using System;
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

        public ActionResult Start(int offerid)
        {
            var offer = cpRepo.GetOffer(offerid);
            return View(offer);
        }

        public ActionResult Index(int? offerid, int? campaignid)
        {
            var drops = cpRepo.CampaignDrops(offerid, campaignid, true); // ?do we need to include copies?
            return View(drops.OrderByDescending(d => d.Date));
        }

        public ActionResult Create(int campaignid, string from)
        {
            var drop = SetupForCreate(campaignid, from);
            if (drop == null)
                return HttpNotFound();

            return View(drop);
        }

        private CampaignDrop SetupForCreate(int campaignId, string from, CampaignDrop drop = null)
        {
            var campaign = cpRepo.GetCampaign(campaignId);
            if (campaign == null)
                return null;

            ViewData["from"] = from; // only used for non-ajax page

            if (drop == null)
            {
                drop = new CampaignDrop
                {
                    Date = DateTime.Today,
                };
            }
            drop.CampaignId = campaign.CampaignId;
            drop.Campaign = campaign;

            return drop;
        }

        [HttpPost]
        public ActionResult Create(CampaignDrop drop, int creativeid, string from)
        {
            if (ModelState.IsValid)
            {
                bool success = cpRepo.AddCampaignDrop(drop, creativeid, true);
                if (success)
                    return RedirectToAction("Show", new { id = drop.CampaignDropId });

                ModelState.AddModelError("", "Campaign Drop could not be saved");
            }
            SetupForCreate(drop.CampaignId, from, drop);
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
                    return RedirectToAction("Show", new { id = drop.CampaignDropId });

                ModelState.AddModelError("", "Campaign Drop could not be saved");
            }
            cpRepo.FillExtended_CampaignDrop(drop); // for campaign/affiliate info
            return View(drop);
        }

        public ActionResult Delete(int id)
        {
            //TODO: check to see if drop exists in any reports first
            Campaign campaign = cpRepo.DeleteCampaignDrop(id, true);
            if (campaign == null)
                return HttpNotFound();

            return RedirectToAction("Show", "Offers", new { id = campaign.OfferId });
        }

        public ActionResult DeleteCopy(int id)
        {
            cpRepo.DeleteCampaignDropCopy(id);
            return Content("Drop Copy deleted. Click 'back' and refresh.");
        }

        public ActionResult SynchStats(int id)
        {
            bool success = SynchStatsForDrop(cpRepo, id);
            if (!success)
                return HttpNotFound();

            if (Request.IsAjaxRequest())
                return RedirectToAction("Show", new { id = id });
            else
                return Content("Synch complete. Click 'back' and refresh.");
        }

        public static bool SynchStatsForDrop(IClientPortalRepository cpRepo, int dropId)
        {
            var drop = cpRepo.GetCampaignDrop(dropId);
            if (drop == null)
                return false;

            if (drop.CreativeStats.Any())
            {
                cpRepo.DuplicateDropIfNecessary(drop);

                foreach (var creativeStat in drop.CreativeStats)
                {
                    SynchCreativeSummariesCommand.RunStatic(
                        creativeStat.Creative.DateCreated.Date,
                        null,
                        null,
                        creativeStat.CreativeId
                    );
                    cpRepo.UpdateCreativeStatFromSummaries(creativeStat.CreativeStatId, true);
                }
            }
            return true;
        }

        public static void SynchStatsForAllDrops(IClientPortalRepository cpRepo, int offerId)
        {
            var drops = cpRepo.CampaignDrops(offerId, null);
            if (drops.Any(d => d.CreativeStats.Any()))
            {
                var creativeStats = drops.SelectMany(d => d.CreativeStats).ToList();
                var creatives = creativeStats.Select(cs => cs.Creative).Distinct();
                foreach (var creative in creatives)
                {
                    SynchCreativeSummariesCommand.RunStatic(
                        creative.DateCreated.Date,
                        null,
                        null,
                        creative.CreativeId
                    );
                }
                cpRepo.UpdateCreativeStatsFromSummaries(creativeStats, true);
            }
        }

    }
}
