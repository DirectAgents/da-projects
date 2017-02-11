using System;
using System.Web.Mvc;
using DirectAgents.Domain.Abstract;
using DirectAgents.Domain.Entities.AB;

namespace DirectAgents.Web.Areas.AB.Controllers
{
    public class AcctPaymentsController : DirectAgents.Web.Controllers.ControllerBase
    {
        public AcctPaymentsController(IABRepository abRepository)
        {
            this.abRepo = abRepository;
        }

        public ActionResult Index(int acctId)
        {
            var clientAccount = abRepo.ClientAccount(acctId);
            if (clientAccount == null)
                return HttpNotFound();

            return View(clientAccount);
        }

        //TODO: replace with Create (w/View)?
        //    ? have a boolean "draft" / "live" ?
        public ActionResult New(int acctId)
        {
            var clientAccount = abRepo.ClientAccount(acctId);
            if (clientAccount == null)
                return HttpNotFound();

            var payment = new AcctPayment
            {
                Date = DateTime.Today
            };
            clientAccount.Payments.Add(payment);
            abRepo.SaveChanges();

            return RedirectToAction("Index", new { acctId = clientAccount.Id });
        }

    }
}