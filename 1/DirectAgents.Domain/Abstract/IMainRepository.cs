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

        IQueryable<Advertiser> GetAdvertisers();
        Advertiser GetAdvertiser(int advertiserId);
        IQueryable<Offer> GetOffers(int? advertiserId);
        Offer GetOffer(int offerId);

        IQueryable<OfferDailySummary> GetOfferDailySummaries(int offerId);
    }
}
