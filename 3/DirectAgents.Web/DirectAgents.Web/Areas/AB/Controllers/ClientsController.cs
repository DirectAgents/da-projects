using System.Linq;
using System.Web.Mvc;
using DirectAgents.Domain.Abstract;
using DirectAgents.Domain.Entities.AB;

namespace DirectAgents.Web.Areas.AB.Controllers
{
    public class ClientsController : DirectAgents.Web.Controllers.ControllerBase
    {
        public ClientsController(IABRepository abRepository)
        {
            this.abRepo = abRepository;
        }

        public ActionResult Index()
        {
            var clients = abRepo.Clients()
                .OrderBy(c => c.Name);

            return View(clients);
        }

        [HttpGet]
        public ActionResult Edit(int id)
        {
            var abClient = abRepo.Client(id);
            if (abClient == null)
                return HttpNotFound();

            return View(abClient);
        }
        [HttpPost]
        public ActionResult Edit(ABClient client)
        {
            if (ModelState.IsValid)
            {
                if (abRepo.SaveClient(client))
                    return RedirectToAction("Index");
                ModelState.AddModelError("", "Client could not be saved.");
            }
            return View(client);
        }

	}
}