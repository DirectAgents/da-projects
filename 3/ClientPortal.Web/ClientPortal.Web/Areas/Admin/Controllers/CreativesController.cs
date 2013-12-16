using ClientPortal.Data.Contexts;
using ClientPortal.Data.Contracts;
using ClientPortal.Web.Controllers;
using System;
using System.Linq;
using System.Web.Mvc;

namespace ClientPortal.Web.Areas.Admin.Controllers
{
    [Authorize(Users = "admin")]
    public class CreativesController : CPController
    {
        public CreativesController(IClientPortalRepository cpRepository)
        {
            this.cpRepo = cpRepository;
        }

        public ActionResult Index(int? offerid)
        {
            var creatives = cpRepo.Creatives(offerid);
            return View(creatives);
        }

        public ActionResult Edit(int id)
        {
            var creative = cpRepo.GetCreative(id);
            if (creative == null)
                return HttpNotFound();

            return View(creative);
        }

        [HttpPost]
        public ActionResult Edit(Creative creative)
        {
            if (ModelState.IsValid)
            {
                bool success = cpRepo.SaveCreative(creative, true);
                if (success)
                    return RedirectToAction("Show", "Offers", new { id = creative.OfferId });

                ModelState.AddModelError("", "Creative could not be saved");
            }
            cpRepo.FillExtended_Creative(creative);
            return View(creative);
        }
    }
}
