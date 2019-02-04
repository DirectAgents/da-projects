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
using System.IO;
using System.Linq;
using System.Net;
using Amazon.Entities.HelperEntities;
using Amazon.Entities.HelperEntities.DownloadInfoResponses;
using Amazon.Entities.HelperEntities.PreparedDataResponses;
using Amazon.Entities.Summaries;
using Amazon.Exceptions;

namespace Amazon
{
    public class AmazonUtility
    {
        private const int LimitOfReturnedValues = 5000;

        private const string TokenDelimiter = "|AMZNAMZN|";
        private const int NumAlts = 10; // including the default (0)

        private static readonly object RequestLock = new object();
        private static readonly object FileLock = new object();
        private static readonly object AccessTokenLock = new object();

        // From Config File
        // Wait time is increased after every third attempt by the value of waitTimeSeconds. (15 min - async report generation time, service guarantees)
        private readonly int waitTimeSeconds = int.Parse(ConfigurationManager.AppSettings["AmazonWaitTimeSeconds"]);
        private readonly int waitAttemptsNumber = int.Parse(ConfigurationManager.AppSettings["AmazonWaitAttemptsNumber"]);
        private readonly int unauthorizedAttemptsNumber = int.Parse(ConfigurationManager.AppSettings["AmazonUnauthorizedAttemptsNumber"]);
        private readonly int failedRequestAttemptsNumber = int.Parse(ConfigurationManager.AppSettings["AmazonFailedRequestAttemptsNumber"]);

        private readonly string amazonClientId = ConfigurationManager.AppSettings["AmazonClientId"];
        private readonly string amazonClientSecret = ConfigurationManager.AppSettings["AmazonClientSecret"];
        private readonly string amazonApiEndpointUrl = ConfigurationManager.AppSettings["AmazonAPIEndpointUrl"];
        private readonly string amazonAuthorizeUrl = ConfigurationManager.AppSettings["AmazonAuthorizeUrl"];
        private readonly string amazonTokenUrl = ConfigurationManager.AppSettings["AmazonTokenUrl"];
        private readonly string amazonClientUrl = ConfigurationManager.AppSettings["AmazonClientUrl"];
        private readonly string amazonReportsFolder = GetReportFolderName(ConfigurationManager.AppSettings["AmazonReportsBaseFolder"]);

        private static readonly string[] AccessToken = new string[NumAlts];
        private static readonly string[] RefreshToken = new string[NumAlts];
        private readonly string[] authCode = new string[NumAlts];
        private readonly string[] altAccountIDs = new string[NumAlts];

        public int WhichAlt { get; set; } // default: 0
        public bool KeepReports { get; set; }
        public string ReportPrefix { get; set; }

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

        private void LogInfo(string info, int retryNumber)
        {
            var message = GetAttemptMessage(info, retryNumber);
            LogInfo(message);
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

        private void LogWaiting<T>(string formattedMessageWithoutTime, TimeSpan timeSpan, int retryNumber, DelegateResult<IRestResponse<T>> response)
        {
            var waitDetails = response.Exception == null ? response.Result.Content : response.Exception.Message;
            LogWaiting(formattedMessageWithoutTime, timeSpan, retryNumber, waitDetails);
        }

        private void LogWaiting(string formattedMessageWithoutTime, TimeSpan timeSpan, int retryNumber, string waitDetails)
        {
            var waitSeconds = timeSpan.TotalSeconds;
            var message = $"{string.Format(formattedMessageWithoutTime, waitSeconds)}: {waitDetails}";
            LogInfo(message, retryNumber);
        }

        private string GetAttemptMessage(string info, int retryNumber, string baseMessage = null)
        {
            var details = baseMessage == null ? string.Empty : $": {baseMessage}";
            return $"{info} (attempt - {retryNumber}){details}";
        }

        private string GetMessageInCorrectFormat(string message)
        {
            var updatedMessage = message.Replace('{', '\'').Replace('}', '\'');
            return "[AmazonUtility] " + updatedMessage;
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
            Setup();
        }

        public AmazonUtility(Action<string> logInfo, Action<string> logError)
            : this()
        {
            this.logInfo = logInfo;
            this.logError = logError;
        }
        
        private static string GetReportFolderName(string baseFolderName)
        {
            var today = DateTime.Today.ToUniversalTime();
            return $"{baseFolderName}_{today.Year}_{today.Month}_{today.Day}";
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

        /// For Sponsored Brands only the following attributed metrics are available:
        /// attributedSales14d, attributedSales14dSameSKU, attributedConversions14d, attributedConversions14dSameSKU
        public List<AmazonDailySummary> ReportCampaigns(CampaignType campaignType, DateTime date, string profileId, bool includeCampaignName)
        {
            var param = AmazonApiHelper.CreateReportParams(EntitesType.Campaigns, campaignType, date, includeCampaignName);
            return GetReportInfo<AmazonDailySummary>(EntitesType.Campaigns, campaignType, param, profileId);
        }

        /// For Sponsored Brands only the following attributed metrics are available:
        /// attributedSales14d, attributedSales14dSameSKU, attributedConversions14d, attributedConversions14dSameSKU
        public List<AmazonAdGroupSummary> ReportAdGroups(CampaignType campaignType, DateTime date, string profileId, bool includeCampaignName)
        {
            var param = AmazonApiHelper.CreateReportParams(EntitesType.AdGroups, campaignType, date, includeCampaignName);
            return GetReportInfo<AmazonAdGroupSummary>(EntitesType.AdGroups, campaignType, param, profileId);
        }

        /// Only for Sponsored Product
        /// sku metric - is not available for vendor accounts
        public List<AmazonAdDailySummary> ReportProductAds(DateTime date, string profileId, bool includeCampaignName)
        {
            const CampaignType campaignType = CampaignType.SponsoredProducts;
            var param = AmazonApiHelper.CreateReportParams(EntitesType.ProductAds, campaignType, date, includeCampaignName);
            return GetReportInfo<AmazonAdDailySummary>(EntitesType.ProductAds, campaignType, param, profileId);
        }

        /// For Sponsored Brands only the following attributed metrics are available:
        /// attributedSales14d, attributedSales14dSameSKU, attributedConversions14d, attributedConversions14dSameSKU
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
            return GetReportInfo<AmazonSearchTermDailySummary>(EntitesType.SearchTerm, campaignType, param, profileId);
        }

        /// Only for Sponsored Product
        public List<AmazonTargetKeywordDailySummary> ReportTargetKeywords(DateTime date, string profileId, bool includeCampaignName)
        {
            const CampaignType campaignType = CampaignType.SponsoredProducts;
            var param = AmazonApiHelper.CreateReportParams(EntitesType.TargetKeywords, campaignType, date, includeCampaignName);
            return GetReportInfo<AmazonTargetKeywordDailySummary>(EntitesType.TargetKeywords, campaignType, param, profileId);
        }

        /// Only for Sponsored Product
        public List<AmazonTargetSearchTermDailySummary> ReportTargetSearchTerms(DateTime date, string profileId, bool includeCampaignName)
        {
            const CampaignType campaignType = CampaignType.SponsoredProducts;
            var param = AmazonApiHelper.CreateReportParams(EntitesType.TargetSearchTerm, campaignType, date, includeCampaignName);
            return GetReportInfo<AmazonTargetSearchTermDailySummary>(EntitesType.TargetSearchTerm, campaignType, param, profileId);
        }

        /// Only for Sponsored Product
        public List<AmazonAsinSummaries> ReportAsins(DateTime date, string profileId)
        {
            const CampaignType campaignType = CampaignType.SponsoredProducts;
            var param = AmazonApiHelper.CreateAsinReportParams(date);
            return GetReportInfo<AmazonAsinSummaries>(EntitesType.Asins, campaignType, param, profileId);
        }

        private List<T> GetEntities<T>(EntitesType entitiesType, CampaignType campaignType = CampaignType.Empty,
            Dictionary<string, string> parameters = null, string profileId = null)
        {
            try
            {
                parameters = parameters ?? new Dictionary<string, string>();
                var data = GetEntityList<T>(entitiesType, campaignType, parameters, profileId);
                return data;
            }
            catch (Exception e)
            {
                throw new ExtractDataException(e, "load items", entitiesType, campaignType, profileId);
            }
        }

        /// Use it instead of the GetEntities method when you need to extract a large number of objects (more than 15,000) and you know about it.
        private List<TEntity> GetSnapshotInfo<TEntity>(EntitesType entitiesType, CampaignType campaignType,
            string profileId)
        {
            var snapshotName = GetSnapshotName(entitiesType, campaignType);
            try
            {
                var submitReportResponse = SubmitSnapshot(campaignType, entitiesType, profileId);
                var data = DownloadPreparedData<ReportResponseDownloadInfo, TEntity>("snapshots", submitReportResponse.SnapshotId, profileId, snapshotName);
                return data;
            }
            catch (Exception e)
            {
                throw new ExtractDataException(e, snapshotName, entitiesType, campaignType, profileId);
            }
        }

        private List<TStat> GetReportInfo<TStat>(EntitesType reportType, CampaignType campaignType, AmazonApiReportParams parameters, string profileId)
            where TStat : AmazonDailySummary
        {
            var reportName = GetReportName(parameters.reportDate, reportType, campaignType);
            try
            {
                var submitReportResponse = SubmitReport(parameters, campaignType, reportType, profileId);
                var data = DownloadPreparedData<ReportResponseDownloadInfo, TStat>("reports", submitReportResponse.ReportId, profileId, reportName);
                SetCampaignType(data, campaignType);
                return data;
            }
            catch (Exception e)
            {
                throw new ExtractDataException(e, reportName, reportType, campaignType, profileId);
            }
        }

        private string GetReportName(string date, EntitesType entitiesType, CampaignType campaignType)
        {
            return $"AmazonReport_{ReportPrefix}_{date}_{entitiesType}_{campaignType}";
        }

        private string GetSnapshotName(EntitesType entitiesType, CampaignType campaignType)
        {
            return $"AmazonSnapshot_{ReportPrefix}_{entitiesType}_{campaignType}";
        }

        private void SetCampaignType<TStat>(List<TStat> summaries, CampaignType campaignType)
            where TStat : AmazonDailySummary
        {
            var campaignTypeName = AmazonApiHelper.GetCampaignTypeName(campaignType);
            summaries.ForEach(x => x.CampaignType = campaignTypeName);
        }

        private SnapshotRequestResponse SubmitSnapshot(CampaignType campaignType, EntitesType recordType, string profileId)
        {
            var snapshotParams = AmazonApiHelper.CreateSnapshotParams();
            var snapshotResponse = SubmitRequestForPreparedDataManyTimes<SnapshotRequestResponse>("snapshot", snapshotParams, campaignType, recordType, profileId);
            var snapshotInfo = snapshotResponse?.Data;
            if (string.IsNullOrWhiteSpace(snapshotInfo?.SnapshotId))
            {
                ThrowGenerationTimedOutException(snapshotResponse);
            }
            return snapshotInfo;
        }

        private ReportRequestResponse SubmitReport(AmazonApiReportParams reportParams, CampaignType campaignType, EntitesType recordType, string profileId)
        {
            var reportResponse = SubmitRequestForPreparedDataManyTimes<ReportRequestResponse>("report", reportParams, campaignType, recordType, profileId);
            var reportInfo = reportResponse?.Data;
            if (string.IsNullOrWhiteSpace(reportInfo?.ReportId))
            {
                ThrowGenerationTimedOutException(reportResponse);
            }
            return reportInfo;
        }

        private List<T> GetEntityList<T>(EntitesType entitiesType, CampaignType campaignType, Dictionary<string, string> parameters, string profileId, bool retrieveAllData = true)
        {
            var resourcePath = AmazonApiHelper.GetEntityListRelativePath(entitiesType, campaignType);
            var request = CreateRestRequest(resourcePath, profileId);
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

        private IRestResponse<T> SubmitRequestForPreparedDataManyTimes<T>(string dataType, object requestParams, CampaignType campaignType, EntitesType entitiesType, string profileId)
            where T : PreparedDataRequestResponse, new()
        {
            var response = LogErrorIfException(() =>
                Policy
                    .Handle<Exception>()
                    .OrResult<IRestResponse<T>>(resp => resp?.Content == null || !resp.IsSuccessful)
                    .WaitAndRetry(waitAttemptsNumber, GetTimeSpanForWaiting, (exception, timeSpan, retryCount, context) =>
                        LogWaiting($"Waiting {{0}} seconds to get a {dataType} ID.", timeSpan, retryCount, exception))
                    .Execute(() => SubmitRequestForPreparedData<T>(dataType, requestParams, campaignType, entitiesType, profileId))
            );
            return response;
        }

        private IRestResponse<T> SubmitRequestForPreparedData<T>(string dataType, object requestParams, CampaignType campaignType, EntitesType entitiesType, string profileId)
            where T : PreparedDataRequestResponse, new()
        {
            var resourcePath = AmazonApiHelper.GetDataRequestRelativePath(entitiesType, campaignType, dataType);
            var request = CreateRestRequest(resourcePath, profileId);
            request.AddJsonBody(requestParams);
            var response = ProcessRequest<T>(request, true);
            return response;
        }

        private List<TEntity> DownloadPreparedData<T, TEntity>(string dataType, string dataId, string profileId, string reportName)
            where T : ResponseDownloadInfo, new()
        {
            var url = GetPreparedDataUrl<T>(dataType, dataId, profileId);
            var responseStream = GetResponseStream(url, profileId);
            var json = ReadJsonFromStream(responseStream, reportName);
            var data = JsonConvert.DeserializeObject<List<TEntity>>(json);
            return data;
        }

        private string GetPreparedDataUrl<T>(string dataType, string dataId, string profileId)
            where T : ResponseDownloadInfo, new()
        {
            var response = RequestPreparedDataManyTimes<T>(dataType, dataId, profileId);
            var downloadInfo = response?.Data;
            if (string.IsNullOrWhiteSpace(downloadInfo?.Location))
            {
                ThrowGenerationTimedOutException(response);
            }

            LogInfo($"Successful generation: {downloadInfo.Location}");
            return downloadInfo.Location;
        }

        private IRestResponse<T> RequestPreparedDataManyTimes<T>(string dataType, string dataId, string profileId)
            where T : ResponseDownloadInfo, new()
        {
            var response = LogErrorIfException(() =>
                Policy
                    .Handle<Exception>()
                    .OrResult<IRestResponse<T>>(resp => resp.Data.Status != "SUCCESS")
                    .WaitAndRetry(waitAttemptsNumber, GetTimeSpanForWaiting, (exception, timeSpan, retryCount, context) => 
                        LogWaiting($"Waiting {{0}} seconds for {dataType} to finish generating.", timeSpan, retryCount, exception))
                    .Execute(() => RequestPreparedData<T>(dataType, dataId, profileId))
            );
            return response;
        }

        private IRestResponse<T> RequestPreparedData<T>(string dataType, string dataId, string profileId)
            where T : ResponseDownloadInfo, new()
        {
            var resourcePath = AmazonApiHelper.GetPreparedDataRelativePath(dataType, dataId);
            var request = CreateRestRequest(resourcePath, profileId);
            var restResponse = ProcessRequest<T>(request);
            return restResponse;
        }

        private Stream GetResponseStream(string url, string profileId)
        {
            var response = GetResponseManyTimes(url, profileId);
            var responseStream = response?.GetResponseStream();
            if (responseStream == null)
            {
                ThrowGenerationTimedOutException(response);
            }

            return responseStream;
        }

        private string ReadJsonFromStream(Stream responseStream, string reportName)
        {
            string json;
            lock (FileLock)
            {
                json = KeepReports
                    ? FileManager.ReadJsonFromDecompressedStream(amazonReportsFolder, reportName, responseStream)
                    : FileManager.ReadJsonFromDecompressedStream(responseStream);
            }

            if (KeepReports)
            {
                LogInfo($"Report {reportName} has been saved.");
            }
            return json;
        }

        private HttpWebResponse GetResponseManyTimes(string url, string profileId)
        {
            var response = LogErrorIfException(() =>
                Policy
                    .Handle<Exception>()
                    .WaitAndRetry(failedRequestAttemptsNumber, GetTimeSpanForWaiting, (exception, timeSpan, retryCount, context) =>
                        LogWaiting($"Waiting {{0}} seconds because URL ({url}) response failed.", timeSpan, retryCount, exception.Message))
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
                .Handle<Exception>()
                .OrResult<IRestResponse<T>>(resp => resp.StatusCode == HttpStatusCode.Unauthorized)
                .Retry(unauthorizedAttemptsNumber, (exception, retryCount, context) => UpdateAccessTokenForRequest(restRequest))
                .Execute(() => GetRestResponse<T>(restClient, restRequest, isPostMethod));

            return response;
        }

        private IRestRequest CreateRestRequest(string resourceUri, string profileId)
        {
            var request = new RestRequest(resourceUri);
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
            try
            {
                var response = (HttpWebResponse) request.GetResponse();
                return response;
            }
            catch (Exception e)
            {
                LogError(e.Message);
                throw;
            }
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

        private void ThrowGenerationTimedOutException(IRestResponse response)
        {
            var exceptionMessage = GetMessageInCorrectFormat(response.Content);
            throw new ReportGenerationTimedOutException(exceptionMessage);
        }

        private void ThrowGenerationTimedOutException(HttpWebResponse response)
        {
            var exceptionMessage = GetMessageInCorrectFormat(response.ToString());
            throw new ReportGenerationTimedOutException(exceptionMessage);
        }

        /// <summary>
        /// The method returns the time interval for the delay after some attempt of the http request failed.
        /// The return value is exponentially dependent on the retryNumber parameter. It is increased after every third attempt by the value of waitTimeSeconds.
        /// </summary>
        /// <param name="retryNumber">Current attempt number</param>
        /// <returns>Time span for waiting</returns>
        private TimeSpan GetTimeSpanForWaiting(int retryNumber)
        {
            var waitTime = waitTimeSeconds * ((retryNumber - 1) / 3 + 1);
            return TimeSpan.FromSeconds(waitTime);
        }
    }
}