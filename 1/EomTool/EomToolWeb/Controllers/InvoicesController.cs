using DAgents.Common;
using EomTool.Domain.Abstract;
using EomTool.Domain.Entities;
using EomToolWeb.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Configuration;
using System.Web.Mvc;

namespace EomToolWeb.Controllers
{
    public class InvoicesController : EOMController
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
            var invoices = mainRepo.Invoices(true);

            return View(invoices);
        }

        public ActionResult Start()
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

        [HttpPost]
        public ActionResult Generate(string[] idpairs)
        {
            var campAffIds = Util.ExtractCampAffIds(idpairs);
            var invoice = mainRepo.GenerateInvoice(campAffIds);
            Session["invoice"] = invoice;

            return RedirectToAction("Show");
        }

        public ActionResult Show(int? id, bool asEmail = false)
        {
            Invoice invoice;
            if (id.HasValue)
                invoice = mainRepo.GetInvoice(id.Value, true);
            else
                invoice = (Invoice)Session["invoice"];

            if (invoice == null)
                return null;

            if (asEmail)
                return View("ShowEmail", invoice);
            else
                return View(invoice);
        }

        public ActionResult Send(string note)
        {
            var invoice = (Invoice)Session["invoice"];
            if (invoice == null)
                return null;

            mainRepo.SaveInvoice(invoice, note, true);
            //COMMENTED OUT FOR TESTING !

            string from = WebConfigurationManager.AppSettings["EmailFromDefault"];
            string to = "kevin@directagents.com"; //TESTING
            string cc = null;
            string subject = eomEntitiesConfig.CurrentEomDateString + ": Invoice Request";
            string body = RenderPartialViewToString("ShowEmail", invoice);

            EmailUtility.SendEmail(from, to, cc, subject, body, true);

            return Content("okay");
        }
    }
}
