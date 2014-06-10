using DirectAgents.Domain.Entities;
using DirectAgents.Domain.Entities.Cake;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DirectAgents.Domain.Abstract
{
    public interface IMainRepository : IDisposable
    {
        void SaveChanges();

        Contact GetContact(int contactId);
        IQueryable<Contact> GetAccountManagers();
        IQueryable<Advertiser> GetAdvertisers(int? acctMgrId, bool? withBudgetedOffers);
        Advertiser GetAdvertiser(int advertiserId);
        IQueryable<Offer> GetOffers(bool includeExtended, int? acctMgrId, int? advertiserId, bool? withBudget, bool includeInactive, bool? hidden);
        Offer GetOffer(int offerId, bool includeExtended, bool fillBudgetStats);

        void FillOfferBudgetStats(Offer offer);

        IQueryable<OfferDailySummary> GetOfferDailySummaries(int offerId, DateTime? startDate = null, DateTime? endDate = null);
        IQueryable<OfferDailySummary> GetOfferDailySummariesForBudget(int offerId);
        IQueryable<OfferDailySummary> GetOfferDailySummariesForBudget(Offer offer);
    }
}
