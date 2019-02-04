using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using DirectAgents.Domain.Abstract;
using DirectAgents.Domain.Entities.CPSearch;

namespace DirectAgents.Web.Areas.ClientAdmin.Controllers
{
    public class ClientReportsController : DirectAgents.Web.Controllers.ControllerBase
    {
        public ClientReportsController(ICPProgRepository cpProgRepository, ICPSearchRepository cpSearchRepository, IClientRepository clientRepository)
        {
            this.cpProgRepo = cpProgRepository;
            this.cpSearchRepo = cpSearchRepository;
            this.clientRepo = clientRepository;
        }

        public ActionResult Index()
        {
            var reps = clientRepo.ClientReports();
            return View(reps);
        }

        public ActionResult CreateNew(string name, int? id)
        {
            if (id == null)
            {
                int maxId = 0;
                var clientReports = cpSearchRepo.ClientReports();
                if (clientReports.Any())
                    maxId = clientReports.Max(x => x.Id);
                id = maxId + 1;
            }
            var clientReport  = new ClientReport
            {
                Id = id.Value,
                Name = String.IsNullOrEmpty(name) ? "zNew" : name,
                StartDayOfWeek = (int)DayOfWeek.Monday
            };
            if (cpSearchRepo.SaveClientReport(clientReport, createIfDoesntExist: true, saveIfExists: false))
                return RedirectToAction("Index");
            else
                return Content("Error creating ClientReport");
        }
        public ActionResult Delete(int id)
        {
            cpSearchRepo.DeleteClientReport(id);
            return RedirectToAction("Index");
        }

        [HttpGet]
        public ActionResult Edit(int id)
        {
            var rep = cpSearchRepo.GetClientReport(id);
            if (rep == null)
                return HttpNotFound();
            SetupForEdit();
            return View(rep);
        }
        [HttpPost]
        public ActionResult Edit(ClientReport clientReport)
        {
            if (ModelState.IsValid)
            {
                if (cpSearchRepo.SaveClientReport(clientReport))
                    return RedirectToAction("Index");
                ModelState.AddModelError("", "ClientReport could not be saved.");
            }
            //fillextended?
            SetupForEdit();
            return View(clientReport);
        }
        private void SetupForEdit()
        {
            ViewBag.SearchProfiles = cpSearchRepo.SearchProfiles().OrderBy(x => x.SearchProfileName).ToList();
            ViewBag.ProgCampaigns = cpProgRepo.Campaigns().OrderBy(x => x.Advertiser.Name).ThenBy(x => x.Name).ToList();
        }

    }
}