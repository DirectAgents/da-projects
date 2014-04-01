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

        public ActionResult Show(int? id, int? offerid, CampaignDropWizardVM.WizardStep step = 0, bool? fromreportwizard = null)
        {
            if (!fromreportwizard.HasValue)
                fromreportwizard = (bool?)Session["FromReportWizard"];

            var model = new CampaignDropWizardVM
            {
                Step = step,
                FromReportWizard = fromreportwizard ?? false
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
        public ActionResult ChooseCampaign(int campaignid, bool fromreportwizard = false)
        {
            Session["FromReportWizard"] = fromreportwizard;

            var drop = cpRepo.AddCampaignDrop(campaignid, DateTime.Today, true);
            if (drop == null)
                return HttpNotFound();

            return RedirectToAction("Show", new { id = drop.CampaignDropId, step = (int)(CampaignDropWizardVM.WizardStep.ChooseCampaign + 1) });
            //return ShowNextStep(offerid, CampaignDropWizardVM.WizardStep.ChooseCampaign, drop);
        }

        [HttpPost]
        public ActionResult Details(CampaignDrop drop)
        {
            return SaveDrop(drop, CampaignDropWizardVM.WizardStep.Details);
        }

        private ActionResult SaveDrop(CampaignDrop drop, CampaignDropWizardVM.WizardStep step, CampaignDropWizardVM.WizardStep? nextStep = null)
        {
            if (ModelState.IsValid)
            {
                bool success = cpRepo.SaveCampaignDrop(drop, true);
                if (success)
                {
                    if (nextStep == null)
                        return RedirectToAction("Show", new { id = drop.CampaignDropId, step = (int)(step + 1) });
                    else
                        return RedirectToAction("Show", new { id = drop.CampaignDropId, step = (int)(nextStep.Value) });
                }
                ModelState.AddModelError("", "Campaign Drop could not be saved");
            }
            cpRepo.FillExtended_CampaignDrop(drop); // for campaign/affiliate info

            var model = new CampaignDropWizardVM
            {
                Step = step,
                CampaignDrop = drop,
            };
            return View(step.ToString(), model);
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

        [HttpPost]
        public ActionResult FinalReviewEdit(CampaignDrop drop)
        {
            return SaveDrop(drop, CampaignDropWizardVM.WizardStep.FinalReviewEdit, CampaignDropWizardVM.WizardStep.FinalReview);
        }

        public ActionResult CreativeThumbnail(int campaigndropid, int creativeid)
        {
            var model = new CampaignDropWizardVM
            {
                Creative = cpRepo.GetCreative(creativeid),
                CampaignDrop = cpRepo.GetCampaignDrop(campaigndropid) //note: only the id is used; ?allow to just set id in the VM?
            };
            return View(model);
        }

    }
}
