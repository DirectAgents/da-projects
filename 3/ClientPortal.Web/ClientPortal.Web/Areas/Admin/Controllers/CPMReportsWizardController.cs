using CakeExtracter.Commands;
using ClientPortal.Data.Contracts;
using ClientPortal.Web.Areas.Admin.Models;
using ClientPortal.Web.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ClientPortal.Web.Areas.Admin.Controllers
{
    [Authorize(Users = "admin")]
    public class CPMReportsWizardController : CPController
    {
        public CPMReportsWizardController(IClientPortalRepository cpRepository)
        {
            this.cpRepo = cpRepository;
        }

        public ActionResult Show(int offerid, CPMReportWizardVM.WizardStep step = 0)
        {
            var offer = cpRepo.GetOffer(offerid);
            var model = new CPMReportWizardVM
            {
                Offer = offer,
                Step = step
            };

            return View(step.ToString(), model);
        }

        public ActionResult ShowNextStep(int offerId, CPMReportWizardVM.WizardStep step)
        {
            return Show(offerId, step + 1);
        }

        public ActionResult SynchCampaigns(int offerid)
        {
            SynchCampaignsCommand.RunStatic(0, offerid);
            return ShowNextStep(offerid, CPMReportWizardVM.WizardStep.SynchCampaigns);
        }

        public ActionResult SynchCreatives(int offerid)
        {
            SynchCreativesCommand.RunStatic(0, offerid, false);
            return ShowNextStep(offerid, CPMReportWizardVM.WizardStep.SynchCreatives);
        }

        public ActionResult CreateReport(string reportname)
        {
            return null;
        }
    }
}
