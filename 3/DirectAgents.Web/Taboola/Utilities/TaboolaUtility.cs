using System;
using System.Collections.Generic;
using System.Configuration;
using System.Net;
using Polly;
using RestSharp;
using RestSharp.Authenticators;
using RestSharp.Deserializers;
using Taboola.Entities.ResponseEntities;

namespace Taboola.Utilities
{
    /// <summary>
    /// Taboola utility to send API requests
    /// </summary>
    public class TaboolaUtility
    {
        private const int NumAlts = 1;
        private const string DateFormat = "yyyy-MM-ddTHH:mm:ssZ"; // The API expects ISO 8601 datetime
        private const string TaboolaAPIEndpointUrl = "https://backstage.taboola.com";
        private const string PrefixUrl = "/backstage/api/1.0";
        private const string TokenUrl = "/backstage/oauth/token";
        private const string AllowedAccountsUrl = "/users/current/allowed-accounts/";
        private const string UrlEncodedContentType = "application/x-www-form-urlencoded";
        private const string ClientCredentialsGrantType = "client_credentials";
        private const string HttpPostMethod = "POST";
        private const string HttpGetMethod = "GET";
        private const string AuthorizationHeader = "Authorization";
        private const string GrantTypeHeader = "grant_type";

        private static readonly object RequestLock = new object();

        private readonly string taboolaClientId = ConfigurationManager.AppSettings["TaboolaClientId"];
        private readonly string taboolaClientSecret = ConfigurationManager.AppSettings["TaboolaClientSecret"];
        private readonly int unauthorizedAttemptsNumber = int.Parse(ConfigurationManager.AppSettings["TaboolaUnauthorizedAttemptsNumber"]);

        private static readonly string[] accessTokens = new string[NumAlts];
        private readonly string[] altAccountIDs = new string[NumAlts];

        public int WhichAlt { get; set; } // default: 0

        public static string[] TokenSets
        {
            get => CreateTokenSets();
            set => SetTokens(value);
        }

        private static string[] CreateTokenSets()
        {
            var tokenSets = new string[NumAlts];
            for (var i = 0; i < NumAlts; i++)
            {
                tokenSets[i] = accessTokens[i];
            }
            return tokenSets;
        }

        private static void SetTokens(string[] tokens)
        {
            for (var i = 0; i < tokens.Length && i < NumAlts; i++)
            {
                accessTokens[i] = tokens[i];
            }
        }

        // for alternative credentials...
        public void SetWhichAlt(string accountId)
        {
            WhichAlt = 0; //default
            for (var i = 1; i < NumAlts; i++)
            {
                if (altAccountIDs[i] != null && altAccountIDs[i].Contains(',' + accountId + ','))
                {
                    WhichAlt = i;
                    break;
                }
            }
        }

        public TaboolaUtility()
        {
            
        }

        private IRestRequest CreateRestRequest(string resourceUri)
        {
            var request = new RestRequest(resourceUri);
            AddAuthorizationHeader(request);
            return request;
        }

        private IRestResponse<T> ProcessRequest<T>(IRestRequest restRequest, bool isPostMethod = false)
            where T : new()
        {
            IRestResponse<T> response;
            var restClient = new RestClient(TaboolaAPIEndpointUrl);
            lock (RequestLock)
            {
                response = ProcessRequest<T>(restClient, restRequest, isPostMethod);
            }

            if (response.IsSuccessful)
            {
                return response;
            }

            var message = string.IsNullOrWhiteSpace(response.ErrorMessage)
                ? response.Content
                : response.ErrorMessage;
            //LogError(message);
            return response;
        }

        private IRestResponse<T> ProcessRequest<T>(IRestClient restClient, IRestRequest restRequest, bool isPostMethod)
            where T : new()
        {
            if (string.IsNullOrEmpty(accessTokens[WhichAlt]))
            {
                GetAccessToken();
            }

            var response = Policy
                .Handle<Exception>()
                .OrResult<IRestResponse<T>>(resp => resp.StatusCode == HttpStatusCode.Unauthorized)
                .Retry(unauthorizedAttemptsNumber, (exception, retryCount, context) => UpdateAccessTokenForRestRequest(restRequest))
                .Execute(() => GetRestResponse<T>(restClient, restRequest, isPostMethod));

            return response;
        }

        private static IRestResponse<T> GetRestResponse<T>(IRestClient restClient, IRestRequest restRequest, bool isPostMethod)
            where T : new()
        {
            var response = isPostMethod
                ? restClient.ExecuteAsPost<T>(restRequest, HttpPostMethod)
                : restClient.ExecuteAsGet<T>(restRequest, HttpGetMethod);
            return response;
        }

        private void UpdateAccessTokenForRestRequest(IRestRequest request)
        {
            // Get a new access token and use that.
            GetAccessToken();
            AddAuthorizationHeader(request);
        }

        private void AddAuthorizationHeader(IRestRequest request)
        {
            var headerValue = GetAuthorizationHeaderValue();
            var param = request.Parameters.Find(p => p.Type == ParameterType.HttpHeader && p.Name == AuthorizationHeader);
            if (param != null)
            {
                param.Value = headerValue;
                return;
            }

            request.AddHeader(AuthorizationHeader, headerValue);
        }

        private string GetAuthorizationHeaderValue()
        {
            return $"bearer {accessTokens[WhichAlt]}";
        }

        private void GetAccessToken()
        {
            var client = GetAccessTokenClient();
            var request = GetAccessTokenRequest();

            var response = client.ExecuteAsPost<TaboolaTokenResponse>(request, HttpPostMethod);

            if (response.Data?.AccessToken == null)
            {
                //LogError("Failed to get access token");
            }
            accessTokens[WhichAlt] = response.Data?.AccessToken;
        }

        private IRestClient GetAccessTokenClient()
        {
            var restClient = new RestClient
            {
                BaseUrl = new Uri(TaboolaAPIEndpointUrl),
                Authenticator = new HttpBasicAuthenticator(taboolaClientId, taboolaClientSecret)
            };
            restClient.AddHandler(UrlEncodedContentType, new JsonDeserializer());
            return restClient;
        }

        private static IRestRequest GetAccessTokenRequest()
        {
            var restRequest = new RestRequest(TokenUrl);
            restRequest.AddParameter(GrantTypeHeader, ClientCredentialsGrantType);
            return restRequest;
        }
        
        #region HelpAPIMethods

        public void GetAccounts()
        {
            var accounts = GetUsers();
            // LOG or Create
        }

        private IEnumerable<TaboolaAccountResponse> GetUsers()
        {
            var resourcePath = $"{PrefixUrl}{AllowedAccountsUrl}";
            var request = CreateRestRequest(resourcePath);
            var response = ProcessRequest<TaboolaAccountsResponse>(request);
            return response.Data.Results;
        }

        #endregion
    }
}
