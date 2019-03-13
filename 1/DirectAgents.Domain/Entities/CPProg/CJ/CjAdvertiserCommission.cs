using DirectAgents.Domain.Entities.CPProg;
using System.ComponentModel.DataAnnotations.Schema;

namespace DirectAgents.Domain.Entities.CPProg.CJ
{
    /// <summary>
    /// Db related entity for comission 
    /// </summary>
    public class CjAdvertiserCommission
    {
        /// <summary>
        /// Gets or sets the item unique db identifier.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets the account identifier. Foreign key to Accounts db table.
        /// </summary>
        public int? AccountId { get; set; }

        /// <summary>
        /// Account entity from Accounts table. Mapped by AccountId property.
        /// </summary>
        [ForeignKey("AccountId")]
        public virtual ExtAccount ExtAccount { get; set; }

        /// <summary>
        /// Status of the commission. Possible values: closed, extended, locked, new.
        /// </summary>
        public string ActionStatus { get; set; }

        /// <summary>
        /// Unique identifier of an advertiser’s defined action associated with the commission
        /// </summary>
        public string ActionTrackerId { get; set; }

        /// <summary>
        /// Name of the action (as specified by the advertiser) for that commission
        /// </summary>
        public string ActionTrackerName { get; set; }

        /// <summary>
        /// Action type for the commission. Possible values: bonus, click, imp, item_lead, item_sale, perf_inc, sim_lead, sim_sale.
        /// </summary>
        public string ActionType { get; set; }

        /// <summary>
        /// Advertiser commission amount in USD (Advertiser-only attribute)
        /// </summary>
        public decimal AdvCommissionAmountUsd { get; set; }

        /// <summary>
        /// CID of the advertiser for this commission
        /// </summary>
        public string AdvertiserId { get; set; }

        /// <summary>
        /// Name of the advertiser for this commission
        /// </summary>
        public string AdvertiserName { get; set; }

        /// <summary>
        /// Ad idenification number
        /// </summary>
        public string Aid { get; set; }

        /// <summary>
        /// CJ fee in USD (Advertiser only attribute)
        /// </summary>
        public decimal CjFeeUsd { get; set; }

        /// <summary>
        /// Click referring URL for the commission
        /// </summary>
        public string ClickReferringUrl { get; set; }

        /// <summary>
        /// Commission identification number
        /// </summary>
        public string CommissionId { get; set; }

        /// <summary>
        /// The associated browser that the transaction concluded in.
        /// </summary>
        public string ConcludingBrowser { get; set; }

        /// <summary>
        /// The associated device name that the transaction concluded in.
        /// </summary>
        public string ConcludingDeviceName { get; set; }

        /// <summary>
        /// The associated device type that the transaction concluded in.
        /// </summary>
        public string ConcludingDeviceType { get; set; }

        /// <summary>
        /// Country where the transaction occurred
        /// </summary>
        public string Country { get; set; }

        /// <summary>
        /// The coupon/voucher code used in the transaction
        /// </summary>
        public string Coupon { get; set; }

        /// <summary>
        /// Event date for the commission in UTC time zone. An ISO 8601 datetime, rendered in JSON as a string.
        /// </summary>
        public int EventDate { get; set; }

        /// <summary>
        /// The associated browser that the transaction initiated in.
        /// </summary>
        public string InitiatingBrowser { get; set; }

        /// <summary>
        /// The associated device name that the transaction initiated in.
        /// </summary>
        public string InitiatingDeviceName { get; set; }

        /// <summary>
        /// The associated device type that the transaction initiated in.
        /// </summary>
        public string InitiatingDeviceType { get; set; }

        /// <summary>
        /// Indicates if a transaction was attributed using cross-device tracking with ‘true’ or ‘false’
        /// </summary>
        public bool IsCrossDevice { get; set; }

        /// <summary>
        /// Advertiser defined, indicates if the customer is new or existing to the advertiser (Advertiser only attribute)
        /// </summary>
        public bool NewToFile { get; set; }

        /// <summary>
        /// Discount associated with the order in USD
        /// </summary>
        public decimal OrderDiscountUsd { get; set; }

        /// <summary>
        /// Advertiser-assigned identification number for the order
        /// </summary>
        public string OrderId { get; set; }

        /// <summary>
        /// Displays either a '1' indicating an original transaction or a '0' indicating a non-original or corrected transaction
        /// </summary>
        public bool Original { get; set; }

        /// <summary>
        /// Identification number used for correlation between the original transaction and corrected transaction
        /// </summary>
        public string OriginalActionId { get; set; }

        /// <summary>
        /// Date on which the commission is posted. An ISO 8601 datetime, rendered in JSON as a string.
        /// </summary>
        public int PostingDate { get; set; }

        /// <summary>
        /// CID of the publisher for this commission
        /// </summary>
        public string PublisherId { get; set; }

        /// <summary>
        /// Name of the publisher for this commission
        /// </summary>
        public string PublisherName { get; set; }

        /// <summary>
        /// Indicates if an advertiser has reviewed a transaction.
        /// </summary>
        public string ReviewedStatus { get; set; }

        /// <summary>
        /// Sale amount in USD
        /// </summary>
        public decimal SaleAmountUsd { get; set; }

        /// <summary>
        /// Displays coupon code used, indicates it was a site to store offer transaction
        /// </summary>
        public string SiteToStoreOffer { get; set; }

        /// <summary>
        /// Source for the transaction
        /// </summary>
        public string Source { get; set; }

        /// <summary>
        /// Publisher website identification number
        /// </summary>
        public string WebsiteId { get; set; }

        /// <summary>
        /// Publisher website name
        /// </summary>
        public string WebsiteName { get; set; }

        #region Vertical Attributes

        /// <summary>
        /// Marketing Campaign ID; advertiser-specific
        /// </summary>
        public string CampaignId { get; set; }

        /// <summary>
        /// Marketing Campaign name; advertiser-specific
        /// </summary>
        public string CampaignName { get; set; }

        /// <summary>
        /// Location city name, i.e. for a hotel or event
        /// </summary>
        public string City { get; set; }

        /// <summary>
        /// Country where the transaction occurred
        /// </summary>
        public string CountryCode { get; set; }

        /// <summary>
        /// Id for item/s purchased. If multiple items, it will be a comma-separated lists.
        /// </summary>
        public string ItemId { get; set; }

        /// <summary>
        /// Method of payment used, i.e. credit card, mastercard, paypal, wire transfer, etc.
        /// </summary>
        public string PaymentMethod { get; set; }

        /// <summary>
        /// Device platform customer is using (i.e. mobile, tablet, etc.)
        /// </summary>
        public string PlatformId { get; set; }

        /// <summary>
        /// Identifies the state/province code the store or location is in, per ISO 3166-2 country subdivision standards (e.g. Alaska would be "or_state=US-AK", Bangkok would be "or_state=TH-10")
        /// </summary>
        public string State { get; set; }

        /// <summary>
        /// Indicates if someone converted from a trial to a subscription
        /// </summary>
        public string Upsell { get; set; }

        #endregion Vertical Attributes
    }
}
