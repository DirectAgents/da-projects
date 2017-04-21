﻿using System;
using System.Collections.Generic;
using System.Configuration;
using System.Threading;
using RestSharp;
using RestSharp.Authenticators;
using RestSharp.Deserializers;

namespace Yahoo
{
    public class YAMUtility
    {
        public const string TOKEN_DELIMITER = "|DELIMITER|";
        private const int WAITTIME_SECONDS = 6;
        private const int NUMTRIES_GETREPORTSTATUS = 15;

        private string AuthBaseUrl { get; set; }
        private string ClientID { get; set; }
        private string ClientSecret { get; set; }
        private string ApplicationAccessCode { get; set; }
        public string RefreshToken { get; set; }
        public string AccessToken { get; set; }
        private string YAMBaseUrl { get; set; }

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
            ClientID = ConfigurationManager.AppSettings["YahooClientID"];
            ClientSecret = ConfigurationManager.AppSettings["YahooClientSecret"];
            ApplicationAccessCode = ConfigurationManager.AppSettings["YahooApplicationAccessCode"]; // aka Auth Code

            AuthBaseUrl = ConfigurationManager.AppSettings["YahooAuthBaseUrl"];
            YAMBaseUrl = ConfigurationManager.AppSettings["YAMBaseUrl"];
        }

        // Use the refreshToken if we have one, otherwise use the auth code
        public void GetAccessToken()
        {
            var restClient = new RestClient
            {
                BaseUrl = new Uri(AuthBaseUrl),
                Authenticator = new HttpBasicAuthenticator(ClientID, ClientSecret)
            };
            restClient.AddHandler("application/json", new JsonDeserializer());

            var request = new RestRequest();
            request.AddParameter("redirect_uri", "oob");
            if (String.IsNullOrWhiteSpace(RefreshToken))
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
            RefreshToken = response.Data.refresh_token; // update this in case it changed
        }

        private IRestResponse<T> ProcessRequest<T>(RestRequest restRequest, bool postNotGet = false)
            where T : new()
        {
            var restClient = new RestClient
            {
                BaseUrl = new Uri(YAMBaseUrl)
            };
            restClient.AddHandler("application/json", new JsonDeserializer());

            if (String.IsNullOrEmpty(AccessToken))
                GetAccessToken();

            restRequest.AddHeader("X-Auth-Method", "OAUTH");
            restRequest.AddHeader("X-Auth-Token", AccessToken);

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

                if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized && tries < 2)
                { // Get a new access token and use that.
                    GetAccessToken();
                    var param = restRequest.Parameters.Find(p => p.Type == ParameterType.HttpHeader && p.Name == "X-Auth-Token");
                    param.Value = AccessToken;
                }
                else
                    done = true; //TODO: distinguish between success and failure of ProcessRequest
            }
            if (!String.IsNullOrWhiteSpace(response.ErrorMessage))
                LogError(response.ErrorMessage);

            return response;
        }

        // ---

        // (TODO) Checks if the CreateReportResponse is valid...
        public string ObtainReportUrl(CreateReportResponse createReportResponse)
        {
            //createReportResponse.status should be "SUBMITTED"...
            if (createReportResponse == null || String.IsNullOrWhiteSpace(createReportResponse.customerReportId))
            {
                LogError("Invalid createReportResponse");
                return null;
            }
            if (createReportResponse.status.ToUpper() != "SUBMITTED")
            {
                LogError(String.Format("createReportResponse.status is: {0}", createReportResponse.status));
                return null;
            }
            return WaitForReportUrl(createReportResponse.customerReportId);
        }
        // Keeps checking until the report is ready, then returns the location(url) of the CSV
        private string WaitForReportUrl(string customerReportId)
        {
            if (String.IsNullOrWhiteSpace(customerReportId))
                return null;

            LogInfo("YAM Report ID: " + customerReportId);

            GetReportResponse getReportResponse = null;
            var waitTime = new TimeSpan(0, 0, WAITTIME_SECONDS);
            for (int i = 0; i < NUMTRIES_GETREPORTSTATUS; i++)
            {
                LogInfo(String.Format("Will check if the report is ready in {0} seconds...", waitTime.Seconds));
                Thread.Sleep(waitTime);

                getReportResponse = GetReportStatus(customerReportId);
                if (getReportResponse != null && getReportResponse.status.ToUpper() == "SUCCESS")
                    break;

                LogInfo("The report is not yet ready for download.");
            }

            if (getReportResponse != null && getReportResponse.status.ToUpper() == "SUCCESS")
                return getReportResponse.url;

            LogInfo("Failed to obtain report URL");
            return null;
        }

        public GetReportResponse GetReportStatus(string reportId)
        {
            var request = new RestRequest("extreport/" + reportId);
            var response = ProcessRequest<GetReportResponse>(request);

            if (response == null)
                return null;
            return response.Data;
        }

        public CreateReportResponse RequestReport(DateTime startDate, DateTime endDate, int? accountId = null, bool byAdvertiser = false, bool byCampaign = false, bool byLine = false, bool byAd = false, bool byCreative = false)
        {
            var accountList = new List<int>();
            if (accountId.HasValue)
                accountList.Add(accountId.Value);

            var dimensionList = new List<int>();
            if (byAdvertiser)
                dimensionList.Add(Dimension.ADVERTISER);
            if (byCampaign)
                dimensionList.Add(Dimension.CAMPAIGN);
            if (byLine)
                dimensionList.Add(Dimension.LINE);
            if (byAd)
                dimensionList.Add(Dimension.AD);
            if (byCreative)
                dimensionList.Add(Dimension.CREATIVE);

            var metricList = new List<int>(new[] { Metric.IMPRESSIONS, Metric.CLICKS, Metric.ADVERTISER_SPENDING, Metric.CLICK_THROUGH_CONVERSIONS, Metric.VIEW_THROUGH_CONVERSIONS, Metric.ROAS_ACTION_VALUE });
            var reportOption = new ReportOption
            {
                timezone = Timezone.NEW_YORK,
                currency = Currency.USD,
                accountIds = accountList.ToArray(),
                dimensionTypeIds = dimensionList.ToArray(),
                metricTypeIds = metricList.ToArray()
            };
            var json = new
            {
                reportOption = reportOption,
                intervalTypeId = IntervalTypeId.DAY,
                dateTypeId = DateTypeId.CUSTOM_RANGE,
                startDate = startDate.ToString("s"),
                endDate = endDate.ToString("s")
            };
            var request = new RestRequest("extreport/");
            request.AddJsonBody(json);

            var response = ProcessRequest<CreateReportResponse>(request, postNotGet: true);

            if (response == null)
                return null;
            return response.Data;
        }
    }

}
