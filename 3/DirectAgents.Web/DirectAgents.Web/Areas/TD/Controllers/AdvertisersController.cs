using System.Linq;
using System.Web.Mvc;
using DirectAgents.Domain.Abstract;
using DirectAgents.Domain.Entities.TD;

namespace DirectAgents.Web.Areas.TD.Controllers
{
    public class AdvertisersController : DirectAgents.Web.Controllers.ControllerBase
    {
        public AdvertisersController(ITDRepository tdRepository)
        {
            this.tdRepo = tdRepository;
        }

        public ActionResult Index()
        {
            var advertisers = tdRepo.Advertisers()
                .OrderBy(a => a.Name);

            return View(advertisers);
        }

        [HttpGet]
        public ActionResult Edit(int id)
        {
            var advertiser = tdRepo.Advertiser(id);
            if (advertiser == null)
                return HttpNotFound();
            SetupForEdit();
            return View(advertiser);
        }
        [HttpPost]
        public ActionResult Edit(Advertiser adv)
        {
            if (ModelState.IsValid)
            {
                if (tdRepo.SaveAdvertiser(adv))
                    return RedirectToAction("Index");
                ModelState.AddModelError("", "Campaign could not be saved.");
            }
            //fillextended?
            SetupForEdit();
            return View(adv);
        }
        private void SetupForEdit()
        {
            ViewBag.Employees = tdRepo.Employees().OrderBy(e => e.FirstName).ThenBy(e => e.LastName).ToList();
        }
	}
}