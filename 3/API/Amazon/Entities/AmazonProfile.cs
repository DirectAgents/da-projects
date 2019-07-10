namespace Amazon.Entities
{
    /// <summary>
    /// Entity for Amazon Profiles, gets from API
    /// <see cref="https://advertising.amazon.com/API/docs/v2/reference/profiles"/>.
    /// </summary>
    public class AmazonProfile
    {
        /// <summary>
        /// Gets or sets the ID of the profile.
        /// </summary>
        public string ProfileId { get; set; }

        /// <summary>
        /// Gets or sets the country code identifying the publisher(s) on which ads will run.
        /// </summary>
        public string CountryCode { get; set; }

        /// <summary>
        /// Gets or sets the currency used for all monetary values for entities under this profile.
        /// </summary>
        public string CurrencyCode { get; set; }

        /// <summary>
        /// Gets or sets the tz database time zone used for all date-based campaign management and reporting.
        /// </summary>
        public string Timezone { get; set; }

        /// <summary>
        /// Gets or sets the additional information about account.
        /// </summary>
        public AmazonAccountInfo AccountInfo { get; set; }
    }
}