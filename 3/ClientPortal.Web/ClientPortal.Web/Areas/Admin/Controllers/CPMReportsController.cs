using ClientPortal.Data.Contexts;
using ClientPortal.Data.Contracts;
using ClientPortal.Web.Controllers;
using System;
using System.Linq;
using System.Web.Mvc;

namespace ClientPortal.Web.Areas.Admin.Controllers
{
    [Authorize(Users = "admin")]
    public class CPMReportsController : CPController
    {
        public CPMReportsController(IClientPortalRepository cpRepository)
        {
            this.cpRepo = cpRepository;
        }

        public ActionResult Index(int? offerid)
        {
            var reports = cpRepo.CPMReports(offerid);
            return View(reports);
        }

        public ActionResult Create(int offerid)
        {
            var cpmReport = SetupForCreate(offerid);
            if (cpmReport.Offer == null)
                return HttpNotFound();

            return View(cpmReport);
        }

        private CPMReport SetupForCreate(int offerid)
        {
            var cpmReport = new CPMReport
            {
                OfferId = offerid
            };
            SetupForCreate(cpmReport);
            return cpmReport;
        }
        private void SetupForCreate(CPMReport cpmReport)
        {
            cpmReport.Offer = cpRepo.GetOffer(cpmReport.OfferId);
            //if (cpmReport.Offer == null)
            //    return;
            //ViewData["CampaignDrops"] = cpmReport.Offer.Campaigns.SelectMany(c => c.CampaignDrops);
        }

        [HttpPost]
        public ActionResult Create(CPMReport report)
        {
            if (ModelState.IsValid)
            {
                cpRepo.SaveCPMReport(report, true);
                return RedirectToAction("Show", "CPMReports", new { id = report.CPMReportId });
            }
            SetupForCreate(report);
            return View(report);
        }

        public ActionResult Show(int id)
        {
            var report = SetupForView(id);
            if (report == null)
                return HttpNotFound();

            return View(report);
        }

        private CPMReport SetupForView(int cpmReportId)
        {
            var report = cpRepo.GetCPMReport(cpmReportId, true);
            if (report == null)
                return null;

            var dropIds = report.CampaignDrops.Select(cd => cd.CampaignDropId).ToList();
            ViewData["CampaignDrops"] = cpRepo.CampaignDrops(report.OfferId, null)
                                                .Where(cd => !dropIds.Contains(cd.CampaignDropId))
                                                .OrderByDescending(cd => cd.Date);
            return report;
        }

        public ActionResult Edit(int id)
        {
            var report = cpRepo.GetCPMReport(id, true);
            if (report == null)
                return HttpNotFound();

            //ViewData["CampaignDrops"] = cpRepo.CampaignDrops(report.OfferId, null);
            return View(report);
        }

        [HttpPost]
        public ActionResult Edit(CPMReport report)
        {
            if (ModelState.IsValid)
            {
                cpRepo.SaveCPMReport(report, true);
                return RedirectToAction("Show", new { id = report.CPMReportId });
            }
            cpRepo.FillExtended_CPMReport(report);
            //ViewData["CampaignDrops"] = cpRepo.CampaignDrops(report.OfferId, null);
            return View(report);
        }

        [HttpPost]
        public ActionResult AddDrop(int cpmreportid, int campaigndropid)
        {
            bool success = cpRepo.AddDropToCPMReport(cpmreportid, campaigndropid, true);

            return RedirectToAction("Show", new { id = cpmreportid });
        }

        public ActionResult RemoveDrop(int cpmreportid, int campaigndropid)
        {
            bool success = cpRepo.RemoveDropFromCPMReport(cpmreportid, campaigndropid, true);

            return RedirectToAction("Show", new { id = cpmreportid });
        }

        public ActionResult Preview(int id)
        {
            var report = cpRepo.GetCPMReport(id, true);
            if (report == null)
                return HttpNotFound();

            var dropReport = new DropReport(report.CampaignDropsOrdered);
            return View("DropReport", dropReport);
        }
    }
}
