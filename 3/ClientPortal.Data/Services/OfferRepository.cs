using ClientPortal.Data.Contexts;
using ClientPortal.Data.Contracts;
using ClientPortal.Data.DTOs;
using System;
using System.Linq;

namespace ClientPortal.Data.Services
{
    public class OfferRepository : IOfferRepository
    {
        CakeContext cakeContext;

        public OfferRepository(CakeContext cakeContext)
        {
            this.cakeContext = cakeContext;
        }

        public IQueryable<CakeOffer> GetAll()
        {
            return cakeContext.CakeOffers;
        }

        public IQueryable<OfferInfo> GetOfferInfos(DateTime since)
        {
            var summaryGroups = from ds in cakeContext.DailySummaries
                                where ds.date >= since
                                group ds by ds.offer_id;

            var offerInfos = from offer in cakeContext.CakeOffers
                             join sumGroup in summaryGroups on offer.Offer_Id equals sumGroup.Key
//                             join summaryGroup in summaryGroups on offer.Offer_Id equals summaryGroup.Key into gj
//                             from sumGroup in gj.DefaultIfEmpty() // used to left join to DailySummaries
                             select new OfferInfo()
                             {
                                 OfferId = offer.Offer_Id,
                                 AdvertiserId = offer.Advertiser_Id,
                                 Name = offer.OfferName,
                                 Format = offer.DefaultPriceFormat,
                                 Clicks = (sumGroup.Count() == 0) ? 0 : sumGroup.Sum(s => s.clicks),
                                 Conversions = (sumGroup.Count() == 0) ? 0 : sumGroup.Sum(s => s.conversions),
                                 Revenue = (sumGroup.Count() == 0) ? 0 : sumGroup.Sum(s => s.revenue),
                             };
            return offerInfos;
        }
/*
        public IQueryable<OfferInfo> GetOfferInfos()
        {
            var offerInfos = cakeContext.CakeOffers.
                Select(o => new OfferInfo()
                {
                    OfferId = o.Offer_Id,
                    AdvertiserId = o.Advertiser_Id,
                    Name = o.OfferName,
                    Format = o.DefaultPriceFormat
                }).ToList();
            // TODO: use a join? groupjoin?
            var dailySummaries = cakeContext.DailySummaries.Where(ds => ds.date >= new DateTime(2013, 2, 1));
            foreach (var offerInfo in offerInfos)
            {
                var offerSummaries = dailySummaries.Where(ds => ds.offer_id == offerInfo.OfferId);
                offerInfo.Clicks = (offerSummaries.Count() > 0) ? offerSummaries.Sum(os => os.clicks) : 0;
            }
            return offerInfos.AsQueryable();
        }
*/

    }
}
