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
	}
}