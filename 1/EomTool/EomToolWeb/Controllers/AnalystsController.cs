using System.Linq;
using System.Web.Mvc;
using EomTool.Domain.Abstract;
using EomTool.Domain.Entities;

namespace EomToolWeb.Controllers
{
    public class AnalystsController : EOMController
    {
        public AnalystsController(IMainRepository mainRepository, ISecurityRepository securityRepository, IDAMain1Repository daMain1Repository, IEomEntitiesConfig eomEntitiesConfig)
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

            var analysts = mainRepo.Analysts().OrderBy(x => x.name);

            SetAccountingPeriodViewData();
            return View(analysts);
        }
        public ActionResult NewAnalyst()
        {
            mainRepo.NewAnalyst();
            return RedirectToAction("Index");
        }
        [HttpGet]
        public ActionResult Edit(int id)
        {
            var analyst = mainRepo.GetAnalyst(id);
            if (analyst == null)
                return HttpNotFound();

            ViewBag.Managers = mainRepo.AnalystManagers().OrderBy(x => x.name);
            return View(analyst);
        }
        [HttpPost]
        public ActionResult Edit(Analyst analyst)
        {
            if (ModelState.IsValid)
            {
                bool saved = mainRepo.SaveAnalyst(analyst);
                if (saved)
                    return RedirectToAction("Index");
                ModelState.AddModelError("", "Analyst could not be saved");
            }
            return View(analyst);
        }

        // --- AnalystManagers ---

        public ActionResult Managers()
        {
            if (!securityRepo.IsAccountantOrAdmin(User))
                return Content("unauthorized");

            var anMgrs = mainRepo.AnalystManagers().OrderBy(x => x.name);

            SetAccountingPeriodViewData();
            return View(anMgrs);
        }
        public ActionResult NewAnalystManager()
        {
            mainRepo.NewAnalystManager();
            return RedirectToAction("Managers");
        }
        [HttpGet]
        public ActionResult EditManager(int id)
        {
            var anMgr = mainRepo.GetAnalystManager(id);
            if (anMgr == null)
                return HttpNotFound();

            return View(anMgr);
        }
        [HttpPost]
        public ActionResult EditManager(AnalystManager anMgr)
        {
            if (ModelState.IsValid)
            {
                bool saved = mainRepo.SaveAnalystManager(anMgr);
                if (saved)
                    return RedirectToAction("Managers");
                ModelState.AddModelError("", "AnalystManager could not be saved");
            }
            return View(anMgr);
        }

    }
}