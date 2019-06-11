using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using BingAds.Models;

namespace BingAds.Helpers
{
    /// <summary>
    /// Provides methods for working with client credentials.
    /// </summary>
    internal static class ClientCredentialsProvider
    {
        private const int MaxNumAlts = 10;

        private static readonly Dictionary<int, string[]> AltAccountIDs = new Dictionary<int, string[]>(MaxNumAlts);
        private static readonly ClientCredentialsInfo DefaultCredentials;

        /// <summary>
        /// Gets or sets refresh tokens for accounts.
        /// </summary>
        public static string[] RefreshTokens { get; set; }

        static ClientCredentialsProvider()
        {
            BindAltsWithAccountIds();
            DefaultCredentials = new ClientCredentialsInfo { ClientSecret = string.Empty };
            SetCredentials(DefaultCredentials);
        }

        /// <summary>
        /// Returns client credentials for the corresponding client.
        /// </summary>
        /// <param name="clientId">Client ID.</param>
        /// <returns>Client credentials.</returns>
        public static ClientCredentialsInfo GetClientCredentials(long clientId)
        {
            var credentials = new ClientCredentialsInfo();
            SetCredentials(credentials, clientId.ToString());
            return credentials;
        }

        /// <summary>
        /// Update refresh token for account with a corresponding credentials.
        /// </summary>
        /// <param name="credentials">Client credentials.</param>
        /// <param name="token">Refresh token.</param>
        public static void UpdateRefreshToken(ClientCredentialsInfo credentials, string token)
        {
            RefreshTokens[credentials.Alt] = token;
        }

        private static void BindAltsWithAccountIds()
        {
            const char accountSeparatorInConfigVariable = ',';
            for (var i = 1; i < MaxNumAlts; i++)
            {
                var altIdsString = ConfigurationManager.AppSettings["BingAccountsAlt" + i];
                if (string.IsNullOrEmpty(altIdsString))
                {
                    continue;
                }
                var altIds = altIdsString.Split(accountSeparatorInConfigVariable);
                AltAccountIDs.Add(i, altIds);
            }
        }

        private static void SetCredentials(ClientCredentialsInfo credentials, string accountId = "")
        {
            credentials.Alt = GetAlt(accountId);
            credentials.RefreshToken = RefreshTokens[credentials.Alt];
            SetCredentialsFromConfiguration(credentials, accountId);
        }

        private static int GetAlt(string accountId)
        {
            return AltAccountIDs.FirstOrDefault(x => x.Value.Contains(accountId)).Key;
        }

        private static void SetCredentialsFromConfiguration(ClientCredentialsInfo credentials, string postfixInConfigName = "")
        {
            var customerId = ConfigurationManager.AppSettings["BingCustomerID" + postfixInConfigName];
            credentials.CustomerId = string.IsNullOrWhiteSpace(customerId)
                ? DefaultCredentials.CustomerId
                : Convert.ToInt64(customerId);

            var clientId = ConfigurationManager.AppSettings["BingClientId" + postfixInConfigName];
            credentials.ClientId = string.IsNullOrWhiteSpace(clientId)
                ? DefaultCredentials.ClientId
                : clientId;

            var clientSecret = ConfigurationManager.AppSettings["BingClientSecret" + postfixInConfigName];
            credentials.ClientSecret = string.IsNullOrWhiteSpace(clientSecret)
                ? DefaultCredentials.ClientSecret
                : clientSecret;

            var token = ConfigurationManager.AppSettings["BingApiToken" + postfixInConfigName];
            credentials.DeveloperToken = string.IsNullOrWhiteSpace(token)
                ? DefaultCredentials.DeveloperToken
                : token;

            var userName = ConfigurationManager.AppSettings["BingApiUsername" + postfixInConfigName];
            credentials.UserName = string.IsNullOrWhiteSpace(userName)
                ? DefaultCredentials.UserName
                : userName;

            var password = ConfigurationManager.AppSettings["BingApiPassword" + postfixInConfigName];
            credentials.Password = string.IsNullOrWhiteSpace(password)
                ? DefaultCredentials.Password
                : password;
        }
    }
}
