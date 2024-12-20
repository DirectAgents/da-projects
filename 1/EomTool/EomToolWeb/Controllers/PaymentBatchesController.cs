﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Mime;
using System.Web.Configuration;
using System.Web.Mvc;
using DAgents.Common;
using EomTool.Domain.Abstract;
using EomTool.Domain.Concrete;
using EomTool.Domain.Entities;
using EomToolWeb.Infrastructure;
using EomToolWeb.Models;
using Microsoft.Win32;

namespace EomToolWeb.Controllers
{
    public class PaymentBatchesController : Controller
    {
        private IDAMain1Repository daMain1Repository;
        private Dictionary<string, IPaymentBatchRepository> pbRepositories = new Dictionary<string, IPaymentBatchRepository>();
            // the keys are accounting periods (e.g. "Dec2012")

        private EomToolWebConfigSection eomToolConfig = EomToolWebConfigSection.GetConfigSection();
        private List<string> AccountingPeriods { get; set; }

        public PaymentBatchesController(IDAMain1Repository daMain1Repository)
        {
            this.daMain1Repository = daMain1Repository;

            AccountingPeriods = new List<string>();
            var today = DateTime.Now;
            int numAccountingPeriods = eomToolConfig.PaymentBatches.NumAccountingPeriods;
            if (eomToolConfig.DebugMode)
            {
                today = new DateTime(2014, 5, 5);
                numAccountingPeriods = 2; // debug with zMar & zApr
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

            IPaymentBatchRepository repo = null;
            if (config.DatabaseExistsForDate(dateTime))
            {
                var eomEntities = new EomEntities(config);
                repo = new PaymentBatchRepository(eomEntities);
            }

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

        // Index of Batches
        public ActionResult Index(string test, int? batchid, string acctperiod, int? state)
        {
            if (!state.HasValue) state = PaymentBatchState.Sent; // Use state == -1 to show all batches
            string identityName = GetIdentityName(test);
            var model = new PaymentBatchesViewModel()
            {
                Test = test,
                ShowActions = false,
                AllowHold = AllowHold(identityName)
            };

            for (int i = 0; i < AccountingPeriods.Count; i++)
            {
                var accountingPeriod = AccountingPeriods[i];
                if (!string.IsNullOrWhiteSpace(acctperiod) && acctperiod != accountingPeriod)
                    continue;

                var pbRepo = pbRepositories[accountingPeriod];
                if (pbRepo == null) continue;

                IQueryable<PaymentBatch> pbatches;

                if (batchid.HasValue)
                    pbatches = pbRepo.PaymentBatches.Where(pb => pb.id == batchid.Value);
                else
                    pbatches = pbRepo.PaymentBatchesForUser(identityName, state.Value);

                if (pbatches.Count() > 0)
                {
                    var payments = pbRepo.PublisherPayments;
                    var pubNotes = pbRepo.PubNotes;
                    var pubAttachments = pbRepo.PubAttachments;
                    foreach (var payment in payments)
                    {
                        payment.AccountingPeriod = accountingPeriod;
                        payment.NumNotes = pubNotes.Where(n => n.publisher_name == payment.Publisher).Count();
                        payment.NumAttachments = pubAttachments.Where(a => a.publisher_name == payment.Publisher).Count();
                    }
                    foreach (var pbatch in pbatches)
                    {
                        pbatch.AccountingPeriod = accountingPeriod;
                        pbatch.Payments = payments.Where(p => p.PaymentBatchId == pbatch.id);
                    }
                }
                model.Batches = model.Batches == null ? pbatches : model.Batches.Concat(pbatches);
            }

            return View(model);
        }

        // Summary of Payments
        public ActionResult Summary(string test, int? state)
        {
            if (!state.HasValue) state = PaymentBatchState.Sent; // Use state == -1 to show all regardless of batch state
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
                if (pbRepo == null) continue;

                var pubNotes = pbRepo.PubNotes;
                var pubAttachments = pbRepo.PubAttachments;

                var payments = pbRepo.PublisherPaymentsForUser(identityName, state.Value);
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
            return ChangeItems(itemids, acctperiod, ItemAccountingStatus.CheckSignedAndPaid, "(released)", true);
        }
        public ActionResult HoldItems(string itemids, string acctperiod)
        {
            return ChangeItems(itemids, acctperiod, ItemAccountingStatus.Hold, "(held)", true);
        }
        public ActionResult ResetItems(string itemids, string acctperiod)
        {
            return ChangeItems(itemids, acctperiod, ItemAccountingStatus.Approved, "(reset)", true);
        }

        private ActionResult ChangeItems(string itemids, string acctperiod, int toStatus, string msg, bool checkIfBatchComplete)
        {
            int[] itemIdsArray = itemids.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries).Select(id => Convert.ToInt32(id)).ToArray();

            var pbRepo = pbRepositories[acctperiod];
            pbRepo.SetAccountingStatus(itemIdsArray, toStatus);

            if (checkIfBatchComplete)
            {
                bool changedToComplete = pbRepo.CheckIfBatchesComplete(itemIdsArray);
                if (changedToComplete && !eomToolConfig.DebugMode)
                {   // Generally there should just be one batch
                    var batches = pbRepo.PaymentBatchesForItemIds(itemIdsArray);
                    SendBatchCompleteEmail(acctperiod, batches.ToList());
                }
            }

            if (Request.IsAjaxRequest())
                return Content(msg);
            else
                return RedirectToAction("Index");
        }

        static void SendBatchCompleteEmail(string acctperiod, List<PaymentBatch> batches)
        {
            string batchUrl = "";
            foreach (var batch in batches)
            {
                batchUrl += acctperiod + " - " + batch.name +
                    "<br/>http://eomweb.directagents.local/PaymentBatches/Index?acctperiod=" + acctperiod + "&batchid=" + batch.id + "<br/>";
            }
            string from = WebConfigurationManager.AppSettings["EmailFromDefault"];
            string to = WebConfigurationManager.AppSettings["EmailToEOM"];
            string subject = "Payment Batch complete";
            string body = (@"The following payment batch was completed on [[Date]]:
<p>
[[BatchUrl]]
</p>")
               .Replace("[[Date]]", DateTime.Now.ToString())
               .Replace("[[BatchUrl]]", batchUrl);

            EmailUtility.SendEmail(from, to, null, subject, body, true);
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

        // --- Pub Selector ---

        public ActionResult Selector(string acctperiod, int? acctstatus)
        {
            var model = SetupSelector(acctperiod, acctstatus);
            model.PubGroups = model.PayoutsQueryable.ToList().GroupBy(p => p.Publisher).OrderBy(g => g.Key);
            return View(model);
        }
        public ActionResult Payouts(string acctperiod, int? acctstatus)
        {
            var model = SetupSelector(acctperiod, acctstatus);
            model.PubPayouts = model.PayoutsQueryable.OrderBy(p => p.Publisher).ThenBy(p => p.Campaign_Name);

            return View("Selector", model);
        }
        private SelectorViewModel SetupSelector(string acctperiod, int? acctstatus)
        {
            if (string.IsNullOrWhiteSpace(acctperiod))
            {
                // get the most recent repo that exists
                int i = AccountingPeriods.Count - 1;
                while (pbRepositories[AccountingPeriods[i]] == null && i > 0)
                {
                    i--;
                }
                acctperiod = AccountingPeriods[i];
            }
            var pbRepo = pbRepositories[acctperiod];

            var payouts = pbRepo.PublisherPayouts.Where(p => p.status_id == CampaignStatus.Verified && p.Pub_Payout > 0);
            if (acctstatus.HasValue)
                payouts = payouts.Where(p => p.accounting_status_id == acctstatus.Value);

            var model = new SelectorViewModel()
            {
                AccountingPeriods = AccountingPeriods.ToArray(),
                AccountingPeriod = acctperiod,
                AccountingStatus = acctstatus,
                PayoutsQueryable = payouts
            };
            return model;
        }

    }
}
