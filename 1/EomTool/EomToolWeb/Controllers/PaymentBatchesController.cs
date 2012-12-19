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

            return View(model);
        }

        public ActionResult Approve(int[] id)
        {
            foreach (var pbId in id)
            {
                pbRepository.Approve(pbId, User.Identity.Name);
            }
            if (Request.IsAjaxRequest())
                return null;
            else
                return RedirectToAction("Index");
        }

    }
}
