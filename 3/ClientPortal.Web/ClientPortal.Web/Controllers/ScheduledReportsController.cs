﻿using ClientPortal.Data.Contexts;
using ClientPortal.Data.Contracts;
using ClientPortal.Web.Models;
using System.Web.Mvc;
using System.Linq;
using System.Collections.Generic;

namespace ClientPortal.Web.Controllers
{
    public class ScheduledReportsController : CPController
    {
        public ScheduledReportsController(IClientPortalRepository cpRepository)
        {
            this.cpRepo = cpRepository;
        }

        public ActionResult Index()
        {
            var advId = GetAdvertiserId();
            if (advId == null) return null;
            var reps = cpRepo.GetScheduledReports(advId.Value).ToList().Select(sr => new ScheduledReportVM(sr));
            return PartialView(reps);
        }

        public ActionResult List()
        {
            var advId = GetAdvertiserId();
            if (advId == null) return null;
            var reps = cpRepo.GetScheduledReports(advId.Value).ToList().Select(sr => new ScheduledReportVM(sr));
            return PartialView(reps);
        }

        public ActionResult Add()
        {
            var defaultScheduledReport = new ScheduledReportVM();
            return DoEdit(defaultScheduledReport);
        }

        public ActionResult Edit(int id)
        {
            var scheduledReport = cpRepo.GetScheduledReport(id);
            var scheduledReportVM = new ScheduledReportVM(scheduledReport);
            return DoEdit(scheduledReportVM);
        }

        private ActionResult DoEdit(ScheduledReportVM scheduledReportVM)
        {
            return PartialView("Edit", scheduledReportVM);
        }

        public ActionResult Save(ScheduledReportVM scheduledReportVM)
        {
            bool success = false;
            int? advId = GetAdvertiserId();

            if (ModelState.IsValid)
            {
                ScheduledReport rep;
                List<ScheduledReportRecipient> recipientsToDelete;
                if (scheduledReportVM.Id < 0)
                {
                    rep = new ScheduledReport() { AdvertiserId = advId.HasValue ? advId.Value : 0 };
                    scheduledReportVM.SetEntityProperties(rep, out recipientsToDelete);
                    cpRepo.AddScheduledReport(rep);
                    cpRepo.SaveChanges();
                    success = true;
                }
                else
                {
                    rep = cpRepo.GetScheduledReport(scheduledReportVM.Id);
                    if (rep != null && (advId == null || advId.Value == rep.AdvertiserId))
                    {
                        scheduledReportVM.SetEntityProperties(rep, out recipientsToDelete);
                        foreach (var recipient in recipientsToDelete)
                        {
                            cpRepo.DeleteScheduledReportRecipient(recipient);
                        }
                        cpRepo.SaveChanges();
                        success = true;
                    }
                }

                return Json(new { success = success, ScheduledReportId = scheduledReportVM.Id });
            }
            else
            {
                return DoEdit(scheduledReportVM);
            }
        }

        [HttpPost]
        public ActionResult Delete(int id)
        {
            var advId = GetAdvertiserId();
            cpRepo.DeleteScheduledReport(id, advId);
            return null;
        }

    }
}
