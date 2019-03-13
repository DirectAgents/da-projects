namespace CommissionJunction.Utilities
{
    internal class CjQueries
    {
        private const string AdvertiserCommissionsFiltersTemplate = "forAdvertisers: [\"{0}\"], sincePostingDate: \"{1}\", beforePostingDate: \"{2}\"";

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

        public static string GetAdvertiserCommissionsQuery(string advertiserId, string sinceDateTime, string beforeDateTime)
        {
            var filters = string.Format(AdvertiserCommissionsFiltersTemplate, advertiserId, sinceDateTime, beforeDateTime);
            return MergeQueryAndFilters(AdvertiserCommissionsQueryTemplate, filters);
        }

        private static string MergeQueryAndFilters(string query, string filters)
        {
            return string.Format(query, filters);
        }
    }
}
