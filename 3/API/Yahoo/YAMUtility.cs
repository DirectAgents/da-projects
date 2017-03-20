using System;
using System.Configuration;
using RestSharp;
using RestSharp.Authenticators;
using RestSharp.Deserializers;

namespace Yahoo
{
    public class YAMUtility
    {
        public string AuthBaseUrl { get; set; }
        public string ClientID { get; set; }
        public string ClientSecret { get; set; }
        public string ApplicationAccessCode { get; set; }
        public string RefreshToken { get; set; }
        public string AccessToken { get; set; }
        public string YAMBaseUrl { get; set; }

        // --- Logging ---
        private Action<string> _LogInfo;
        private Action<string> _LogError;

        private void LogInfo(string message)
        {
            if (_LogInfo == null)
                Console.WriteLine(message);
            else
                _LogInfo("[YAMUtility] " + message);
        }

        private void LogError(string message)
        {
            if (_LogError == null)
                Console.WriteLine(message);
            else
                _LogError("[YAMUtility] " + message);
        }

        // --- Constructors ---
        public YAMUtility()
        {
            Setup();
        }
        public YAMUtility(Action<string> logInfo, Action<string> logError)
            : this()
        {
            _LogInfo = logInfo;
            _LogError = logError;
        }
        private void Setup()
        {
            AuthBaseUrl = ConfigurationManager.AppSettings["YahooAuthBaseUrl"];
            ClientID = ConfigurationManager.AppSettings["YahooClientID"];
            ClientSecret = ConfigurationManager.AppSettings["YahooClientSecret"];
            ApplicationAccessCode = ConfigurationManager.AppSettings["YahooApplicationAccessCode"];

            //for testing
            AccessToken = ConfigurationManager.AppSettings["YahooAccessToken"];
            if (String.IsNullOrEmpty(AccessToken))
                GetAccessToken(firstTime: true);

            YAMBaseUrl = ConfigurationManager.AppSettings["YAMBaseUrl"];
        }
        private void GetAccessToken(bool firstTime)
        {
            var restClient = new RestClient
            {
                BaseUrl = new Uri(AuthBaseUrl),
                Authenticator = new HttpBasicAuthenticator(ClientID, ClientSecret)
            };
            restClient.AddHandler("application/json", new JsonDeserializer());

            var request = new RestRequest();
            request.AddParameter("redirect_uri", "oob");
            if (firstTime)
            {
                request.AddParameter("grant_type", "authorization_code");
                request.AddParameter("code", ApplicationAccessCode);
            }
            else
            {
                request.AddParameter("grant_type", "refresh_token");
                request.AddParameter("refresh_token", RefreshToken);
            }
            var response = restClient.ExecuteAsPost<GetTokenResponse>(request, "POST");
            AccessToken = response.Data.access_token;
            RefreshToken = response.Data.refresh_token;
        }

        public void CreateTestReport()
        {
            var restClient = new RestClient
            {
                BaseUrl = new Uri(YAMBaseUrl)
            };
            restClient.AddHandler("application/json", new JsonDeserializer());

            var request = new RestRequest("extreport/");
            request.AddHeader("X-Auth-Method", "OAUTH");
            request.AddHeader("X-Auth-Token", AccessToken);
            var reportOption = new ReportOption
            {
                //timezone = Timezone.NEW_YORK,
                //accountIds = new[] { 19685 },
                //dimensionTypeIds = new[] { Dimension.ADVERTISER },
                metricTypeIds = new[] { Metric.IMPRESSIONS, Metric.CLICKS, Metric.ADVERTISER_SPENDING, Metric.CLICK_THROUGH_CONVERSIONS, Metric.VIEW_THROUGH_CONVERSIONS } //, Metric.ROAS_ACTION_VALUE }
            };
            var json = new
            {
                reportOption = reportOption,
                //intervalTypeId = IntervalTypeId.DAY,
                IntervalTypeId = IntervalTypeId.CUMULATIVE,
                dateTypeId = DateTypeId.LAST_WEEK
            };
            request.AddJsonBody(json);

            var response = restClient.ExecuteAsPost<CreateReportResponse>(request, "POST");

        }
    }

    public class GetTokenResponse
    {
        public string access_token { get; set; }
        public string token_type { get; set; }
        public int expires_in { get; set; }
        public string refresh_token { get; set; }
        public string xoauth_yahoo_guid { get; set; }
    }

    public class CreateReportResponse
    {
        public string customerReportId { get; set; }
        public string status { get; set; }
    }

    public class ReportOption
    {
        //public string timezone { get; set; }
        //Currency
        //public int[] accountIds { get; set; }
        //public int[] dimensionTypeIds { get; set; }
        public int[] metricTypeIds { get; set; }
        // filterOptions, having, limitSpec
    }
}
