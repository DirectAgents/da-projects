using CakeExtracter.Commands;
using DirectAgents.Domain.Abstract;
using DirectAgents.Domain.Concrete;
using DirectAgents.Domain.Contexts;
using DirectAgents.Domain.Entities.Cake;
using DirectAgents.Web.Models;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;

namespace DirectAgents.Web.Controllers
{
    public class BudgetsController : Controller
    {
        private IMainRepository mainRepo;
        private ISecurityRepository securityRepo;

        public BudgetsController()
        {
            this.mainRepo = new MainRepository(new DAContext());
            this.securityRepo = new SecurityRepository();
        }

        // ---

        private IEnumerable<Contact> GetAccountManagers()
        {
            IEnumerable<Contact> accountManagers = mainRepo.GetAccountManagers().ToList();
            if (!securityRepo.IsAdmin(User))
            {
                var amNames = securityRepo.AccountManagersForUser(User);
                accountManagers = accountManagers.Where(am => amNames.Contains(am.FullName));
            }
            return accountManagers;
        }
        private IEnumerable<int> GetAccountManagerIds()
        {
            return GetAccountManagers().Select(am => am.ContactId);
        }

        // ---

        public ActionResult Index()
        {
            return RedirectToAction("Start");
        }

        public ActionResult Start()
        {
            var accountManagers = GetAccountManagers();
            return View(accountManagers.OrderBy(c => c.FirstName).ThenBy(c => c.LastName));
        }

        public ActionResult AccountManagers()
        {
            var accountManagers = GetAccountManagers();
            return View(accountManagers.OrderBy(c => c.FirstName).ThenBy(c => c.LastName));
        }

        public ActionResult Advertisers(int? am, bool? withBudget)
        {
            var advertisers = mainRepo.GetAdvertisers(am, withBudget).OrderBy(a => a.AdvertiserName);
            var model = new BudgetsVM
            {
                Advertisers = advertisers
            };
            if (am.HasValue)
                model.AccountManager = mainRepo.GetContact(am.Value);

            return View(model);
        }

        public ActionResult AdvertiserRow(int advId)
        {
            var advertiser = mainRepo.GetAdvertiser(advId);
            if (advertiser == null)
                return null;
            return PartialView(advertiser);
        }

        public ActionResult Offers(int? am, int? advId, bool? withBudget, int? minPercent)
        {
            var offers = mainRepo.GetOffers(false, am, advId, withBudget);
            foreach (var offer in offers)
            {
                mainRepo.FillOfferBudgetStats(offer);
            }
            IEnumerable<Offer> offersList;
            if (minPercent.HasValue)
            {
                decimal minPercentDec = minPercent.Value / 100m;
                offersList = offers.ToList().Where(o => o.BudgetUsedPercent.HasValue && o.BudgetUsedPercent.Value >= minPercentDec)
                                   .OrderBy(o => o.Advertiser.AdvertiserName).ThenBy(o => o.OfferId);
            }
            else
            {
                offersList = offers.OrderBy(o => o.Advertiser.AdvertiserName).ThenBy(o => o.OfferId).ToList();
            }
            return View(offersList);
            //TODO: verify offer properties are lazy-loaded
        }

        public ActionResult OfferRow(int offerId)
        {
            var offer = mainRepo.GetOffer(offerId);
            if (offer == null)
                return null;
            return PartialView(offer);
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
            offer.BudgetEnd = inOffer.BudgetEnd;
            mainRepo.SaveChanges();

            return RedirectToAction("Show", new { offerId = inOffer.OfferId });
        }

        // --- Cake Synching ---

        public ActionResult SynchAdvertisers()
        {
            DASynchAdvertisersCommand.RunStatic(0, true, false);
            return RedirectToAction("Advertisers");
        }

        public ActionResult SynchAllStats()
        {
            DASynchOfferBudgetStatsCommand.RunStatic();
            return Content("Synch Stats complete");
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

        // Set budget to 0 (only if it is doesn't exist)
        public JsonResult Initialize(int offerId)
        {
            var offer = mainRepo.GetOffer(offerId);
            if (offer != null && !offer.HasBudget)
            {
                offer.Budget = 0;
                mainRepo.SaveChanges();
            }
            return null;
        }

        //---

        public ActionResult Test()
        {
            StringBuilder text = new StringBuilder();
            var affIds = new List<int>();
            var groups = securityRepo.GroupsForUser(User);
            foreach (var g in groups)
            {
                text.Append("Group: " + g.WindowsIdentity + "<br>");
            }
            return Content(text.ToString());
        }

        protected override void Dispose(bool disposing)
        {
            mainRepo.Dispose();
            base.Dispose(disposing);
        }
    }
}
