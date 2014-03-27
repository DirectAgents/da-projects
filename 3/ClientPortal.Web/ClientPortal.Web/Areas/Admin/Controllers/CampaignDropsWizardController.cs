using ClientPortal.Data.Contexts;
using ClientPortal.Data.Contracts;
using ClientPortal.Web.Areas.Admin.Models;
using ClientPortal.Web.Controllers;
using System;
using System.Web.Mvc;

namespace ClientPortal.Web.Areas.Admin.Controllers
{
    [Authorize(Users = "admin")]
    public class CampaignDropsWizardController : CPController
    {
        public CampaignDropsWizardController(IClientPortalRepository cpRepository)
        {
            this.cpRepo = cpRepository;
        }

        //public ActionResult Show(int offerid, CampaignDropWizardVM.WizardStep step = 0, CampaignDrop drop = null)
        //{
        //    var offer = cpRepo.GetOffer(offerid);
        //    var model = new CampaignDropWizardVM
        //    {
        //        Step = step,
        //        Offer = offer,
        //        CampaignDrop = drop
        //    };

        //    return View(step.ToString(), model);
        //}
        public ActionResult Show(int? id, int? offerid, CampaignDropWizardVM.WizardStep step = 0)
        {
            var model = new CampaignDropWizardVM
            {
                Step = step
            };

            if (id.HasValue)
                model.CampaignDrop = cpRepo.GetCampaignDrop(id.Value);
            if (offerid.HasValue)
                model.Offer = cpRepo.GetOffer(offerid.Value);

            return View(step.ToString(), model);
        }

        public ActionResult ShowNextStep(int? dropId, int? offerId, CampaignDropWizardVM.WizardStep step)
        {
            return Show(dropId, offerId, step + 1);
        }

        [HttpPost]
        public ActionResult ChooseCampaign(int campaignid)
        {
            var drop = cpRepo.AddCampaignDrop(campaignid, DateTime.Today, true);
            if (drop == null)
                return HttpNotFound();

            return RedirectToAction("Show", new { id = drop.CampaignDropId, step = (int)(CampaignDropWizardVM.WizardStep.ChooseCampaign + 1) });
            //return ShowNextStep(offerid, CampaignDropWizardVM.WizardStep.ChooseCampaign, drop);
        }

        [HttpPost]
        public ActionResult Details(CampaignDrop drop)
        {
            if (ModelState.IsValid)
            {
                bool success = cpRepo.SaveCampaignDrop(drop, true);
                if (success)
                    return ShowNextStep(drop.CampaignDropId, null, CampaignDropWizardVM.WizardStep.Details);

                ModelState.AddModelError("", "Campaign Drop could not be saved");
            }
            cpRepo.FillExtended_CampaignDrop(drop); // for campaign/affiliate info

            var model = new CampaignDropWizardVM
            {
                Step = CampaignDropWizardVM.WizardStep.Details,
                CampaignDrop = drop,
            };
            return View(model);
        }

        [HttpPost]
        public ActionResult AddCreative(int campaigndropid, int creativeid)
        {
            cpRepo.AddCreativeStat(campaigndropid, creativeid, true);
            //TODO: check whether saved successfully

            return RedirectToAction("Show", new { id = campaigndropid, step = (int)CampaignDropWizardVM.WizardStep.Creatives, v = DateTime.Now.Ticks });
            //return Show(campaigndropid, null, CampaignDropWizardVM.WizardStep.Creatives);
        }

        public ActionResult RemoveCreative(int campaigndropid, int creativestatid)
        {
            cpRepo.DeleteCreativeStat(creativestatid, true);
            return RedirectToAction("Show", new { id = campaigndropid, step = (int)CampaignDropWizardVM.WizardStep.Creatives, v = DateTime.Now.Ticks });
        }

        public ActionResult SynchCreativeStats(int id)
        {
            CampaignDropsController.SynchStatsForDrop(cpRepo, id);
            //TODO: handle an error in synching
            return RedirectToAction("Show", new { id = id, step = (int)CampaignDropWizardVM.WizardStep.FinalReview, v = DateTime.Now.Ticks });
        }

    }
}
