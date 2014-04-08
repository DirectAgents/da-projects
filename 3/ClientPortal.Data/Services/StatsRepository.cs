using ClientPortal.Data.Contexts;
using ClientPortal.Data.DTOs;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace ClientPortal.Data.Services
{
    public partial class ClientPortalRepository
    {
        public IQueryable<OfferDailySummary> GetDailySummaries(DateTime? start, DateTime? end, int? advertiserId, int? offerId, out string currency)
        {
            var dailySummaries = context.OfferDailySummaries.AsQueryable();

            if (start.HasValue) dailySummaries = dailySummaries.Where(ds => ds.date >= start);

            if (end.HasValue) dailySummaries = dailySummaries.Where(ds => ds.date <= end);

            var offers = Offers(advertiserId);

            if (offerId.HasValue)
            {
                offers = offers.Where(o => o.OfferId == offerId.Value);
                dailySummaries = dailySummaries.Where(ds => ds.offer_id == offerId.Value);
            }
            else
            {
                dailySummaries = from ds in dailySummaries
                                 join o in offers on ds.offer_id equals o.OfferId
                                 select ds;
            }
            currency = null; // Assume all offers for the advertiser have the same currency
            if (offers.Any()) currency = offers.First().Currency;

            return dailySummaries;
        }

        public DateRangeSummary GetDateRangeSummary(DateTime? start, DateTime? end, int? advertiserId, int? offerId, bool includeConversionData)
        {
            string currency;
            var dailySummaries = GetDailySummaries(start, end, advertiserId, offerId, out currency);

            var any = dailySummaries.Any();
            DateRangeSummary summary = new DateRangeSummary()
            {
                Clicks = any ? dailySummaries.Sum(ds => ds.clicks) : 0,
                Conversions = any ? dailySummaries.Sum(ds => ds.conversions) : 0,
                Revenue = any ? dailySummaries.Sum(ds => ds.revenue) : 0,
                Currency = currency
            };
            if (includeConversionData)
            {
                var conversions = GetConversions(start, end, advertiserId, offerId);
                var conv_datas =
                    from c in conversions
                    join conv_data in context.ConversionDatas on c.conversion_id equals conv_data.conversion_id
                    select conv_data;

                summary.ConVal = conv_datas.Any() ? conv_datas.Sum(c => c.value0) : 0;
            }
            return summary;
        }

        #region Report Queries
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
            var dailySummaries = context.OfferDailySummaries.AsQueryable();
            if (start.HasValue) dailySummaries = dailySummaries.Where(ds => ds.date >= start);
            if (end.HasValue) dailySummaries = dailySummaries.Where(ds => ds.date <= end);

            var summaryGroups = dailySummaries.GroupBy(s => s.offer_id);

            var offers = Offers(advertiserId);
            var offerInfos = from offer in offers
                             join sumGroup in summaryGroups on offer.OfferId equals sumGroup.Key
                             select new OfferInfo()
                             {
                                 OfferId = offer.OfferId,
                                 AdvertiserId_Int = offer.AdvertiserId,
                                 Name = offer.OfferName,
                                 Format = offer.DefaultPriceFormat,
                                 Clicks = (sumGroup.Count() == 0) ? 0 : sumGroup.Sum(s => s.clicks),
                                 Conversions = (sumGroup.Count() == 0) ? 0 : sumGroup.Sum(s => s.conversions),
                                 Revenue = (sumGroup.Count() == 0) ? 0 : sumGroup.Sum(s => s.revenue),
                                 Currency = offer.Currency,
                             };
            return offerInfos;
        }

        public IQueryable<DailyInfo> GetDailyInfos(DateTime? start, DateTime? end, int? advertiserId, int? offerId = null)
        {
            var offers = Offers(advertiserId);
            if (offerId.HasValue)
                offers = offers.Where(o => o.OfferId == offerId.Value);

            var offerIds = offers.Select(o => o.OfferId).ToList();

            string currency = null; // Assume all offers for the advertiser have the same currency
            if (offers.Count() > 0) currency = offers.First().Currency;

            var dailySummaries = context.OfferDailySummaries.Where(ds => offerIds.Contains(ds.offer_id));
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

        public IQueryable<ConversionInfo> GetConversionInfos(DateTime? start, DateTime? end, int? advertiserId, int? offerId)
        {
            var conversions = GetConversions(start, end, advertiserId, offerId);

            var conversionInfos =
                from c in conversions
                join offer in context.Offers on c.offer_id equals offer.OfferId into gj_offer
                from o in gj_offer.DefaultIfEmpty() // left join to CakeOffers
                join conv_data in context.ConversionDatas on c.conversion_id equals conv_data.conversion_id into gj_conv_data
                from cd in gj_conv_data.DefaultIfEmpty() // left join to ConversionData
                select new ConversionInfo()
                {
                    //ConversionIdString = c.conversion_id,
                    ConversionId = c.conversion_id,
                    Date = c.conversion_date,
                    AffId = c.affiliate_id,
                    OfferId = c.offer_id,
                    Offer = (o == null) ? null : o.OfferName,
                    PriceReceived = c.received_amount,
                    CurrencyId = c.received_currency_id,
                    TransactionId = c.transaction_id,
                    ConVal = (cd == null) ? 0 : cd.value0
                };
            return conversionInfos;
        }

        public IQueryable<ConversionSummary> GetConversionSummaries(DateTime? start, DateTime? end, int? advertiserId, int? offerId)
        {
            var conversions = GetConversions(start, end, advertiserId, offerId);
            var conversionGroups = conversions.GroupBy(c => c.offer_id);

            var offers = Offers(advertiserId);

            if (offerId.HasValue)
                offers = offers.Where(o => o.OfferId == offerId.Value);

            var conversionSummaries =
                from offer in offers
                join conversionGroup in conversionGroups on offer.OfferId equals conversionGroup.Key into gj_convgroup
                from convGroup in gj_convgroup.DefaultIfEmpty() // left join to conversionGroups
                select new ConversionSummary()
                {
                    AdvertiserId = offer.AdvertiserId,
                    OfferId = offer.OfferId,
                    OfferName = offer.OfferName,
                    Format = offer.DefaultPriceFormat,
                    Currency = offer.Currency,
                    Count = convGroup.Count(),
                    Revenue = (convGroup.Count() == 0) ? 0 : convGroup.Sum(g => g.received_amount),
                    //ConValTotal = 
                };
            return conversionSummaries;
        }

        public IQueryable<AffiliateSummary> GetAffiliateSummaries(DateTime? start, DateTime? end, int? advertiserId, int? offerId)
        {
            var conversions = GetConversions(start, end, advertiserId, offerId);

            var offers = Offers(advertiserId);
            if (offerId.HasValue)
                offers = offers.Where(o => o.OfferId == offerId.Value);

            var affiliateInfos =
                from conv in conversions
                from offer in offers
                where conv.offer_id == offer.OfferId
                select new
                {
                    AffId = conv.affiliate_id,
                    OfferId = conv.offer_id,
                    Offer = (offer == null) ? String.Empty : offer.OfferName,
                    PriceReceived = conv.received_amount,
                    CurrencyId = conv.received_currency_id
                };

            // Doing group in memory because generated query was not optimal.. (TODO: see if Kevin knows how to improve this?)
            var groupedConversionInfos = affiliateInfos.AsEnumerable().GroupBy(
                    c => new { c.AffId, c.OfferId, c.Offer, c.CurrencyId },
                    (key, group) => new AffiliateSummary()
                    {
                        AffId = key.AffId,
                        OfferId = key.OfferId,
                        Offer = key.Offer,
                        CurrencyId = key.CurrencyId,
                        PriceReceived = group.Sum(c => c.PriceReceived),
                        Count = group.Count(c => true)
                    });

            return groupedConversionInfos.AsQueryable();
        }

        public IQueryable<MonthlyInfo> GetMonthlyInfos(string type, DateTime? start, DateTime? end, int? advertiserId)
        {
            List<MonthlyInfo> stub = new List<MonthlyInfo>();
            return stub.AsQueryable();
        }
        #endregion

        // get conversions through 23:59:59 on the "end" date
        public IQueryable<Conversion> GetConversions(DateTime? start, DateTime? end, int? advertiserId, int? offerId)
        {
            var conversions = context.Conversions.AsQueryable();
            if (start.HasValue)
                conversions = conversions.Where(c => c.conversion_date >= start.Value);
            if (end.HasValue)
            {
                DateTime endOfDay = new DateTime(end.Value.Year, end.Value.Month, end.Value.Day, 23, 59, 59);
                conversions = conversions.Where(c => c.conversion_date <= endOfDay);
            }
            if (advertiserId.HasValue)
                conversions = conversions.Where(c => c.advertiser_id == advertiserId.Value);
            if (offerId.HasValue)
                conversions = conversions.Where(c => c.offer_id == offerId.Value);

            return conversions;
        }

        public IList<DeviceClicks> GetClicksByDeviceName(DateTime? start, DateTime? end, int? advertiserId, int? offerId)
        {
            using (var db = new ClientPortalDWContext())
            {
                var result = db.ClicksByDevice(advertiserId, start, end)
                    .OrderByDescending(c => c.ClickCount)
                    .ToList();

                return result;
            }
        }

        public IList<ConversionsByRegion> GetConversionCountsByRegion(DateTime start, DateTime end, int advertiserId)
        {
            using (var db = new ClientPortalDWContext())
            {
                var result = db.ConversionsByRegion(advertiserId, start, end)
                               .OrderBy(c => c.ClickCount)
                               .ToList();

                return result;
            }
        }

        #region ConversionData
        public void AddConversionData(ConversionData entity)
        {
            context.ConversionDatas.Add(entity);
        }

        public IQueryable<ConversionData> ConversionData
        {
            get { return context.ConversionDatas; }
        }
        #endregion
    }
}
