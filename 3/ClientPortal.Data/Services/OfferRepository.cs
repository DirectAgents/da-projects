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

        // note: we only use the year and month portion of "start" and "end"
        public IQueryable<MonthlyInfo> GetMonthlyInfos(string type, DateTime? start, DateTime? end, int? advertiserId)
        {
            //TODO: make sure advertiser matches by name and offers match by name?

            var summaries = cakeContext.MonthlySummaries.AsQueryable();
            if (!string.IsNullOrWhiteSpace(type))
                summaries = summaries.Where(s => s.UnitType == "CPM");
            if (start.HasValue)
                summaries = summaries.Where(s => (s.Year > start.Value.Year) || (s.Year == start.Value.Year && s.Month >= start.Value.Month));
            if (end.HasValue)
                summaries = summaries.Where(s => (s.Year < end.Value.Year) || (s.Year == end.Value.Year && s.Month <= end.Value.Month));
            if (advertiserId.HasValue)
                summaries = summaries.Where(s => s.AdvertiserId == advertiserId);

            var monthlyInfos =
                from s in summaries
                group s by new { s.Year, s.Month, s.AdvertiserId, s.Pid, s.CampaignName, s.RevCur, s.CostCur, s.CampaignStatusId, s.ItemAccountingStatusId }
                    into g
                    select new MonthlyInfo()
                    {
                        //Period = new DateTime(g.Key.Year, g.Key.Month, 1),
                        Year = g.Key.Year,
                        Month = g.Key.Month,
                        AdvertiserId = g.Key.AdvertiserId,
                        OfferId = g.Key.Pid,
                        Offer = g.Key.CampaignName,
                        Currency = g.Key.RevCur,
                        Revenue = g.Sum(s => s.TotalRev),
                        CampaignStatusId = g.Key.CampaignStatusId,
                        AccountingStatusId = g.Key.ItemAccountingStatusId
                    };
            return monthlyInfos;
        }
    }
}
