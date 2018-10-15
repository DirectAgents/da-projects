using System;
using System.Linq;
using DirectAgents.Domain.Contexts;
using DirectAgents.Domain.Entities.Cake;
using Z.EntityFramework.Plus;

namespace DirectAgents.Domain.Helpers
{
    public class MainHelper
    {
        //Somehow this doesn't work when called from a CakeExtracter command. Give a NullReferenceException when calling .Where()

        //public static IQueryable<EventConversion> FilterEventConversions(IQueryable<EventConversion> ec, int? advertiserId = null, int? offerId = null, int? affiliateId = null, DateTime? startDate = null, DateTime? endDate = null)
        //{
        //    if (advertiserId.HasValue)
        //        ec = ec.Where(x => x.Offer.AdvertiserId == advertiserId.Value);
        //    if (offerId.HasValue)
        //        ec = ec.Where(x => x.OfferId == offerId.Value);
        //    if (affiliateId.HasValue)
        //        ec = ec.Where(x => x.AffiliateId == affiliateId.Value);
        //    if (startDate.HasValue)
        //        ec = ec.Where(x => x.ConvDate >= startDate.Value);
        //    if (endDate.HasValue)
        //    {
        //        var endDatePlusOne = endDate.Value.AddDays(1);
        //        ec = ec.Where(x => x.ConvDate < endDatePlusOne);
        //    }
        //    return ec;
        //}

        public static void DeleteEventConversions(DAContext db, IQueryable<EventConversion> eventConvs)
        {
            eventConvs.Delete();
            db.SaveChanges();
        }
    }
}
