using System;
using System.Linq;
using CakeExtracter.Common;
using CakeExtracter.Helpers;
using DirectAgents.Domain.Contexts;
using DirectAgents.Domain.Entities.Cake;

namespace CakeExtracter.Etl.CakeMarketing.Cleaners
{
    /// <summary>
    /// Cleaner of Cake Event Conversions for daSynchCakeEventConversions.
    /// </summary>
    public class DaEventConversionCleaner
    {
        private readonly int? advertiserId;
        private readonly int? offerId;
        private readonly DateTime? startDate;
        private readonly DateTime? endDate;

        /// <summary>
        /// Initializes a new instance of the <see cref="DaEventConversionCleaner"/> class.
        /// </summary>
        /// <param name="advertiserId">Identifier of advertiser for cleaning.</param>
        /// <param name="offerId">Identifier of offer for cleaning.</param>
        /// <param name="dateRange">Range of dates for cleaning.</param>
        public DaEventConversionCleaner(int? advertiserId, int? offerId, DateRange dateRange)
        {
            this.advertiserId = advertiserId;
            this.offerId = offerId;
            startDate = dateRange.FromDate;
            endDate = dateRange.ToDate;
        }

        /// <summary>
        /// Clears Cake Event Conversions for the specified parameters in the constructor.
        /// </summary>
        public void ClearEventConversions()
        {
            Logger.Info($"The cleaning of Cake Event Conversions has begun (since {startDate}, before {endDate})");
            SafeContextWrapper.TryMakeTransaction(
                (DAContext db) =>
            {
                var allEventConversions = db.EventConversions.AsQueryable();
                var filteredEventConversions = FilterEventConversions(allEventConversions);
                filteredEventConversions.DeleteFromQuery();
            }, "DeleteFromQuery");
            Logger.Info($"The cleaning of Cake Event Conversions is over (since {startDate}, before {endDate})");
        }

        private IQueryable<EventConversion> FilterEventConversions(IQueryable<EventConversion> allEventConversions)
        {
            var filteredEventConversions = allEventConversions;
            if (advertiserId.HasValue)
            {
                filteredEventConversions = GetFilterByAdvertiserId(filteredEventConversions);
            }
            if (offerId.HasValue)
            {
                filteredEventConversions = GetFilterByOfferId(filteredEventConversions);
            }
            if (startDate.HasValue)
            {
                filteredEventConversions = GetFilterByStartDate(filteredEventConversions);
            }
            if (endDate.HasValue)
            {
                filteredEventConversions = GetFilterByEndDate(filteredEventConversions);
            }
            return filteredEventConversions;
        }

        private IQueryable<EventConversion> GetFilterByAdvertiserId(IQueryable<EventConversion> eventConversions)
        {
            return eventConversions.Where(x => x.Offer.AdvertiserId == advertiserId.Value);
        }

        private IQueryable<EventConversion> GetFilterByOfferId(IQueryable<EventConversion> eventConversions)
        {
            return eventConversions.Where(x => x.OfferId == offerId.Value);
        }

        private IQueryable<EventConversion> GetFilterByStartDate(IQueryable<EventConversion> eventConversions)
        {
            return eventConversions.Where(x => x.ConvDate >= startDate.Value);
        }

        private IQueryable<EventConversion> GetFilterByEndDate(IQueryable<EventConversion> eventConversions)
        {
            var nextDateAfterEndDate = endDate.Value.AddDays(1);
            return eventConversions.Where(x => x.ConvDate < nextDateAfterEndDate);
        }
    }
}
