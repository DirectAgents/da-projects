using CakeExtracter.Commands;
using ClientPortal.Data.Contexts;
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

        public ActionResult Show(int? id, int? offerid, CPMReportWizardVM.WizardStep step = 0)
        {
            // to clear this out after returning from drop wizard:
            Session["FromReportWizard"] = null;

            var model = new CPMReportWizardVM
            {
                Step = step
            };

            if (id.HasValue)
                model.Report = cpRepo.GetCPMReport(id.Value);
            if (offerid.HasValue)
                model.Offer = cpRepo.GetOffer(offerid.Value);

            if (id.HasValue && step == CPMReportWizardVM.WizardStep.AddDrops)
            {
                ViewBag.CampaignDrops = cpRepo.CampaignDropsNotInReport(id.Value)
                                                    .OrderByDescending(cd => cd.Date);
            }

            return View(step.ToString(), model);
        }

        public ActionResult ShowNextStep(int? dropId, int? offerId, CPMReportWizardVM.WizardStep step)
        {
            return Show(dropId, offerId, step + 1);
        }

        public ActionResult SynchCampaigns(int offerid)
        {
            SynchCampaignsCommand.RunStatic(0, offerid);
            return ShowNextStep(null, offerid, CPMReportWizardVM.WizardStep.SynchCampaigns);
        }

        public ActionResult SynchCreatives(int offerid)
        {
            SynchCreativesCommand.RunStatic(0, offerid, false);
            return ShowNextStep(null, offerid, CPMReportWizardVM.WizardStep.SynchCreatives);
        }

        [HttpPost]
        public ActionResult CreateReport(CPMReport report)
        {
            if (ModelState.IsValid)
            {
                cpRepo.SaveCPMReport(report, true);
                return RedirectToAction("Show", new { id = report.CPMReportId, step = (int)(CPMReportWizardVM.WizardStep.CreateReport + 1) });
            }
            return Show(null, report.OfferId, CPMReportWizardVM.WizardStep.CreateReport);
        }

        [HttpPost]
        public ActionResult AddDrop(int cpmreportid, int campaigndropid)
        {
            bool success = cpRepo.AddDropToCPMReport(cpmreportid, campaigndropid, true);
            return RedirectToAction("Show", new { id = cpmreportid, step = (int)CPMReportWizardVM.WizardStep.AddDrops });
        }

        public ActionResult RemoveDrop(int cpmreportid, int campaigndropid)
        {
            bool success = cpRepo.RemoveDropFromCPMReport(cpmreportid, campaigndropid, true);
            return RedirectToAction("Show", new { id = cpmreportid, step = (int)CPMReportWizardVM.WizardStep.AddDrops });
        }

        [HttpPost]
        public ActionResult EditReport(CPMReport report)
        {
            if (ModelState.IsValid)
            {
                cpRepo.SaveCPMReport(report, true);
                return RedirectToAction("Show", "CPMReports", new { id = report.CPMReportId });
            }
            cpRepo.FillExtended_CPMReport(report);

            var model = new CPMReportWizardVM
            {
                Step = CPMReportWizardVM.WizardStep.EditReport,
                Report = report
            };
            return View(model);
        }
    }
}
