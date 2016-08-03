using EomTool.Domain.Abstract;
using EomTool.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace EomToolWeb.Controllers
{
    public class SynchController : EOMController
    {

        public SynchController(IMainRepository mainRepository, ISecurityRepository securityRepository, IDAMain1Repository daMain1Repository, IEomEntitiesConfig eomEntitiesConfig)
        {
            this.mainRepo = mainRepository;
            this.securityRepo = securityRepository;
            this.daMain1Repo = daMain1Repository;
            this.eomEntitiesConfig = eomEntitiesConfig;
            this.startDate = eomEntitiesConfig.CurrentEomDate;
            ViewBag.date = startDate.ToString("MMM yyyy");
        }

        private DateTime startDate;

        public ActionResult Index()
        {
            ViewBag.posted = false;
            return View();
        }

        [HttpPost]
        public ActionResult Index(int offerID)
        {
            ViewBag.posted = true;
            ViewBag.offer = offerID;
            var enddate = startDate.AddMonths(1).AddDays(-1);
            int numsynched = CakeExtracter.Commands.EOMSynchCPCCommand.RunStatic(offerID,mainRepo,startDate,enddate);
            ViewBag.synched = numsynched;
            return View();
        }

        

	}
}