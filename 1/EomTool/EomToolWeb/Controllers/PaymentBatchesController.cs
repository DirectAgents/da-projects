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

namespace EomToolWeb.Controllers
{
    public class PaymentBatchesController : Controller
    {
        private Dictionary<string, IPaymentBatchRepository> pbRepositories = new Dictionary<string, IPaymentBatchRepository>(); // the keys are accounting periods
        private List<string> accountingPeriods = new List<string>();
        private IDAMain1Repository daMain1Repository;

        public PaymentBatchesController(IDAMain1Repository daMain1Repository)
        {
            this.daMain1Repository = daMain1Repository;

            //var today = DateTime.Now;
            var today = new DateTime(2012, 12, 12); // for testing

            var lastMonth = today.FirstDayOfMonth(-1);
            accountingPeriods.Add(AccountingPeriod(lastMonth));
            pbRepositories[AccountingPeriod(lastMonth)] = CreateRepository(lastMonth);

            var prevMonth = today.FirstDayOfMonth(-2);
            accountingPeriods.Add(AccountingPeriod(prevMonth));
            pbRepositories[AccountingPeriod(prevMonth)] = CreateRepository(prevMonth);
        }

        private string AccountingPeriod(DateTime dateTime)
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
        private IPaymentBatchRepository GetRepository(string acctperiod)
        {
            if (pbRepositories.Keys.Contains(acctperiod))
                return pbRepositories[acctperiod];
            else
            {
                if (accountingPeriods.Count > 0)
                    return pbRepositories[accountingPeriods[0]];
                else
                    return null;
            }
        }

        public ActionResult Index(string test)
        {
            var model = new PaymentBatchesViewModel()
            {
                AllowHold = true,
                Test = test
            };

            for (int i = accountingPeriods.Count - 1; i >= 0; i--)
            {
                var accountingPeriod = accountingPeriods[i];
                var pbRepo = pbRepositories[accountingPeriod];

                IQueryable<PaymentBatch> pbatches;
                if (test != null)
                {
                    if (test == "all")
                        pbatches = pbRepo.PaymentBatchesForUser(null, false);
                    else
                        pbatches = pbRepo.PaymentBatchesForUser("DIRECTAGENTS\\" + test, false);
                }
                else
                {
                    string identityName = User.Identity.Name;
                    pbatches = pbRepo.PaymentBatchesForUser(identityName, true);
                }
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
            var model = new PaymentsViewModel()
            {
                AllowHold = true,
                Test = test
            };

            string identity = null;
            if (test != "all") identity = "DIRECTAGENTS\\" + test;

            IEnumerable<PublisherPayment> allPayments = null;
            for (int i = accountingPeriods.Count - 1; i >= 0; i--)
            {
                var accountingPeriod = accountingPeriods[i];
                var pbRepo = pbRepositories[accountingPeriod];

                var payments = pbRepo.PublisherPaymentsForUser(identity, true);
                foreach (var payment in payments)
                {
                    payment.AccountingPeriod = accountingPeriod;
                }
                allPayments = allPayments == null ? payments : allPayments.Concat(payments);
            }

            model.SetPayments(allPayments);
            return View(model);
        }

        public ActionResult PubRep(string pubname, string acctperiod)
        {
            var pbRepo = GetRepository(acctperiod);
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

            var pbRepo = GetRepository(acctperiod);
            pbRepo.SetAccountingStatus(itemIdsArray, toStatus);

            if (Request.IsAjaxRequest())
                return Content(msg);
            else
                return RedirectToAction("Index");
        }

        // --- Notes ---

        public ActionResult PubNotes(string pubname)
        {
            var model = daMain1Repository.PublisherNotesForPublisher(pubname).OrderByDescending(n => n.created);
            return PartialView(model);
        }

        public ActionResult SavePubNote(string pubname, string note)
        {
            string identityName = User.Identity.Name;
            daMain1Repository.AddPublisherNote(pubname, note, identityName);

            if (Request.IsAjaxRequest())
                return Content("saved");
            else
                return RedirectToAction("Index");
        }

    }
}
