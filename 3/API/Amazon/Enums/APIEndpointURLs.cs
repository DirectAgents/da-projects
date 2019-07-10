namespace Amazon.Enums
{
    /// <summary>
    /// Enums for regional URLs of API endpoints.
    /// <see cref="https://advertising.amazon.com/API/docs/v2/guides/supported_features#API-Endpoints"/>.
    /// </summary>
    public class APIEndpointURLs
    {
        private APIEndpointURLs(string value)
        {
            Value = value;
        }

        /// <summary>
        /// Gets or sets the current value for the API endpoint.
        /// </summary>
        public string Value { get; set; }

        /// <summary>
        /// Gets the API endpoint URL for North America (NA). Covers US and CA marketplaces.
        /// </summary>
        public static APIEndpointURLs NorthAmerica => new APIEndpointURLs("https://advertising-api.amazon.com");

        /// <summary>
        /// Gets the API endpoint URL for Europe (EU). Covers UK, FR, IT, ES, and DE marketplaces.
        /// </summary>
        public static APIEndpointURLs Europe => new APIEndpointURLs("https://advertising-api-eu.amazon.com");

        /// <summary>
        /// Gets the API endpoint URL for Far East (FE). Covers JP and AU marketplaces.
        /// </summary>
        public static APIEndpointURLs FarEast => new APIEndpointURLs("https://advertising-api-fe.amazon.com");
    }
}