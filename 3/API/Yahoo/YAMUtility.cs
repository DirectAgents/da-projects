using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Threading;
using Polly;
using RestSharp;
using RestSharp.Authenticators;
using RestSharp.Deserializers;
using Yahoo.Constants;
using Yahoo.Exceptions;
using Yahoo.Helpers;
using Yahoo.Models;
using Yahoo.Models.Requests;
using Yahoo.Models.Responses;

namespace Yahoo
{
    /// <summary>
    /// The Yahoo utility to extract data from the OATH API.
    /// </summary>
    public class YamUtility
    {
        private const string TokenDelimiter = "|YAMYAM|";
        private const string ReportResourceUrl = "extreport/";
        private const string AuthorizationHeader = "X-Auth-Token";

        private const int NumAlts = 10; // including the default (0)

        private const int RequestPerMinuteLimit = 5;
        private const int SecondIntervalForLimit = 63; // 1 min + 3 sec for errors

        private const int NumTriesRequestReportDefault = 12; // 240 sec (4 min)
        private const int NumTriesGetReportStatusDefault = 24; // 480 sec (8 min)
        private const int UnauthorizedAttemptsNumberDefault = 2;
        private const int WaitTimeSecondsDefault = 20;

        private static readonly object RequestLock = new object();
        private static readonly object AccessTokenLock = new object();

        private static readonly string[] AccessToken = new string[NumAlts];
        private static readonly string[] RefreshToken = new string[NumAlts];

        private static readonly ConcurrentQueue<DateTime> TimesOfRequestPerMinute = new ConcurrentQueue<DateTime>();
        
        private readonly string[] clientId = new string[NumAlts];
        private readonly string[] clientSecret = new string[NumAlts];
        private readonly string[] applicationAccessCode = new string[NumAlts];
        private readonly string[] altAccountIDs = new string[NumAlts];
        private readonly YamLogger logger;

        public event Action<FailedReportGenerationException> ProcessFailedReportGeneration;

        /// <summary>
        /// Gets or sets access and refresh tokens.
        /// Each string in the array is a combination of Access + Refresh Token.
        /// </summary>
        public static string[] TokenSets
        {
            get => CreateTokenSets().ToArray();
            set => SetTokens(value);
        }

        /// <summary>
        /// Number of an alt account number to use specific access values ​​for Api (for alternate credentials)
        /// </summary>
        public int WhichAlt { get; set; } // default: 0

        private string AuthBaseUrl { get; set; }
        private string YamBaseUrl { get; set; }
        private int NumTriesGetReportStatus { get; set; }
        private int NumTriesRequestReport { get; set; }
        private int WaitTimeSeconds { get; set; }

        static YamUtility()
        {
            for (var i = 0; i < RequestPerMinuteLimit; i++)
            {
                TimesOfRequestPerMinute.Enqueue(DateTime.MinValue);
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="YamUtility"/> class.
        /// </summary>
        public YamUtility()
        {
            if (logger == null)
            {
                logger = new YamLogger(null, null, null);
            }

            Setup();
        }

        /// <inheritdoc />
        /// <summary>
        /// Initializes a new instance of the <see cref="T:Yahoo.YamUtility" /> class.
        /// </summary>
        /// <param name="logInfo">Action that logs infos</param>
        /// <param name="logError">Action that logs errors</param>
        /// <param name="logWarning">Action that logs warnings</param>
        public YamUtility(Action<string> logInfo, Action<string> logWarning, Action<Exception> logError)
            : this()
        {
            logger = new YamLogger(logInfo, logError, logWarning);
        }

        /// <summary>
        /// The common method that generates a statistics report (for different dimensions).
        /// Returns the report URL that can be used to download the generated report.
        /// </summary>
        /// <param name="reportSettings">Report settings to customize the requested report.</param>
        /// <returns>URL of report location (may be null)</returns>
        public virtual string TryGenerateReport(ReportSettings reportSettings)
        {
            try
            {
                var payload = ReportParametersHelper.CreateReportRequestPayload(reportSettings);
                var reportUrl = TryGenerateReport(payload);
                return reportUrl;
            }
            catch (Exception exception)
            {
                var exc = new FailedReportGenerationException(reportSettings, exception);
                logger.LogError(exc);
                ProcessFailedReportGeneration?.Invoke(exc);
                return null;
            }
        }

        /// <summary>
        /// Set an alt account number to use specific access values ​​for Api (for alternative credentials).
        /// </summary>
        /// <param name="accountId">Account external id</param>
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

        #region Private constructor methods

        private void Setup()
        {
            clientId[0] = ConfigurationManager.AppSettings["YahooClientID"];
            clientSecret[0] = ConfigurationManager.AppSettings["YahooClientSecret"];
            applicationAccessCode[0] = ConfigurationManager.AppSettings["YahooApplicationAccessCode"]; // aka Auth Code
            for (var i = 1; i < NumAlts; i++)
            {
                SetupClient(i);
            }

            AuthBaseUrl = ConfigurationManager.AppSettings["YahooAuthBaseUrl"];
            YamBaseUrl = ConfigurationManager.AppSettings["YAMBaseUrl"];
            NumTriesGetReportStatus = GetDefaultOrConfigIntValue("YAM_NumTries_GetReportStatus", NumTriesGetReportStatusDefault);
            NumTriesRequestReport = GetDefaultOrConfigIntValue("YAM_NumTries_RequestReport", NumTriesRequestReportDefault);
            WaitTimeSeconds = GetDefaultOrConfigIntValue("YAM_WaitTime_Seconds", WaitTimeSecondsDefault);
        }

        private void SetupClient(int clientNumber)
        {
            altAccountIDs[clientNumber] = PlaceLeadingAndTrailingCommas(ConfigurationManager.AppSettings["Yahoo_Alt" + clientNumber]);
            clientId[clientNumber] = ConfigurationManager.AppSettings["YahooClientID_Alt" + clientNumber];
            clientSecret[clientNumber] = ConfigurationManager.AppSettings["YahooClientSecret_Alt" + clientNumber];
            applicationAccessCode[clientNumber] = ConfigurationManager.AppSettings["YahooApplicationAccessCode_Alt" + clientNumber]; // aka Auth Code
        }

        private string PlaceLeadingAndTrailingCommas(string idString)
        {
            if (string.IsNullOrEmpty(idString))
            {
                return idString;
            }

            return (idString[0] == ',' ? "" : ",") + idString + (idString[idString.Length - 1] == ',' ? "" : ",");
        }

        private int GetDefaultOrConfigIntValue(string configValueName, int defaultValue)
        {
            return int.TryParse(ConfigurationManager.AppSettings[configValueName], out var tmpInt)
                ? tmpInt
                : defaultValue;
        }

        #endregion

        #region Private token methods

        private static IEnumerable<string> CreateTokenSets()
        {
            for (var i = 0; i < NumAlts; i++)
            {
                lock (AccessTokenLock)
                {
                    yield return AccessToken[i] + TokenDelimiter + RefreshToken[i];
                }
            }
        }

        private static void SetTokens(string[] tokens)
        {
            lock (AccessTokenLock)
            {
                for (var i = 0; i < tokens.Length; i++)
                {
                    var tokenSet = tokens[i].Split(new[] {TokenDelimiter}, StringSplitOptions.None);
                    AccessToken[i] = tokenSet[0];
                    if (tokenSet.Length > 1)
                    {
                        RefreshToken[i] = tokenSet[1];
                    }
                }
            }
        }

        private void GetAccessToken()
        {
            var restClient = GetAccessTokenClient();
            var request = GetAccessTokenRequest();
            var response = restClient.ExecuteAsPost<CreateAccessTokenResponse>(request, "POST");

            if (response.Data?.access_token == null)
            {
                logger.LogError("Failed to get access token");
            }

            if (response.Data != null && response.Data.refresh_token == null)
            {
                logger.LogError("Failed to get refresh token");
            }

            if (response.Data == null)
            {
                return;
            }

            AccessToken[WhichAlt] = response.Data.access_token;
            RefreshToken[WhichAlt] = response.Data.refresh_token; // update this in case it changed
        }

        private IRestClient GetAccessTokenClient()
        {
            var restClient = new RestClient
            {
                BaseUrl = new Uri(AuthBaseUrl),
                Authenticator = new HttpBasicAuthenticator(clientId[WhichAlt], clientSecret[WhichAlt])
            };
            restClient.AddHandler("application/x-www-form-urlencoded", new JsonDeserializer());
            return restClient;
        }

        private IRestRequest GetAccessTokenRequest()
        {
            var request = new RestRequest();
            request.AddParameter("redirect_uri", "oob");
            if (string.IsNullOrWhiteSpace(RefreshToken[WhichAlt]))
            {
                request.AddParameter("grant_type", "authorization_code");
                request.AddParameter("code", applicationAccessCode[WhichAlt]);
            }
            else
            {
                request.AddParameter("grant_type", "refresh_token");
                request.AddParameter("refresh_token", RefreshToken[WhichAlt]);
            }

            return request;
        }

        #endregion

        private string TryGenerateReport(ReportPayload payload)
        {
            var reportUrl = Policy
                .Handle<Exception>()
                .Retry(NumTriesRequestReport, (exception, retryCount, context) => logger.LogWarning(
                    $"Could not get a report URL: {exception.Message}. Try regenerate a report (number of retrying - {retryCount})"))
                .Execute(() => GenerateReport(payload));
            return reportUrl;
        }

        /// <summary>
        /// The method returns the URL of the CSV, or null if there was a problem
        /// </summary>
        /// <param name="payload">Report payload</param>
        /// <returns>URL of report location (may be null)</returns>
        private string GenerateReport(ReportPayload payload)
        {
            var reportResponse = TryRequestReport(payload);
            var reportInfo = reportResponse?.Data;
            if (string.IsNullOrWhiteSpace(reportInfo?.CustomerReportId))
            {
                throw new Exception("Missing Report Id");
            }

            var reportUrl = TryGetReportUrl(reportInfo.CustomerReportId);
            return reportUrl;
        }

        /// <summary>
        /// The method requests a report and waits until a status of response will be "SUBMITTED"
        /// </summary>
        /// <param name="payload">Report settings</param>
        private IRestResponse<CreateReportResponse> TryRequestReport(ReportPayload payload)
        {
            var createReportResponse = Policy
                .Handle<Exception>()
                .OrResult<IRestResponse<CreateReportResponse>>(response => response.Data?.Status == null)
                .OrResult(response => !IsStatus(response.Data.Status, ReportStatus.Submitted))
                .WaitAndRetry(NumTriesRequestReport, GetPauseBetweenAttempts, (exception, timeSpan, retryCount, context) =>
                    logger.LogWaiting("Invalid createReportResponse. Will retry. Waiting {0} ...", timeSpan, retryCount, exception))
                .Execute(() => RequestReport(payload));
            return createReportResponse;
        }

        private IRestResponse<CreateReportResponse> RequestReport(ReportPayload payload)
        {
            var request = CreateRestRequest(ReportResourceUrl);
            request.AddJsonBody(payload);
            var response = ProcessRequest<CreateReportResponse>(request, true);
            return response;
        }

        /// <summary>
        /// The method gets a report status and waits until the report is ready.
        /// Returns the location (URL) of the CSV
        /// </summary>
        /// <param name="customerReportId">Customer report Id</param>
        /// <returns>Report URL (may be null)</returns>
        private string TryGetReportUrl(string customerReportId)
        {
            logger.LogInfo($"YAM Report ID: {customerReportId}");
            var getReportResponse = Policy
                .Handle<Exception>()
                .OrResult<IRestResponse<ReadReportStatusResponse>>(response => response.Data?.Status == null)
                .OrResult(response => !IsStatus(response.Data.Status, ReportStatus.Success) && !IsStatus(response.Data.Status, ReportStatus.Failed))
                .WaitAndRetry(NumTriesGetReportStatus, GetPauseBetweenAttempts, (exception, timeSpan, retryCount, context) =>
                    logger.LogWaiting("Will check if the report is ready. Waiting {0} ...", timeSpan, retryCount, exception))
                .Execute(() => GetReportStatus(customerReportId));

            if (getReportResponse.Data.Status.ToUpper() != ReportStatus.Success)
            {
                throw new Exception("Failed to obtain report URL");
            }
            return getReportResponse.Data.Url;
        }

        private IRestResponse<ReadReportStatusResponse> GetReportStatus(string reportId)
        {
            var request = CreateRestRequest(ReportResourceUrl + reportId);
            var response = ProcessRequest<ReadReportStatusResponse>(request);
            return response;
        }

        private IRestRequest CreateRestRequest(string resourceUri)
        {
            var request = new RestRequest(resourceUri);
            AddAuthorizationHeaders(request);
            return request;
        }

        private IRestResponse<T> ProcessRequest<T>(IRestRequest restRequest, bool isPostMethod = false)
            where T : new()
        {
            IRestResponse<T> response;
            var restClient = GetRequestRestClient();
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
            logger.LogWarning(message);
            return response;
        }

        private RestClient GetRequestRestClient()
        {
            var restClient = new RestClient(YamBaseUrl);
            restClient.AddHandler("application/json", new JsonDeserializer());
            return restClient;
        }

        private IRestResponse<T> ProcessRequest<T>(IRestClient restClient, IRestRequest restRequest, bool isPostMethod)
            where T : new()
        {
            if (string.IsNullOrEmpty(AccessToken[WhichAlt]))
            {
                GetAccessToken();
            }

            var response = Policy
                .Handle<Exception>()
                .OrResult<IRestResponse<T>>(resp => resp.StatusCode == HttpStatusCode.Unauthorized)
                .Retry(UnauthorizedAttemptsNumberDefault, (exception, retryCount, context) => UpdateAccessTokenForRestRequest(restRequest))
                .Execute(() => GetRestResponse<T>(restClient, restRequest, isPostMethod));

            return response;
        }

        private IRestResponse<T> GetRestResponse<T>(IRestClient restClient, IRestRequest restRequest, bool isPostMethod)
            where T : new()
        {
            WaitUntilLimitExpires();
            var response = isPostMethod
                ? restClient.ExecuteAsPost<T>(restRequest, "POST")
                : restClient.ExecuteAsGet<T>(restRequest, "GET");
            return response;
        }

        private void AddAuthorizationHeaders(IRestRequest request)
        {
            request.AddHeader("X-Auth-Method", "OAUTH");
            lock (AccessTokenLock)
            {
                request.AddHeader(AuthorizationHeader, AccessToken[WhichAlt]);
            }
        }

        private void UpdateAccessTokenForRestRequest(IRestRequest request)
        {
            // Get a new access token and use that.
            GetAccessToken();
            var param = request.Parameters.Find(p => p.Type == ParameterType.HttpHeader && p.Name == AuthorizationHeader);
            param.Value = AccessToken[WhichAlt];
        }

        private void WaitUntilLimitExpires()
        {
            DateTime requestTime;
            while (!TimesOfRequestPerMinute.TryDequeue(out requestTime)) { }
            var timeDifference = requestTime.AddSeconds(SecondIntervalForLimit) - DateTime.Now;
            if (timeDifference.TotalSeconds > 0)
            {
                var timeSpan = new TimeSpan(timeDifference.Hours, timeDifference.Minutes, timeDifference.Seconds);
                Thread.Sleep(timeSpan);
            }
            TimesOfRequestPerMinute.Enqueue(DateTime.Now);
        }

        private bool IsStatus(string status, string statusToCompare)
        {
            return string.Equals(status, statusToCompare, StringComparison.OrdinalIgnoreCase);
        }

        private TimeSpan GetPauseBetweenAttempts(int attemptNumber)
        {
            return new TimeSpan(0, 0, WaitTimeSeconds);
        }
    }
}
