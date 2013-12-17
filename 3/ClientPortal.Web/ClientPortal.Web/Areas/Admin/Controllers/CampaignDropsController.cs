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

        public ActionResult Index(int? offerid, int? campaignid)
        {
            var drops = cpRepo.CampaignDrops(offerid, campaignid);
            return View(drops.OrderBy(d => d.Date));
        }

        public ActionResult Create(int campaignid, string from)
        {
            var drop = SetupForCreate(campaignid, from);
            if (drop == null)
                return HttpNotFound();

            return View(drop);
        }

        private CampaignDrop SetupForCreate(int campaignId, string from)
        {
            var campaign = cpRepo.GetCampaign(campaignId);
            if (campaign == null)
                return null;

            ViewData["Creatives"] = campaign.Offer.CreativesByDate();
            ViewData["from"] = from;

            var drop = new CampaignDrop
            {
                Date = DateTime.Today,
                CampaignId = campaign.CampaignId,
                Campaign = campaign
            };
            return drop;
        }

        [HttpPost]
        public ActionResult Create(int campaignid, DateTime? date, int creativeid, string from)
        {
            if (date.HasValue)
            {
                var campaignDrop = cpRepo.AddCampaignDrop(campaignid, date.Value, creativeid, true);
                if (campaignDrop != null)
                {   // success
                    return RedirectToAction("Show", new { id = campaignDrop.CampaignDropId });
                }
                ModelState.AddModelError("", "Campaign Drop could not be saved");
            }
            else
            {
                ModelState.AddModelError("Date", "Date could not be parsed");
            }
            var drop = SetupForCreate(campaignid, from);
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

            ViewData["Creatives"] = campaignDrop.Campaign.Offer.CreativesByDate();
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
            ViewData["Creatives"] = drop.Campaign.Offer.CreativesByDate();
            return View(drop);
        }

        public ActionResult Delete(int id)
        {
            int? campaignId = cpRepo.DeleteCampaignDrop(id, true);
            if (campaignId == null)
                return HttpNotFound();

            return RedirectToAction("Show", "Campaigns", new { id = campaignId.Value });
        }

        public ActionResult SynchStats(int id)
        {
            var drop = cpRepo.GetCampaignDrop(id);
            if (drop == null)
                return HttpNotFound();

            foreach (var creativeStat in drop.CreativeStats)
            {
                var command = new SynchCreativeSummariesCommand
                {
                    CreativeId = creativeStat.CreativeId,
                    StartDate = creativeStat.Creative.DateCreated.Date
                };
                command.Run(null);
                cpRepo.UpdateCreativeStatFromSummaries(creativeStat.CreativeStatId, true);
            }
            return Content("okay");
            //return RedirectToAction("Show", new { id = id });
        }
    }
}
