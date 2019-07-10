namespace Amazon.Entities
{
    /// <summary>
    /// Sub entity for additional information of the Amazon Profile.
    /// </summary>
    public class AmazonAccountInfo
    {
        /// <summary>
        /// Gets or sets the string identifier for the marketplace associated with this profile. This is the same identifier used by MWS.
        /// </summary>
        public string MarketplaceStringId { get; set; }

        /// <summary>
        /// Gets or sets the string identifier for the ID associated with this account.
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// Gets or sets the string identifier for the account name.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the type of account being called.
        /// </summary>
        public string Type { get; set; }
    }
}