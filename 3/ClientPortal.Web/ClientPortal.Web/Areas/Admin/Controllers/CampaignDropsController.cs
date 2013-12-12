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

        public ActionResult Create(int campaignid)
        {
            var drop = SetupForCreate(campaignid);
            if (drop == null)
                return HttpNotFound();

            return View(drop);
        }

        private CampaignDrop SetupForCreate(int campaignId)
        {
            var campaign = cpRepo.GetCampaign(campaignId);
            if (campaign == null)
                return null;

            ViewData["Creatives"] = campaign.Offer.CreativesByDate();

            var drop = new CampaignDrop
            {
                Date = DateTime.Today,
                CampaignId = campaign.CampaignId,
                Campaign = campaign
            };
            return drop;
        }

        [HttpPost]
        public ActionResult Create(int campaignid, DateTime? date, int creativeid)
        {
            if (date.HasValue)
            {
                var campaignDrop = cpRepo.AddCampaignDrop(campaignid, date.Value, creativeid, true);
                if (campaignDrop != null)
                    return RedirectToAction("Show", new { id = campaignDrop.CampaignDropId });

                ModelState.AddModelError("", "Campaign Drop could not be saved");
            }
            else
            {
                ModelState.AddModelError("Date", "Date could not be parsed");
            }
            var drop = SetupForCreate(campaignid);
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
                    return RedirectToAction("Show", "CampaignDrops", new { id = drop.CampaignDropId });
            }
            cpRepo.FillExtended_CampaignDrop(drop); // for campaign/affiliate info
            return View(drop);
        }
    }
}
