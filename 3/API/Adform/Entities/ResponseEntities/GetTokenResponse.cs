namespace Adform.Entities.ResponseEntities
{
    /// <summary>
    /// The result of API response for retrieving a new access token.
    /// </summary>
    internal class GetTokenResponse
    {
        /// <summary>
        /// Gets or sets the new access token from API.
        /// </summary>
        public string AccessToken { get; set; }

        /// <summary>
        /// Gets or sets the number of seconds the new access token will be valid.
        /// </summary>
        public int ExpiresIn { get; set; }

        /// <summary>
        /// Gets or sets the type of the new access token.
        /// </summary>
        public string TokenType { get; set; }
    }
}