using System;
using System.Collections.Generic;
using System.Configuration;
using System.Dynamic;
using System.Net;
using RestSharp;
using RestSharp.Authenticators;
using RestSharp.Deserializers;

namespace Adform
{
    public class AdformUtility
    {
        private const string Scope = "https://api.adform.com/scope/eapi";

        // From Config:
        private string AuthBaseUrl { get; set; }
        private string ClientID { get; set; }
        private string ClientSecret { get; set; }
        private string BaseUrl { get; set; }

        public string AccessToken { get; set; }

        // --- Logging ---
        private Action<string> _LogInfo;
        private Action<string> _LogError;

        private void LogInfo(string message)
        {
            if (_LogInfo == null)
                Console.WriteLine(message);
            else
                _LogInfo("[AdformUtility] " + message);
        }

        private void LogError(string message)
        {
            if (_LogError == null)
                Console.WriteLine(message);
            else
                _LogError("[AdformUtility] " + message);
        }

        // --- Constructors ---
        public AdformUtility()
        {
            Setup();
        }
        public AdformUtility(Action<string> logInfo, Action<string> logError)
            : this()
        {
            _LogInfo = logInfo;
            _LogError = logError;
        }
        private void Setup()
        {
            AuthBaseUrl = ConfigurationManager.AppSettings["AdformAuthBaseUrl"];
            ClientID = ConfigurationManager.AppSettings["AdformClientID"];
            ClientSecret = ConfigurationManager.AppSettings["AdformClientSecret"];
            BaseUrl = ConfigurationManager.AppSettings["AdformBaseUrl"];
        }

        public void GetAccessToken()
        {
            var restClient = new RestClient
            {
                BaseUrl = new Uri(AuthBaseUrl),
                Authenticator = new HttpBasicAuthenticator(ClientID, ClientSecret)
            };
            restClient.AddHandler("application/json", new JsonDeserializer());

            var request = new RestRequest();
            request.AddParameter("grant_type", "client_credentials");
            request.AddParameter("scope", Scope);

            var response = restClient.ExecuteAsPost<GetTokenResponse>(request, "POST");
            if (response.Data != null)
                AccessToken = response.Data.access_token;
        }

        private IRestResponse<T> ProcessRequest<T>(RestRequest restRequest, bool postNotGet = false)
            where T : new()
        {
            var restClient = new RestClient
            {
                BaseUrl = new Uri(BaseUrl)
            };
            restClient.AddHandler("application/json", new JsonDeserializer());

            if (String.IsNullOrEmpty(AccessToken))
                GetAccessToken();
            restRequest.AddHeader("Authorization", "Bearer " + AccessToken);

            bool done = false;
            int tries = 0;
            IRestResponse<T> response = null;
            while (!done)
            {
                if (postNotGet)
                    response = restClient.ExecuteAsPost<T>(restRequest, "POST");
                else
                    response = restClient.ExecuteAsGet<T>(restRequest, "GET");

                tries++;

                if (response.StatusCode == HttpStatusCode.Unauthorized && tries < 2)
                { // Get a new access token and use that.
                    GetAccessToken();
                    var param = restRequest.Parameters.Find(p => p.Type == ParameterType.HttpHeader && p.Name == "Authorization");
                    param.Value = "Bearer " + AccessToken;
                }
                else
                    done = true; //TODO: distinguish between success and failure of ProcessRequest
            }
            if (!String.IsNullOrWhiteSpace(response.ErrorMessage))
                LogError(response.ErrorMessage);

            return response;
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

        public ReportData GetReportData(ReportParams reportParams)
        {
            var request = new RestRequest("/v1/reportingstats/agency/reportdata");
            request.AddJsonBody(reportParams);

            var restResponse = ProcessRequest<ReportResponse>(request, postNotGet: true);
            if (restResponse != null && restResponse.Data != null)
            {
                //ReportResponse reportResponse = restResponse.Data;
                return restResponse.Data.reportData;
            }
            return null;
        }

        // like a constructor...
        public ReportParams CreateReportParams(DateTime startDate, DateTime endDate, int clientId, bool basicMetrics = true, bool convMetrics = false, bool byLineItem = false, bool byBanner = false, bool byMedia = false, bool byAdInteractionType = false, bool RTBonly = false)
        {
            dynamic filter = new ExpandoObject();
            filter.date = new Dates
            {
                from = startDate.ToString("yyyy'-'M'-'d"),
                to = endDate.ToString("yyyy'-'M'-'d")
            };
            filter.client = new int[] { clientId };
            if (RTBonly)
                filter.media = new { name = new string[] { "Real Time Bidding" }};

            var dimensions = new List<string> { "date" };
            if (byLineItem)
                dimensions.Add("lineItem");
            if (byBanner)
                dimensions.Add("banner");
            if (byMedia)
                dimensions.Add("media");
            if (byAdInteractionType)
                dimensions.Add("adInteractionType");

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
    }
}
