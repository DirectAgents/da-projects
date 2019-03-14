namespace CommissionJunction.Entities.QueryParams
{
    /// <inheritdoc />
    /// <summary>
    /// The model that contains properties for query filters.
    /// </summary>
    internal class AdvertiserCommissionQueryParams : BaseQueryParams
    {
        /// <summary>
        /// Advertiser's unique company id
        /// </summary>
        public string AdvertiserId { get; set; }

        /// <summary>
        /// A date that is equal or less than commission date, in ISO 8601 datetime, e.g. "1999-12-31T11:59:59Z"
        /// </summary>
        public string SinceDateTime { get; set; }

        /// <summary>
        /// A date that is greater than commission date, in ISO 8601 datetime, e.g. "1999-12-31T11:59:59Z"
        /// </summary>
        public string BeforeDateTime { get; set; }
    }
}
