using System.Linq;
using System.Web.Mvc;
using EomTool.Domain.Abstract;
using EomTool.Domain.Entities;
using EomToolWeb.Models;

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
    }
}