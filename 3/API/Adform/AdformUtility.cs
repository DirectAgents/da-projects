using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Threading;
using Polly;
using Adform.Entities;
using Adform.Entities.ReportEntities;
using Adform.Entities.RequestEntities;
using Adform.Entities.RequestHelpers;
using Adform.Helpers;
using RestSharp;
using RestSharp.Authenticators;
using RestSharp.Deserializers;

namespace Adform
{
    public class AdformUtility
    {
        private const int MaxPageSize = 0; //3000;
        private const int MaxNumberOfMetrics = 10;
        private const int MaxNumberOfDimensions = 8;
        private const int NumAlts = 10; // including the default (0)
        private const int MaxPollingRetryAttempt = 10;
        private readonly TimeSpan PauseBetweenPollingAttempts = new TimeSpan(0, 0, 30);
        private const int UnauthorizedAttemptsNumber = 1;
        private const int MaxAttemptsNumber = 2;
        private const int QuotaLimitAttemptsNumber = 30;
        
        private static readonly object AccessTokenLock = new object();

        private const string AuthorizationHeader = "Authorization";
        private const string Scope = "https://api.adform.com/scope/buyer.stats";
        private const string MetadataDimensionsPath = "/v1/reportingstats/agency/metadata/dimensions";
        private const string MetadataMetricsPath = "/v1/reportingstats/agency/metadata/metrics";
        private const string CreateDataJobPath = "/v1/buyer/stats/data";
        private const string AllOperationsPath = "/v1/buyer/stats/operations";

        // From Config:
        private static readonly string[] AccessTokens = new string[NumAlts];
        private readonly string[] AltAccountIDs = new string[NumAlts];
        private readonly string[] ClientIDs = new string[NumAlts];
        private readonly string[] ClientSecrets = new string[NumAlts];

        private readonly string adformAuthBaseUrl = ConfigurationManager.AppSettings["AdformAuthBaseUrl"];
        private readonly string adformBaseUrl = ConfigurationManager.AppSettings["AdformBaseUrl"];

        public string TrackingId { get; set; }
        private int WhichAlt { get; set; } // default: 0
        
        #region Logging

        private readonly Action<string> logInfo;
        private readonly Action<string> logError;
        private const string LoggerPrefix = "[AdformUtility]";

        private static string GetMessageInCorrectFormat(string message)
        {
            var updatedMessage = message.Replace('{', '\'').Replace('}', '\'');
            return $"{LoggerPrefix} {updatedMessage}";
        }

        private void LogInfo(string message)
        {
            var updatedMessage = GetMessageInCorrectFormat(message);
            if (logInfo == null)
            {
                Console.WriteLine(updatedMessage);
            }
            else
            {
                logInfo(updatedMessage);
            }
        }

        private void LogError(string message)
        {
            var updatedMessage = GetMessageInCorrectFormat(message);
            if (logError == null)
            {
                Console.WriteLine(updatedMessage);
            }
            else
            {
                logError(updatedMessage);
            }
        }

        private void LogInfo(string info, int retryNumber)
        {
            var message = GetAttemptMessage(info, retryNumber);
            LogInfo(message);
        }

        private void LogGenerationError(string baseMessage, Exception exception, int retryNumber)
        {
            var info = $"{baseMessage}: {exception.Message}";
            var message = GetAttemptMessage(info, retryNumber);
            LogError(message);
        }

        private static string GetAttemptMessage(string info, int retryNumber, string baseMessage = null)
        {
            var details = baseMessage == null ? string.Empty : $": {baseMessage}";
            return $"{info} (attempt - {retryNumber}){details}";
        }

        private void LogWaiting(string baseMessage, int? retryCount)
        {
            if (retryCount.HasValue)
            {
                baseMessage += $" (number of retrying - {retryCount})";
            }
            LogInfo(baseMessage);
        }

        #endregion

        #region Constructors

        public AdformUtility()
        {
            Setup();
        }

        public AdformUtility(Action<string> logInfo, Action<string> logError)
            : this()
        {
            this.logInfo = logInfo;
            this.logError = logError;
        }

        private void Setup()
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

        private string PlaceLeadingAndTrailingCommas(string idString)
        {
            if (string.IsNullOrEmpty(idString))
            {
                return idString;
            }

            return GetCommaOrEmptyString(idString.First()) + idString + GetCommaOrEmptyString(idString.Last());
        }

        private string GetCommaOrEmptyString(char symbol)
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

        public void GetAccessToken()
        {
            var restClient = GetAccessTokenClient();
            var request = GetAccessTokenRequest();
            var response = restClient.ExecuteAsPost<GetTokenResponse>(request, "POST");
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
            restClient.AddHandler("application/json", new JsonDeserializer());
            return restClient;
        }

        private IRestRequest GetAccessTokenRequest()
        {
            var request = new RestRequest();
            request.AddParameter("grant_type", "client_credentials");
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
        
        private IRestResponse<T> ProcessRequest<T>(IRestRequest restRequest, bool isPostMethod = false)
            where T : new()
        {
            var restClient = new RestClient(adformBaseUrl);
            restClient.AddHandler("application/json", new JsonDeserializer());
            var response = ProcessRequest<T>(restClient, restRequest, isPostMethod);
            
            if (response.IsSuccessful || response.StatusCode == HttpStatusCode.Accepted)
            {
                return response;
            }

            var message = string.IsNullOrWhiteSpace(response.Content)
                ? response.ErrorMessage
                : response.Content;
            LogError(message);
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
                .Retry(UnauthorizedAttemptsNumber, (exception, retryCount, context) => UpdateAccessTokenForRestRequest(restRequest))
                .Execute(() => TryGetRestResponse<T>(restClient, restRequest, isPostMethod));

            return response;
        }

        private IRestRequest CreateRestRequest(string resourceUri)
        {
            var request = new RestRequest(resourceUri);
            AddAuthorizationHeader(request);
            request.OnBeforeDeserialization = resp => { resp.ContentType = "application/json"; };
            return request;
        }

        private IRestResponse<T> TryGetRestResponse<T>(IRestClient restClient, IRestRequest restRequest, bool isPostMethod)
            where T : new()
        {
            var response = Policy
                .Handle<Exception>()
                .OrResult<IRestResponse<T>>(resp => resp.StatusDescription != null && resp.StatusDescription.Contains("API calls quota exceeded"))
                .Retry(QuotaLimitAttemptsNumber, (exception, retryCount, context) => WaitingRecommendedTime(exception, retryCount))
                .Execute(() => GetRestResponse<T>(restClient, restRequest, isPostMethod));
            return response;
        }

        private static IRestResponse<T> GetRestResponse<T>(IRestClient restClient, IRestRequest restRequest, bool isPostMethod)
            where T : new()
        {
            var response = isPostMethod
                ? restClient.ExecuteAsPost<T>(restRequest, "POST")
                : restClient.ExecuteAsGet<T>(restRequest, "GET");
            return response;
        }

        private void WaitingRecommendedTime<T>(DelegateResult<IRestResponse<T>> response, int retryCount)
        {
            var throttleRecommendedRetryAfter = (int)response.Result.Headers.First(header => header.Name == "Throttle-Retry-After").Value;
            LogInfo($"API calls quota exceeded. Waiting {throttleRecommendedRetryAfter} seconds.", retryCount);
            Thread.Sleep(TimeSpan.FromSeconds(throttleRecommendedRetryAfter));
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
                    limit = MaxPageSize
                },
                includeRowCount = true
            };
            return reportParams;
        }

        public ReportData TryGetReportData(string dataLocationPath)
        {
            var reportData = Policy
                .Handle<Exception>()
                .Retry(MaxAttemptsNumber, (exception, retryCount, context) =>
                    LogGenerationError("Try retrieving a report data", exception, retryCount))
                .Execute(() => GetReportData(dataLocationPath));
            return reportData;
        }

        private ReportData GetReportData(string dataLocationPath)
        {
            var request = CreateRestRequest(dataLocationPath);
            var restResponse = ProcessRequest<ReportResponse>(request, isPostMethod: false);
            return restResponse?.Data?.reportData;
        }
        
        public IEnumerable<ReportData> GetReportDataWithPaging(ReportParams reportParams)
        {
            var allReportData = new List<ReportData>();
            for (var i = 0; i < reportParams.metrics.Length; i += MaxNumberOfMetrics)
            {
                var reportWithCorrectMetricsParams = reportParams.Clone();
                reportWithCorrectMetricsParams.metrics = GetItemsRange(reportParams.metrics, i, MaxNumberOfMetrics);
                for (var j = 0; j < reportWithCorrectMetricsParams.dimensions.Length; j += MaxNumberOfDimensions)
                {
                    var reportWithCorrectDimensionsParams = reportWithCorrectMetricsParams.Clone();
                    reportWithCorrectDimensionsParams.dimensions = GetItemsRange(reportParams.dimensions, j, MaxNumberOfDimensions);
                    
                    var dataLocationPath = ProcessDataReport(reportWithCorrectDimensionsParams);
                    var reportData = TryGetReportData(dataLocationPath);
                    allReportData.Add(reportData);
                }
            }
            return allReportData;
        }

        private static T[] GetItemsRange<T>(IEnumerable<T> items, int startIndex, int count)
        {
            return items.Skip(startIndex).Take(count).ToArray();
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

        private string TryCreateDataJob(ReportParams reportParams)
        {
            var operationLocation = Policy
                .Handle<Exception>()
                .Retry(MaxAttemptsNumber, (exception, retryCount, context) => 
                    LogGenerationError("Try recreating a Data job", exception, retryCount))
                .Execute(() => CreateDataJob(reportParams));
            return operationLocation;
        }

        private string CreateDataJob(ReportParams reportParams)
        {
            var request = CreateRestRequest(CreateDataJobPath);
            request.AddJsonBody(reportParams);
            var restResponse = ProcessRequest<CreateJobResponse>(request, isPostMethod: true);

            if (restResponse.StatusCode != HttpStatusCode.Accepted)
            {
                throw new Exception("Failed creating data job.");
            }

            return restResponse.Headers.FirstOrDefault(header => header.Name == "Operation-Location")?.Value.ToString();
        }

        private string PollingOperation(string operationLocationPath)
        {
            var response = Policy
                .Handle<Exception>()
                .OrResult<IRestResponse<PollingOperationResponse>>(resp =>
                    resp.Data?.Status?.ToLower() != "succeeded" && resp.Data?.Status?.ToLower() != "failed")
                .WaitAndRetry(MaxPollingRetryAttempt, i => PauseBetweenPollingAttempts,
                    (exception, timeSpan, retryCount, context) =>
                        LogWaiting($"Operation is not ready. Will repeat polling operation status. Waiting {timeSpan}", retryCount))
                .Execute(() =>
                {
                    var request = CreateRestRequest(operationLocationPath);
                    return ProcessRequest<PollingOperationResponse>(request, isPostMethod: false);
                });

            if (response.Data.Status.ToLower() != "succeeded")
            {
                throw new Exception("Failed polling operation.");
            }

            return response.Data.Location;
        }
    }
}
