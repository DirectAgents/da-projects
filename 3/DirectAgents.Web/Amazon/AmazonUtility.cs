using RestSharp;
using RestSharp.Authenticators;
using RestSharp.Deserializers;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Dynamic;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using Newtonsoft.Json;

namespace Amazon
{
    public class AmazonUtility
    {
        // From Config File
        private readonly string _amazonApiClientId = ConfigurationManager.AppSettings["AmazonClientId"];
        private readonly string _amazonClientSecret = ConfigurationManager.AppSettings["AmazonClientSecret"];
        private readonly string _amazonApiUsername = ConfigurationManager.AppSettings["AmazonAPIUsername"];
        private readonly string _amazonApiPassword = ConfigurationManager.AppSettings["AmazonAPIPassword"];        
        private readonly string _amazonApiEndpointUrl = ConfigurationManager.AppSettings["AmazonAPIEndpointUrl"];
        private readonly string _amazonAuthorizeUrl = ConfigurationManager.AppSettings["AmazonAuthorizeUrl"];
        private readonly string _amazonTokenUrl = ConfigurationManager.AppSettings["AmazonTokenUrl"];
        private readonly string _amazonClientUrl = ConfigurationManager.AppSettings["AmazonClientUrl"];
        private readonly string _amazonAccessCode = ConfigurationManager.AppSettings["AmazonAccessCode"];
        private readonly string _amazonRefreshToken = ConfigurationManager.AppSettings["AmazonRefreshToken"];

        private long CustomerID { get; set; }
        private string DeveloperToken { get; set; }
          
        private string UserName { get; set; }
        private string Password { get; set; }
        private string ClientId { get; set; }
        private string ClientSecret { get; set; }
        private string RefreshToken { get; set; }
        private string ApiEndpointUrl { get; set; }
        private string AuthorizeUrl { get; set; }
        private string TokenUrl { get; set; }
        private string ClientUrl { get; set; }
        public static string AccessToken { get; set; }
        public static string ApplicationAccessCode { get; set; }

        private AmazonAuth AmazonAuth = null;
        private AccessTokens AccessTokens = null;

        private void ResetCredentials()
        {
            UserName = _amazonApiUsername;
            Password = _amazonApiPassword;
            ClientId = _amazonApiClientId;
            ClientSecret = _amazonClientSecret;
            ApiEndpointUrl = _amazonApiEndpointUrl;
            AuthorizeUrl = _amazonAuthorizeUrl;
            TokenUrl = _amazonTokenUrl;
            ClientUrl = _amazonClientUrl;
            ApplicationAccessCode = _amazonAccessCode;
            RefreshToken = _amazonRefreshToken;
        }
        private void SetCredentials(long accountId)
        {
            ResetCredentials();

            string customerID = ConfigurationManager.AppSettings["BingCustomerID" + accountId];
            if (!String.IsNullOrWhiteSpace(customerID))
                CustomerID = Convert.ToInt64(customerID);
            string token = ConfigurationManager.AppSettings["BingApiToken" + accountId];
            if (!String.IsNullOrWhiteSpace(token))
                DeveloperToken = token;
            string username = ConfigurationManager.AppSettings["BingApiUsername" + accountId];
            if (!String.IsNullOrWhiteSpace(username))
                UserName = username;
            string password = ConfigurationManager.AppSettings["BingApiPassword" + accountId];
            if (!String.IsNullOrWhiteSpace(password))
                Password = password;

            string _clientId = ConfigurationManager.AppSettings["BingClientId" + accountId];
            if (!String.IsNullOrWhiteSpace(_clientId))
                ClientId = _clientId;
            string _clientSecret = ConfigurationManager.AppSettings["BingClientSecret" + accountId];
            if (!String.IsNullOrWhiteSpace(_clientSecret))
                ClientSecret = _clientSecret;
            string _refreshToken = ConfigurationManager.AppSettings["BingRefreshToken" + accountId];
            if (!String.IsNullOrWhiteSpace(_refreshToken))
                RefreshToken = _refreshToken;
        }
        //private AuthorizationData GetAuthorizationData()
        //{
        //    var authorizationData = new AuthorizationData
        //    {
        //        CustomerId = CustomerID,
        //        //AccountId: not needed?
        //        DeveloperToken = DeveloperToken
        //    };
        //    if (UserName.Contains('@')) // is an email address (Microsoft account); can't use PasswordAuthentication
        //    {
        //        string redirString = ConfigurationManager.AppSettings["BingRedirectionUri"];
        //        var authorization = new OAuthWebAuthCodeGrant(ClientId, ClientSecret, new Uri(redirString));
        //        var task = authorization.RequestAccessAndRefreshTokensAsync(RefreshToken);
        //        task.Wait();
        //        // TODO: see if refreshtoken changed; if so, save the new one

        //        authorizationData.Authentication = authorization;
        //    }
        //    else
        //    {   // old style: BingAds username
        //        authorizationData.Authentication = new PasswordAuthentication(UserName, Password);
        //    }
        //    return authorizationData;
        //}

        #region Logging
        // --- Logging ---
        private Action<string> _LogInfo;
        private Action<string> _LogError;

        private void LogInfo(string message)
        {
            if (_LogInfo == null)
                Console.WriteLine(message);
            else
                _LogInfo("[BingAds.Reports] " + message);
        }

        private void LogError(string message)
        {
            if (_LogError == null)
                Console.WriteLine(message);
            else
                _LogError("[BingAds.Reports] " + message);
        }
        #endregion

        #region Constructors
        // --- Constructors ---
        public AmazonUtility()
        {
            ResetCredentials();
            AmazonAuth = new AmazonAuth(UserName, Password, ClientId, ClientSecret, RefreshToken);

            AccessTokens = AmazonAuth.GetInitialTokens();
        }
        public AmazonUtility(Action<string> logInfo, Action<string> logError)
        {
            _LogInfo = logInfo;
            _LogError = logError;
            ResetCredentials();
            AmazonAuth = new AmazonAuth(UserName, Password, ClientId, ClientSecret, RefreshToken);
        }
        #endregion

        private IRestResponse<T> ProcessRequest<T>(RestRequest restRequest, bool postNotGet = false)
    where T : new()
        {
            var restClient = new RestClient
            {
                BaseUrl = new Uri(_amazonApiEndpointUrl)
            };
            //restClient.AddHandler("application/json", new JsonDeserializer());

            if (String.IsNullOrEmpty(AccessToken))
                GetAccessToken();

            restRequest.AddHeader("Authorization", "bearer " + AccessToken);
            //restRequest.AddHeader("Content-Type", "application/json");

            bool done = false;
            int tries = 0;
            IRestResponse<T> response = null;
            while (!done)
            {
                if (postNotGet)
                    response = restClient.ExecuteAsPost<T>(restRequest, "POST");
                else
                    response = restClient.ExecuteAsGet<T>(restRequest, "GET");

                //var jsonResponse = JsonConvert.DeserializeObject(response.Content);
                tries++;

                if (response.StatusCode == HttpStatusCode.Unauthorized && tries < 2)
                { // Get a new access token and use that.
                    GetAccessToken();
                    var param = restRequest.Parameters.Find(p => p.Type == ParameterType.HttpHeader && p.Name == "Authorization");
                    param.Value = "bearer " + AccessToken;
                }
                else if (response.StatusDescription != null && response.StatusDescription.Contains("API calls quota exceeded") && tries < 5)
                {
                    LogInfo("API calls quota exceeded. Waiting 55 seconds.");
                    Thread.Sleep(55000);
                }
                else
                    done = true; //TODO: distinguish between success and failure of ProcessRequest
            }
            if (!String.IsNullOrWhiteSpace(response.ErrorMessage))
                LogError(response.ErrorMessage);

            return response;
        }
        public void GetAccessToken()
        {
            //var restClient = new RestClient
            //{
            //    BaseUrl = new Uri(AuthorizeUrl),
            //    Authenticator = new HttpBasicAuthenticator(ClientId, ClientSecret)
            //};
            //restClient.AddHandler("application/json", new JsonDeserializer());

            //var request = new RestRequest();
            //request.AddParameter("grant_type", "client_credentials");
            //request.AddParameter("scope", "");

            //var response = restClient.ExecuteAsPost<GetTokenResponse>(request, "POST");
            //if (response.Data != null)
            //    AccessToken = response.Data.access_token;

            AccessTokens = AmazonAuth.GetInitialTokens();

            AccessToken = AccessTokens.AccessToken;
        }
        public void GetDimensions()
        {
            var request = new RestRequest("/v1/reportingstats/agency/metadata/dimensions");

            var parms = new
            {
                dimensions = (object)null
            };
            request.AddJsonBody(parms);
            var restResponse = ProcessRequest<object>(request, postNotGet: true);
        }
        public void SubmitReport(string reportId)
        {
            try
            {
                var request = new RestRequest("v1/reports/"+reportId);
                request.AddHeader("Amazon-Advertising-API-Scope", "2436984122296584");
                
                var restResponse = ProcessRequest<ReportResponse>(request, postNotGet: false);
            }
            catch (Exception x)
            {

                throw;
            }
        }
        public ReportData GetReportData(ReportParams2 reportParams, string reportType)
        {            
            var request = new RestRequest("v1/"+ reportType + "/report");
            request.AddHeader("Amazon-Advertising-API-Scope", "2436984122296584");
            request.AddJsonBody(reportParams);

            var restResponse = ProcessRequest<ReportResponse>(request, postNotGet: true);
            if (restResponse != null && restResponse.Data != null)
            {
                if (restResponse.Data.reportData == null)
                    return null;
                //ReportResponse reportResponse = restResponse.Data;
                return restResponse.Data.reportData;
            }
            return null;


            /////////////////////////////////////////////////////////////////////////
            //var request = new RestRequest("/campaigns");
            //var restResponse = ProcessRequest<ReportResponse>(request, postNotGet: true);


            //var client = new RestClient(_amazonApiEndpointUrl); //"https://advertising-api.amazon.com/v1";
            //client.AddHandler("application/json", new JsonDeserializer());
           // var request = new RestRequest("v1/profiles", Method.GET);

            //request.AddHeader("Content-Type", "application/json");//
            //request.AddHeader("Authorization", "bearer " + AccessTokens.AccessToken);
            
            //IRestResponse response = client.Execute(request);
            //var content = response.Content; // raw content as string

            //return null;
        }

        public ReportParams2 CreateReportParams2(string campaignType, string reportDate)
        {

            var reportParams = new ReportParams2
            {
                campaignType = campaignType,
                //segment = segment,
                reportDate = reportDate,
                metrics = "impressions,clicks"
            };
            return reportParams;
        }
        // like a constructor...
        public ReportParams CreateReportParams(DateTime startDate, DateTime endDate, Int64 clientId, bool basicMetrics = true, 
            bool convMetrics = false, bool byCampaign = false, bool byLineItem = false, bool byBanner = false, 
            bool byMedia = false, bool byAdInteractionType = false, bool RTBonly = false)
        {
            dynamic filter = new ExpandoObject();
            filter.date = new Dates
            {
                from = startDate.ToString("yyyy'-'M'-'d"),
                to = endDate.ToString("yyyy'-'M'-'d")
            };
            filter.client = new Int64[] { clientId };
            if (RTBonly)
                filter.media = new { name = new string[] { "Real Time Bidding" } };

            var dimensions = new List<string> { "date" };
            if (byCampaign)
                dimensions.Add("campaign");
            if (byLineItem)
                dimensions.Add("lineItem");
            if (byBanner)
                dimensions.Add("banner");
            if (byMedia)
                dimensions.Add("media");
            if (byAdInteractionType)
                dimensions.Add("adInteractionType"); // Click, Impression, etc.

            var metrics = new List<string>();
            if (basicMetrics)
                metrics.AddRange(new string[] { "cost", "impressions", "clicks" });
            if (convMetrics)
                metrics.AddRange(new string[] { "conversions", "sales" });

            var reportParams = new ReportParams
            {
                filter = filter,
                dimensions = dimensions.ToArray(),
                metrics = metrics.ToArray(),
                paging = new Paging
                {
                    offset = 0,
                    limit = 3000
                }
            };
            return reportParams;
        }


        public void GetProfiles()
        {
            try
            {                
                var client = new RestClient(_amazonApiEndpointUrl); //"https://advertising-api.amazon.com/";
                client.AddHandler("application/json", new JsonDeserializer());
                var request = new RestRequest("v1/profiles", Method.GET);
                request.AddHeader("Content-Type", "application/json");//
                request.AddHeader("Authorization", "bearer " + AccessTokens.AccessToken);
                IRestResponse response = client.Execute(request);
                var content = response.Content; // raw content as string

            }
            catch (Exception x)
            {

                throw;
            }
        }
    }

}
