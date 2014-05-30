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

        public IQueryable<Advertiser> GetAdvertisers()
        {
            return context.Advertisers.AsQueryable();
        }

        public Advertiser GetAdvertiser(int advertiserId)
        {
            return context.Advertisers.Find(advertiserId);
        }

        public IQueryable<Offer> GetOffers(int? advertiserId)
        {
            var offers = context.Offers.AsQueryable();
            if (advertiserId.HasValue)
                offers = offers.Where(o => o.AdvertiserId == advertiserId);
            return offers;
        }

        public Offer GetOffer(int offerId)
        {
            return context.Offers.Find(offerId);
        }

        public IQueryable<OfferDailySummary> GetOfferDailySummaries(int offerId)
        {
            var ods = context.OfferDailySummaries.Where(o => o.OfferId == offerId);
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
