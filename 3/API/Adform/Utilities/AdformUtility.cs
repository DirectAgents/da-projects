using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Threading;
using Adform.Entities;
using Adform.Entities.ReportEntities;
using Adform.Entities.ReportEntities.ReportParameters;
using Adform.Entities.ResponseEntities;
using Adform.Helpers;
using Polly;
using RestSharp;
using RestSharp.Authenticators;
using RestSharp.Deserializers;

namespace Adform.Utilities
{
    /// <summary>
    /// Adform utility to send API requests.
    /// </summary>
    public class AdformUtility
    {
        private const int NumAlts = 10; // including the default (0)

        private const string Scope = "https://api.adform.com/scope/buyer.stats";
        private const string AuthBaseUrl = "https://id.adform.com/sts/connect/token";
        private const string BaseUrl = "https://api.adform.com";
        private const string MetadataDimensionsPath = "/v1/reportingstats/agency/metadata/dimensions";
        private const string MetadataMetricsPath = "/v1/reportingstats/agency/metadata/metrics";
        private const string CreateDataJobPath = "/v1/buyer/stats/data";
        private const int MaxNumberOfMetrics = 10;
        private const int MaxNumberOfDimensions = 8;

        private const string JsonContentType = "application/json";
        private const string ClientCredentialsGrantType = "client_credentials";
        private const string AuthorizationHeader = "Authorization";
        private const string OperationLocationHeader = "Operation-Location";
        private const string ThrottleRetryAfterHeader = "Throttle-Retry-After";
        private const HttpStatusCode ExceededQuotaStatusCode = (HttpStatusCode)429;
        private const string HttpPostMethod = "POST";
        private const string HttpGetMethod = "GET";

        // From Config:
        private static readonly string[] AccessTokens = new string[NumAlts];
        private static readonly string[] AltAccountIDs = new string[NumAlts];
        private static readonly string[] ClientIDs = new string[NumAlts];
        private static readonly string[] ClientSecrets = new string[NumAlts];

        private static readonly object AccessTokenLock = new object();

        private static int maxPollingRetryAttempt;
        private static int unauthorizedAttemptsNumber;
        private static int maxAttemptsNumber;
        private static int quotaLimitAttemptsNumber;
        private static TimeSpan pauseBetweenPollingAttemptsSeconds;

        private readonly AdformLogger logger;

        /// <summary>
        /// Gets or sets array of access tokens.
        /// </summary>
        public static string[] TokenSets
        {
            get => CreateTokenSets().ToArray();
            set
            {
                lock (AccessTokenLock)
                {
                    SetTokens(value);
                }
            }
        }

        /// <summary>
        /// Gets or sets tracking identifier for report settings.
        /// </summary>
        public string TrackingId { get; set; }

        private int WhichAlt { get; set; } // default: 0

        static AdformUtility()
        {
            InitializeAccessTokens();
            InitializeVariablesFromConfig();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="AdformUtility"/> class.
        /// </summary>
        /// <param name="logInfo">Action that logs infos</param>
        /// <param name="logError">Action that logs errors</param>
        public AdformUtility(Action<string> logInfo, Action<string> logWarning, Action<Exception> logError)
        {
            logger = new AdformLogger(logInfo, logWarning, logError);
        }

        /// <summary>
        /// Set an alt account number to use specific access values ​​for Api (for alternate credentials)
        /// </summary>
        /// <param name="accountId">Account external id</param>
        public void SetWhichAlt(string accountId)
        {
            WhichAlt = 0; // default
            for (var i = 1; i < NumAlts; i++)
            {
                if (AltAccountIDs[i] != null && AltAccountIDs[i].Contains(',' + accountId + ','))
                {
                    WhichAlt = i;
                    break;
                }
            }
        }

        /// <summary>
        /// Returns all available dimensions to be used in Reporting Stats API.
        /// </summary>
        public void GetDimensions()
        {
            var request = CreateRestRequest(MetadataDimensionsPath);
            var parameters = new
            {
                dimensions = (object)null,
            };
            request.AddJsonBody(parameters);
            var restResponse = ProcessRequest<object>(request, isPostMethod: true);
        }

        /// <summary>
        /// Returns all available metrics to be used in Reporting Stats API.
        /// </summary>
        public void GetMetrics()
        {
            var request = CreateRestRequest(MetadataMetricsPath);
            var parameters = new
            {
                metrics = (object)null,
            };
            request.AddJsonBody(parameters);
            var restResponse = ProcessRequest<object>(request, isPostMethod: true);
        }

        /// <summary>
        /// Returns a report parameters by report settings.
        /// </summary>
        /// <param name="settings">Settings of report <see cref="ReportSettings"/>.</param>
        /// <returns>Parameters of report <see cref="ReportParams"/>.</returns>
        public ReportParams CreateReportParams(ReportSettings settings)
        {
            var reportParams = new ReportParams
            {
                Filter = AdformApiHelper.GetFilters(settings),
                Dimensions = AdformApiHelper.GetDimensions(settings),
                Metrics = AdformApiHelper.GetMetrics(settings),
                IncludeRowCount = true,
            };
            return reportParams;
        }

        /// <summary>
        /// Returns all report data with dimension and metrics limits.
        /// </summary>
        /// <param name="reportParams">Parameters of report.</param>
        /// <returns>List of report data <see cref="ReportData"/>.</returns>
        public IEnumerable<ReportData> GetReportDataWithLimits(ReportParams reportParams)
        {
            var allReportData = new List<ReportData>();
            for (var i = 0; i < reportParams.Metrics.Length; i += MaxNumberOfMetrics)
            {
                var reportWithCorrectMetricsParams = reportParams.Clone();
                reportWithCorrectMetricsParams.Metrics = GetItemsRange(reportParams.Metrics, i, MaxNumberOfMetrics);
                for (var j = 0; j < reportWithCorrectMetricsParams.Dimensions.Length; j += MaxNumberOfDimensions)
                {
                    var reportWithCorrectDimensionsParams = reportWithCorrectMetricsParams.Clone();
                    reportWithCorrectDimensionsParams.Dimensions = GetItemsRange(reportParams.Dimensions, j, MaxNumberOfDimensions);

                    var reportData = TryGetReportData(reportWithCorrectDimensionsParams);
                    allReportData.Add(reportData);
                }
            }

            return allReportData;
        }

        /// <summary>
        /// Tries to get the report data several times using the specified path.
        /// </summary>
        /// <param name="reportParameters">Parameters of report.</param>
        /// <returns>Report data <see cref="ReportData"/>.</returns>
        public ReportData TryGetReportData(ReportParams reportParameters)
        {
            logger.LogInfo($"Try to get a report: {reportParameters}");
            var dataLocationPath = ProcessDataReport(reportParameters);
            logger.LogInfo($"The report path: {BaseUrl}{dataLocationPath}");
            var reportData = TryToRetrieveReportData(dataLocationPath, reportParameters);
            logger.LogInfo("The report is received.");
            return reportData;
        }

        private static IEnumerable<string> CreateTokenSets()
        {
            for (var i = 0; i < NumAlts; i++)
            {
                yield return AccessTokens[i];
            }
        }

        private static void SetTokens(string[] tokens)
        {
            for (var i = 0; i < tokens.Length && i < NumAlts; i++)
            {
                AccessTokens[i] = tokens[i];
            }
        }

        private void GetAccessToken()
        {
            var restClient = GetAccessTokenClient();
            var request = GetAccessTokenRequest();
            var response = restClient.ExecuteAsPost<GetTokenResponse>(request, HttpPostMethod);
            if (response.Data != null)
            {
                AccessTokens[WhichAlt] = response.Data.AccessToken;
            }
        }

        private IRestClient GetAccessTokenClient()
        {
            var restClient = new RestClient
            {
                BaseUrl = new Uri(AuthBaseUrl),
                Authenticator = new HttpBasicAuthenticator(ClientIDs[WhichAlt], ClientSecrets[WhichAlt]),
            };
            restClient.AddHandler(JsonContentType, new JsonDeserializer());
            return restClient;
        }

        private static IRestRequest GetAccessTokenRequest()
        {
            var request = new RestRequest();
            request.AddParameter("grant_type", ClientCredentialsGrantType);
            request.AddParameter("scope", Scope);
            return request;
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
            return $"Bearer {AccessTokens[WhichAlt]}";
        }

        private static void InitializeVariablesFromConfig()
        {
            maxPollingRetryAttempt = int.Parse(ConfigurationManager.AppSettings["AdformMaxPollingRetryAttempt"]);
            unauthorizedAttemptsNumber = int.Parse(ConfigurationManager.AppSettings["AdformUnauthorizedAttemptsNumber"]);
            maxAttemptsNumber = int.Parse(ConfigurationManager.AppSettings["AdformMaxAttemptsNumber"]);
            quotaLimitAttemptsNumber = int.Parse(ConfigurationManager.AppSettings["AdformQuotaLimitAttemptsNumber"]);
            pauseBetweenPollingAttemptsSeconds = TimeSpan.FromSeconds(int.Parse(ConfigurationManager.AppSettings["AdformPauseBetweenPollingAttemptsSeconds"]));
        }

        private static void InitializeAccessTokens()
        {
            ClientIDs[0] = ConfigurationManager.AppSettings["AdformClientID"];
            ClientSecrets[0] = ConfigurationManager.AppSettings["AdformClientSecret"];
            for (var i = 1; i < NumAlts; i++)
            {
                AltAccountIDs[i] = PlaceLeadingAndTrailingCommas(ConfigurationManager.AppSettings["Adform_Alt" + i]);
                ClientIDs[i] = ConfigurationManager.AppSettings["AdformClientID_Alt" + i];
                ClientSecrets[i] = ConfigurationManager.AppSettings["AdformClientSecret_Alt" + i];
            }
        }

        private static string PlaceLeadingAndTrailingCommas(string idString)
        {
            if (string.IsNullOrEmpty(idString))
            {
                return idString;
            }

            return GetCommaOrEmptyString(idString.First()) + idString + GetCommaOrEmptyString(idString.Last());
        }

        private static string GetCommaOrEmptyString(char symbol)
        {
            return symbol == ',' ? string.Empty : ",";
        }

        private ReportData TryToRetrieveReportData(string dataLocationPath, ReportParams reportParameters)
        {
            var reportData = Policy
                .Handle<Exception>()
                .OrResult<ReportData>(x => x == null)
                .Retry(maxAttemptsNumber, (exception, retryCount, context) =>
                    logger.LogGenerationError("Try retrieving a report data", exception, retryCount))
                .Execute(() => GetReportData(dataLocationPath, reportParameters));
            return reportData;
        }

        /// <summary>
        /// Initiates a process of generation a report data.
        /// </summary>
        /// <param name="reportParams">Parameters of report.</param>
        /// <returns>Relative path to the data job result.</returns>
        private string ProcessDataReport(ReportParams reportParams)
        {
            try
            {
                var operationLocation = TryCreateDataJob(reportParams);
                var dataLocationPath = PollingOperation(operationLocation);
                return dataLocationPath;
            }
            catch (Exception e)
            {
                throw new Exception("Failed processing report data", e);
            }
        }

        private IRestResponse<T> ProcessRequest<T>(IRestRequest restRequest, bool isPostMethod = false)
            where T : new()
        {
            var restClient = new RestClient(BaseUrl);
            restClient.AddHandler(JsonContentType, new JsonDeserializer());
            var response = ProcessRequest<T>(restClient, restRequest, isPostMethod);

            if (response.IsSuccessful || response.StatusCode == HttpStatusCode.Accepted)
            {
                return response;
            }

            var message = string.IsNullOrWhiteSpace(response.Content)
                ? response.ErrorMessage
                : response.Content;
            logger.LogWarning(message);
            return response;
        }

        private IRestResponse<T> ProcessRequest<T>(IRestClient restClient, IRestRequest restRequest, bool isPostMethod)
            where T : new()
        {
            if (string.IsNullOrEmpty(AccessTokens[WhichAlt]))
            {
                GetAccessToken();
            }

            var response = Policy
                .Handle<Exception>()
                .OrResult<IRestResponse<T>>(resp => resp.StatusCode == HttpStatusCode.Unauthorized)
                .Retry(unauthorizedAttemptsNumber, (exception, retryCount, context) => UpdateAccessTokenForRestRequest(restRequest))
                .Execute(() => TryGetRestResponse<T>(restClient, restRequest, isPostMethod));

            return response;
        }

        private IRestRequest CreateRestRequest(string resourceUri)
        {
            var request = new RestRequest(resourceUri);
            AddAuthorizationHeader(request);
            request.OnBeforeDeserialization = resp => { resp.ContentType = JsonContentType; };
            return request;
        }

        private IRestResponse<T> TryGetRestResponse<T>(IRestClient restClient, IRestRequest restRequest, bool isPostMethod)
            where T : new()
        {
            var response = Policy
                .Handle<Exception>()
                .OrResult<IRestResponse<T>>(resp => resp.StatusCode == ExceededQuotaStatusCode)
                .Retry(quotaLimitAttemptsNumber, (exception, retryCount, context) => WaitingRecommendedTime(exception, retryCount))
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

        private void WaitingRecommendedTime<T>(DelegateResult<IRestResponse<T>> response, int retryCount)
        {
            var recommendedThrottleRetryAfter = GetRecommendedRetryAfter(response.Result);
            logger.LogInfo($"API calls quota exceeded. Waiting {recommendedThrottleRetryAfter} seconds.", retryCount);
            Thread.Sleep(TimeSpan.FromSeconds(recommendedThrottleRetryAfter));
        }

        private static int GetRecommendedRetryAfter(IRestResponse response)
        {
            var throttleRetryAfterHeaderValue = GetHeaderValue(response, ThrottleRetryAfterHeader);
            var throttleRecommendedRetryAfter = Convert.ToInt32(throttleRetryAfterHeaderValue);
            return throttleRecommendedRetryAfter;
        }

        private ReportData GetReportData(string dataLocationPath, ReportParams reportParameters)
        {
            var request = CreateRestRequest(dataLocationPath);
            var restResponse = ProcessRequest<ReportResponse>(request, isPostMethod: false);
            var reportData = restResponse?.Data?.reportData;
            if (reportData == null)
            {
                throw new Exception($"Failed generating the report ({restResponse?.Content}): {reportParameters}");
            }
            return reportData;
        }

        private static T[] GetItemsRange<T>(IEnumerable<T> items, int startIndex, int count)
        {
            return items.Skip(startIndex).Take(count).ToArray();
        }

        private string TryCreateDataJob(ReportParams reportParams)
        {
            var operationLocation = Policy
                .Handle<Exception>()
                .Retry(maxAttemptsNumber, (exception, retryCount, context) => 
                    logger.LogGenerationError("Try recreating a data job", exception, retryCount))
                .Execute(() => CreateDataJob(reportParams));
            return operationLocation;
        }

        private string CreateDataJob(ReportParams reportParams)
        {
            var request = CreateRestRequest(CreateDataJobPath);
            request.AddJsonBody(reportParams);
            var restResponse = ProcessRequest<object>(request, isPostMethod: true);

            if (restResponse.StatusCode != HttpStatusCode.Accepted)
            {
                throw new Exception("Failed creating data job.");
            }

            return GetOperationLocationPath(restResponse);
        }

        private static string GetOperationLocationPath(IRestResponse response)
        {
            var operationLocationHeaderValue = GetHeaderValue(response, OperationLocationHeader);
            var operationLocation = (string) operationLocationHeaderValue;
            return operationLocation;
        }

        private static object GetHeaderValue(IRestResponse response, string headerValue)
        {
            return response.Headers.FirstOrDefault(header => header.Name == headerValue)?.Value;
        }

        private string PollingOperation(string operationLocationPath)
        {
            var response = Policy
                .Handle<Exception>()
                .OrResult<IRestResponse<PollingOperationResponse>>(resp =>
                    !IsResponseWithStatus(resp, AdformResponseStatus.Succeeded) &&
                    !IsResponseWithStatus(resp, AdformResponseStatus.Failed))
                .WaitAndRetry(
                    maxPollingRetryAttempt,
                    i => pauseBetweenPollingAttemptsSeconds,
                    (exception, timeSpan, retryCount, context) =>
                        logger.LogWaiting($"Operation is not ready. Will repeat polling operation status. Waiting {timeSpan}", retryCount))
                .Execute(() =>
                {
                    var request = CreateRestRequest(operationLocationPath);
                    return ProcessRequest<PollingOperationResponse>(request, isPostMethod: false);
                });

            if (!IsResponseWithStatus(response, AdformResponseStatus.Succeeded))
            {
                throw new Exception("Failed polling operation.");
            }

            return response.Data.Location;
        }

        private static bool IsResponseWithStatus<T>(IRestResponse<T> response, string status)
            where T : PollingOperationResponse
        {
            return string.Equals(response.Data.Status, status, StringComparison.InvariantCultureIgnoreCase);
        }
    }
}
