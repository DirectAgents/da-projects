using EomTool.Domain.Abstract;
using EomTool.Domain.Entities;
using EomToolWeb.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace EomToolWeb.Controllers
{
    public class InvoicesController : Controller
    {
        private IMainRepository mainRepo;
        private IDAMain1Repository daMain1Repository;
        private IEomEntitiesConfig eomEntitiesConfig;

        public InvoicesController(IMainRepository mainRepository, IDAMain1Repository daMain1Repository, IEomEntitiesConfig eomEntitiesConfig)
        {
            this.mainRepo = mainRepository;
            this.daMain1Repository = daMain1Repository;
            this.eomEntitiesConfig = eomEntitiesConfig;
        }

        public ActionResult Index()
        {
            ViewBag.ChooseMonthSelectList = daMain1Repository.ChooseMonthSelectList(eomEntitiesConfig);
            ViewBag.DebugMode = eomEntitiesConfig.DebugMode;
            ViewBag.CurrentEomDate = eomEntitiesConfig.CurrentEomDate;

            ViewBag.AccountManagers = mainRepo.AccountManagers(true).OrderBy(a => a.name);
            return View();
        }

        public ActionResult ChooseAdvertiser(int? am)
        {
            var campaignAmounts = mainRepo.CampaignAmounts(am, null)
                .OrderBy(ca => ca.AdvertiserName)
                .ThenBy(ca => ca.CampaignName);

            return View(campaignAmounts);
        }

        public ActionResult ChooseAmounts(int advId)
        {
            var advertiser = mainRepo.GetAdvertiser(advId);

            var campaignAmounts = mainRepo.CampaignAmounts(null, advId, true);

            var model = new ChooseAmountsModel()
            {
                AdvertiserName = advertiser.name,
                CampaignAmounts = campaignAmounts
            };
            return View(model);
        }

        //[HttpPost]
        //public ActionResult PreviewAmounts(string[] items)
        //{
        //    var campAffIds = Util.ExtractCampAffIds(items);

        //    var campaignAmounts = mainRepo.CampaignAmounts(campAffIds)
        //        .OrderBy(ca => ca.CampaignName)
        //        .ThenBy(ca => ca.AffiliateName);

        //    var model = new ChooseAmountsModel()
        //    {
        //        AdvertiserName = "...",
        //        CampaignAmounts = campaignAmounts
        //    };
        //    return View("ChooseAmounts", model);
        //}

        [HttpPost]
        public ActionResult Generate(string[] idpairs)
        {
            var campAffIds = Util.ExtractCampAffIds(idpairs);
            var invoice = mainRepo.GenerateInvoice(campAffIds);
            Session["invoice"] = invoice;

            return RedirectToAction("Show");
        }

        public ActionResult Show()
        {
            var invoice = (Invoice)Session["invoice"];
            if (invoice == null)
                return null;

            return View(invoice);
        }
    }
}
