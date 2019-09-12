using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Threading;
using CakeExtracter.Common;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Util.Store;

namespace CakeExtracter.Commands.Test
{
    /// <inheritdoc />
    /// <summary>
    /// The command generates an OAuth2 refresh token.
    /// </summary>
    [Export(typeof(ConsoleCommand))]
    public class AdwordsAuth2AuthorizationCommand : ConsoleCommand
    {
        /// <summary>
        /// The AdWords API scope.
        /// </summary>
        private const string AdwordsApiScope = "https://www.googleapis.com/auth/adwords";

        /// <summary>
        /// The Ad Manager API scope.
        /// </summary>
        private const string AdManagerApiScope = "https://www.googleapis.com/auth/dfp";

        /// <summary>
        /// Initializes a new instance of the <see cref="AdwordsAuth2AuthorizationCommand"/> class.
        /// </summary>
        public AdwordsAuth2AuthorizationCommand()
        {
            NoNeedToCreateRepeatRequests = true;
            IsCommand("AdwordsAuth2AuthorizationCommand", "The command to fetch the access and refresh tokens");
        }

        /// <inheritdoc />
        public override int Execute(string[] remainingArguments)
        {
            OutputAdditionalInfo();
            var clientId = AcceptClientIdFromUser();
            var clientSecret = AcceptClientSecretFromUser();
            var scopes = AcceptScopes();
            var secrets = GetClientSecrets(clientId, clientSecret);
            try
            {
                var refreshToken = GetRefreshToken(secrets, scopes);
                OutputResultConfigPart(clientId, clientSecret, refreshToken);
                Console.WriteLine("Press <Enter> to continue...");
                Console.ReadLine();
            }
            catch (AggregateException)
            {
                Console.WriteLine("An error occured while authorizing the user.");
            }
            return 0;
        }

        /// <summary>
        /// Accepts the input with limited options.
        /// </summary>
        /// <param name="prompt">The user prompt.</param>
        /// <param name="options">The acceptable options.</param>
        /// <returns>The user response.</returns>
        /// <remarks>The options and user responses are converted to lower case.</remarks>
        private static string AcceptInputWithLimitedOptions(
            string prompt, IEnumerable<string> options)
        {
            var sanitizedOptions = new List<string>(options).Select(item => item.ToLower()).ToList();
            var allowedOptionsPrompt = string.Join(" / ", sanitizedOptions);
            var foundMatch = false;
            var response = "";
            while (!foundMatch)
            {
                Console.Write($@"{prompt} ({allowedOptionsPrompt}): ");
                response = Console.ReadLine().Trim().ToLower();
                if (sanitizedOptions.Contains(response))
                {
                    foundMatch = true;
                }
                else
                {
                    foundMatch = false;
                    Console.WriteLine($"Invalid input: please enter {allowedOptionsPrompt}.");
                }
            }
            return response;
        }

        private static IEnumerable<string> AcceptScopes()
        {
            var isAdWordsApiScope = ShouldApiScopesIncludeAdwordsApi();
            var isAdManagerApiScope = ShouldApiScopesIncludeAdManagerApi();
            var additionalScopes = AcceptAdditionalScopes();
            var scopes = SetScopes(isAdWordsApiScope, isAdManagerApiScope, additionalScopes);
            return scopes;
        }

        private static string GetRefreshToken(ClientSecrets secrets, IEnumerable<string> scopes)
        {
            // Authorize the user using installed application flow.
            var task = GoogleWebAuthorizationBroker.AuthorizeAsync(
                secrets, scopes, string.Empty, CancellationToken.None, new NullDataStore());
            task.Wait();
            var credential = task.Result;
            var refreshToken = credential.Token.RefreshToken;
            return refreshToken;
        }

        private static string AcceptClientIdFromUser()
        {
            Console.Write("Enter the client ID: ");
            return Console.ReadLine();
        }

        private static string AcceptClientSecretFromUser()
        {
            Console.Write("Enter the client secret: ");
            return Console.ReadLine();
        }

        private static ClientSecrets GetClientSecrets(string clientId, string clientSecret)
        {
            return new ClientSecrets
            {
                ClientId = clientId,
                ClientSecret = clientSecret,
            };
        }

        private static void OutputAdditionalInfo()
        {
            Console.WriteLine(@"This application generates an OAuth2 refresh token for use with
the Google Ads API .NET client library. To use this application
1) Follow the instructions on
https://developers.google.com/adwords/api/docs/guides/authentication#create_a_client_id_and_client_secret
to generate a new client ID and secret.
2) Enter the client ID and client Secret when prompted.
3) Once the output is generated, copy its contents into your App.config file.
");
            Console.WriteLine(@"Important note: The client ID you use with the example should be
of type 'Other'. If you are using a Web application client, you should add
'http://127.0.0.1/authorize' and 'http://localhost/authorize' to the list of
Authorized redirect URIs in your Google Developer Console to avoid getting a
redirect_uri_mismatch error.
");
        }

        private static void OutputResultConfigPart(string clientId, string clientSecret, string refreshToken)
        {
            Console.WriteLine($@"
Copy the following content into your App.config file.

<add key=""OAuth2Mode"" value=""APPLICATION"" />
<add key=""OAuth2ClientId"" value=""{clientId}"" />
<add key=""OAuth2ClientSecret"" value=""{clientSecret}"" />
<add key=""OAuth2RefreshToken"" value=""{refreshToken}"" />
");
        }

        private static bool ShouldApiScopesIncludeAdwordsApi()
        {
            var useAdWordsApiScope = AcceptInputWithLimitedOptions(
                "Authenticate for AdWords API?", new[] { "yes", "no" });
            return useAdWordsApiScope.ToLower().Trim() == "yes";
        }

        private static bool ShouldApiScopesIncludeAdManagerApi()
        {
            var useAdManagerApiScope = AcceptInputWithLimitedOptions(
                "Authenticate for Ad Manager API?", new[] { "yes", "no" });
            return useAdManagerApiScope.ToLower().Trim() == "yes";
        }

        private static IEnumerable<string> AcceptAdditionalScopes()
        {
            Console.Write("Enter additional OAuth2 scopes to authenticate for (space separated): ");
            var additionalScopesString = Console.ReadLine();
            return additionalScopesString.Split(' ')
                .Select(s => s.Trim())
                .Where(s => !string.IsNullOrEmpty(s));
        }

        private static IEnumerable<string> SetScopes(
            bool isAdWordsApiScope, bool isAdManagerApiScope, IEnumerable<string> additionalScopes)
        {
            var scopes = new List<string>();
            if (isAdWordsApiScope)
            {
                scopes.Add(AdwordsApiScope);
            }
            if (isAdManagerApiScope)
            {
                scopes.Add(AdManagerApiScope);
            }
            scopes.AddRange(additionalScopes);
            return scopes;
        }
    }
}