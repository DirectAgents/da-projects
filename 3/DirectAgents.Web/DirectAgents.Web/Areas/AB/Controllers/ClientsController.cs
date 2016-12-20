using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DirectAgents.Domain.Abstract;

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
	}
}