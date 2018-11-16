using System.Linq;
using System.Web.Mvc;
using DirectAgents.Domain.Abstract;
using DirectAgents.Domain.Entities.CPProg;

namespace DirectAgents.Web.Areas.ProgAdmin.Controllers
{
    public class TDadsController : DirectAgents.Web.Controllers.ControllerBase
    {
        public TDadsController(ICPProgRepository cpProgRepository)
        {
            this.cpProgRepo = cpProgRepository;
        }

        public ActionResult Index(int? id, string sort)
        {
            var ads = sort == "adset" 
                ? cpProgRepo.TDads(acctId: id).OrderBy(a => a.AdSet.Name).ThenBy(a => a.AdSetId) 
                : cpProgRepo.TDads(acctId: id).OrderBy(a => a.Name).ThenBy(a => a.Id);
            Session["acctId"] = id.ToString();

            // Don't show images if not filtered (i.e. showing all creatives), unless requested explicitly
            ViewBag.ShowImages = (id.HasValue || (Request["images"] != null && Request["images"].ToUpper() == "TRUE"));
            var extIds = ads.SelectMany(x => x.ExternalIds).Select(x => x.Type.Name).Distinct();
            ViewBag.ExternalIdTypes = extIds.ToList();
            return View(ads);
        }

        [HttpGet]
        public ActionResult Edit(int id)
        {
            var ad = cpProgRepo.TDad(id);
            if (ad == null)
            {
                return HttpNotFound();
            }
            return View(ad);
        }
        
        [HttpPost]
        public ActionResult Edit(TDad ad)
        {
            if (ModelState.IsValid)
            {
                if (cpProgRepo.SaveTDad(ad))
                {
                    return RedirectToAction("Index", new { id = Session["acctId"] });
                }
                ModelState.AddModelError("", "Creative could not be saved.");
            }
            cpProgRepo.FillExtended(ad);
            return View(ad);
        }
    }
}