using EomTool.Domain.Abstract;
using EomTool.Domain.Entities;
using EomToolWeb.Models;
using System.Collections.Generic;
using System.Linq;
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
                daMain1Repo.SaveSetting("FinalizationWorkflow_MinimumMargin", settingsVM.FinalizationWorkflow_MinimumMargin);
                return RedirectToAction("Settings");
            }
            return View(settingsVM);
        }

        private SettingsVM GetSettingsVM()
        {
            var model = new SettingsVM
            {
                FinalizationWorkflow_MinimumMargin = daMain1Repo.GetSettingDecimalValue("FinalizationWorkflow_MinimumMargin")
            };
            return model;
        }

        // ---

        public ActionResult MarginApprovals()
        {
            var model = mainRepo.MarginApprovals(true);

            SetAccountingPeriodViewData();
            return View(model);
        }

        // ---

        public ActionResult AdvertiserMaintenance()
        {
            var advertisers = mainRepo.Advertisers().ToList();
            var advIds = advertisers.Select(a => a.id).ToList();

            var prevRepo = CreateMainRepository(eomEntitiesConfig.CurrentEomDate.AddMonths(-1));
            var prevAdvertisers = prevRepo.Advertisers().ToList();
            var prevAdvIds = prevAdvertisers.Select(a => a.id).ToList();

            var newAdvertisers = advertisers.Where(a => !prevAdvIds.Contains(a.id));
            var expiredAdvertisers = prevAdvertisers.Where(a => !advIds.Contains(a.id)); // exist in prev month but not current

            List<Advertiser> changedAdvertisers = new List<Advertiser>();
            foreach (var adv in advertisers)
            {
                var prevAdv = prevAdvertisers.Where(a => a.id == adv.id).SingleOrDefault();
                if (prevAdv != null && adv.name != prevAdv.name)
                {
                    adv.PreviousMonthAdvertiser = prevAdv;
                    changedAdvertisers.Add(adv);
                }
            }

            var model = new PeriodMaintenanceVM
            {
                CurrentEomDateString = eomEntitiesConfig.CurrentEomDateString,
                NewAdvertisers = newAdvertisers,
                ExpiredAdvertisers = expiredAdvertisers,
                ChangedAdvertisers = changedAdvertisers
            };
            return View(model);
        }

        public ActionResult AffiliateMaintenance()
        {
            var affiliates = mainRepo.Affiliates().ToList();
            var affIds = affiliates.Select(a => a.id).ToList();

            var prevRepo = CreateMainRepository(eomEntitiesConfig.CurrentEomDate.AddMonths(-1));
            var prevAffiliates = prevRepo.Affiliates().ToList();
            var prevAffIds = prevAffiliates.Select(a => a.id).ToList();

            var newAffiliates = affiliates.Where(a => !prevAffIds.Contains(a.id));
            var expiredAffiliates = prevAffiliates.Where(a => !affIds.Contains(a.id));

            List<Affiliate> changedAffiliates = new List<Affiliate>();
            foreach (var aff in affiliates)
            {
                var prevAff = prevAffiliates.Where(a => a.id == aff.id).SingleOrDefault();
                if (prevAff != null && aff.name != prevAff.name)
                {
                    aff.PreviousMonthAffiliate = prevAff;
                    changedAffiliates.Add(aff);
                }
            }
            var model = new PeriodMaintenanceVM
            {
                CurrentEomDateString = eomEntitiesConfig.CurrentEomDateString,
                NewAffiliates = newAffiliates,
                ExpiredAffiliates = expiredAffiliates,
                ChangedAffiliates = changedAffiliates
            };
            return View(model);
        }

        public ActionResult ClearInvoicingStatus()
        {
            if (!securityRepo.IsAccountantOrAdmin(User))
                return Content("unauthorized");

            var advertisers = mainRepo.Advertisers().Where(a => a.invoicing_status != null);
            foreach (var adv in advertisers)
                adv.invoicing_status = null;
            mainRepo.SaveChanges();

            return Content("cleared");
        }

        public ActionResult Copy1()
        {
            var prevRepo = CreateMainRepository(eomEntitiesConfig.CurrentEomDate.AddMonths(-1));
            var prevAdvertisers = prevRepo.Advertisers().ToList();

            //TODO: check if ToListing is efficient & doesn't bring in extra stuff

            var advertisers = mainRepo.Advertisers();
            foreach (var adv in advertisers)
            {
                var prevAdv = prevAdvertisers.SingleOrDefault(a => a.id == adv.id);
                if (prevAdv != null)
                {
                    adv.status = prevAdv.status;
                    adv.payment_terms = prevAdv.payment_terms;
                    adv.invoicing_status = prevAdv.invoicing_status;
                }
            }
            mainRepo.SaveChanges();
            return Content("done");
        }
        public ActionResult Copy2()
        {
            var prevRepo = CreateMainRepository(eomEntitiesConfig.CurrentEomDate.AddMonths(-1));
            var prevAffiliates = prevRepo.Affiliates().ToList();

            var affiliates = mainRepo.Affiliates();
            foreach (var aff in affiliates)
            {
                var prevAff = prevAffiliates.SingleOrDefault(a => a.id == aff.id);
                if (prevAff != null)
                {
                    aff.status = prevAff.status;
                }
            }
            mainRepo.SaveChanges();
            return Content("done");
        }

        public ActionResult IncAdvStatus()
        {
            var advertisers = mainRepo.Advertisers();
            foreach (var adv in advertisers)
            {
                adv.status = NextStatus(adv.status);
            }
            mainRepo.SaveChanges();
            return Content("done");
        }
        public ActionResult IncAffStatus()
        {
            var affiliates = mainRepo.Affiliates();
            foreach (var aff in affiliates)
            {
                aff.status = NextStatus(aff.status);
            }
            mainRepo.SaveChanges();
            return Content("done");
        }

        private string NextStatus(string status)
        {
            if (status == "New")
                return "New1";
            if (status == "New1")
                return "New2";
            if (status == "New2")
                return "Existing";
            return status;
        }
    }
}
