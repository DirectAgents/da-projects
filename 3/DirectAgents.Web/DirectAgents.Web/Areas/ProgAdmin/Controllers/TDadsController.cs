using System.Linq;
using System.Web.Mvc;
using DirectAgents.Domain.Abstract;
using DirectAgents.Domain.Entities.CPProg;
using DirectAgents.Web.Constants;

namespace DirectAgents.Web.Areas.ProgAdmin.Controllers
{
    public class TDadsController : DirectAgents.Web.Controllers.ControllerBase
    {
        public TDadsController(ICPProgRepository cpProgRepository)
        {
            this.cpProgRepo = cpProgRepository;
        }

        public ActionResult Index(int? id, int? adId, OrderBy sort = OrderBy.AdName)
        {
            var ads = cpProgRepo.TDads(acctId: id, id: adId);
            IOrderedQueryable<TDad> orderedAds;
            switch (sort)
            {
                case OrderBy.AdSetName:
                    orderedAds = ads.OrderBy(a => a.AdSet.Name).ThenBy(a => a.AdSetId).ThenBy(x => x.Name);
                    break;
                default:
                    orderedAds = ads.OrderBy(x => x.Name);
                    break;
            }
            var adList = orderedAds.ThenBy(x => x.Id).ToList();

            Session["acctId"] = id.ToString();

            // Don't show images if not filtered (i.e. showing all creatives), unless requested explicitly
            ViewBag.ShowImages = (id.HasValue || (Request["images"] != null && Request["images"].ToUpper() == "TRUE"));
            var extIds = adList.SelectMany(x => x.ExternalIds).Select(x => x.Type.Name).Distinct();
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