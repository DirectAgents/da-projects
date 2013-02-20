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

        public IQueryable<OfferInfo> GetOfferInfos(DateTime? start, DateTime? end)
        {
            var dailySummaries = cakeContext.DailySummaries.AsQueryable();
            if (start.HasValue) dailySummaries = dailySummaries.Where(ds => ds.date >= start);
            if (end.HasValue) dailySummaries = dailySummaries.Where(ds => ds.date <= end);

            var summaryGroups = dailySummaries.GroupBy(s => s.offer_id);

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
                                 Currency = offer.Currency,
                             };
            return offerInfos;
        }

        public IQueryable<DailyInfo> GetDailyInfos(DateTime? start, DateTime? end, int advertiserId)
        {
            string advId = advertiserId.ToString();
            var offers = cakeContext.CakeOffers.Where(o => o.Advertiser_Id == advId);
            var offerIds = offers.Select(o => o.Offer_Id).ToList();

            string currency = null; // Assume all offers for the advertiser have the same currency
            if (offers.Count() > 0) currency = offers.First().Currency;

            var dailySummaries = cakeContext.DailySummaries.Where(ds => offerIds.Contains(ds.offer_id));
            if (start.HasValue) dailySummaries = dailySummaries.Where(ds => ds.date >= start);
            if (end.HasValue) dailySummaries = dailySummaries.Where(ds => ds.date <= end);

            var dailyInfos = from sumGroup in dailySummaries.GroupBy(s => s.date)
                             select new DailyInfo()
                             {
                                 Date = sumGroup.Key,
                                 Impressions = sumGroup.Sum(s => s.views),
                                 Clicks = sumGroup.Sum(s => s.clicks),
                                 Conversions = sumGroup.Sum(s => s.conversions),
                                 Revenue = sumGroup.Sum(s => s.revenue),
                                 Currency = currency
                             };
            return dailyInfos;
        }
    }
}
