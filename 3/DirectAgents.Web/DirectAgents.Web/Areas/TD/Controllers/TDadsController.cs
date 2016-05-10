using DirectAgents.Domain.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DirectAgents.Web.Areas.TD.Controllers
{
    public class TDadsController : DirectAgents.Web.Controllers.ControllerBase
    {
        public TDadsController(ITDRepository tdRepository)
        {
            this.tdRepo = tdRepository;
        }

        //
        // GET: /TD/TDAd/
        public ActionResult Index()
        {
            var ads = tdRepo.TDads(acctId: null).OrderBy(a => a.Name).ThenBy(a => a.Id);
            return View(ads);
        }
	}
}