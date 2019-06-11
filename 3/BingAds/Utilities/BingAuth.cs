using System;
using BingAds.Helpers;
using BingAds.Models;
using Microsoft.BingAds;

namespace BingAds.Utilities
{
    /// <summary>
    /// Bing utility to update corrupted refresh tokens for a corresponding client.
    /// </summary>
    public class BingAuth
    {
        private readonly ClientCredentialsInfo credentials;

        /// <summary>
        /// Gets or sets refresh tokens for Bing accounts.
        /// </summary>
        public static string[] RefreshTokens
        {
            get => ClientCredentialsProvider.RefreshTokens;
            set => ClientCredentialsProvider.RefreshTokens = value;
        }

        /// <summary>
        /// Gets Bing customer ID.
        /// </summary>
        public long BingCustomerId => credentials.CustomerId;

        /// <summary>
        /// Gets Bing client ID.
        /// </summary>
        public string BingClientId => credentials.ClientId;

        /// <summary>
        /// Initializes a new instance of the <see cref="BingAuth"/> class.
        /// </summary>
        /// <param name="alt">Alt number for corresponding Bing client.</param>
        public BingAuth(int alt)
        {
            credentials = ClientCredentialsProvider.GetAltCredentials(alt);
        }

        /// <summary>
        /// Requests user consent by connecting to the Microsoft Account authorization endpoint.
        /// </summary>
        /// <returns>User consent URL.</returns>
        public string GetClientConsentUrl()
        {
            var oAuthWebAuthCodeGrant = GetOathCodeGrant(credentials);
            var endpoint = oAuthWebAuthCodeGrant.GetAuthorizationEndpoint();
            return endpoint.ToString();
        }

        /// <summary>
        /// Update client refresh token.
        /// </summary>
        /// <param name="authCodeUrl">User consent callback URL with authorization code.</param>
        public async void UpdateClientRefreshToken(string authCodeUrl)
        {
            var oAuthWebAuthCodeGrant = GetOathCodeGrant(credentials);
            await oAuthWebAuthCodeGrant.RequestAccessAndRefreshTokensAsync(authCodeUrl);
            ClientCredentialsProvider.UpdateRefreshToken(credentials, oAuthWebAuthCodeGrant.OAuthTokens.RefreshToken);
        }

        private static OAuthWebAuthCodeGrant GetOathCodeGrant(ClientCredentialsInfo credentials)
        {
            var redirectUrl = new Uri(ClientCredentialsProvider.RedirectUrl);
            var oAuthWebAuthCodeGrant = new OAuthWebAuthCodeGrant(credentials.ClientId, credentials.ClientSecret, redirectUrl);
            return oAuthWebAuthCodeGrant;
        }
    }
}
