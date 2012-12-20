using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using EomTool.Domain.Abstract;

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

        public ActionResult Payments()
        {
            var model = pbRepository.PublisherPayments;
            return View(model);
        }

    }
}
