using System;
using System.Collections.Generic;
using System.Data.Objects;
using System.Linq;

namespace EomApp1.Screens.Synch.Models.Cake
{
    public partial class CakeEntities
    {
    }

    public static class CakeEntitiesExtensions
    {
        public static CakeConversion Create(this ObjectSet<CakeConversion> cakeConversions, int conversionID)
        {
            var newConversion = new CakeConversion 
            { 
                Conversion_Id = conversionID 
            };
            cakeConversions.AddObject(newConversion);
            return newConversion;
        }

        public static CakeConversion ById(this List<CakeConversion> cakeConversions, int conversionID)
        {
            var cakeConversionQuery = from c in cakeConversions
                                      where c.Conversion_Id == conversionID
                                      select c;

            return cakeConversionQuery.FirstOrDefault();
        }

        public static IQueryable<CakeConversionSummary> ByOfferIdAndDateRange(this ObjectSet<CakeConversionSummary> cakeConversionSummaries, int offerID, DateTime fromDate, DateTime toDate, int? affiliateID = null)
        {
            var dayAfterToDate = toDate.AddDays(1);
            var conversionSummariesQuery = from c in cakeConversionSummaries
                                           where c.ConversionDate >= fromDate && c.ConversionDate < dayAfterToDate && c.Offer_Id == offerID
                                           select c;
            if (affiliateID.HasValue)
                conversionSummariesQuery = conversionSummariesQuery.Where(x => x.Affiliate_Id == affiliateID.Value);

            return conversionSummariesQuery;
        }

        public static IQueryable<CakeConversion> ByOfferIdAndDateRange(this ObjectSet<CakeConversion> cakeConversions, int offerID, DateTime? fromDate = null, DateTime? toDate = null, int? affiliateID = null)
        {
            var cakeConversionsQuery = from c in cakeConversions
                                       where c.Offer_Id == offerID
                                       select c;
            if (fromDate.HasValue)
                cakeConversionsQuery = cakeConversionsQuery.Where(x => x.ConversionDate >= fromDate.Value);
            if (toDate.HasValue)
            {
                var dayAfterToDate = toDate.Value.AddDays(1);
                cakeConversionsQuery = cakeConversionsQuery.Where(x => x.ConversionDate < dayAfterToDate);
            }
            if (affiliateID.HasValue)
                cakeConversionsQuery = cakeConversionsQuery.Where(x => x.Affiliate_Id == affiliateID.Value);

            return cakeConversionsQuery;
        }
    }
}
