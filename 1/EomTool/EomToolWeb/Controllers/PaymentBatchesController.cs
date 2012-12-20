using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using EomTool.Domain.Abstract;
using EomTool.Domain.Entities;

namespace EomToolWeb.Controllers
{
    public class PaymentBatchesController : Controller
    {
        private IPaymentBatchRepository pbRepository;

        public PaymentBatchesController(IPaymentBatchRepository paymentBatchRepository)
        {
            this.pbRepository = paymentBatchRepository;
        }

        public ActionResult Index()
        {
            //var model = pbRepository.PaymentBatchesForUser(User);
            var model = pbRepository.PaymentBatches;
            var payments = pbRepository.PublisherPayments;
            foreach (var pbatch in model)
            {
                pbatch.Payments = payments.Where(p => p.PaymentBatchId == pbatch.id);
            }

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

        // ---testing---
        public ActionResult Payments()
        {
            var model = pbRepository.PublisherPayments;
            return View(model);
        }

    }
}
