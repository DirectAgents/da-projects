namespace BingAds.Models
{
    /// <summary>
    /// Model to keep a special information for client authentication through Bing API requests.
    /// </summary>
    internal class ClientCredentialsInfo
    {
        /// <summary>
        /// Gets or sets client serial number delegated by configuration.
        /// </summary>
        public int Alt { get; set; }

        /// <summary>
        /// Gets or sets customer ID.
        /// </summary>
        public long CustomerId { get; set; }

        /// <summary>
        /// Gets or sets developer token.
        /// </summary>
        public string DeveloperToken { get; set; }

        /// <summary>
        /// Gets or sets user name.
        /// </summary>
        public string UserName { get; set; }

        /// <summary>
        /// Gets or sets password.
        /// </summary>
        public string Password { get; set; }

        /// <summary>
        /// Gets or sets client ID.
        /// </summary>
        public string ClientId { get; set; }

        /// <summary>
        /// Gets or sets client secret.
        /// </summary>
        public string ClientSecret { get; set; }

        /// <summary>
        /// Gets or sets client refresh token.
        /// </summary>
        public string RefreshToken { get; set; }
    }
}
