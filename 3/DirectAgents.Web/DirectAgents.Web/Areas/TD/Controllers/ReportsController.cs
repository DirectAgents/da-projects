using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using DirectAgents.Domain.Abstract;
using DirectAgents.Domain.DTO;
using DirectAgents.Web.Areas.TD.Models;

namespace DirectAgents.Web.Areas.TD.Controllers
{
    public class ReportsController : DirectAgents.Web.Controllers.ControllerBase
    {
        public ReportsController(ITDRepository tdRepository)
        {
            this.tdRepo = tdRepository;
        }

        public ActionResult Home(int campId)
        {
            var campaign = tdRepo.Campaign(campId);
            if (campaign == null)
                return HttpNotFound();
            return View(campaign);
        }
	}
}