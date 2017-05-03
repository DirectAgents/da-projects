using System;
using System.Configuration;
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

            //TODO: tries, handle expired token
            IRestResponse<T> response = null;
            if (postNotGet)
                response = restClient.ExecuteAsPost<T>(restRequest, "POST");
            else
                response = restClient.ExecuteAsGet<T>(restRequest, "GET");

            if (!String.IsNullOrWhiteSpace(response.ErrorMessage))
                LogError(response.ErrorMessage);

            return response;
        }

        public ReportData GetReportData(DateTime startDate, DateTime endDate, int clientId)
        {
            var parms = CreateReportParams(startDate, endDate, clientId);
            var request = new RestRequest("/v1/reportingstats/agency/reportdata");
            request.AddJsonBody(parms);

            var restResponse = ProcessRequest<ReportResponse>(request, postNotGet: true);
            if (restResponse != null && restResponse.Data != null)
            {
                //ReportResponse reportResponse = restResponse.Data;
                return restResponse.Data.reportData;
            }
            return null;
        }

        // like a constructor...
        private ReportParams CreateReportParams(DateTime startDate, DateTime endDate, int clientId)
        {
            var filter = new ReportFilter
            {
                date = new Dates
                {
                    from = startDate.ToString("yyyy'-'M'-'d"),
                    to = endDate.ToString("yyyy'-'M'-'d")
                },
                client = new int[] { clientId }
            };
            var reportParams = new ReportParams
            {
                dimensions = new string[] { "date" },
                metrics = new string[] { "cost", "impressions", "clicks", "conversions" },
                filter = filter
            };
            return reportParams;
        }

        //---

        public void TestReport()
        {
            var parms = new ReportParams
            {
                dimensions = new string[] { "client" },
                metrics = new string[] { "impressions", "clicks" },
                filter = new ReportFilter { date = new Dates { from = "2017-4-1", to = "2017-4-25" } }
            };

            var request = new RestRequest("/v1/reportingstats/agency/reportdata");
            request.AddJsonBody(parms);

            var response = ProcessRequest<ReportResponse>(request, postNotGet: true);
        }
    }
}
