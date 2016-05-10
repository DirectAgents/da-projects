using DirectAgents.Domain.Abstract;
using DirectAgents.Domain.Entities.TD;
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

        [HttpGet]
        public ActionResult Edit(int id)
        {
            var ad = tdRepo.TDad(id);
            if (ad == null)
                return HttpNotFound();
            return View(ad);
        }
        
        [HttpPost]
        public ActionResult Edit(TDad ad)
        {
            if (ModelState.IsValid)
            {
                if (tdRepo.SaveTDad(ad))
                    return RedirectToAction("Index", new { platform = Session["platformCode"], campId = Session["campId"] });
                ModelState.AddModelError("", "TDad could not be saved.");
            }
            tdRepo.FillExtended(ad);
            return View(ad);
        }
    }
}