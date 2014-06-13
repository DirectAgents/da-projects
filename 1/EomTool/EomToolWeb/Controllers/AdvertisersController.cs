using EomTool.Domain.Abstract;
using EomTool.Domain.Entities;
using System.Linq;
using System.Web.Mvc;

namespace EomToolWeb.Controllers
{
    public class AdvertisersController : EOMController
    {
        public AdvertisersController(IMainRepository mainRepository, ISecurityRepository securityRepository, IDAMain1Repository daMain1Repository, IEomEntitiesConfig eomEntitiesConfig)
        {
            this.mainRepo = mainRepository;
            this.securityRepo = securityRepository;
            this.daMain1Repo = daMain1Repository;
            this.eomEntitiesConfig = eomEntitiesConfig;
        }

        public ActionResult Index()
        {
            if (!securityRepo.IsAccountantOrAdmin(User))
                return Content("unauthorized");

            var advertisers = mainRepo.Advertisers().OrderBy(a => a.name);

            SetAccountingPeriodViewData();
            return View(advertisers);
        }

        public ActionResult Show(int id)
        {
            var advertiser = mainRepo.GetAdvertiser(id);
            if (advertiser == null)
                return Content("Advertiser not found");

            return View(advertiser);
        }

        [HttpGet]
        public ActionResult Edit(int id)
        {
            var advertiser = mainRepo.GetAdvertiser(id);
            if (advertiser == null)
                return Content("Advertiser not found");

            return View(advertiser);
        }

        [HttpPost]
        public ActionResult Edit(Advertiser inAdv)
        {
            if (ModelState.IsValid)
            {
                bool saved = SaveAdvertiser(inAdv);
                if (saved)
                    return RedirectToAction("Show", new { id = inAdv.id });
                ModelState.AddModelError("", "Advertiser could not be saved");
            }
            return View(inAdv);
        }

        public bool SaveAdvertiser(Advertiser inAdv)
        {
            var adv = mainRepo.GetAdvertiser(inAdv.id);
            if (adv == null)
                return false;

            adv.status = inAdv.status;
            adv.invoicing_status = inAdv.invoicing_status;
            mainRepo.SaveChanges();
            return true;
        }

    }
}
