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
        private ISecurityRepository securityRepo;
        private IDAMain1Repository daMain1Repo;
        private IEomEntitiesConfig eomEntitiesConfig;

        public InvoicesController(IMainRepository mainRepository, ISecurityRepository securityRepository, IDAMain1Repository daMain1Repository, IEomEntitiesConfig eomEntitiesConfig)
        {
            this.mainRepo = mainRepository;
            this.securityRepo = securityRepository;
            this.daMain1Repo = daMain1Repository;
            this.eomEntitiesConfig = eomEntitiesConfig;
        }

        //public ActionResult Index()
        //{
        //    var invoices = mainRepo.Invoices(true);

        //    return View(invoices);
        //}
        public ActionResult Index()
        {
            var groups = securityRepo.GroupsForUser(User);
            if (groups.Any(g => g.Name == "Accountants" || g.Name == "Administrators"))
                return RedirectToAction("Summary");
            else
                return RedirectToAction("Start");
        }

        public ActionResult Summary()
        {
            var invoices = mainRepo.Invoices(true);
            var model = new InvoicesSummary(invoices);

            SetAccountingPeriodViewData();
            return View(model);
        }

        public ActionResult SetStatus(int id, int statusid)
        {
            mainRepo.SetInvoiceStatus(id, statusid);
            // ? add a note to record when the status was changed ?

            return RedirectToAction("Summary");
        }

        private void SetAccountingPeriodViewData()
        {
            ViewBag.ChooseMonthSelectList = daMain1Repo.ChooseMonthSelectList(eomEntitiesConfig);
            ViewBag.DebugMode = eomEntitiesConfig.DebugMode;
            ViewBag.CurrentEomDate = eomEntitiesConfig.CurrentEomDate;
        }

        // --- AM section ---

        public ActionResult Start()
        {
            SetAccountingPeriodViewData();
            ViewBag.AMs = mainRepo.AccountManagerTeams(true).OrderBy(a => a.name);
            return View();
        }

        public ActionResult ChooseAdvertiser(int? am, bool includeInvoiced = false)
        {
            var campaignAmounts = mainRepo.CampaignAmounts(am, null, false, CampaignStatus.Default);
            if (!includeInvoiced)
                campaignAmounts = campaignAmounts.Where(ca => ca.InvoicedAmount < ca.Revenue);

            var model = campaignAmounts.OrderBy(ca => ca.AdvertiserName).ThenBy(ca => ca.CampaignName);
            return View(model);
        }

        public ActionResult ChooseAmounts(int advId)
        {
            var advertiser = mainRepo.GetAdvertiser(advId);
            var model = new CampaignAffiliateAmountsModel
            {
                AdvertiserId = advId,
                AdvertiserName = advertiser.name,
                CampaignAmounts = mainRepo.CampaignAmounts(null, advId, true, CampaignStatus.Default)
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

            invoice.AddNote(User.Identity.Name, note); // to record when the invoice was submitted
            //TEST...
            //mainRepo.SaveInvoice(invoice, true);

            var idGroup = securityRepo.WindowsIdentityGroup(User.Identity.Name);

            string from = WebConfigurationManager.AppSettings["EmailFromDefault"]; // this is ignored
            string to = WebConfigurationManager.AppSettings["EmailToInvoiceRequest"];
            string cc = null;
            string subject = eomEntitiesConfig.CurrentEomDateString + ": Invoice Request";

            ViewBag.From = (idGroup != null) ? idGroup.EmailAddress : "Unknown user";
            ViewBag.ShowNotes = true;
            string body = RenderPartialViewToString("Show", invoice);

            EmailUtility.SendEmail(from, to, cc, subject, body, true);

            return Content("okay");
        }

    }
}
