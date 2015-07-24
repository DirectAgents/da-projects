using System;
using System.Linq;
using System.Web.Mvc;
using EomTool.Domain.Abstract;

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
            SetAccountingPeriodViewDataSimple();
            //mainRepo.EnableLogging();

            var aSums = mainRepo.AuditSummaries().ToList().OrderBy(a => a.Date);
            return View(aSums);
        }

        public ActionResult Entries(DateTime? date, string operation, string primaryKey, string sysUser)
        {
            SetAccountingPeriodViewDataSimple();

            var audits = mainRepo.Audits(date, operation, primaryKey, sysUser).OrderBy(a => a.AuditID);
            return View(audits);
        }
	}
}