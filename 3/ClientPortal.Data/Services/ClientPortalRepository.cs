using ClientPortal.Data.Contexts;
using ClientPortal.Data.Contracts;
using ClientPortal.Data.DTOs;
using System;
using System.Linq;

namespace ClientPortal.Data.Services
{
    public class ClientPortalRepository : IClientPortalRepository
    {
        ClientPortalContext context;

        public ClientPortalRepository(ClientPortalContext clientPortalContext)
        {
            this.context = clientPortalContext;
        }

        public DateRangeSummary GetDateRangeSummary(DateTime? start, DateTime? end, string advertiserId, int? offerId)
        {
            int? advId = ParseInt(advertiserId);

            var clicks = GetClicks(start, end, advId, offerId);
            var conversions = GetConversions(start, end, advId, offerId);

            var anyConv = conversions.Any();
            DateRangeSummary summary = new DateRangeSummary()
            {
                Clicks = clicks.Count(),
                Conversions = anyConv ? conversions.Count() : 0,
                Revenue = anyConv ? conversions.Sum(c => c.received_amount) : 0,
                Currency = null // TODO: determine this... need Offers in ClientPortalContext
            };
            return summary;
        }

        public IQueryable<ConversionInfo> GetConversionInfos(DateTime? start, DateTime? end, int? advertiserId, int? offerId)
        {
            var conversions = GetConversions(start, end, advertiserId, offerId);

            var conversionInfos =
                from c in conversions
//                join curr in cakeContext.CakeCurrencies on c.PriceReceivedCurrencyId.Value equals curr.Id
//                join offer in cakeContext.CakeOffers on c.Offer_Id.Value equals offer.Offer_Id into gj
//                from o in gj.DefaultIfEmpty() // left join to CakeOffers
                select new ConversionInfo()
                {
                    ConversionIdString = c.conversion_id,
                    Date = c.conversion_date,
                    AffId = c.affiliate_id,
                    OfferId = c.offer_id,
                    Offer = String.Empty,
//                    Offer = (o == null) ? String.Empty : o.OfferName,
                    PriceReceived = c.received_amount,
                    Currency = null,
//                    Currency = curr.Name,
                    TransactionId = c.transaction_id,
//                    Positive = c.Positive
                };
            return conversionInfos;
        }

        // get clicks through 23:59:59 on the "end" date
        public IQueryable<Click> GetClicks(DateTime? start, DateTime? end, int? advertiserId, int? offerId)
        {
            var clicks = context.Clicks.AsQueryable();
            if (start.HasValue)
                clicks = clicks.Where(c => c.click_date >= start.Value);
            if (end.HasValue)
            {
                DateTime endOfDay = new DateTime(end.Value.Year, end.Value.Month, end.Value.Day, 23, 59, 59);
                clicks = clicks.Where(c => c.click_date <= endOfDay);
            }
            if (advertiserId.HasValue)
                clicks = clicks.Where(c => c.advertiser_id == advertiserId.Value);
            if (offerId.HasValue)
                clicks = clicks.Where(c => c.offer_id == offerId.Value);

            return clicks;
        }

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


        public static int? ParseInt(string stringVal)
        {
            int intVal;
            if (Int32.TryParse(stringVal, out intVal))
                return intVal;
            else
                return null;
        }
    }
}
