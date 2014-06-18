using EomTool.Domain.Abstract;
using EomToolWeb.Models;
using System.Web.Mvc;

namespace EomToolWeb.Controllers
{
    public class AdminController : EOMController
    {
        public AdminController(IMainRepository mainRepository, ISecurityRepository securityRepository, IDAMain1Repository daMain1Repository, IEomEntitiesConfig eomEntitiesConfig)
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

            SetAccountingPeriodViewData();
            return View();
        }

        public ActionResult Settings()
        {
            return View(GetSettingsVM());
        }

        [HttpGet]
        public ActionResult EditSettings()
        {
            return View(GetSettingsVM());
        }

        [HttpPost]
        public ActionResult EditSettings(SettingsVM settingsVM)
        {
            if (ModelState.IsValid)
            {
                daMain1Repo.SaveSetting("MinimumFinalizationMargin", settingsVM.MinimumFinalizationMargin);
                return RedirectToAction("Settings");
            }
            return View(settingsVM);
        }

        private SettingsVM GetSettingsVM()
        {
            var model = new SettingsVM
            {
                MinimumFinalizationMargin = daMain1Repo.GetSettingDecimalValue("MinimumFinalizationMargin")
            };
            return model;
        }

    }
}
