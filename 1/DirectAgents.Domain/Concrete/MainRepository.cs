using DirectAgents.Domain.Abstract;
using DirectAgents.Domain.Contexts;
using DirectAgents.Domain.Entities;
using DirectAgents.Domain.Entities.Cake;
using System;
using System.Collections.Generic;
using System.Linq;

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

        public IQueryable<Advertiser> GetAdvertisers(int? acctMgrId)
        {
            var advertisers = context.Advertisers.AsQueryable();
            if (acctMgrId.HasValue)
                advertisers = advertisers.Where(a => a.AccountManagerId == acctMgrId.Value);

            return advertisers;
        }

        public Advertiser GetAdvertiser(int advertiserId)
        {
            return context.Advertisers.Find(advertiserId);
        }

        public IQueryable<Offer> GetOffers(int? advertiserId, bool? withBudget)
        {
            var offers = context.Offers.AsQueryable();
            if (advertiserId.HasValue)
                offers = offers.Where(o => o.AdvertiserId == advertiserId);
            if (withBudget.HasValue && withBudget.Value)
                offers = offers.Where(o => o.Budget.HasValue);
            if (withBudget.HasValue && !withBudget.Value)
                offers = offers.Where(o => !o.Budget.HasValue);

            return offers;
        }

        public Offer GetOffer(int offerId)
        {
            return context.Offers.Find(offerId);
        }

        public decimal? GetOfferAvailableBudget(int offerId)
        {
            var offer = GetOffer(offerId);
            if (offer == null) return null;
//            return offer.GetAvailableBudget(this);
            if (offer.Budget == null)
                return null;

            decimal spent = 0;
            var ods = GetOfferDailySummariesForBudget(offer);
            if (ods.Any())
                spent = ods.Sum(o => o.Revenue);

            return (offer.Budget.Value - spent);
        }

        public IQueryable<OfferDailySummary> GetOfferDailySummaries(int offerId, DateTime? startDate = null, DateTime? endDate = null)
        {
            var ods = context.OfferDailySummaries.Where(o => o.OfferId == offerId);
            if (startDate.HasValue)
                ods = ods.Where(o => o.Date >= startDate.Value);
            if (endDate.HasValue)
                ods = ods.Where(o => o.Date <= endDate.Value);
            return ods;
        }

        // get OfferDailySummaries used to compute budget spent for the specified offer
        public IQueryable<OfferDailySummary> GetOfferDailySummariesForBudget(int offerId)
        {
            var offer = GetOffer(offerId);
            return GetOfferDailySummariesForBudget(offer);
        }
        public IQueryable<OfferDailySummary> GetOfferDailySummariesForBudget(Offer offer)
        {
            if (offer == null)
                return new List<OfferDailySummary>().AsQueryable();

            DateTime statStart;
            if (offer.BudgetIsMonthly)
            {
                statStart = new DateTime(DateTime.Today.Year, DateTime.Today.Month, 1);
                if (offer.BudgetStart.HasValue && offer.BudgetStart.Value > statStart)
                    statStart = offer.BudgetStart.Value;
            }
            else
            {
                statStart = (offer.BudgetStart.HasValue ? offer.BudgetStart.Value : offer.DateCreated);
            }
            var ods = context.OfferDailySummaries.Where(o => o.OfferId == offer.OfferId && o.Date >= statStart);
            return ods;
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
