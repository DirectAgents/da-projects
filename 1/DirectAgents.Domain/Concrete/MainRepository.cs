using System;
using System.Collections.Generic;
using System.Linq;
using DirectAgents.Domain.Abstract;
using DirectAgents.Domain.Contexts;
using DirectAgents.Domain.DTO;
using DirectAgents.Domain.Entities.Cake;

namespace DirectAgents.Domain.Concrete
{
    public class MainRepository : IMainRepository, IDisposable
    {
        private DAContext context;

        public MainRepository(DAContext context)
        {
            this.context = context;
        }

        public void SaveChanges()
        {
            context.SaveChanges();
        }

        // ---

        public Contact GetContact(int contactId)
        {
            return context.Contacts.Find(contactId);
        }

        public IQueryable<Contact> GetAccountManagers()
        {
            var accountManagers = context.Advertisers.Where(a => a.AccountManagerId.HasValue).Select(a => a.AccountManager).Distinct();
            return accountManagers;
        }

        public IQueryable<Advertiser> GetAdvertisers(int? acctMgrId, bool? withBudgetedOffers)
        {
            var advertisers = context.Advertisers.AsQueryable();
            if (acctMgrId.HasValue)
                advertisers = advertisers.Where(a => a.AccountManagerId == acctMgrId.Value);

            if (withBudgetedOffers.HasValue && withBudgetedOffers.Value)
                advertisers = advertisers.Where(a => a.Offers.Any(o => o.OfferBudgets.Any()));
            if (withBudgetedOffers.HasValue && !withBudgetedOffers.Value)
                advertisers = advertisers.Where(a => !a.Offers.Any() || a.Offers.Any(o => !o.Budget.HasValue));

            return advertisers;
        }

        public Advertiser GetAdvertiser(int advertiserId)
        {
            return context.Advertisers.Find(advertiserId);
        }

        public IQueryable<Offer> GetOffers(bool includeExtended, int? acctMgrId, int? advertiserId, bool? withBudget, bool includeInactive, bool? hidden)
        {
            IQueryable<Offer> offers;
            if (includeExtended)
                offers = context.Offers.Include("Advertiser.AccountManager").Include("Advertiser.AdManager").AsQueryable();
            else
                offers = context.Offers.AsQueryable();

            if (acctMgrId.HasValue)
                offers = offers.Where(o => o.Advertiser.AccountManagerId == acctMgrId.Value);
            if (advertiserId.HasValue)
                offers = offers.Where(o => o.AdvertiserId == advertiserId);

            if (withBudget.HasValue && withBudget.Value)
                offers = offers.Where(o => o.OfferBudgets.Count > 0);
            if (withBudget.HasValue && !withBudget.Value)
                offers = offers.Where(o => o.OfferBudgets.Count == 0);

            if (!includeInactive)
                offers = offers.Where(o => o.OfferStatusId != OfferStatus.Inactive);
            if (hidden.HasValue && hidden.Value)
                offers = offers.Where(o => o.Hidden);
            if (hidden.HasValue && !hidden.Value)
                offers = offers.Where(o => !o.Hidden);

            return offers;
        }

        public Offer GetOffer(int offerId, bool includeExtended, bool fillBudgetStats)
        {
            Offer offer;
            if (includeExtended)
                offer = context.Offers.Include("Advertiser.AccountManager").Include("Advertiser.AdManager").SingleOrDefault(o => o.OfferId == offerId);
            else
                offer = context.Offers.Find(offerId);

            if (offer != null)
                FillOfferBudgetStats(offer);
            return offer;
        }

        public void FillOfferBudgetStats(Offer offer)
        {
            if (offer.Budget == null || offer.Budget <= 0)
                return;

            var ods = GetOfferDailySummariesForBudget(offer);
            if (ods.Any())
            {
                offer.BudgetUsed = ods.Sum(o => o.Revenue);
                offer.EarliestStatDate = ods.Min(o => o.Date);
                offer.LatestStatDate = ods.Max(o => o.Date);
            }
            else
                offer.BudgetUsed = 0;
        }

        public IEnumerable<CampaignSummary> TopOffers(int num, TopCampaignsBy by) //, string trafficType)  //maybe: offerIds
        {
            int minClicks = 50;
            var minDate = DateTime.Now.AddDays(-16);
            var offerDailySummaries = GetOfferDailySummaries(null, minDate, null);
            var offers = context.Offers.Where(o => !o.OfferName.Contains("pause") && !o.OfferName.Contains("not live yet")
                                                && !o.OfferName.Contains("cpm"));

            //if (trafficType != null)
            //    offers = offers.Where(

            //if (trafficType != null)
            //    campaigns = campaigns.Where(c => c.TrafficTypes.Select(t => t.Name).Contains(trafficType));

            var query = from ods in offerDailySummaries
                        join o in offers on ods.OfferId equals o.OfferId
                        group ods by new { o.OfferId, o.OfferName } into g
                        select new CampaignSummary
                        {
                            Pid = g.Key.OfferId,
                            CampaignName = g.Key.OfferName,
                            Revenue = g.Sum(ds => ds.Revenue),
                            Cost = g.Sum(ds => ds.Cost),
                            Clicks = g.Sum(ds => ds.Clicks)
                        };

            switch (by)
            {
                case TopCampaignsBy.Revenue:
                    return query.OrderByDescending(c => c.Revenue).Take(num).ToList();
                case TopCampaignsBy.Cost:
                    return query.OrderByDescending(c => c.Cost).Take(num).ToList();
                case TopCampaignsBy.EPC:
                    return query.Where(ds => ds.Clicks >= minClicks).ToList().OrderByDescending(c => c.EPC).Take(num);
                default:
                    throw new Exception("Invalid TopCampaignsBy: " + by.ToString());
            }
        }

        public IQueryable<OfferDailySummary> GetOfferDailySummaries(int? offerId, DateTime? startDate = null, DateTime? endDate = null)
        {
            var ods = context.OfferDailySummaries.AsQueryable();
            if (offerId.HasValue)
                ods = ods.Where(o => o.OfferId == offerId);
            if (startDate.HasValue)
                ods = ods.Where(o => o.Date >= startDate.Value);
            if (endDate.HasValue)
                ods = ods.Where(o => o.Date <= endDate.Value);
            return ods;
        }

        // get OfferDailySummaries used to compute budget spent for the specified offer
        public IQueryable<OfferDailySummary> GetOfferDailySummariesForBudget(int offerId)
        {
            var offer = GetOffer(offerId, false, false);
            return GetOfferDailySummariesForBudget(offer);
        }
        public IQueryable<OfferDailySummary> GetOfferDailySummariesForBudget(Offer offer)
        {
            if (offer == null)
                return new List<OfferDailySummary>().AsQueryable();

            DateTime? start = null, end = null;
            if (offer.HasBudget)
            {
                start = offer.BudgetStart;
                end = offer.BudgetEnd;
            }
            return GetOfferDailySummaries(offer.OfferId, start, end);
        }

        // ---

        private bool disposed = false;

        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                    context.Dispose();
            }
            this.disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}
