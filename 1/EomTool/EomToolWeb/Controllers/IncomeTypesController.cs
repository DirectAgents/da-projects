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

        public ActionResult Seed(int super)
        {
            if (super != 1)
                return HttpNotFound();
            SeedType(1, "Advertising", "5003");
            SeedType(2, "Affiliate Management", "5401");
            SeedType(3, "Creative", null);
            SeedType(4, "Media Buying", "5101");
            SeedType(5, "Mobile", null);
            SeedType(6, "Search", "5201");
            SeedType(7, "Social", "5801");
            SeedType(8, "Trading Desk", null);
            SetForUnitType(4, 1); //CPA
            SetForUnitType(7, 4); //CPM
            SetForUnitType(8, 4); //CPC
            SetForUnitType(9, 1); //RevShare
            SetForUnitType(14, 2); //AffiliateProgramMgmt
            SetForUnitType(15, 6); //PPC
            SetForUnitType(16, 5); //CPI
            SetForUnitType(18, 8); //TradingDesk
            SetForUnitType(19, 6); //SEO
            SetForUnitType(20, 3); //Creative
            SetForUnitType(22, 7); //SocialMedia
            SetForUnitType(24, 6); //Analytics
            mainRepo.SaveChanges();
            return Content("Seeded.");
        }
        private void SeedType(int id, string name, string qb_code)
        {
            var incomeType = new IncomeType
            {
                id = id,
                name = name,
                qb_code = qb_code
            };
            mainRepo.NewIncomeType(incomeType);
        }
        private void SetForUnitType(int unitTypeId, int incomeTypeId)
        {
            var unitType = mainRepo.GetUnitType(unitTypeId);
            unitType.income_type_id = incomeTypeId;
        }

    }
}