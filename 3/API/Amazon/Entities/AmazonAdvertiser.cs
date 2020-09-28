namespace Amazon.Entities
{
    /// <summary>
    /// Entity for Amazon Advertiser, gets from API
    /// <see cref="https://advertising.amazon.com/API/docs/en-us/amazon-attribution-prod-3p/#/Advertisers"/>.
    /// </summary>
    public class AmazonAdvertiser
    {
        /// <summary>
        /// Gets or sets the Advertiser name.
        /// </summary>
        public string AdvertiserName { get; set; }

        /// <summary>
        /// Gets or sets the Advertiser Id.
        /// </summary>
        public string AdvertiserId { get; set; }
    }
}