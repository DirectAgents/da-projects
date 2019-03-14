using System.Collections.Generic;
using System.Linq;

namespace CommissionJunction.Utilities
{
    internal class CjQueries
    {
        private const string ForAdvertisersFilterTemplate = "forAdvertisers: [\"{0}\"]";
        private const string SincePostingDateFilterTemplate = "sincePostingDate: \"{0}\"";
        private const string BeforePostingDateFilterTemplate = "beforePostingDate: \"{0}\"";
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

        public static string GetAdvertiserCommissionsQuery(string advertiserId, string sinceDateTime, string beforeDateTime, string sinceCommissionId = null)
        {
            var filters = GetFormattedFilters(
                new List<string> {ForAdvertisersFilterTemplate, SincePostingDateFilterTemplate, BeforePostingDateFilterTemplate},
                new List<object> {advertiserId, sinceDateTime, beforeDateTime}, 
                sinceCommissionId);
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
    }
}
