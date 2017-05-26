using System.Linq;
using System.Web.Mvc;
using EomTool.Domain.Abstract;
using EomTool.Domain.Entities;

namespace EomToolWeb.Controllers
{
    public class UnitTypesController : EOMController
    {
        public UnitTypesController(IMainRepository mainRepository, ISecurityRepository securityRepository, IDAMain1Repository daMain1Repository, IEomEntitiesConfig eomEntitiesConfig)
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

            var unitTypes = mainRepo.UnitTypes().OrderBy(x => x.id);

            SetAccountingPeriodViewData();
            return View(unitTypes);
        }

        public ActionResult New()
        {
            mainRepo.NewUnitType();
            return RedirectToAction("Index");
        }

        [HttpGet]
        public ActionResult Edit(int id)
        {
            var unitType = mainRepo.GetUnitType(id);
            if (unitType == null)
                return Content("UnitType not found");

            ViewBag.IncomeTypes = mainRepo.IncomeTypes().OrderBy(x => x.name);
            return View(unitType);
        }
        [HttpPost]
        public ActionResult Edit(UnitType inUnitType)
        {
            if (ModelState.IsValid)
            {
                bool saved = mainRepo.SaveUnitType(inUnitType);
                if (saved)
                    return RedirectToAction("Index");
                ModelState.AddModelError("", "UnitType could not be saved");
            }
            return View(inUnitType);
        }

    }
}