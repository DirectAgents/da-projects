using System;
using System.Collections.Generic;
using System.Linq;
using CommissionJunction.Entities.QueryParams;
using CommissionJunction.Enums;

namespace CommissionJunction.Utilities
{
    /// <summary>
    /// The class defines methods for receiving queries to execute API Commission Junction requests.
    /// </summary>
    internal class CjQueries
    {
        private const string ForAdvertisersFilterTemplate = "forAdvertisers: [\"{0}\"]"; //required filter for Advertiser Commissions queries
        private const string SincePostingDateFilterTemplate = "sincePostingDate: \"{0}\"";
        private const string BeforePostingDateFilterTemplate = "beforePostingDate: \"{0}\"";
        private const string SinceEventDateFilterTemplate = "sinceEventDate: \"{0}\"";
        private const string BeforeEventDateFilterTemplate = "beforeEventDate: \"{0}\"";
        private const string SinceLockingDateFilterTemplate = "sinceLockingDate: \"{0}\"";
        private const string BeforeLockingDateFilterTemplate = "beforeLockingDate: \"{0}\"";
        private const string SinceCommissionIdFilterTemplate = "sinceCommissionId: \"{0}\"";

        private const string FiltersDelimiter = ",";

        private const string AdvertiserCommissionsQueryTemplate = @"
        {{
          advertiserCommissions({0}) {{
            count
            limit
            maxCommissionId
            payloadComplete
            records {{
              actionStatus
              actionTrackerId
              actionTrackerName
              actionType
              advertiserId
              advertiserName
              aid
              concludingBrowser
              concludingDeviceName
              concludingDeviceType
              country
              coupon
              initiatingBrowser
              initiatingDeviceName
              initiatingDeviceType
              isCrossDevice
              newToFile
              orderDiscountUsd
              orderId
              original
              originalActionId
              pubCommissionAmountUsd
              publisherId
              publisherName
              saleAmountUsd
              source
              websiteId
              websiteName
              clickReferringURL
              eventDate
              commissionId
              advCommissionAmountUsd
              cjFeeUsd
              postingDate
              reviewedStatus
              siteToStoreOffer
              items {{
                sku
                itemListId
                perItemSaleAmountUsd
                quantity
                totalCommissionUsd
                discountUsd
              }}
              verticalAttributes {{
                state
                campaignName
                campaignId
                countryCode
                city
                itemId
                paymentMethod
                platformId
                upsell
              }}
            }}
          }}
        }}";

        /// <summary>
        /// Returns a prepared query with certain filters as a string.
        /// </summary>
        /// <param name="queryParams">A model that contains values ​​that should be in a returned query. <see cref="AdvertiserCommissionQueryParams"/></param>
        /// <param name="dateRangeType">Date range filter type <see cref="DateRangeType"/></param>
        /// <returns>Advertiser Commissions query</returns>
        public static string GetAdvertiserCommissionsQuery(AdvertiserCommissionQueryParams queryParams, DateRangeType dateRangeType)
        {
            var filters = GetFormattedFilters(
                new List<string> {ForAdvertisersFilterTemplate, GetSinceDateFilterTemplate(dateRangeType),GetBeforeDateFilterTemplate(dateRangeType)},
                new List<object> {queryParams.AdvertiserId, queryParams.SinceDateTime, queryParams.BeforeDateTime},
                queryParams.SinceCommissionId);
            var query = string.Format(AdvertiserCommissionsQueryTemplate, filters);
            return query;
        }

        private static string GetFormattedFilters(List<string> filterNames, List<object> filterValues, string sinceCommissionId = null)
        {
            if (!string.IsNullOrEmpty(sinceCommissionId))
            {
                filterNames.Add(SinceCommissionIdFilterTemplate);
                filterValues.Add(sinceCommissionId);
            }

            var filters = filterNames.Select((filterName, i) => string.Format(filterName, filterValues[i])).ToList();
            var filter = string.Join(FiltersDelimiter, filters);
            return filter;
        }

        private static string GetSinceDateFilterTemplate(DateRangeType dateRangeType)
        {
            switch (dateRangeType)
            {
                case DateRangeType.Posting:
                    return SincePostingDateFilterTemplate;
                case DateRangeType.Locking:
                    return SinceLockingDateFilterTemplate;
                case DateRangeType.Event:
                    return SinceEventDateFilterTemplate;
                default:
                    throw new ArgumentOutOfRangeException(nameof(dateRangeType), dateRangeType, null);
            }
        }

        private static string GetBeforeDateFilterTemplate(DateRangeType dateRangeType)
        {
            switch (dateRangeType)
            {
                case DateRangeType.Posting:
                    return BeforePostingDateFilterTemplate;
                case DateRangeType.Locking:
                    return BeforeLockingDateFilterTemplate;
                case DateRangeType.Event:
                    return BeforeEventDateFilterTemplate;
                default:
                    throw new ArgumentOutOfRangeException(nameof(dateRangeType), dateRangeType, null);
            }
        }
    }
}
