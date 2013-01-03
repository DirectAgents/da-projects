using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using EomTool.Domain.Abstract;
using EomTool.Domain.Entities;
using EomToolWeb.Models;

namespace EomToolWeb.Controllers
{
    public class PaymentBatchesController : Controller
    {
        private IPaymentBatchRepository pbRepository;
        private IDAMain1Repository daMain1Repository;
        private IEomEntitiesConfig eomEntitiesConfig;

        public PaymentBatchesController(IPaymentBatchRepository paymentBatchRepository, IDAMain1Repository daMain1Repository, IEomEntitiesConfig eomEntitiesConfig)
        {
            this.pbRepository = paymentBatchRepository;
            this.daMain1Repository = daMain1Repository;
            this.eomEntitiesConfig = eomEntitiesConfig;
        }

        public ActionResult Index(string test)
        {
            IQueryable<PaymentBatch> pbatches;
            if (test != null)
            {
                if (test == "all")
                    pbatches = pbRepository.PaymentBatches;
                else
                    pbatches = pbRepository.PaymentBatchesForUser("DIRECTAGENTS\\" + test, false);
            }
            else
            {
                string identityName = User.Identity.Name;
                pbatches = pbRepository.PaymentBatchesForUser(identityName, true);
            }
            var payments = pbRepository.PublisherPayments;
            foreach (var pbatch in pbatches)
            {
                pbatch.Payments = payments.Where(p => p.PaymentBatchId == pbatch.id);
            }

            var model = new PaymentBatchesViewModel()
            {
                Batches = pbatches,
                AllowHold = true
            };
            ViewBag.ChooseMonthSelectList = new SelectList(daMain1Repository.ChooseMonthListItems(), "Value", "Text", eomEntitiesConfig.CurrentEomDate.ToString());
            return View(model);
        }

        public ActionResult ReleaseItems(string itemids)
        {
            int[] itemIdsArray = itemids.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries).Select(id => Convert.ToInt32(id)).ToArray();
            pbRepository.SetAccountingStatus(itemIdsArray, ItemAccountingStatus.CheckSignedAndPaid);

            if (Request.IsAjaxRequest())
                return Content("(released)");
            else
                return RedirectToAction("Index");
        }

        public ActionResult HoldItems(string itemids)
        {
            int[] itemIdsArray = itemids.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries).Select(id => Convert.ToInt32(id)).ToArray();
            pbRepository.SetAccountingStatus(itemIdsArray, ItemAccountingStatus.Hold);

            if (Request.IsAjaxRequest())
                return Content("(held)");
            else
                return RedirectToAction("Index");
        }

        public ActionResult PubNotes(string pubName)
        {
            var model = pbRepository.PublisherNotesForPublisher(pubName).OrderByDescending(n => n.created);
            return PartialView(model);
        }

        public ActionResult SavePubNote(string pubname, string note)
        {
            string identityName = User.Identity.Name;
            pbRepository.AddPublisherNote(pubname, note, identityName);

            if (Request.IsAjaxRequest())
                return Content("saved");
            else
                return RedirectToAction("Index");
        }

        // ---testing---
        public ActionResult Payments()
        {
            var model = pbRepository.PublisherPayments;
            return View(model);
        }

    }
}
