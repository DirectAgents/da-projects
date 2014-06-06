﻿using DirectAgents.Domain.Abstract;
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

        public Offer GetOffer(int offerId, bool includeExtended) //TODO: add arg: bool fillBudgetStats
        {
            if (includeExtended)
            {
                return context.Offers.Include("Advertiser.AccountManager").Include("Advertiser.AdManager").SingleOrDefault(o => o.OfferId == offerId);
            }
            else
                return context.Offers.Find(offerId);
        }

        public decimal? GetOfferAvailableBudget(int offerId)
        {
            var offer = GetOffer(offerId, false);
            if (offer == null) return null;

            FillOfferBudgetStats(offer);
            return offer.BudgetAvailable;
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
            var offer = GetOffer(offerId, false);
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
