using System;
using System.Collections.Generic;
using System.Linq;
using DirectAgents.Domain.DTO;
using DirectAgents.Domain.Entities.AdRoll;
using DirectAgents.Domain.Entities.Cake;

namespace DirectAgents.Domain.Abstract
{
    public interface IMainRepository : IDisposable
    {
        void SaveChanges();

        // Cake
        Contact GetContact(int contactId);
        IQueryable<Contact> GetAccountManagers();
        IQueryable<Advertiser> GetAdvertisers(int? acctMgrId = null, bool? withBudgetedOffers = null);
        Advertiser GetAdvertiser(int advertiserId);
        IQueryable<Offer> GetOffers(bool includeExtended, int? acctMgrId, int? advertiserId, bool? withBudget, bool includeInactive, bool? hidden);
        IQueryable<Offer> GetOffersUnion(bool includeExtendedInfo, bool excludeInactive, bool? withBudget, int[] offerIds);
        Offer GetOffer(int offerId, bool includeExtended, bool fillBudgetStats);

        void FillOfferBudgetStats(Offer offer);

        IEnumerable<CampaignSummary> TopOffers(int num, TopCampaignsBy by);

        IQueryable<OfferDailySummary> GetOfferDailySummaries(int? offerId, DateTime? startDate = null, DateTime? endDate = null);
        IQueryable<OfferDailySummary> GetOfferDailySummariesForBudget(int offerId);
        IQueryable<OfferDailySummary> GetOfferDailySummariesForBudget(Offer offer);

        StatsSummary GetStatsSummary(int? offerId, DateTime? startDate, DateTime? endDate);

        // AdRoll
        IQueryable<Advertisable> Advertisables();
        IQueryable<AdvertisableStat> AdvertisableStats(int? advertisableId, DateTime? startDate, DateTime? endDate);
        void FillStats(Advertisable adv, DateTime? startDate, DateTime? endDate);
    }
}
