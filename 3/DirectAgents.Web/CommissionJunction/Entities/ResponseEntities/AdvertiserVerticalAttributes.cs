namespace CommissionJunction.Entities.ResponseEntities
{
    /// <summary>
    /// A record of advertiser vertical attributes that the advertiser may have supplied as part of a commissionable event.
    /// </summary>
    public class AdvertiserVerticalAttributes
    {
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
    }
}
