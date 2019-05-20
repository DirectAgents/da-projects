using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Threading;
using Adform.Entities;
using Adform.Entities.ReportEntities;
using Adform.Entities.RequestEntities;
using Adform.Helpers;
using Polly;
using RestSharp;
using RestSharp.Authenticators;
using RestSharp.Deserializers;

namespace Adform.Utilities
{
    public class AdformUtility
    {
        public string TrackingId { get; set; }

        private static readonly object AccessTokenLock = new object();

        private const int NumAlts = 10; // including the default (0)

        private const string Scope = "https://api.adform.com/scope/buyer.stats";
        private const string MetadataDimensionsPath = "/v1/reportingstats/agency/metadata/dimensions";
        private const string MetadataMetricsPath = "/v1/reportingstats/agency/metadata/metrics";
        private const string CreateDataJobPath = "/v1/buyer/stats/data";
        private const string AllOperationsPath = "/v1/buyer/stats/operations";

        private const string JsonContentType = "application/json";
        private const string ClientCredentialsGrantType = "client_credentials";
        private const string AuthorizationHeader = "Authorization";
        private const string OperationLocationHeader = "Operation-Location";
        private const string ThrottleRetryAfterHeader = "Throttle-Retry-After";
        private const HttpStatusCode ExceededQuotaStatusCode = (HttpStatusCode) 429;
        private const string HttpPostMethod = "POST";
        private const string HttpGetMethod = "GET";
        private const string SucceededResponseStatus = "SUCCEEDED";
        private const string FailedResponseStatus = "FAILED";

        // From Config:
        private static readonly string[] AccessTokens = new string[NumAlts];
        private static readonly string[] AltAccountIDs = new string[NumAlts];
        private static readonly string[] ClientIDs = new string[NumAlts];
        private static readonly string[] ClientSecrets = new string[NumAlts];

        private static string adformAuthBaseUrl;
        private static string adformBaseUrl;
        private static int maxPageSize;
        private static int maxNumberOfMetrics;
        private static int maxNumberOfDimensions;
        private static int maxPollingRetryAttempt;
        private static int unauthorizedAttemptsNumber;
        private static int maxAttemptsNumber;
        private static int quotaLimitAttemptsNumber;
        private static TimeSpan pauseBetweenPollingAttemptsSeconds;

        private readonly AdformLogger logger;

        private int WhichAlt { get; set; } // default: 0
        
        #region Constructors

        static AdformUtility()
        {
            InitializeAccessTokens();
            InitializeVariablesFromConfig();
        }

        public AdformUtility(Action<string> logInfo, Action<Exception> logError)
        {
            logger = new AdformLogger(logInfo, logError);
        }

        private static void InitializeVariablesFromConfig()
        {
            adformAuthBaseUrl = ConfigurationManager.AppSettings["AdformAuthBaseUrl"];
            adformBaseUrl = ConfigurationManager.AppSettings["AdformBaseUrl"];
            maxPageSize = int.Parse(ConfigurationManager.AppSettings["AdformMaxPageSize"]);
            maxNumberOfMetrics = int.Parse(ConfigurationManager.AppSettings["AdformMaxNumberOfMetrics"]);
            maxNumberOfDimensions = int.Parse(ConfigurationManager.AppSettings["AdformMaxNumberOfDimensions"]);
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

        #endregion

        #region Tokens

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

        // for alternative credentials...
        public void SetWhichAlt(string accountId)
        {
            WhichAlt = 0; //default
            for (var i = 1; i < NumAlts; i++)
            {
                if (AltAccountIDs[i] != null && AltAccountIDs[i].Contains(',' + accountId + ','))
                {
                    WhichAlt = i;
                    break;
                }
            }
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
                BaseUrl = new Uri(adformAuthBaseUrl),
                Authenticator = new HttpBasicAuthenticator(ClientIDs[WhichAlt], ClientSecrets[WhichAlt])
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

        #endregion

        #region AdditionalMethods

        /// <summary>
        /// Returns all available dimensions to be used in Reporting Stats API.
        /// </summary>
        public void GetDimensions()
        {
            var request = CreateRestRequest(MetadataDimensionsPath);
            var parameters = new
            {
                dimensions = (object)null
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
                metrics = (object)null
            };
            request.AddJsonBody(parameters);
            var restResponse = ProcessRequest<object>(request, isPostMethod: true);
        }

        public void GetAllOperations()
        {
            var request = CreateRestRequest(AllOperationsPath);
            var response = ProcessRequest<List<PollingOperationResponse>>(request, false);
        }

        #endregion

        public IEnumerable<ReportData> GetReportDataWithPaging(ReportParams reportParams)
        {
            var allReportData = new List<ReportData>();
            for (var i = 0; i < reportParams.metrics.Length; i += maxNumberOfMetrics)
            {
                var reportWithCorrectMetricsParams = reportParams.Clone();
                reportWithCorrectMetricsParams.metrics = GetItemsRange(reportParams.metrics, i, maxNumberOfMetrics);
                for (var j = 0; j < reportWithCorrectMetricsParams.dimensions.Length; j += maxNumberOfDimensions)
                {
                    var reportWithCorrectDimensionsParams = reportWithCorrectMetricsParams.Clone();
                    reportWithCorrectDimensionsParams.dimensions = GetItemsRange(reportParams.dimensions, j, maxNumberOfDimensions);

                    var dataLocationPath = ProcessDataReport(reportWithCorrectDimensionsParams);
                    var reportData = TryGetReportData(dataLocationPath);
                    allReportData.Add(reportData);
                }
            }
            return allReportData;
        }

        public ReportData TryGetReportData(string dataLocationPath)
        {
            var reportData = Policy
                .Handle<Exception>()
                .Retry(maxAttemptsNumber, (exception, retryCount, context) =>
                    logger.LogGenerationError("Try retrieving a report data", exception, retryCount))
                .Execute(() => GetReportData(dataLocationPath));
            return reportData;
        }

        public ReportParams CreateReportParams(ReportSettings settings)
        {
            var reportParams = new ReportParams
            {
                filter = AdformApiHelper.GetFilters(settings),
                dimensions = AdformApiHelper.GetDimensions(settings),
                metrics = AdformApiHelper.GetMetrics(settings),
                paging = new Paging
                {
                    limit = maxPageSize
                },
                includeRowCount = true
            };
            return reportParams;
        }

        public string ProcessDataReport(ReportParams reportParams)
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
            var restClient = new RestClient(adformBaseUrl);
            restClient.AddHandler(JsonContentType, new JsonDeserializer());
            var response = ProcessRequest<T>(restClient, restRequest, isPostMethod);

            if (response.IsSuccessful || response.StatusCode == HttpStatusCode.Accepted)
            {
                return response;
            }

            var message = string.IsNullOrWhiteSpace(response.Content)
                ? response.ErrorMessage
                : response.Content;
            logger.LogError(new Exception(message));
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
            var throttleRecommendedRetryAfter = (int) throttleRetryAfterHeaderValue;
            return throttleRecommendedRetryAfter;
        }

        private ReportData GetReportData(string dataLocationPath)
        {
            var request = CreateRestRequest(dataLocationPath);
            var restResponse = ProcessRequest<ReportResponse>(request, isPostMethod: false);
            return restResponse?.Data?.reportData;
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
                    logger.LogGenerationError("Try recreating a Data job", exception, retryCount))
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
                    !IsResponseWithStatus(resp, SucceededResponseStatus) &&
                    !IsResponseWithStatus(resp, FailedResponseStatus))
                .WaitAndRetry(maxPollingRetryAttempt, i => pauseBetweenPollingAttemptsSeconds,
                    (exception, timeSpan, retryCount, context) =>
                        logger.LogWaiting($"Operation is not ready. Will repeat polling operation status. Waiting {timeSpan}", retryCount))
                .Execute(() =>
                {
                    var request = CreateRestRequest(operationLocationPath);
                    return ProcessRequest<PollingOperationResponse>(request, isPostMethod: false);
                });

            if (!IsResponseWithStatus(response, SucceededResponseStatus))
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
