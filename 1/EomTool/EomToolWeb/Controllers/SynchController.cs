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
        //TODO: display exceptions to user (see below)

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
            DateTime startDate = eomEntitiesConfig.CurrentEomDate;
            DateTime enddate = startDate.AddMonths(1).AddDays(-1);

            var offerIDstrings = offerID.Split(new char[] { ',', ' ' }, StringSplitOptions.RemoveEmptyEntries);

            foreach (var offerIDstring in offerIDstrings)
            {
                int offerIDint;
                if (int.TryParse(offerIDstring, out offerIDint))
                {
                    var result = new SynchVM.SynchResults
                    {
                        OfferID = offerIDint
                    };

                    //TODO: This doesn't work to catch exceptions, e.g. in CPCLoader
                    //TODO: Try putting catch in command / loader, see if it catches; find a way to catch it here
                    try
                    {
                        result.NumItemsSynched = CakeExtracter.Commands.EOMSynchCPCCommand.RunStatic(offerIDint, mainRepo, startDate, enddate);
                    }
                    catch (Exception ex)
                    {
                        result.Message = ex.Message;
                    }
                    resultsList.Add(result);
                }
            }

            var model = new SynchVM
            {
                CurrentEomDateString = eomEntitiesConfig.CurrentEomDateString,
                Posted = true,
                Results = resultsList
            };
            return View(model);
        }

        public ActionResult RestoreX()
        {
            //mainRepo.RestoreX();
            return Content("okay");
        }
	}
}