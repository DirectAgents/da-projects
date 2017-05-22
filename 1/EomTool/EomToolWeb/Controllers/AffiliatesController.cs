using System.Linq;
using System.Web.Mvc;
using EomTool.Domain.Abstract;
using EomTool.Domain.Entities;
using EomToolWeb.Models;

namespace EomToolWeb.Controllers
{
    public class AffiliatesController : EOMController
    {
        public AffiliatesController(IMainRepository mainRepository, ISecurityRepository securityRepository, IDAMain1Repository daMain1Repository, IEomEntitiesConfig eomEntitiesConfig)
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

            var affiliates = mainRepo.Affiliates().OrderBy(a => a.name).ThenBy(a => a.affid);

            SetAccountingPeriodViewData();
            return View(affiliates);
        }

        public JsonResult IdValueText()
        {
            var affiliates = mainRepo.Affiliates().OrderBy(a => a.name).ThenBy(a => a.affid);
            var valueTexts = affiliates.Select(a => new IntValueText() { value = a.affid, text = a.name2 });
            return Json(valueTexts, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Show(int id)
        {
            var affiliate = mainRepo.GetAffiliateById(id);
            if (affiliate == null)
                return Content("Affiliate not found");

            return View(affiliate);
        }

        [HttpGet]
        public ActionResult Edit(int id)
        {
            var affiliate = mainRepo.GetAffiliateById(id);
            if (affiliate == null)
                return Content("Affiliate not found");

            return View(affiliate);
        }

        [HttpPost]
        public ActionResult Edit(Affiliate inAff)
        {
            if (ModelState.IsValid)
            {
                bool saved = SaveAffiliate(inAff);
                if (saved)
                    return RedirectToAction("Show", new { id = inAff.id });
                ModelState.AddModelError("", "Affiliate could not be saved");
            }
            return View(inAff);
        }

        public bool SaveAffiliate(Affiliate inAff)
        {
            var aff = mainRepo.GetAffiliateById(inAff.id);
            if (aff == null)
                return false;

            aff.status = inAff.status;
            aff.margin_exempt = inAff.margin_exempt;
            aff.payment_on_hold = inAff.payment_on_hold;
            aff.qb_name = inAff.qb_name;
            mainRepo.SaveChanges();
            return true;
        }
    }
}
