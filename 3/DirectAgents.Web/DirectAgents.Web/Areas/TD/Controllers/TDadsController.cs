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

        public ActionResult Index(int? acctId)
        {
            var ads = tdRepo.TDads(acctId: acctId).OrderBy(a => a.Name).ThenBy(a => a.Id);
            Session["acctId"] = acctId.ToString();

            // Don't show images if not filtered (i.e. showing all creatives), unless requested explicitly
            ViewBag.ShowImages = (acctId.HasValue || (Request["images"] != null && Request["images"].ToUpper() == "TRUE"));
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
                    return RedirectToAction("Index", new { acctId = Session["acctId"] });
                ModelState.AddModelError("", "Creative could not be saved.");
            }
            tdRepo.FillExtended(ad);
            return View(ad);
        }
    }
}