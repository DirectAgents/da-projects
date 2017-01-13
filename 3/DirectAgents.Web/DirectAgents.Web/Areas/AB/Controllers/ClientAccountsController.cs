using System.Linq;
using System.Web.Mvc;
using DirectAgents.Domain.Abstract;
using DirectAgents.Domain.Entities.AB;

namespace DirectAgents.Web.Areas.AB.Controllers
{
    public class ClientAccountsController : DirectAgents.Web.Controllers.ControllerBase
    {
        public ClientAccountsController(IABRepository abRepository)
        {
            this.abRepo = abRepository;
        }

        [HttpGet]
        public ActionResult Edit(int? id, int? clientId)
        {
            ClientAccount clientAccount = null;
            if (id.HasValue)
            {
                clientAccount = abRepo.ClientAccount(id.Value);
            }
            if (clientAccount == null && clientId.HasValue)
            {
                var abClient = abRepo.Client(clientId.Value);
                if (abClient != null && abClient.ClientAccounts.Any())
                    clientAccount = abClient.ClientAccounts.First();
            }
            if (clientAccount == null)
                return HttpNotFound();

            return View(clientAccount);
        }
        [HttpPost]
        public ActionResult Edit(ClientAccount clientAccount)
        {
            if (ModelState.IsValid)
            {
                if (abRepo.SaveClientAccount(clientAccount))
                    return RedirectToAction("Show", "Clients", new { id = clientAccount.ClientId });
                ModelState.AddModelError("", "ClientAccount could not be saved.");
            }
            return View(clientAccount);
        }
	}
}