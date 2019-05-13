using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using Polly;
using System.Threading;
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
        public const int MaxPageSize = 0; //3000;
        public const int MaxNumberOfMetrics = 10;
        public const int MaxNumberOfDimensions = 8;
        public const int NumAlts = 10; // including the default (0)
        public const int MaxRetryAttempt = 10;
        public TimeSpan PauseBetweenAttempts = new TimeSpan(0, 0, 30);

        private const string Scope = "https://api.adform.com/scope/buyer.stats";
        private const string MetadataDimensionsPath = "/v1/reportingstats/agency/metadata/dimensions";
        private const string MetadataMetricsPath = "/v1/reportingstats/agency/metadata/metrics";
        private const string CreateDataJobPath = "/v1/buyer/stats/data";
        private const string AllOperationsPath = "/v1/buyer/stats/operations";

        // From Config:
        public string[] AccessTokens = new string[NumAlts];
        private readonly string[] AltAccountIDs = new string[NumAlts];
        private readonly string[] ClientIDs = new string[NumAlts];
        private readonly string[] ClientSecrets = new string[NumAlts];

        private readonly string adformAuthBaseUrl = ConfigurationManager.AppSettings["AdformAuthBaseUrl"];
        private readonly string adformBaseUrl = ConfigurationManager.AppSettings["AdformBaseUrl"];

        public int WhichAlt { get; set; } // default: 0
        public string TrackingId { get; set; }

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
            var restClient = new RestClient
            {
                BaseUrl = new Uri(adformAuthBaseUrl),
                Authenticator = new HttpBasicAuthenticator(ClientIDs[WhichAlt], ClientSecrets[WhichAlt])
            };
            restClient.AddHandler("application/json", new JsonDeserializer());

            var request = new RestRequest();
            request.AddParameter("grant_type", "client_credentials");
            request.AddParameter("scope", Scope);

            var response = restClient.ExecuteAsPost<GetTokenResponse>(request, "POST");
            if (response.Data != null)
            {
                AccessTokens[WhichAlt] = response.Data.access_token;
            }
        }

        private IRestResponse<T> ProcessRequest<T>(RestRequest restRequest, bool isPostMethod = false)
            where T : new()
        {
            var restClient = new RestClient(adformBaseUrl);
            restClient.AddHandler("application/json", new JsonDeserializer());

            if (string.IsNullOrEmpty(AccessTokens[WhichAlt]))
            {
                GetAccessToken();
            }

            restRequest.AddHeader("Authorization", "Bearer " + AccessTokens[WhichAlt]);

            var done = false;
            var tries = 0;
            IRestResponse<T> response = null;
            while (!done)
            {
                response = isPostMethod
                    ? restClient.ExecuteAsPost<T>(restRequest, "POST")
                    : restClient.ExecuteAsGet<T>(restRequest, "GET");
                tries++;

                if (response.StatusCode == HttpStatusCode.Unauthorized && tries < 2)
                {
                    // Get a new access token and use that.
                    GetAccessToken();
                    var param = restRequest.Parameters.Find(p =>
                        p.Type == ParameterType.HttpHeader && p.Name == "Authorization");
                    param.Value = "Bearer " + AccessTokens[WhichAlt];
                }
                else
                {
                    if (response.StatusDescription != null && response.StatusDescription.Contains("API calls quota exceeded") && tries < 5)
                    {
                        LogInfo("API calls quota exceeded. Waiting 62 seconds.");
                        Thread.Sleep(62000);
                    }
                    else
                    {
                        done = true; //TODO: distinguish between success and failure of ProcessRequest
                    }
                }
            }

            if (!string.IsNullOrWhiteSpace(response.ErrorMessage))
            {
                LogError(response.ErrorMessage);
            }

            return response;
        }

        /// <summary>
        /// Returns all available dimensions to be used in Reporting Stats API.
        /// </summary>
        public void GetDimensions()
        {
            var request = new RestRequest(MetadataDimensionsPath);
            var parms = new
            {
                dimensions = (object)null
            };
            request.AddJsonBody(parms);
            var restResponse = ProcessRequest<object>(request, isPostMethod: true);
        }

        /// <summary>
        /// Returns all available metrics to be used in Reporting Stats API.
        /// </summary>
        public void GetMetrics()
        {
            var request = new RestRequest(MetadataMetricsPath);
            var parms = new
            {
                metrics = (object)null
            };
            request.AddJsonBody(parms);
            var restResponse = ProcessRequest<object>(request, isPostMethod: true);
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
                    //offset = 0,
                    limit = MaxPageSize
                },
                includeRowCount = true
            };
            return reportParams;
        }
        
        public ReportData GetReportData(string dataLocationPath)
        {
            var request = new RestRequest(dataLocationPath);
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

                    var operationLocation = CreateDataJob(reportWithCorrectDimensionsParams);
                    var dataLocationPath = PollingOperation(operationLocation);
                    var reportData = GetReportData(dataLocationPath);
                    allReportData.Add(reportData);
                }
            }

            return allReportData;
        }

        private T[] GetItemsRange<T>(IEnumerable<T> items, int startIndex, int count)
        {
            return items.Skip(startIndex).Take(count).ToArray();
        }
        
        // ---Asynchronous operations---
        public string CreateDataJob(ReportParams reportParams)
        {
            var request = new RestRequest(CreateDataJobPath);
            request.AddJsonBody(reportParams);
            var restResponse = ProcessRequest<CreateJobResponse>(request, isPostMethod: true);

            if (restResponse.StatusCode != HttpStatusCode.Accepted)
            {
                LogError("Creating a data job failed.");
                return null;
            }

            return restResponse.Headers.First(header => header.Name == "Operation-Location").Value.ToString();
        }

        public string PollingOperation(string operationLocationPath)
        {
            var response = Policy
                .Handle<Exception>()
                .OrResult<IRestResponse<PollingOperationResponse>>(resp => 
                    resp.Data.Status.ToLower() != "succeeded" && resp?.Data.Status.ToLower() != "failed")
                .WaitAndRetry(MaxRetryAttempt, i => PauseBetweenAttempts, (exception, timeSpan, retryCount, context) =>
                    LogInfo($"Operation is not ready. Will repeat polling operation status. Waiting {timeSpan}. (number of retrying - {retryCount})"))
                .Execute(() =>
                {
                    var request = new RestRequest(operationLocationPath);
                    return ProcessRequest<PollingOperationResponse>(request, isPostMethod: false);
                });

            if (response.Data.Status.ToLower() != "succeeded")
            {
                throw new Exception("Failed");
            }
            return response.Data.Location;
        }

        public void GetAllOperations()
        {
            var request = new RestRequest(AllOperationsPath);
            var response = ProcessRequest<List<PollingOperationResponse>>(request, false);
        }
    }
}
