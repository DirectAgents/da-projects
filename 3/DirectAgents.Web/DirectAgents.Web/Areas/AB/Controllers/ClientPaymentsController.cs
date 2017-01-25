using System;
using System.Linq;
using System.Web.Mvc;
using DirectAgents.Domain.Abstract;
using DirectAgents.Domain.Entities.AB;

namespace DirectAgents.Web.Areas.AB.Controllers
{
    public class ClientPaymentsController : DirectAgents.Web.Controllers.ControllerBase
    {
        public ClientPaymentsController(IABRepository abRepository)
        {
            this.abRepo = abRepository;
        }

        public ActionResult EditViaLink(int id, DateTime? date, decimal value)
        {
            var clientPayment = abRepo.ClientPayment(id);
            if (clientPayment == null)
                return HttpNotFound();

            if (date.HasValue)
                clientPayment.Date = date.Value;
            if (clientPayment.Bits != null && clientPayment.Bits.Count() == 1)
            {
                clientPayment.Bits.First().Value = value;
            }
            abRepo.SaveChanges();
            return RedirectToAction("Show", "Clients", new { id = clientPayment.ClientId });
        }
    }
}