namespace Yahoo.Models.Responses
{
    /// <summary>
    /// The class used to respond after a request to obtain a new access token.
    /// </summary>
    internal class CreateAccessTokenResponse
    {
        /// <summary>
        /// The access token signed by Yahoo. Use this token to access Oath Ad Platforms DSP API. This token has a 1-hour lifetime.
        /// </summary>
        public string access_token { get; set; }

        /// <summary>
        /// The refresh token that you can use to acquire a new access token after the current one expires.
        /// </summary>
        public string refresh_token { get; set; }
    }
}