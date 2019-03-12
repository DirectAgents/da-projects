using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Threading;
using Adform.Entities;
using Adform.Entities.ReportEntities;
using Adform.Entities.RequestHelpers;
using Adform.Helpers;
using RestSharp;
using RestSharp.Authenticators;
using RestSharp.Deserializers;

namespace Adform
{
    public class AdformUtility
    {
        public const int MaxPageSize = 3000;
        public const int MaxNumberOfMetrics = 10;
        public const int MaxNumberOfDimensions = 8;
        public const int NumAlts = 10; // including the default (0)

        private const string Scope = "https://api.adform.com/scope/eapi";
        private const string ReportDataPath = "/v1/reportingstats/agency/reportdata";
        private const string MetadataDimensionsPath = "/v1/reportingstats/agency/metadata/dimensions";
        private const string MetadataMetricsPath = "/v1/reportingstats/agency/metadata/metrics";
        private const string CreateDataJobPath = "/v1/buyer/stats/data";

        // From Config:
        public string[] AccessTokens = new string[NumAlts];
        private readonly string[] AltAccountIDs = new string[NumAlts];
        private readonly string[] ClientIDs = new string[NumAlts];
        private readonly string[] ClientSecrets = new string[NumAlts];

        public int WhichAlt { get; set; } // default: 0
        public string TrackingId { get; set; }
        private string AuthBaseUrl { get; set; }
        private string BaseUrl { get; set; }

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
            ClientIDs[0] = ConfigurationManager.AppSettings["AdformClientID"];
            ClientSecrets[0] = ConfigurationManager.AppSettings["AdformClientSecret"];
            for (var i = 1; i < NumAlts; i++)
            {
                AltAccountIDs[i] = PlaceLeadingAndTrailingCommas(ConfigurationManager.AppSettings["Adform_Alt" + i]);
                ClientIDs[i] = ConfigurationManager.AppSettings["AdformClientID_Alt" + i];
                ClientSecrets[i] = ConfigurationManager.AppSettings["AdformClientSecret_Alt" + i];
            }
            BaseUrl = ConfigurationManager.AppSettings["AdformBaseUrl"];
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
                BaseUrl = new Uri(AuthBaseUrl),
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

        private IRestResponse<T> ProcessRequest<T>(RestRequest restRequest, bool postNotGet = false)
            where T : new()
        {
            var restClient = new RestClient
            {
                BaseUrl = new Uri(BaseUrl)
            };
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
                response = postNotGet
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
            var restResponse = ProcessRequest<object>(request, postNotGet: true);
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
            var restResponse = ProcessRequest<object>(request, postNotGet: true);
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
                    offset = 0,
                    limit = MaxPageSize
                },
                includeRowCount = true
            };
            return reportParams;
        }

        public ReportData GetReportData(ReportParams reportParams)
        {
            var request = new RestRequest(ReportDataPath);
            request.AddJsonBody(reportParams);
            var restResponse = ProcessRequest<ReportResponse>(request, postNotGet: true);
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
                    var reportData = GetReportDataWithPagingAndParamsUnderLimit(reportWithCorrectDimensionsParams);
                    allReportData.AddRange(reportData);
                }
            }

            return allReportData;
        }

        private T[] GetItemsRange<T>(IEnumerable<T> items, int startIndex, int count)
        {
            return items.Skip(startIndex).Take(count).ToArray();
        }

        private IEnumerable<ReportData> GetReportDataWithPagingAndParamsUnderLimit(ReportParams reportParams)
        {
            var offset = reportParams.paging.offset;
            var calculatedSize = CalculatePageSize(offset, MaxPageSize);
            while (calculatedSize > 0)
            {
                var request = new RestRequest(ReportDataPath);
                reportParams.paging.offset = offset;
                reportParams.paging.limit = calculatedSize;
                request.AddJsonBody(reportParams);

                var restResponse = ProcessRequest<ReportResponse>(request, postNotGet: true);
                if (restResponse?.Data?.reportData != null)
                {
                    yield return restResponse.Data.reportData;

                    offset += calculatedSize;
                    calculatedSize = CalculatePageSize(offset, MaxPageSize, restResponse.Data.totalRowCount);
                    if (calculatedSize > 0)
                    {
                        LogInfo($"Next page. offset {offset} limit {calculatedSize} totalRows {restResponse.Data.totalRowCount}");
                    }
                }
                else
                {
                    calculatedSize = 0;
                }
            }
        }

        private static int CalculatePageSize(int offset, int limit, int total = -1)
        {
            return total < 0 || limit <= 0
                ? limit
                : offset + limit > total
                    ? total - offset
                    : offset == total // from api docs; don't think will ever be the case
                        ? 0
                        : limit;
        }

        // ---Asynchronous operations---
        public void CreateDataJob(ReportParams reportParams)
        {
            var request = new RestRequest(CreateDataJobPath);
            request.AddJsonBody(reportParams);
            var restResponse = ProcessRequest<CreateJobResponse>(request, postNotGet: true);
        }
    }
}
