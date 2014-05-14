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

        public ActionResult Accounting()
        {
            var invoices = mainRepo.Invoices(true);
            var model = new InvoicesAccounting(invoices);

            return View(model);
        }

        public ActionResult Index()
        {
            var invoices = mainRepo.Invoices(true);

            return View(invoices);
        }

        public ActionResult SetStatus(int id, int statusid)
        {
            mainRepo.SetInvoiceStatus(id, statusid);
            return RedirectToAction("Accounting");
        }

        // --- AM section ---

        public ActionResult Start()
        {
            ViewBag.ChooseMonthSelectList = daMain1Repository.ChooseMonthSelectList(eomEntitiesConfig);
            ViewBag.DebugMode = eomEntitiesConfig.DebugMode;
            ViewBag.CurrentEomDate = eomEntitiesConfig.CurrentEomDate;

            ViewBag.AMs = mainRepo.AccountManagerTeams(true).OrderBy(a => a.name);
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

        // Note: We assume that each pid/affid pair has a unique currency
        [HttpPost]
        public ActionResult Generate(string[] idpairs)
        {
            var campAffIds = Util.ExtractCampAffIds(idpairs);
            var invoice = mainRepo.GenerateInvoice(campAffIds);
            Session["invoice"] = invoice;

            return RedirectToAction("Preview");
        }

        public ActionResult Preview()
        {
            var invoice = (Invoice)Session["invoice"];
            if (invoice == null)
                return null;

            ViewBag.Expandable = true;
            ViewBag.IncludeSubmit = true;
            return View("Show", invoice);
        }

        public ActionResult Show(int id)
        {
            var invoice = mainRepo.GetInvoice(id, true);
            if (invoice == null)
                return null;

            ViewBag.Expandable = true;
            ViewBag.ShowNotes = true;
            return View(invoice);
        }

        // Submit invoice request
        public ActionResult Submit(string note)
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
            string body = RenderPartialViewToString("Show", invoice);

            EmailUtility.SendEmail(from, to, cc, subject, body, true);

            return Content("okay");
        }

        // ---

        [HttpGet]
        public ActionResult EditCampaign(int pid)
        {
            var campaign = mainRepo.GetCampaign(pid);
            if (campaign == null)
                return Content("Campaign not found");
            return View(campaign);
        }

        [HttpPost]
        public ActionResult EditCampaign(Campaign campaign)
        {
            if (mainRepo.SaveCampaign(campaign))
                return Content("Saved. You may now close this tab.");
            else
                return null;
        }
    }
}
