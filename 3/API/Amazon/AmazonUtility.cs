using Amazon.Entities;
using Amazon.Enums;
using Amazon.Helpers;
using Newtonsoft.Json;
using RestSharp;
using RestSharp.Authenticators;
using RestSharp.Deserializers;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading;

namespace Amazon
{
    public class AmazonUtility
    {
        private const int LimitOfReturnedValues = 5000;
        private const int WaitTimeSeconds = 5;
        private const int WaitAttemptsNumber = 60; // 5 sec * 60 = 300 sec => 5 min

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
        private Action<string> _LogInfo;
        private Action<string> _LogError;

        private void LogInfo(string message)
        {
            if (_LogInfo == null)
                Console.WriteLine(message);
            else
                _LogInfo("[AmazonUtility] " + message);
        }

        private void LogError(string message)
        {
            if (_LogError == null)
                Console.WriteLine(message);
            else
                _LogError("[AmazonUtility] " + message);
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
                var tokenSet = tokens[i].Split(new[] { TokenDelimiter }, StringSplitOptions.None);
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
            var restClient = new RestClient
            {
                BaseUrl = new Uri(amazonTokenUrl),
                Authenticator = new HttpBasicAuthenticator(amazonClientId, amazonClientSecret)
            };
            restClient.AddHandler("application/x-www-form-urlencoded", new JsonDeserializer());

            var request = new RestRequest();
            request.AddParameter("redirect_uri", ClientUrl);
            if (String.IsNullOrWhiteSpace(RefreshToken[WhichAlt]))
            {
                request.AddParameter("grant_type", "authorization_code");
                request.AddParameter("code", authCode[WhichAlt]);
            }
            else
            {
                request.AddParameter("grant_type", "refresh_token");
                request.AddParameter("refresh_token", RefreshToken[WhichAlt]);
            }

            var response = restClient.ExecuteAsPost<GetTokenResponse>(request, "POST");

            if (response.Data?.access_token == null)
            {
                LogError("Failed to get access token");
            }

            if (response.Data != null && response.Data.refresh_token == null)
            {
                LogError("Failed to get refresh token");
            }

            if (response.Data == null)
            {
                return;
            }

            AccessToken[WhichAlt] = response.Data.access_token;
            RefreshToken[WhichAlt] = response.Data.refresh_token; // update this in case it changed
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

            return (idString[0] == ',' ? "" : ",") + idString + (idString[idString.Length - 1] == ',' ? "" : ",");
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

        public List<Profile> GetProfiles()
        {
            return GetEntities<Profile>(EntitesType.Profiles);
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

        private List<T> GetEntities<T>(EntitesType entitiesType, CampaignType campaignType = CampaignType.Empty,
            Dictionary<string, string> parameters = null, string profileId = null)
        {
            try
            {
                parameters = parameters ?? new Dictionary<string, string>();
                var data = GetEntityList<T>(entitiesType, campaignType, parameters, profileId);
                return data;
            }
            catch (Exception x)
            {
                LogError(x.Message);
            }
            return null;
        }

        /// Use it instead of the GetEntities method when you need to extract a large number of objects (more than 15,000) and you know about it.
        private List<TEntity> GetSnapshotInfo<TEntity>(EntitesType entitesType, CampaignType campaignType, string profileId)
        {
            var submitReportResponse = SubmitSnapshot(campaignType, entitesType, profileId);
            if (submitReportResponse != null)
            {
                var json = DownloadPreparedData<ReportResponseDownloadInfo>("snapshots", submitReportResponse.snapshotId, profileId);
                if (json != null)
                {
                    var stats = JsonConvert.DeserializeObject<List<TEntity>>(json);
                    return stats;
                }
            }
            return new List<TEntity>();
        }

        private List<TStat> GetReportInfo<TStat>(EntitesType reportType, CampaignType campaignType, AmazonApiReportParams parameters, string profileId)
        {
            var submitReportResponse = SubmitReport(parameters, campaignType, reportType, profileId);
            if (submitReportResponse != null)
            {
                var json = DownloadPreparedData<ReportResponseDownloadInfo>("reports", submitReportResponse.reportId, profileId);
                if (json != null)
                {
                    var stats = JsonConvert.DeserializeObject<List<TStat>>(json);
                    return stats;
                }
            }
            return new List<TStat>();
        }

        private SnapshotRequestResponse SubmitSnapshot(CampaignType campaignType, EntitesType recordType, string profileId)
        {
            try
            {
                var snapshotParams = new AmazonApiSnapshotParams { stateFilter = "enabled,paused,archived" };
                return SubmitRequestForPreparedData<SnapshotRequestResponse>("snapshot", snapshotParams, campaignType, recordType, profileId);
            }
            catch (Exception x)
            {
                LogError(x.Message);
            }
            return null;
        }

        private ReportRequestResponse SubmitReport(AmazonApiReportParams reportParams, CampaignType campaignType, EntitesType recordType, string profileId)
        {
            try
            {
                return SubmitRequestForPreparedData<ReportRequestResponse>("report", reportParams, campaignType, recordType, profileId);
            }
            catch (Exception x)
            {
                LogError(x.Message);
            }
            return null;
        }

        private List<T> GetEntityList<T>(EntitesType entitiesType, CampaignType campaignType, Dictionary<string, string> parameters,
            string profileId, bool retrieveAllData = true)
        {
            var resourcePath = AmazonApiHelper.GetEntityListRelativePath(entitiesType, campaignType);
            var request = new RestRequest(resourcePath, Method.GET);
            request.AddHeader("Content-Type", "application/json");
            request.AddHeader("Amazon-Advertising-API-Scope", profileId);
            foreach (var param in parameters)
            {
                request.AddQueryParameter(param.Key, param.Value);
            }
            if (retrieveAllData && campaignType != CampaignType.Empty)
            {
                return RetrieveAllData<T>(request);
            }
            var response = ProcessRequest<List<T>>(request, postNotGet: false);
            return response.Data;
        }

        private List<T> RetrieveAllData<T>(RestRequest getRequest)
        {
            var data = new List<T>();
            var isCompleted = false;
            for (var startIndex = 0; !isCompleted; startIndex += LimitOfReturnedValues)
            {
                getRequest.AddOrUpdateParameter("startIndex", startIndex);
                var restResponse = ProcessRequest<List<T>>(getRequest, postNotGet: false);
                data.AddRange(restResponse.Data);
                isCompleted = restResponse.Data.Count < LimitOfReturnedValues;
            }
            return data;
        }

        private T SubmitRequestForPreparedData<T>(string dataType, object requestParams, CampaignType campaignType, EntitesType entitiesType, string profileId)
            where T : PreparedDataRequestResponse, new()
        {
            var resourcePath = AmazonApiHelper.GetDataRequestRelativePath(entitiesType, campaignType, dataType);
            var request = new RestRequest(resourcePath);
            request.AddHeader("Amazon-Advertising-API-Scope", profileId);
            request.AddJsonBody(requestParams);
            var response = ProcessRequest<T>(request, postNotGet: true);
            return response?.Content != null ? response.Data : null;
        }

        private string DownloadPreparedData<T>(string dataType, string dataId, string profileId)
            where T : ResponseDownloadInfo, new()
        {
            var triesLeft = WaitAttemptsNumber;
            while (triesLeft > 0)
            {
                var downloadInfo = RequestPreparedData<T>(dataType, dataId, profileId);
                if (downloadInfo != null && !String.IsNullOrWhiteSpace(downloadInfo.location))
                {
                    var json = GetJsonStringFromDownloadFile(downloadInfo.location, profileId);
                    return json;
                }
                triesLeft--;
            }
            return null;
        }

        private T RequestPreparedData<T>(string dataType, string dataId, string profileId)
            where T : ResponseDownloadInfo, new()
        {
            try
            {
                var resourcePath = AmazonApiHelper.GetPreparedDataRelativePath(dataType, dataId);
                var request = new RestRequest(resourcePath);
                request.AddHeader("Amazon-Advertising-API-Scope", profileId);
                var restResponse = ProcessRequest<T>(request, postNotGet: false);
                if (restResponse.Data.status == "SUCCESS")
                {
                    return restResponse.Data;
                }
                if (restResponse.Content.Contains("IN_PROGRESS"))
                {
                    LogInfo($"Waiting {WaitTimeSeconds} seconds for {dataType} to finish generating.");
                    var timeSpan = new TimeSpan(0, 0, WaitTimeSeconds);
                    Thread.Sleep(timeSpan);
                }
            }
            catch (Exception x)
            {
                LogError(x.Message);
            }
            return null;
        }

        private string GetJsonStringFromDownloadFile(string url, string profileId)
        {
            try
            {
                var responseStream = GetResponseStream(url, profileId);
                lock (FileLock)
                {
                    var json = FileManager.ReadJsonFromDecompressedStream(responseStream);
                    return json;
                }
            }
            catch (Exception e)
            {
                LogError(e.Message);
            }
            return string.Empty;
        }

        private Stream GetResponseStream(string url, string profileId)
        {
            var request = (HttpWebRequest)WebRequest.Create(url);
            request.Headers.Add("Authorization", "bearer " + AccessToken[WhichAlt]);
            request.Headers.Add("Amazon-Advertising-API-Scope", profileId);
            var response = (HttpWebResponse)request.GetResponse();
            return response.GetResponseStream();
        }

        private IRestResponse<T> ProcessRequest<T>(IRestRequest restRequest, bool postNotGet = false)
            where T : new()
        {
            lock (RequestLock)
            {
                var restClient = new RestClient(amazonApiEndpointUrl);

                if (String.IsNullOrEmpty(AccessToken[WhichAlt]))
                {
                    GetAccessToken();
                }
                AddAuthorizationHeader(restRequest);
                restRequest.OnBeforeDeserialization = resp => { resp.ContentType = "application/json"; };
                restRequest.AddHeader("Amazon-Advertising-API-ClientId", amazonClientId);

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
                        AddAuthorizationHeader(restRequest);
                    }
                    else if (response.StatusDescription != null && response.StatusDescription.Contains("IN_PROGRESS") &&
                             tries < 5)
                    {
                        LogInfo($"API calls quota exceeded. Waiting {WaitTimeSeconds} seconds.");
                        var timeSpan = new TimeSpan(0, 0, WaitTimeSeconds);
                        Thread.Sleep(timeSpan);
                    }
                    else
                    {
                        done = true; //TODO: distinguish between success and failure of ProcessRequest
                    }
                }

                if (!String.IsNullOrWhiteSpace(response.ErrorMessage))
                {
                    LogError(response.ErrorMessage);
                }

                return response;
            }
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
    }
}
