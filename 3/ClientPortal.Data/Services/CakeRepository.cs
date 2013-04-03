using ClientPortal.Data.Contexts;
using ClientPortal.Data.Contracts;
using ClientPortal.Data.DTOs;
using System;
using System.Linq;

namespace ClientPortal.Data.Services
{
    public class CakeRepository : ICakeRepository
    {
        CakeContext cakeContext;

        public CakeRepository(CakeContext cakeContext)
        {
            this.cakeContext = cakeContext;
        }

        public void SaveChanges()
        {
            cakeContext.SaveChanges();
        }

        public IQueryable<CakeAdvertiser> Advertisers
        {
            get { return cakeContext.CakeAdvertisers; }
        }

        public CakeAdvertiser Advertiser(int advertiserId)
        {
            var advertiser = cakeContext.CakeAdvertisers.Where(a => a.Advertiser_Id == advertiserId).FirstOrDefault();
            return advertiser;
        }

        public IQueryable<CakeOffer> Offers(int? advertiserId)
        {
            var offers = cakeContext.CakeOffers.AsQueryable();
            if (advertiserId.HasValue)
            {
                var advId = advertiserId.Value.ToString();
                offers = offers.Where(o => o.Advertiser_Id == advId);
            }
            return offers;
        }

        public IQueryable<DailySummary> GetDailySummaries(DateTime? start, DateTime? end, int advertiserId, int? offerId, out string currency)
        {
            var dailySummaries = cakeContext.DailySummaries.AsQueryable();
            if (start.HasValue) dailySummaries = dailySummaries.Where(ds => ds.date >= start);
            if (end.HasValue) dailySummaries = dailySummaries.Where(ds => ds.date <= end);

            var offers = Offers(advertiserId);

            if (offerId.HasValue)
            {
                offers = offers.Where(o => o.Offer_Id == offerId.Value);
                dailySummaries = dailySummaries.Where(ds => ds.offer_id == offerId.Value);
            }
            else
            {
                dailySummaries = from ds in dailySummaries
                                 join o in offers on ds.offer_id equals o.Offer_Id
                                 select ds;
            }
            currency = null; // Assume all offers for the advertiser have the same currency
            if (offers.Any()) currency = offers.First().Currency;

            return dailySummaries;
        }

        public DateRangeSummary GetDateRangeSummary(DateTime? start, DateTime? end, int advertiserId, int? offerId)
        {
            string currency;
            var dailySummaries = GetDailySummaries(start, end, advertiserId, offerId, out currency);

            var any = dailySummaries.Any();
            DateRangeSummary ai = new DateRangeSummary()
            {
                Clicks = any ? dailySummaries.Sum(ds => ds.clicks) : 0,
                Conversions = any ? dailySummaries.Sum(ds => ds.conversions) : 0,
                Revenue = any ? dailySummaries.Sum(ds => ds.revenue) : 0,
                Currency = currency
            };
            return ai;
        }

        public IQueryable<MonthlyInfo> GetMonthlyInfosFromDaily(DateTime? start, DateTime? end, int advertiserId, int? offerId)
        {
            string currency;
            var dailySummaries = GetDailySummaries(start, end, advertiserId, offerId, out currency);

            var m = from ds in dailySummaries
                    group ds by new { ds.date.Year, ds.date.Month } into g
                    select new MonthlyInfo()
                    {
                        Year = g.Key.Year,
                        Month = g.Key.Month,
                        AdvertiserId = advertiserId,
                        OfferId = offerId.HasValue ? offerId.Value : -1,
                        Revenue = g.Sum(ds => ds.revenue),
                        Currency = currency
                    };
            return m;
        }

        public IQueryable<OfferInfo> GetOfferInfos(DateTime? start, DateTime? end, int? advertiserId)
        {
            var dailySummaries = cakeContext.DailySummaries.AsQueryable();
            if (start.HasValue) dailySummaries = dailySummaries.Where(ds => ds.date >= start);
            if (end.HasValue) dailySummaries = dailySummaries.Where(ds => ds.date <= end);

            var summaryGroups = dailySummaries.GroupBy(s => s.offer_id);

            var offers = Offers(advertiserId);
            var offerInfos = from offer in offers
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

        public IQueryable<DailyInfo> GetDailyInfos(DateTime? start, DateTime? end, int? advertiserId)
        {
            var offers = Offers(advertiserId);
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

        // note: we'll go until 23:59:59 on the "end" date
        public IQueryable<ConversionInfo> GetConversionInfos(DateTime? start, DateTime? end, int? advertiserId, int? offerId)
        {
            var conversions = GetConversions(start, end, advertiserId, offerId);

            var conversionInfos =
                from c in conversions
                join curr in cakeContext.CakeCurrencies on c.PriceReceivedCurrencyId.Value equals curr.Id
                join offer in cakeContext.CakeOffers on c.Offer_Id.Value equals offer.Offer_Id into gj
                from o in gj.DefaultIfEmpty() // left join to CakeOffers
                select new ConversionInfo()
                {
                    ConversionId = c.Conversion_Id,
                    Date = c.ConversionDate.Value,
                    AffId = c.Affiliate_Id ?? 0,
                    OfferId = c.Offer_Id ?? 0,
                    Offer = (o == null) ? String.Empty : o.OfferName,
                    PriceReceived = c.PriceReceived ?? 0,
                    Currency = curr.Name,
                    TransactionId = c.Transaction_Id,
                    Positive = c.Positive
                };
            return conversionInfos;
        }

        // note: we'll go until 23:59:59 on the "end" date
        public IQueryable<ConversionSummary> GetConversionSummaries(DateTime? start, DateTime? end, int? advertiserId, int? offerId)
        {
            var conversions = GetConversions(start, end, advertiserId, offerId);
            var conversionGroups = conversions.GroupBy(c => c.Offer_Id);

            var offers = Offers(advertiserId);

            if (offerId.HasValue)
                offers = offers.Where(o => o.Offer_Id == offerId.Value);

            var conversionSummaries =
                from offer in offers
                join conversionGroup in conversionGroups on offer.Offer_Id equals conversionGroup.Key into gj
                from convGroup in gj.DefaultIfEmpty() // used to left join to conversionGroups
                select new ConversionSummary()
                {
                    AdvertiserId = offer.Advertiser_Id,
                    OfferId = offer.Offer_Id,
                    OfferName = offer.OfferName,
                    Format = offer.DefaultPriceFormat,
                    Currency = offer.Currency,
                    Count = convGroup.Count(),
                    PositiveCount = (convGroup.Count() == 0) ? 0 : convGroup.Count(g => g.Positive == true),
                    Revenue = (convGroup.Count() == 0) ? 0 : convGroup.Sum(g => g.PriceReceived ?? 0)
                };
            return conversionSummaries;
        }

        public IQueryable<CakeConversion> GetConversions(DateTime? start, DateTime? end, int? advertiserId, int? offerId)
        {
            var conversions = cakeContext.CakeConversions.AsQueryable();
            if (start.HasValue)
                conversions = conversions.Where(c => c.ConversionDate >= start.Value);
            if (end.HasValue)
            {
                DateTime endOfDay = new DateTime(end.Value.Year, end.Value.Month, end.Value.Day, 23, 59, 59);
                conversions = conversions.Where(c => c.ConversionDate <= endOfDay);
            }
            if (advertiserId.HasValue)
                conversions = conversions.Where(c => c.Advertiser_Id == advertiserId.Value);
            if (offerId.HasValue)
                conversions = conversions.Where(c => c.Offer_Id == offerId);

            return conversions;
        }

        public IQueryable<CakeConversion> Conversions
        {
            get { return cakeContext.CakeConversions; }
        }
    }
}
