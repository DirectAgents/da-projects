using Newtonsoft.Json;

namespace Amazon.Entities.Summaries
{
    /// <summary>
    /// Amazon Attribution summary API entity.
    /// Represents Amazon Attribution metrics.
    /// </summary>
    public class AmazonAttributionSummary
    {
        /// <summary>
        /// Gets or sets an advertiser name.
        /// </summary>
        public string AdvertiserName { get; set; }

        /// <summary>
        /// Gets or sets summary publisher.
        /// </summary>
        public string Publisher { get; set; }

        /// <summary>
        /// Gets or sets a campaign external identifier.
        /// </summary>
        public string CampaignId { get; set; }

        /// <summary>
        /// Gets or sets a statistic date.
        /// </summary>
        public string Date { get; set; }

        /// <summary>
        /// Gets or sets an ad group external identifier.
        /// </summary>
        public string AdGroupId { get; set; }

        /// <summary>
        /// Gets or sets a creative external identifier.
        /// </summary>
        public string CreativeId { get; set; }

        /// <summary>
        /// Gets or sets total ad clicks.
        /// </summary>
        [JsonProperty("Click-throughs")]
        public int Clicks { get; set; }

        /// <summary>
        /// Gets or sets the number of promoted detail page view conversions attributed to ad click-throughs within 14 days.
        /// </summary>
        [JsonProperty("attributedDetailPageViewsClicks14d")]
        public int DetailedPageViewsClicks { get; set; }

        /// <summary>
        /// Gets or sets the number of promoted add to cart conversions attributed to ad click-throughs within 14 days.
        /// </summary>
        [JsonProperty("attributedAddToCartClicks14d")]
        public int AddToCartClicks { get; set; }

        /// <summary>
        /// Gets or sets the number of promoted purchases attributed to ad click-throughs within 14 days.
        /// </summary>
        [JsonProperty("attributedPurchases14d")]
        public int Purchases { get; set; }

        /// <summary>
        /// Gets or sets the number of promoted units sold attributed to ad click-throughs within 14 days.
        /// </summary>
        [JsonProperty("unitsSold14d")]
        public int UnitsSold { get; set; }

        /// <summary>
        /// Gets or sets aggregate value of promoted attributed sales occurring within 14 days of an attribution event in local currency.
        /// </summary>
        [JsonProperty("attributedSales14d")]
        public decimal Sales { get; set; }

        /// <summary>
        /// Gets or sets the number of total detail page view conversions attributed to ad click-throughs within 14 days, including brand halo.
        /// </summary>
        [JsonProperty("attributedTotalDetailPageViewsClicks14d")]
        public int TotalDetailedPageViewsClicks { get; set; }

        /// <summary>
        /// Gets or sets the number of total add to cart conversions attributed to ad click-throughs within 14 days, including brand halo.
        /// </summary>
        [JsonProperty("attributedTotalAddToCartClicks14d")]
        public int TotalAddToCartClicks { get; set; }

        /// <summary>
        /// Gets or sets the number of total purchases attributed to ad click-throughs within 14 days, including brand halo.
        /// </summary>
        [JsonProperty("attributedTotalPurchases14d")]
        public int TotalPurchases { get; set; }

        /// <summary>
        /// Gets or sets the number of total units sold attributed to ad click-throughs within 14 days, including brand halo.
        /// </summary>
        [JsonProperty("totalUnitsSold14d")]
        public int TotalUnitsSold { get; set; }

        /// <summary>
        /// Gets or sets aggregate value of total attributed sales occurring within 14 days of an attribution event in local currency, including brand halo.
        /// </summary>
        [JsonProperty("totalAttributedSales14d")]
        public decimal TotalSales { get; set; }
    }
}