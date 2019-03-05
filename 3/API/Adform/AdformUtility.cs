using System;
using System.Collections.Generic;
using System.Configuration;
using System.Dynamic;
using System.Net;
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
        private const string Scope = "https://api.adform.com/scope/eapi";
        private const string ReportDataPath = "/v1/reportingstats/agency/reportdata";
        private const string MetadataDimensionsPath = "/v1/reportingstats/agency/metadata/dimensions";
        private const string CreateDataJobPath = "/v1/buyer/stats/data";
        public const int MaxPageSize = 3000;
        public const int NumAlts = 10; // including the default (0)

        // From Config:
        private string AuthBaseUrl { get; set; }
        private string BaseUrl { get; set; }

        private string[] AltAccountIDs = new string[NumAlts];
        private string[] ClientIDs = new string[NumAlts];
        private string[] ClientSecrets = new string[NumAlts];
        public string[] AccessTokens = new string[NumAlts];
        public int WhichAlt { get; set; } // default: 0

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
            for (int i = 1; i < NumAlts; i++)
            {
                AltAccountIDs[i] = PlaceLeadingAndTrailingCommas(ConfigurationManager.AppSettings["Adform_Alt" + i]);
                ClientIDs[i] = ConfigurationManager.AppSettings["AdformClientID_Alt" + i];
                ClientSecrets[i] = ConfigurationManager.AppSettings["AdformClientSecret_Alt" + i];
            }
            BaseUrl = ConfigurationManager.AppSettings["AdformBaseUrl"];
        }
        private string PlaceLeadingAndTrailingCommas(string idString)
        {
            if (idString == null || idString.Length == 0)
                return idString;
            return (idString[0] == ',' ? "" : ",") + idString + (idString[idString.Length - 1] == ',' ? "" : ",");
        }

        // for alternative credentials...
        public void SetWhichAlt(string accountId)
        {
            WhichAlt = 0; //default
            for (int i = 1; i < NumAlts; i++)
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
                AccessTokens[WhichAlt] = response.Data.access_token;
        }

        private IRestResponse<T> ProcessRequest<T>(RestRequest restRequest, bool postNotGet = false)
            where T : new()
        {
            var restClient = new RestClient
            {
                BaseUrl = new Uri(BaseUrl)
            };
            restClient.AddHandler("application/json", new JsonDeserializer());

            if (String.IsNullOrEmpty(AccessTokens[WhichAlt]))
                GetAccessToken();
            restRequest.AddHeader("Authorization", "Bearer " + AccessTokens[WhichAlt]);

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
                    param.Value = "Bearer " + AccessTokens[WhichAlt];
                }
                else if (response.StatusDescription != null && response.StatusDescription.Contains("API calls quota exceeded") && tries < 5)
                {
                    LogInfo("API calls quota exceeded. Waiting 62 seconds.");
                    Thread.Sleep(62000);
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
            var request = new RestRequest(MetadataDimensionsPath);

            var parms = new
            {
                dimensions = (object)null
            };
            request.AddJsonBody(parms);
            var restResponse = ProcessRequest<object>(request, postNotGet: true);
        }

        public ReportData GetReportData(ReportParams reportParams)
        {
            var request = new RestRequest(ReportDataPath);
            request.AddJsonBody(reportParams);

            var restResponse = ProcessRequest<ReportResponse>(request, postNotGet: true);
            if (restResponse != null && restResponse.Data != null)
            {
                if (restResponse.Data.reportData == null)
                    return null;
                //ReportResponse reportResponse = restResponse.Data;
                return restResponse.Data.reportData;
            }
            return null;
        }

        public IEnumerable<ReportData> GetReportDataWithPaging(ReportParams reportParams)
        {
            int offset = reportParams.paging.offset;
            int calculatedSize = CalculatePageSize(offset, MaxPageSize);
            while (calculatedSize > 0)
            {
                var request = new RestRequest(ReportDataPath);
                reportParams.paging.offset = offset;
                reportParams.paging.limit = calculatedSize;
                request.AddJsonBody(reportParams);

                var restResponse = ProcessRequest<ReportResponse>(request, postNotGet: true);
                if (restResponse != null && restResponse.Data != null && restResponse.Data.reportData != null)
                {
                    yield return restResponse.Data.reportData;

                    offset += calculatedSize;
                    calculatedSize = CalculatePageSize(offset, MaxPageSize, restResponse.Data.totalRowCount);
                    if (calculatedSize > 0)
                        LogInfo(String.Format("Next page. offset {0} limit {1} totalRows {2}", offset, calculatedSize, restResponse.Data.totalRowCount));
                }
                else
                    calculatedSize = 0;
            }
        }
        private static int CalculatePageSize(int offset, int limit, int total = -1)
        {
            int result = limit;
            if (total >= 0 && limit > 0)
            {
                if (offset + limit > total)
                    result = total - offset;
                else if (offset == total) // from api docs; don't think will ever be the case
                    result = 0;
            }
            return result;
        }

        // ---Asynchronous operations---
        public void CreateDataJob(ReportParams reportParams)
        {
            var request = new RestRequest(CreateDataJobPath);
            request.AddJsonBody(reportParams);
            var restReponse = ProcessRequest<CreateJobResponse>(request, postNotGet: true);
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
    }
}
