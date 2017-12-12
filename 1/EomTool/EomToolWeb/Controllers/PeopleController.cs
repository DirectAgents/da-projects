﻿using System.Linq;
using System.Web.Mvc;
using EomTool.Domain.Abstract;
using EomTool.Domain.Entities;

namespace EomToolWeb.Controllers
{
    public class PeopleController : EOMController
    {
        public PeopleController(IMainRepository mainRepository, ISecurityRepository securityRepository, IDAMain1Repository daMain1Repository, IEomEntitiesConfig eomEntitiesConfig)
        {
            this.mainRepo = mainRepository;
            this.securityRepo = securityRepository;
            this.daMain1Repo = daMain1Repository;
            this.eomEntitiesConfig = eomEntitiesConfig;
        }

        public ActionResult Index()
        {
            if (!securityRepo.IsAccountantOrAdmin(User))
                return Content("unauthorized");

            var people = mainRepo.People().OrderBy(p => p.first_name).ThenBy(p => p.last_name);

            SetAccountingPeriodViewData();
            return View(people);
        }
        public ActionResult NewPerson()
        {
            mainRepo.NewPerson();
            return RedirectToAction("Index");
        }
        [HttpGet]
        public ActionResult Edit(int id)
        {
            var person = mainRepo.GetPerson(id);
            if (person == null)
                return Content("Person not found");
            return View(person);
        }
        [HttpPost]
        public ActionResult Edit(Person inPerson)
        {
            if (ModelState.IsValid)
            {
                bool saved = mainRepo.SavePerson(inPerson);
                if (saved)
                    return RedirectToAction("Index");
                    //return RedirectToAction("Show", new { id = inPerson.id });
                ModelState.AddModelError("", "Person could not be saved");
            }
            return View(inPerson);
        }

        // --- Strategists ---

        public ActionResult Strategists()
        {
            var strategists = mainRepo.Strategists().OrderBy(x => x.name);
            SetAccountingPeriodViewData();
            return View(strategists);
        }
        public ActionResult NewStrategist()
        {
            mainRepo.NewStrategist();
            return RedirectToAction("Strategists");
        }
        [HttpGet]
        public ActionResult EditStrategist(int id)
        {
            var strategist = mainRepo.GetStrategist(id);
            if (strategist == null)
                return HttpNotFound();
            return View(strategist);
        }
        [HttpPost]
        public ActionResult EditStrategist(Strategist strategist)
        {
            if (ModelState.IsValid)
            {
                bool saved = mainRepo.SaveStrategist(strategist);
                if (saved)
                    return RedirectToAction("Strategists");
                ModelState.AddModelError("", "Strategist could not be saved");
            }
            return View(strategist);
        }

        // --- AccountManagers ---

        public ActionResult AccountManagers()
        {
            var AMs = mainRepo.AccountManagers().OrderBy(x => x.name);
            SetAccountingPeriodViewData();
            return View(AMs);
        }
        [HttpGet]
        public ActionResult EditAccountManager(int id)
        {
            var am = mainRepo.GetAccountManager(id);
            if (am == null)
                return HttpNotFound();
            return View(am);
        }
        [HttpPost]
        public ActionResult EditAccountManager(AccountManager am)
        {
            if (ModelState.IsValid)
            {
                bool saved = mainRepo.SaveAccountManager(am);
                if (saved)
                    return RedirectToAction("AccountManagers");
                ModelState.AddModelError("", "AccountManager could not be saved");
            }
            return View(am);
        }

        // --- AdManagers ---

        public ActionResult AdManagers()
        {
            var adMgrs = mainRepo.AdManagers().OrderBy(x => x.name);
            SetAccountingPeriodViewData();
            return View(adMgrs);
        }

        // --- MediaBuyers ---

        public ActionResult MediaBuyers()
        {
            var MBs = mainRepo.MediaBuyers().OrderBy(x => x.name);
            SetAccountingPeriodViewData();
            return View(MBs);
        }

    }
}