using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using EomTool.Domain.Abstract;
using EomTool.Domain.Concrete;
using EomTool.Domain.Entities;
using EomToolWeb.Infrastructure;
using EomToolWeb.Models;
using System.IO;
using Microsoft.Win32;
using System.Net.Mime;

namespace EomToolWeb.Controllers
{
    public class PaymentBatchesController : Controller
    {
        private IDAMain1Repository daMain1Repository;
        private Dictionary<string, IPaymentBatchRepository> pbRepositories = new Dictionary<string, IPaymentBatchRepository>();
            // the keys are accounting periods (e.g. "Dec2012")

        private List<string> AccountingPeriods { get; set; }

        public PaymentBatchesController(IDAMain1Repository daMain1Repository)
        {
            this.daMain1Repository = daMain1Repository;

            AccountingPeriods = new List<string>();
            var today = DateTime.Now;
            var eomToolConfig = EomToolWebConfigSection.GetConfigSection();
            int numAccountingPeriods = eomToolConfig.PaymentBatches.NumAccountingPeriods;
            if (eomToolConfig.DebugMode)
            {
                today = new DateTime(2012, 12, 25);
                numAccountingPeriods = 2; // debug with zOct & zNov
            }
            for (int i = 0; i < numAccountingPeriods; i++)
            {
                DateTime firstOfMonth = today.FirstDayOfMonth(i - numAccountingPeriods);
                string accountingPeriod = AccountingPeriodString(firstOfMonth);
                AccountingPeriods.Add(accountingPeriod);
                pbRepositories[accountingPeriod] = CreateRepository(firstOfMonth);
            }
        }

        private string AccountingPeriodString(DateTime dateTime)
        {
            return dateTime.ToString("MMM") + dateTime.Year; // e.g. "Dec2012"
        }
        private IPaymentBatchRepository CreateRepository(DateTime dateTime)
        {
            var config = new EomEntitiesConfigBase()
            {
                CurrentEomDate = dateTime
            };
            var eomEntities = new EomEntities(config);
            var repo = new PaymentBatchRepository(eomEntities);
            return repo;
        }

        private string GetIdentityName(string test)
        {
            string identityName = null;
            if (test == null)
                identityName = User.Identity.Name;
            else if (test != "all")
                identityName = "DIRECTAGENTS\\" + test;
            return identityName;
        }

        private bool AllowHold(string identity)
        {
            var eomToolConfig = EomToolWebConfigSection.GetConfigSection();
            var identitiesCanHold = eomToolConfig.PaymentBatches.CanHold.Split(new char[] { ',' });
            return (identity == null || identitiesCanHold.Contains(identity));
        }

        public ActionResult Index(string test)
        {
            string identityName = GetIdentityName(test);
            var model = new PaymentBatchesViewModel()
            {
                Test = test,
                AllowHold = AllowHold(identityName)
            };

            for (int i = 0; i < AccountingPeriods.Count; i++)
            {
                var accountingPeriod = AccountingPeriods[i];
                var pbRepo = pbRepositories[accountingPeriod];

                bool sentOnly = (test == null);
                var pbatches = pbRepo.PaymentBatchesForUser(identityName, sentOnly);
                var payments = pbRepo.PublisherPayments;

                foreach (var pbatch in pbatches)
                {
                    pbatch.AccountingPeriod = accountingPeriod;
                    pbatch.Payments = payments.Where(p => p.PaymentBatchId == pbatch.id);
                }

                model.Batches = model.Batches == null ? pbatches : model.Batches.Concat(pbatches);
            }

            return View(model);
        }

        public ActionResult Summary(string test)
        {
            string identityName = GetIdentityName(test);
            var model = new PaymentsViewModel()
            {
                Test = test,
                AllowHold = AllowHold(identityName)
            };

            IEnumerable<PublisherPayment> allPayments = null;
            for (int i = 0; i < AccountingPeriods.Count; i++)
            {
                var accountingPeriod = AccountingPeriods[i];
                var pbRepo = pbRepositories[accountingPeriod];
                var pubNotes = pbRepo.PubNotes;
                var pubAttachments = pbRepo.PubAttachments;

                var payments = pbRepo.PublisherPaymentsForUser(identityName, true);
                foreach (var payment in payments)
                {
                    payment.AccountingPeriod = accountingPeriod;
                    payment.NumNotes = pubNotes.Where(n => n.publisher_name == payment.Publisher).Count();
                    payment.NumAttachments = pubAttachments.Where(a => a.publisher_name == payment.Publisher).Count();
                }
                allPayments = allPayments == null ? payments : allPayments.Concat(payments);
            }

            model.SetPayments(allPayments);
            return View(model);
        }

        public ActionResult PubRep(string pubname, string acctperiod)
        {
            var pbRepo = pbRepositories[acctperiod];
            var payouts = pbRepo.PublisherPayouts.Where(p => p.Publisher.StartsWith(pubname) && p.status_id == CampaignStatus.Verified);
            var date = DateTime.Parse(acctperiod);
            var pubRep = PayoutsController.GetPublisherReport(payouts, date);
            var pubRepEncoded = MvcHtmlString.Create(pubRep).ToHtmlString();
            return Content(pubRepEncoded);
        }

        // --- Release & Hold ---

        public ActionResult ReleaseItems(string itemids, string acctperiod)
        {
            return ChangeItems(itemids, acctperiod, ItemAccountingStatus.CheckSignedAndPaid, "(released)");
        }
        public ActionResult HoldItems(string itemids, string acctperiod)
        {
            return ChangeItems(itemids, acctperiod, ItemAccountingStatus.Hold, "(held)");
        }
        public ActionResult ResetItems(string itemids, string acctperiod)
        {
            return ChangeItems(itemids, acctperiod, ItemAccountingStatus.Approved, "(reset)");
        }

        private ActionResult ChangeItems(string itemids, string acctperiod, int toStatus, string msg)
        {
            int[] itemIdsArray = itemids.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries).Select(id => Convert.ToInt32(id)).ToArray();

            var pbRepo = pbRepositories[acctperiod];
            pbRepo.SetAccountingStatus(itemIdsArray, toStatus);

            if (Request.IsAjaxRequest())
                return Content(msg);
            else
                return RedirectToAction("Index");
        }

        // --- Notes ---

        public ActionResult PubNotes(string pubname, string acctperiod)
        {
            if (string.IsNullOrEmpty(acctperiod))
            {
                var model = daMain1Repository.PublisherNotesForPublisher(pubname)
                    .Select(p => new PubNote() {publisher_name = p.publisher_name, note = p.note, added_by_system_user = p.added_by_system_user, created = p.created})
                    .OrderByDescending(n => n.created);
                return PartialView(model);
            }
            else
            {
                var pbRepo = pbRepositories[acctperiod];
                var model = pbRepo.PubNotesForPublisher(pubname).OrderByDescending(n => n.created);
                return PartialView(model);
            }
        }

        public ActionResult SavePubNote(string pubname, string acctperiod, string note)
        {
            string identityName = User.Identity.Name;
            if (string.IsNullOrEmpty(acctperiod))
            {
                daMain1Repository.AddPublisherNote(pubname, note, identityName);
            }
            else
            {
                var pbRepo = pbRepositories[acctperiod];
                pbRepo.AddPubNote(pubname, note, identityName);
            }
            if (Request.IsAjaxRequest())
                return Content("saved");
            else
                return RedirectToAction("Index");
        }

        // --- Attachments ---

        public ActionResult PubAttachments(string pubname, string acctperiod)
        {
            var pbRepo = pbRepositories[acctperiod];
            var model = pbRepo.PubAttachmentsForPublisher(pubname).OrderBy(pa => pa.id);
            ViewData["acctperiod"] = acctperiod;
            return PartialView(model);
        }

        public FileContentResult PubAttachment(int id, string acctperiod, bool download = false)
        {
            var pbRepo = pbRepositories[acctperiod];
            var pa = pbRepo.PubAttachments.Where(a => a.id == id).First();

            var cd = new ContentDisposition
            {
                FileName = pa.name,
                Inline = !download
            };
            Response.AppendHeader("Content-Disposition", cd.ToString());
            return File(pa.binary_content, GetContentType(pa.name));
        }

        public static string GetContentType(string filename)
        {
            string mimeType = "application/unknown";
            string extension = Path.GetExtension(filename);

            if (string.IsNullOrWhiteSpace(extension))
                return mimeType;

            RegistryKey regKey = Registry.ClassesRoot.OpenSubKey(extension.ToLower());

            if (regKey != null)
            {
                object contentType = regKey.GetValue("Content Type");
                if (contentType != null)
                    mimeType = contentType.ToString();
            }
            return mimeType;
        }
    }
}
