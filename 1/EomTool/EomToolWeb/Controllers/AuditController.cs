using System;
using System.Linq;
using System.Web.Mvc;
using EomTool.Domain.Abstract;
using EomToolWeb.Models;

namespace EomToolWeb.Controllers
{
    public class AuditController : EOMController
    {
        public AuditController(IMainRepository mainRepository, ISecurityRepository securityRepository, IDAMain1Repository daMain1Repository, IEomEntitiesConfig eomEntitiesConfig)
        {
            this.mainRepo = mainRepository;
            this.securityRepo = securityRepository;
            this.daMain1Repo = daMain1Repository;
            this.eomEntitiesConfig = eomEntitiesConfig;
        }

        public ActionResult Index()
        {
            SetAccountingPeriodViewData();
            return View();
        }

        public ActionResult Summaries()
        {
            //mainRepo.EnableLogging();
            var aSums = mainRepo.AuditSummaries();
            var model = new AuditVM
            {
                CurrentEomDateString = eomEntitiesConfig.CurrentEomDateString,
                AuditSummaries = aSums.ToList().OrderBy(a => a.Date)
            };
            return View(model);
        }

        public ActionResult Entries(DateTime? date, string operation, string primaryKey, string sysUser)
        {
            var audits = mainRepo.Audits(date, operation, primaryKey, sysUser);
            var model = new AuditVM
            {
                CurrentEomDateString = eomEntitiesConfig.CurrentEomDateString,
                Audits = audits.OrderBy(a => a.AuditID)
            };
            int itemId;
            if (!String.IsNullOrWhiteSpace(primaryKey) && int.TryParse(primaryKey, out itemId))
            {
                model.Item = mainRepo.GetItem(itemId, true);
            }
            return View(model);
        }

        public ActionResult Advertisers()
        {
            var model = new AuditVM
            {
                CurrentEomDateString = eomEntitiesConfig.CurrentEomDateString,
                Advertisers = mainRepo.Advertisers(true).OrderBy(a => a.name)
            };
            return View(model);
        }

        public ActionResult Campaigns(int? advId, int? affId)
        {
            var model = new AuditVM
            {
                CurrentEomDateString = eomEntitiesConfig.CurrentEomDateString,
                Campaigns = mainRepo.Campaigns(advertiserId: advId, affId: affId, activeOnly: true),
                AdvId = advId,
                AffId = affId
            };
            return View(model);
        }

        public ActionResult Affiliates()
        {
            var model = new AuditVM
            {
                CurrentEomDateString = eomEntitiesConfig.CurrentEomDateString,
                Affiliates = mainRepo.Affiliates(true).OrderBy(a => a.name2)
            };
            return View(model);
        }

        public ActionResult Items(string pid, string affid) // TODO? make these nullable ints (int?) ?
        {
            int temp;
            int? pidInt = null, affIdInt = null;
            if (int.TryParse(pid, out temp)) pidInt = temp;
            if (int.TryParse(affid, out temp)) affIdInt = temp;

            var model = new AuditVM
            {
                CurrentEomDateString = eomEntitiesConfig.CurrentEomDateString,
                Items = mainRepo.GetItems(pid: pidInt, affId: affIdInt)
            };
            return View(model);
        }
	}
}