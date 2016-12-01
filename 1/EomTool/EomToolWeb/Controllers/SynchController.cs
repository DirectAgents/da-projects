using System;
using System.Collections.Generic;
using System.Web.Mvc;
using EomTool.Domain.Abstract;
using EomToolWeb.Models;

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
        }

        //TODO: cleanup
        //TODO: a way to automate determining what the CPC offers are

        public ActionResult Index()
        {
            var model = new SynchVM
            {
                CurrentEomDateString = eomEntitiesConfig.CurrentEomDateString,
                Posted = false
            };
            return View(model);
        }

        [HttpPost]
        public ActionResult Index(string offerID)
        {
            var resultsList = new List<SynchVM.SynchResults>();

            //TODO: allow multiple offerIDs
            //TODO: handle the case when no offerID(s) could be parsed

            int offerIDint;
            if (int.TryParse(offerID, out offerIDint))
            {
                DateTime startDate = eomEntitiesConfig.CurrentEomDate;
                DateTime enddate = startDate.AddMonths(1).AddDays(-1);
                int numsynched = CakeExtracter.Commands.EOMSynchCPCCommand.RunStatic(offerIDint, mainRepo, startDate, enddate);

                resultsList.Add(new SynchVM.SynchResults
                {
                    OfferID = offerIDint,
                    NumItemsSynched = numsynched
                });
            }

            var model = new SynchVM
            {
                CurrentEomDateString = eomEntitiesConfig.CurrentEomDateString,
                Posted = true,
                Results = resultsList
            };
            return View(model);
        }

	}
}