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
        IQueryable<Offer> GetOffers(bool includeExtended, int? acctMgrId, int? advertiserId, bool? withBudget);
        Offer GetOffer(int offerId);

        decimal? GetOfferAvailableBudget(int offerId);
        //decimal? GetOfferAvailableBudget(Offer offer);
        void FillOfferBudgetStats(Offer offer);

        IQueryable<OfferDailySummary> GetOfferDailySummaries(int offerId, DateTime? startDate = null, DateTime? endDate = null);
        IQueryable<OfferDailySummary> GetOfferDailySummariesForBudget(int offerId);
        IQueryable<OfferDailySummary> GetOfferDailySummariesForBudget(Offer offer);
    }
}
