using Amazon.Entities;
using Amazon.Enums;
using Amazon.Helpers;
using Newtonsoft.Json;
using Polly;
using RestSharp;
using RestSharp.Authenticators;
using RestSharp.Deserializers;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using Amazon.Entities.HelperEntities;
using Amazon.Entities.HelperEntities.DownloadInfoResponses;
using Amazon.Entities.HelperEntities.PreparedDataResponses;
using Amazon.Entities.Summaries;

namespace Amazon
{
    public class AmazonUtility
    {
        private const int LimitOfReturnedValues = 5000;
        private const int WaitTimeSeconds = 5;
        private const int WaitAttemptsNumber = 60; // 5 sec * 60 = 300 sec => 5 min
        private const int UnauthorizedAttemptsNumber = 1;
        private const int FailedRequestAttemptsNumber = 5;

        private const string TokenDelimiter = "|AMZNAMZN|";
        private const int NumAlts = 10; // including the default (0)

        private static readonly object RequestLock = new object();
        private static readonly object FileLock = new object();
        private static readonly object AccessTokenLock = new object();

        // From Config File
        private readonly string amazonClientId = ConfigurationManager.AppSettings["AmazonClientId"];
        private readonly string amazonClientSecret = ConfigurationManager.AppSettings["AmazonClientSecret"];
        private readonly string amazonApiEndpointUrl = ConfigurationManager.AppSettings["AmazonAPIEndpointUrl"];
        private readonly string amazonAuthorizeUrl = ConfigurationManager.AppSettings["AmazonAuthorizeUrl"];
        private readonly string amazonTokenUrl = ConfigurationManager.AppSettings["AmazonTokenUrl"];
        private readonly string amazonClientUrl = ConfigurationManager.AppSettings["AmazonClientUrl"];

        private static readonly string[] AccessToken = new string[NumAlts];
        private static readonly string[] RefreshToken = new string[NumAlts];
        private readonly string[] authCode = new string[NumAlts];
        private readonly string[] altAccountIDs = new string[NumAlts];

        public int WhichAlt { get; set; } // default: 0

        private string ApiEndpointUrl { get; set; }
        private string AuthorizeUrl { get; set; }
        private string TokenUrl { get; set; }
        private string ClientUrl { get; set; }

        #region Logging

        private readonly Action<string> logInfo;
        private readonly Action<string> logError;

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

        private string GetMessageInCorrectFormat(string message)
        {
            var updatedMessage = message.Replace('{', '\'').Replace('}', '\'');
            return "[AmazonUtility] " + updatedMessage;
        }

        private void LogInfo(string info, int retryNumber)
        {
            var message = GetAttemptMessage(info, retryNumber);
            LogInfo(message);
        }

        private void LogError(string info, int retryNumber, Exception exception)
        {
            var message = GetAttemptMessage(info, retryNumber, exception.Message);
            LogError(message);
        }

        private T LogErrorIfException<T>(Func<T> getSomethingFunc)
            where T : class
        {
            try
            {
                return getSomethingFunc();
            }
            catch (Exception x)
            {
                LogError(x.Message);
            }

            return null;
        }

        private TimeSpan LogPreparedDataWaiting(string dataType, int retryNumber)
        {
            return LogWaiting($"Waiting {WaitTimeSeconds} seconds for {dataType} to finish generating.", retryNumber);
        }

        private TimeSpan LogApiCallWaiting(int retryNumber)
        {
            return LogWaiting($"API calls quota exceeded. Waiting {WaitTimeSeconds} seconds.", retryNumber);
        }

        private void LogFailedRequest(string url, int retryNumber, Exception exception)
        {
            LogError($"URL response ({url}) failed", retryNumber, exception);
        }

        private void LogSuccessfulGeneration(ResponseDownloadInfo downloadInfo)
        {
            LogInfo($"Successful generation: {downloadInfo.Location}");
        }

        private void LogTimeOutGeneration(string message)
        {
            LogError($"Generation timed out: {message}");
        }

        private TimeSpan LogWaiting(string message, int retryNumber)
        {
            LogInfo(message, retryNumber);
            return new TimeSpan(0, 0, WaitTimeSeconds);
        }

        private string GetAttemptMessage(string info, int retryNumber, string baseMessage = null)
        {
            var details = baseMessage == null ? string.Empty : $": {baseMessage}";
            return $"{info} (attempt - {retryNumber}){details}";
        }

        #endregion

        #region Tokens

        public static string[] TokenSets // each string in the array is a combination of Access + Refresh Token
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
                yield return AccessToken[i] + TokenDelimiter + RefreshToken[i];
            }
        }

        private static void SetTokens(string[] tokens)
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

        // Use the refreshToken if we have one, otherwise use the auth code
        public void GetAccessToken()
        {
            var restClient = GetAccessTokenClient();
            var request = GetAccessTokenRequest();
            var response = restClient.ExecuteAsPost<GetTokenResponse>(request, "POST");

            if (response.Data?.AccessToken == null)
            {
                LogError("Failed to get access token");
            }

            if (response.Data != null && response.Data.RefreshToken == null)
            {
                LogError("Failed to get refresh token");
            }

            if (response.Data == null)
            {
                return;
            }

            AccessToken[WhichAlt] = response.Data.AccessToken;
            RefreshToken[WhichAlt] = response.Data.RefreshToken; // update this in case it changed
        }

        private IRestClient GetAccessTokenClient()
        {
            var restClient = new RestClient
            {
                BaseUrl = new Uri(amazonTokenUrl),
                Authenticator = new HttpBasicAuthenticator(amazonClientId, amazonClientSecret)
            };
            restClient.AddHandler("application/x-www-form-urlencoded", new JsonDeserializer());
            return restClient;
        }

        private IRestRequest GetAccessTokenRequest()
        {
            var request = new RestRequest();
            request.AddParameter("redirect_uri", ClientUrl);
            if (string.IsNullOrWhiteSpace(RefreshToken[WhichAlt]))
            {
                request.AddParameter("grant_type", "authorization_code");
                request.AddParameter("code", authCode[WhichAlt]);
            }
            else
            {
                request.AddParameter("grant_type", "refresh_token");
                request.AddParameter("refresh_token", RefreshToken[WhichAlt]);
            }

            return request;
        }

        #endregion

        #region Constructors

        public AmazonUtility()
        {
            ResetCredentials();
            //AmazonAuth = new AmazonAuth(_amazonApiClientId, _amazonClientSecret, _amazonAccessCode);
            Setup();
        }

        public AmazonUtility(Action<string> logInfo, Action<string> logError)
            : this()
        {
            this.logInfo = logInfo;
            this.logError = logError;
        }

        private void ResetCredentials()
        {
            ApiEndpointUrl = amazonApiEndpointUrl;
            AuthorizeUrl = amazonAuthorizeUrl;
            TokenUrl = amazonTokenUrl;
            ClientUrl = amazonClientUrl;
        }

        private void Setup()
        {
            authCode[0] = ConfigurationManager.AppSettings["AmazonAuthCode"];
            for (var i = 1; i < NumAlts; i++)
            {
                altAccountIDs[i] = PlaceLeadingAndTrailingCommas(ConfigurationManager.AppSettings["Amazon_Alt" + i]);
                authCode[i] = ConfigurationManager.AppSettings["AmazonAuthCode_Alt" + i];
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
                if (altAccountIDs[i] != null && altAccountIDs[i].Contains(',' + accountId + ','))
                {
                    WhichAlt = i;
                    break;
                }
            }
        }

        public List<AmazonProfile> GetProfiles()
        {
            return GetEntities<AmazonProfile>(EntitesType.Profiles);
        }

        public List<AmazonCampaign> GetCampaigns(CampaignType campaignType, string profileId)
        {
            return GetEntities<AmazonCampaign>(EntitesType.Campaigns, campaignType, null, profileId);
        }

        // For Sponsored Brands only the following attributed metrics are available:
        // attributedSales14d, attributedSales14dSameSKU, attributedConversions14d, attributedConversions14dSameSKU
        public List<AmazonDailySummary> ReportCampaigns(CampaignType campaignType, DateTime date, string profileId, bool includeCampaignName)
        {
            var param = AmazonApiHelper.CreateReportParams(EntitesType.Campaigns, campaignType, date, includeCampaignName);
            return GetReportInfo<AmazonDailySummary>(EntitesType.Campaigns, campaignType, param, profileId);
        }

        // For Sponsored Brands only the following attributed metrics are available:
        // attributedSales14d, attributedSales14dSameSKU, attributedConversions14d, attributedConversions14dSameSKU
        public List<AmazonAdGroupSummary> ReportAdGroups(CampaignType campaignType, DateTime date, string profileId, bool includeCampaignName)
        {
            var param = AmazonApiHelper.CreateReportParams(EntitesType.AdGroups, campaignType, date, includeCampaignName);
            return GetReportInfo<AmazonAdGroupSummary>(EntitesType.AdGroups, campaignType, param, profileId);
        }

        // Only for Sponsored Product
        // sku metric - is not available
        public List<AmazonAdDailySummary> ReportProductAds(DateTime date, string profileId, bool includeCampaignName)
        {
            const CampaignType campaignType = CampaignType.SponsoredProducts;
            var param = AmazonApiHelper.CreateReportParams(EntitesType.ProductAds, campaignType, date, includeCampaignName);
            return GetReportInfo<AmazonAdDailySummary>(EntitesType.ProductAds, campaignType, param, profileId);
        }

        // For Sponsored Brands only the following attributed metrics are available:
        // attributedSales14d, attributedSales14dSameSKU, attributedConversions14d, attributedConversions14dSameSKU
        public List<AmazonKeywordDailySummary> ReportKeywords(CampaignType campaignType, DateTime date, string profileId, bool includeCampaignName)
        {
            var param = AmazonApiHelper.CreateReportParams(EntitesType.Keywords, campaignType, date, includeCampaignName);
            return GetReportInfo<AmazonKeywordDailySummary>(EntitesType.Keywords, campaignType, param, profileId);
        }

        /// Only for Sponsored Product
        public List<AmazonSearchTermDailySummary> ReportSearchTerms(DateTime date, string profileId, bool includeCampaignName)
        {
            const CampaignType campaignType = CampaignType.SponsoredProducts;
            var param = AmazonApiHelper.CreateReportParams(EntitesType.SearchTerm, campaignType, date, includeCampaignName);
            return GetReportInfo<AmazonSearchTermDailySummary>(EntitesType.Keywords, campaignType, param, profileId);
        }

        private List<T> GetEntities<T>(EntitesType entitiesType, CampaignType campaignType = CampaignType.Empty, Dictionary<string, string> parameters = null, string profileId = null)
        {
            parameters = parameters ?? new Dictionary<string, string>();
            var data = GetEntityList(() => GetEntityList<T>(entitiesType, campaignType, parameters, profileId));
            return data;
        }

        /// Use it instead of the GetEntities method when you need to extract a large number of objects (more than 15,000) and you know about it.
        private List<TEntity> GetSnapshotInfo<TEntity>(EntitesType entitiesType, CampaignType campaignType, string profileId)
        {
            var submitReportResponse = SubmitSnapshot(campaignType, entitiesType, profileId);
            if (submitReportResponse != null)
            {
                var json = DownloadPreparedData<ReportResponseDownloadInfo>("snapshots", submitReportResponse.SnapshotId, profileId);
                if (json != null)
                {
                    var data = GetEntityList(() => JsonConvert.DeserializeObject<List<TEntity>>(json));
                    return data;
                }
            }

            return new List<TEntity>();
        }

        private List<TStat> GetReportInfo<TStat>(EntitesType reportType, CampaignType campaignType, AmazonApiReportParams parameters, string profileId)
        {
            var submitReportResponse = SubmitReport(parameters, campaignType, reportType, profileId);
            if (submitReportResponse != null)
            {
                var json = DownloadPreparedData<ReportResponseDownloadInfo>("reports", submitReportResponse.ReportId, profileId);
                if (json != null)
                {
                    var data = GetEntityList(() => JsonConvert.DeserializeObject<List<TStat>>(json));
                    return data;
                }
            }

            return new List<TStat>();
        }

        private List<T> GetEntityList<T>(Func<List<T>> getListFunc)
        {
            var list = LogErrorIfException(getListFunc);
            return list ?? new List<T>();
        }

        private SnapshotRequestResponse SubmitSnapshot(CampaignType campaignType, EntitesType recordType, string profileId)
        {
            var snapshotParams = AmazonApiHelper.CreateSnapshotParams();
            var snapshotResponse = LogErrorIfException(() =>
                SubmitRequestForPreparedData<SnapshotRequestResponse>("snapshot", snapshotParams, campaignType, recordType, profileId));
            return snapshotResponse;
        }

        private ReportRequestResponse SubmitReport(AmazonApiReportParams reportParams, CampaignType campaignType, EntitesType recordType, string profileId)
        {
            var reportResponse = LogErrorIfException(() =>
                SubmitRequestForPreparedData<ReportRequestResponse>("report", reportParams, campaignType, recordType, profileId));
            return reportResponse;
        }

        private List<T> GetEntityList<T>(EntitesType entitiesType, CampaignType campaignType, Dictionary<string, string> parameters, string profileId, bool retrieveAllData = true)
        {
            var resourcePath = AmazonApiHelper.GetEntityListRelativePath(entitiesType, campaignType);
            var request = CreateRestRequest(resourcePath, Method.GET, profileId);
            AddQueryParametersToRequest(request, parameters);

            if (retrieveAllData && campaignType != CampaignType.Empty)
            {
                return RetrieveAllData<T>(request);
            }

            var response = ProcessRequest<List<T>>(request);
            return response.Data;
        }

        private List<T> RetrieveAllData<T>(IRestRequest getRequest)
        {
            var data = new List<T>();
            var isCompleted = false;
            for (var startIndex = 0; !isCompleted; startIndex += LimitOfReturnedValues)
            {
                getRequest.AddOrUpdateParameter("startIndex", startIndex);
                var restResponse = ProcessRequest<List<T>>(getRequest);
                data.AddRange(restResponse.Data);
                isCompleted = restResponse.Data.Count < LimitOfReturnedValues;
            }

            return data;
        }

        private T SubmitRequestForPreparedData<T>(string dataType, object requestParams, CampaignType campaignType, EntitesType entitiesType, string profileId)
            where T : PreparedDataRequestResponse, new()
        {
            var resourcePath = AmazonApiHelper.GetDataRequestRelativePath(entitiesType, campaignType, dataType);
            var request = CreateRestRequest(resourcePath, Method.POST, profileId);
            request.AddJsonBody(requestParams);
            var response = ProcessRequest<T>(request, true);
            return response?.Content != null ? response.Data : null;
        }

        private string DownloadPreparedData<T>(string dataType, string dataId, string profileId)
            where T : ResponseDownloadInfo, new()
        {
            var response = RequestPreparedDataManyTimes<T>(dataType, dataId, profileId);
            var downloadInfo = response?.Data;
            if (downloadInfo == null || string.IsNullOrWhiteSpace(downloadInfo.Location))
            {
                LogTimeOutGeneration(response.Content);
                return null;
            }
            LogSuccessfulGeneration(downloadInfo);
            var json = GetJsonStringFromDownloadFile(downloadInfo.Location, profileId);
            return json;
        }

        private IRestResponse<T> RequestPreparedDataManyTimes<T>(string dataType, string dataId, string profileId)
            where T : ResponseDownloadInfo, new()
        {
            var response = LogErrorIfException(() =>
                Policy
                    .Handle<Exception>()
                    .OrResult<IRestResponse<T>>(resp => resp.Data.Status != "SUCCESS")
                    .WaitAndRetry(WaitAttemptsNumber, retryNumber => LogPreparedDataWaiting(dataType, retryNumber))
                    .Execute(() => RequestPreparedData<T>(dataType, dataId, profileId))
            );
            return response;
        }

        private IRestResponse<T> RequestPreparedData<T>(string dataType, string dataId, string profileId)
            where T : ResponseDownloadInfo, new()
        {
            var resourcePath = AmazonApiHelper.GetPreparedDataRelativePath(dataType, dataId);
            var request = CreateRestRequest(resourcePath, Method.POST, profileId);
            var restResponse = ProcessRequest<T>(request);
            return restResponse;
        }

        private string GetJsonStringFromDownloadFile(string url, string profileId)
        {
            var response = GetResponseManyTimes(url, profileId);
            var responseStream = response?.GetResponseStream();
            if (responseStream == null)
            {
                return null;
            }

            var json = LogErrorIfException(() =>
            {
                lock (FileLock)
                {
                    return FileManager.ReadJsonFromDecompressedStream(responseStream);
                }
            });
            return json;
        }

        private HttpWebResponse GetResponseManyTimes(string url, string profileId)
        {
            var response = LogErrorIfException(() =>
                Policy
                    .Handle<Exception>()
                    .Retry(FailedRequestAttemptsNumber, (exception, retryCount, context) => LogFailedRequest(url, retryCount, exception))
                    .Execute(() => GetHttpResponse(url, profileId))
            );
            return response;
        }

        private IRestResponse<T> ProcessRequest<T>(IRestRequest restRequest, bool isPostMethod = false)
            where T : new()
        {
            IRestResponse<T> response;
            var restClient = new RestClient(amazonApiEndpointUrl);
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
            LogError(message);

            return response;
        }

        private IRestResponse<T> ProcessRequest<T>(IRestClient restClient, IRestRequest restRequest, bool isPostMethod)
            where T : new()
        {
            if (string.IsNullOrEmpty(AccessToken[WhichAlt]))
            {
                GetAccessToken();
            }

            var response = Policy
                .HandleResult<IRestResponse<T>>(resp => resp.StatusCode == HttpStatusCode.Unauthorized)
                .Retry(UnauthorizedAttemptsNumber,
                    (exception, retryCount, context) => UpdateAccessTokenForRequest(restRequest))
                .Execute(() => GetRestResponse<T>(restClient, restRequest, isPostMethod));

            if (IsRequestProcessed(response))
            {
                response = Policy
                    .HandleResult<IRestResponse<T>>(IsRequestProcessed)
                    .WaitAndRetry(WaitAttemptsNumber, LogApiCallWaiting)
                    .Execute(() => GetRestResponse<T>(restClient, restRequest, isPostMethod));
            }

            return response;
        }

        private bool IsRequestProcessed(IRestResponse response)
        {
            return response.StatusDescription != null && response.StatusDescription.Contains("IN_PROGRESS");
        }

        private IRestRequest CreateRestRequest(string resourceUri, Method method, string profileId)
        {
            var request = new RestRequest(resourceUri);//, method);
            AddAuthorizationHeader(request);
            request.OnBeforeDeserialization = resp => { resp.ContentType = "application/json"; };
            request.AddHeader("Amazon-Advertising-API-Scope", profileId);
            request.AddHeader("Amazon-Advertising-API-ClientId", amazonClientId);
            return request;
        }

        private IRestResponse<T> GetRestResponse<T>(IRestClient restClient, IRestRequest restRequest, bool isPostMethod)
            where T : new()
        {
            var response = isPostMethod
                ? restClient.ExecuteAsPost<T>(restRequest, "POST")
                : restClient.ExecuteAsGet<T>(restRequest, "GET");
            return response;
        }

        private HttpWebRequest CreateHttpRequest(string url, string profileId)
        {
            var request = (HttpWebRequest)WebRequest.Create(url);
            request.Headers.Add("Authorization", "bearer " + AccessToken[WhichAlt]);
            request.Headers.Add("Amazon-Advertising-API-Scope", profileId);
            return request;
        }

        private HttpWebResponse GetHttpResponse(string url, string profileId)
        {
            var request = CreateHttpRequest(url, profileId);
            return GetHttpResponse(request);
        }

        private HttpWebResponse GetHttpResponse(WebRequest request)
        {
            var response = (HttpWebResponse)request.GetResponse();
            return response;
        }

        private void UpdateAccessTokenForRequest(IRestRequest request)
        {
            // Get a new access token and use that.
            GetAccessToken();
            AddAuthorizationHeader(request);
        }

        private void AddAuthorizationHeader(IRestRequest request)
        {
            const string headerName = "Authorization";
            var headerValue = "bearer " + AccessToken[WhichAlt];
            var param = request.Parameters.Find(p => p.Type == ParameterType.HttpHeader && p.Name == headerName);
            if (param != null)
            {
                param.Value = headerValue;
                return;
            }

            request.AddHeader(headerName, headerValue);
        }

        private void AddQueryParametersToRequest(IRestRequest request, Dictionary<string, string> parameters)
        {
            foreach (var param in parameters)
            {
                request.AddQueryParameter(param.Key, param.Value);
            }
        }
    }
}