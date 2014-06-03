using CakeExtracter.Commands;
using DirectAgents.Domain.Abstract;
using DirectAgents.Domain.Concrete;
using DirectAgents.Domain.Contexts;
using DirectAgents.Domain.Entities.Cake;
using DirectAgents.Web.Models;
using System.Linq;
using System.Web.Mvc;

namespace DirectAgents.Web.Controllers
{
    public class BudgetsController : Controller
    {
        private IMainRepository mainRepo;

        public BudgetsController()
        {
            this.mainRepo = new MainRepository(new DAContext());
        }

        public ActionResult AccountManagers()
        {
            var accountManagers = mainRepo.GetAccountManagers();
            return View(accountManagers.OrderBy(c => c.FirstName).ThenBy(c => c.LastName));
        }

        public ActionResult Advertisers(int? am)
        {
            var advertisers = mainRepo.GetAdvertisers(am);
            return View(advertisers.OrderBy(a => a.AdvertiserName));
        }

        public ActionResult AdvertiserRow(int advId)
        {
            var advertiser = mainRepo.GetAdvertiser(advId);
            if (advertiser == null)
                return null;
            return PartialView(advertiser);
        }

        public ActionResult Offers(int? advId, bool? withBudget)
        {
            var offers = mainRepo.GetOffers(advId, withBudget);
            return View(offers.OrderBy(o => o.OfferId));
        }

        public ActionResult Show(int offerId)
        {
            var offer = mainRepo.GetOffer(offerId);
            if (offer == null)
                return Content("Offer not found");

            var ods = mainRepo.GetOfferDailySummariesForBudget(offer);
            var model = new OfferVM(offer, ods);
            return View(model);
        }

        [HttpGet]
        public ActionResult Edit(int offerId)
        {
            var offer = mainRepo.GetOffer(offerId);
            if (offer == null)
                return Content("Offer not found");

            return View(offer);
        }

        [HttpPost]
        public ActionResult Edit(Offer inOffer)
        {
            var offer = mainRepo.GetOffer(inOffer.OfferId);
            if (offer == null)
                return Content("Offer not found");

            offer.Budget = inOffer.Budget;
            offer.BudgetIsMonthly = inOffer.BudgetIsMonthly;
            offer.BudgetStart = inOffer.BudgetStart;
            mainRepo.SaveChanges();

            return RedirectToAction("Show", new { offerId = inOffer.OfferId });
        }

        // --- Cake Synching ---

        public ActionResult SynchAdvertisers()
        {
            DASynchAdvertisersCommand.RunStatic(0, true, false);
            return RedirectToAction("Advertisers");
        }

        public ActionResult SynchStats(int offerId)
        {
            var offer = mainRepo.GetOffer(offerId);
            if (offer == null)
                return Content("Offer not found");

            DASynchOfferDailySummariesCommand.RunStatic(0, offerId, offer.DateCreated.Date);

            return RedirectToAction("Show", new { offerId = offerId });
        }

        // --- AJAX actions ---

        public JsonResult SynchOffers(int advId)
        {
            DASynchOffersCommand.RunStatic(advId);
            return null;
        }

        //// Set budget to 0 (only if it is null)
        //public JsonResult Initialize(int offerId)
        //{
        //    var offer = mainRepo.GetOffer(offerId);
        //    if (offer != null && !offer.Budget.HasValue)
        //    {
        //        offer.Budget = 0;
        //        mainRepo.SaveChanges();
        //    }
        //    return null;
        //}

        //---

        public ActionResult Setup()
        {
            //DASynchAdvertisersCommand.RunStatic(0);
            DASynchOffersCommand.RunStatic(278);

            return Content("ok");
        }

        protected override void Dispose(bool disposing)
        {
            mainRepo.Dispose();
            base.Dispose(disposing);
        }
    }
}
