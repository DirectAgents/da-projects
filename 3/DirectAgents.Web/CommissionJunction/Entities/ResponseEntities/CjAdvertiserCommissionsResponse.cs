namespace CommissionJunction.Entities.ResponseEntities
{
    /// <summary>
    /// This class is a wrapper for Advertiser Commission data returned by API requests.
    /// </summary>
    internal class CjAdvertiserCommissionsResponse
    {
        /// <summary>
        /// Returned Advertiser Commission data
        /// </summary>
        public CjQueryResponse<AdvertiserCommission> AdvertiserCommissions { get; set; }
    }
}
