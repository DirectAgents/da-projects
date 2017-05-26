using System.Linq;
using System.Web.Mvc;
using EomTool.Domain.Abstract;
using EomTool.Domain.Entities;

namespace EomToolWeb.Controllers
{
    public class IncomeTypesController : EOMController
    {
        public IncomeTypesController(IMainRepository mainRepository, ISecurityRepository securityRepository, IDAMain1Repository daMain1Repository, IEomEntitiesConfig eomEntitiesConfig)
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

            var incomeTypes = mainRepo.IncomeTypes().OrderBy(x => x.id);

            SetAccountingPeriodViewData();
            return View(incomeTypes);
        }

        public ActionResult New()
        {
            mainRepo.NewIncomeType();
            return RedirectToAction("Index");
        }

        [HttpGet]
        public ActionResult Edit(int id)
        {
            var incomeType = mainRepo.GetIncomeType(id);
            if (incomeType == null)
                return Content("IncomeType not found");

            return View(incomeType);
        }
        [HttpPost]
        public ActionResult Edit(IncomeType inIncomeType)
        {
            if (ModelState.IsValid)
            {
                bool saved = mainRepo.SaveIncomeType(inIncomeType);
                if (saved)
                    return RedirectToAction("Index");
                ModelState.AddModelError("", "IncomeType could not be saved");
            }
            return View(inIncomeType);
        }

    }
}