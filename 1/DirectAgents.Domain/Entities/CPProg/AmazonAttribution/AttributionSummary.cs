using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DirectAgents.Domain.Entities.CPProg.AmazonAttribution
{
    /// <summary>
    /// Amazon Attribution summary entity.
    /// Represents Amazon Attribution metrics.
    /// </summary>
    public class AttributionSummary
    {
        /// <summary>
        /// Gets or sets associated AccountId.
        /// </summary>
        [Key]
        [Column(Order = 0)]
        public int AccountId { get; set; }

        /// <summary>
        /// Gets or sets associated external account.
        /// </summary>
        [ForeignKey("AccountId")]
        public virtual ExtAccount ExtAccount { get; set; }

        /// <summary>
        /// Gets or sets an advertiser name.
        /// </summary>
        [Key]
        [Column(Order = 2)]
        public string AdvertiserName { get; set; }

        /// <summary>
        /// Gets or sets a campaign external identifier.
        /// </summary>
        [Key]
        [Column(Order = 3)]
        public string CampaignName { get; set; }

        /// <summary>
        /// Gets or sets an ad group name.
        /// </summary>
        [Key]
        [Column(Order = 4)]
        public string AdGroupName { get; set; }

        /// <summary>
        /// Gets or sets a creative external identifier.
        /// </summary>
        [Key]
        [Column(Order = 5)]
        public string CreativeName { get; set; }

        /// <summary>
        /// Gets or sets the associated date.
        /// </summary>
        [Key]
        [Column(Order = 1)]
        public DateTime Date { get; set; }

        /// <summary>
        /// Gets or sets total ad clicks.
        /// </summary>
        public int Clicks { get; set; }

        /// <summary>
        /// Gets or sets the number of promoted detail page view conversions attributed to ad click-throughs within 14 days.
        /// </summary>
        public int DetailedPageViewsClicks { get; set; }

        /// <summary>
        /// Gets or sets the number of promoted add to cart conversions attributed to ad click-throughs within 14 days.
        /// </summary>
        public int AddToCartClicks { get; set; }

        /// <summary>
        /// Gets or sets the number of promoted purchases attributed to ad click-throughs within 14 days.
        /// </summary>
        public int Purchases { get; set; }

        /// <summary>
        /// Gets or sets the number of promoted units sold attributed to ad click-throughs within 14 days.
        /// </summary>
        public int UnitsSold { get; set; }

        /// <summary>
        /// Gets or sets aggregate value of promoted attributed sales occurring within 14 days of an attribution event in local currency.
        /// </summary>
        public decimal Sales { get; set; }

        /// <summary>
        /// Gets or sets the number of total detail page view conversions attributed to ad click-throughs within 14 days, including brand halo.
        /// </summary>
        public int TotalDetailedPageViewsClicks { get; set; }

        /// <summary>
        /// Gets or sets the number of total add to cart conversions attributed to ad click-throughs within 14 days, including brand halo.
        /// </summary>
        public int TotalAddToCartClicks { get; set; }

        /// <summary>
        /// Gets or sets the number of total purchases attributed to ad click-throughs within 14 days, including brand halo.
        /// </summary>
        public int TotalPurchases { get; set; }

        /// <summary>
        /// Gets or sets the number of total units sold attributed to ad click-throughs within 14 days, including brand halo.
        /// </summary>
        public int TotalUnitsSold { get; set; }

        /// <summary>
        /// Gets or sets aggregate value of total attributed sales occurring within 14 days of an attribution event in local currency, including brand halo.
        /// </summary>
        public decimal TotalSales { get; set; }
    }
}