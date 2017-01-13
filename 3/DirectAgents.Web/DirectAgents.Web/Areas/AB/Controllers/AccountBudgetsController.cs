using System;
using System.Linq;
using System.Web.Mvc;
using DirectAgents.Domain.Abstract;
using DirectAgents.Domain.Entities.AB;

namespace DirectAgents.Web.Areas.AB.Controllers
{
    public class AccountBudgetsController : DirectAgents.Web.Controllers.ControllerBase
    {
        public AccountBudgetsController(IABRepository abRepository)
        {
            this.abRepo = abRepository;
        }

        public ActionResult EditViaLink(int clientAccountId, DateTime date, decimal value)
        {
            var accountBudget = abRepo.AccountBudget(clientAccountId, date);
            if (accountBudget == null)
                return HttpNotFound();

            accountBudget.Value = value;
            abRepo.SaveChanges();
            return RedirectToAction("Show", "Clients", new { id = accountBudget.ClientAccount.ClientId });
        }
	}
}