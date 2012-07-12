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

        public static IQueryable<CakeConversionSummary> ByOfferIdAndDateRange(this ObjectSet<CakeConversionSummary> cakeConversionSummaries, int offerID, DateTime fromDate, DateTime toDate)
        {
            var dayAfterToDate = toDate.AddDays(1);
            var conversionSummariesQuery = from c in cakeConversionSummaries
                                           where c.ConversionDate >= fromDate && c.ConversionDate < dayAfterToDate && c.Offer_Id == offerID
                                           select c;

            return conversionSummariesQuery;
        }

        public static IQueryable<CakeConversion> ByOfferIdAndDateRange(this ObjectSet<CakeConversion> cakeConversions, int offerID, DateTime fromDate, DateTime toDate)
        {
            var dayAfterToDate = toDate.AddDays(1);
            var cakeConversionsQuery = from c in cakeConversions
                                       where c.ConversionDate >= fromDate && c.ConversionDate < dayAfterToDate && c.Offer_Id == offerID
                                       select c;

            return cakeConversionsQuery;
        }
    }
}
