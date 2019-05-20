namespace Adform.Entities.RequestEntities
{
    /// <summary>
    /// The result of API response for retrieving a new access token
    /// </summary>
    internal class GetTokenResponse
    {
        /// <summary>
        /// The new access token from API
        /// </summary>
        public string AccessToken { get; set; }

        /// <summary>
        /// The number of seconds the new access token will be valid
        /// </summary>
        public int ExpiresIn { get; set; }

        /// <summary>
        /// The type of the new access token
        /// </summary>
        public string TokenType { get; set; }
    }
}