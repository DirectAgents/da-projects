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
        public ActionResult Edit(int clientId)
        {
            var abClient = abRepo.Client(clientId);
            if (abClient == null || !abClient.ClientAccounts.Any())
                return HttpNotFound();

            return View(abClient.ClientAccounts.First());
        }
        [HttpPost]
        public ActionResult Edit(ClientAccount clientAccount)
        {
            if (ModelState.IsValid)
            {
                if (abRepo.SaveClientAccount(clientAccount))
                    return RedirectToAction("Index", "Dashboard");
                ModelState.AddModelError("", "ClientAccount could not be saved.");
            }
            return View(clientAccount);
        }
	}
}