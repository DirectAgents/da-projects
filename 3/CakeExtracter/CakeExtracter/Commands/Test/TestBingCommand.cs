using System.ComponentModel.Composition;
using System.Threading;
using BingAds.Utilities;
using CakeExtracter.Common;
using DirectAgents.Domain.Entities.CPProg;
using WatiN.Core;

namespace CakeExtracter.Commands.Test
{
    /// <inheritdoc />
    /// <summary>
    /// The command to refresh API tokens of a Bing clint.
    /// </summary>
    [Export(typeof(ConsoleCommand))]
    public class TestBingCommand : ConsoleCommand
    {
        /// <summary>
        /// Gets or sets a command argument for alt number of a Bing client.
        /// </summary>
        public int AltNumber { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="TestBingCommand"/> class.
        /// </summary>
        public TestBingCommand()
        {
            NoNeedToCreateRepeatRequests = true;
            IsCommand("testBing", "The command for refreshing of token (Bing API)");
            HasOption<int>("a|alt|altNumber=", "Alt number for corresponding Bing client (default = 0)", c => AltNumber = c);
        }

        /// <inheritdoc />
        public override int Execute(string[] remainingArguments)
        {
            BingAuth.RefreshTokens = GetTokens();
            UpdateClientTokens();
            SaveTokens(BingAuth.RefreshTokens);
            return 0;
        }

        private void UpdateClientTokens()
        {
            var utility = new BingAuth(AltNumber);
            Logger.Info($"Bing API client ID: {utility.BingClientId}. Bing API customer ID: {utility.BingCustomerId} (alt = {AltNumber})");
            var consentUrl = utility.GetClientConsentUrl();
            Logger.Info($"Customer consent URL: {consentUrl}");
            var authCodeUrl = GetAuthCodeUrl(consentUrl);
            Logger.Info($"URL with authorization code: {authCodeUrl}");
            utility.UpdateClientRefreshToken(authCodeUrl);
            Logger.Info("Refresh token was updated.");
        }

        private string GetAuthCodeUrl(string consentUrl)
        {
            using (var browser = new IE(consentUrl))
            {
                Logger.Info("Please enter valid client credentials in the IE window that will open!");
                while (!browser.Url.Contains("code="))
                {
                    Logger.Info("Waiting for credentials...");
                    Thread.Sleep(1000);
                    browser.WaitForComplete();
                }

                return browser.Url;
            }
        }

        private string[] GetTokens()
        {
            return Platform.GetPlatformTokens(Platform.Code_Bing);
        }

        private void SaveTokens(string[] tokens)
        {
            Platform.SavePlatformTokens(Platform.Code_Bing, tokens);
        }
    }
}
