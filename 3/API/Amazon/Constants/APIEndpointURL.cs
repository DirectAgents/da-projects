namespace Amazon.Constants
{
    /// <summary>
    /// Enums for regional URLs of API endpoints.
    /// <see cref="https://advertising.amazon.com/API/docs/v2/guides/supported_features#API-Endpoints"/>.
    /// </summary>
    public static class ApiEndpointUrl
    {
        /// <summary>
        /// Gets the API endpoint URL for North America (NA). Covers US and CA marketplaces.
        /// </summary>
        public const string NorthAmerica = "https://advertising-api.amazon.com";

        /// <summary>
        /// Gets the API endpoint URL for Europe (EU). Covers UK, FR, IT, ES, and DE marketplaces.
        /// </summary>
        public const string Europe = "https://advertising-api-eu.amazon.com";

        /// <summary>
        /// Gets the API endpoint URL for Far East (FE). Covers JP and AU marketplaces.
        /// </summary>
        public const string FarEast = "https://advertising-api-fe.amazon.com";
    }
}