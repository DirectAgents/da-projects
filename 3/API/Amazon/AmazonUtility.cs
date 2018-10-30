using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Net;
using System.Threading;
using Amazon.Entities;
using Newtonsoft.Json;
using RestSharp;
using RestSharp.Authenticators;
using RestSharp.Deserializers;

namespace Amazon
{
    public enum CampaignType
    {
        SponsoredProducts, SponsoredBrands, Empty
    }

    public enum EntitesType
    {
        Campaigns, AdGroups, Keywords, ProductAds, Asins, Profiles
    }

    public class AmazonUtility
    {
        private const int LimitOfReturnedValues = 5000;
        private const int WaitTimeSeconds = 5;
        private const int WaitAttemptsNumber = 60; // 5 sec * 60 = 300 sec => 5 min

        private readonly Dictionary<CampaignType, string> campaignTypeNames = new Dictionary<CampaignType, string>
        {
            {CampaignType.SponsoredProducts, "sp" },
            {CampaignType.SponsoredBrands, "hsa" },
            {CampaignType.Empty, String.Empty}
        };

        private readonly Dictionary<EntitesType, string> entitiesTypeName = new Dictionary<EntitesType, string>
        {
            {EntitesType.Campaigns, "campaigns" },
            {EntitesType.AdGroups, "adGroups" },
            {EntitesType.Keywords, "keywords" },
            {EntitesType.ProductAds, "productAds" },
            {EntitesType.Asins, "asins" },
            {EntitesType.Profiles, "profiles" }
        };

        private const string TOKEN_DELIMITER = "|AMZNAMZN|";
        private const int NumAlts = 10; // including the default (0)

        private static readonly object RequestLock = new object();
        private static readonly object FileLock = new object();
        private static readonly object AccessTokenLock = new object();

        // From Config File
        private readonly string _amazonClientId = ConfigurationManager.AppSettings["AmazonClientId"];
        private readonly string _amazonClientSecret = ConfigurationManager.AppSettings["AmazonClientSecret"];
        //private readonly string _amazonApiUsername = ConfigurationManager.AppSettings["AmazonAPIUsername"];
        //private readonly string _amazonApiPassword = ConfigurationManager.AppSettings["AmazonAPIPassword"];
        private readonly string _amazonApiEndpointUrl = ConfigurationManager.AppSettings["AmazonAPIEndpointUrl"];
        private readonly string _amazonAuthorizeUrl = ConfigurationManager.AppSettings["AmazonAuthorizeUrl"];
        private readonly string _amazonTokenUrl = ConfigurationManager.AppSettings["AmazonTokenUrl"];
        private readonly string _amazonClientUrl = ConfigurationManager.AppSettings["AmazonClientUrl"];
        //private readonly string _amazonAccessCode = ConfigurationManager.AppSettings["AmazonAccessCode"];
        private readonly string _amazonRefreshToken = ConfigurationManager.AppSettings["AmazonRefreshToken"];

        //private long CustomerID { get; set; }
        //private string DeveloperToken { get; set; }
        //private string UserName { get; set; }
        //private string Password { get; set; }
        //private string ClientId { get; set; }
        //private string ClientSecret { get; set; }

        private string[] AuthCode = new string[NumAlts];
        private static string[] AccessToken = new string[NumAlts];
        private static string[] RefreshToken = new string[NumAlts];
        private string[] AltAccountIDs = new string[NumAlts];
        public int WhichAlt { get; set; } // default: 0

        private string ApiEndpointUrl { get; set; }
        private string AuthorizeUrl { get; set; }
        private string TokenUrl { get; set; }
        private string ClientUrl { get; set; }
        //private string ProfileId { get; set; }
        //public static string AccessToken { get; set; }
        //public static string RefreshToken { get; set; }
        //public static string ApplicationAccessCode { get; set; }

        //private AmazonAuth AmazonAuth = null;

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
            get
            {
                return CreateTokenSets().ToArray();
            }
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
                yield return AccessToken[i] + TOKEN_DELIMITER + RefreshToken[i];
            }
        }

        private static void SetTokens(string[] tokens)
        {
            for (var i = 0; i < tokens.Length; i++)
            {
                var tokenSet = tokens[i].Split(new[] { TOKEN_DELIMITER }, StringSplitOptions.None);
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
                BaseUrl = new Uri(_amazonTokenUrl),
                Authenticator = new HttpBasicAuthenticator(_amazonClientId, _amazonClientSecret)
            };
            restClient.AddHandler("application/x-www-form-urlencoded", new JsonDeserializer());

            var request = new RestRequest();
            request.AddParameter("redirect_uri", "https://portal.directagents.com");
            if (String.IsNullOrWhiteSpace(RefreshToken[WhichAlt]))
            {
                request.AddParameter("grant_type", "authorization_code");
                request.AddParameter("code", AuthCode[WhichAlt]);
            }
            else
            {
                request.AddParameter("grant_type", "refresh_token");
                request.AddParameter("refresh_token", RefreshToken[WhichAlt]);
            }

            var response = restClient.ExecuteAsPost<GetTokenResponse>(request, "POST");

            if (response.Data?.access_token == null)
                LogError("Failed to get access token");

            if (response.Data != null && response.Data.refresh_token == null)
                LogError("Failed to get refresh token");

            if (response.Data != null)
            {
                AccessToken[WhichAlt] = response.Data.access_token;
                RefreshToken[WhichAlt] = response.Data.refresh_token; // update this in case it changed
            }
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
            _LogInfo = logInfo;
            _LogError = logError;
        }

        private void ResetCredentials()
        {
            //UserName = _amazonApiUsername;
            //Password = _amazonApiPassword;
            //ClientId = _amazonApiClientId;
            //ClientSecret = _amazonClientSecret;
            ApiEndpointUrl = _amazonApiEndpointUrl;
            AuthorizeUrl = _amazonAuthorizeUrl;
            TokenUrl = _amazonTokenUrl;
            ClientUrl = _amazonClientUrl;
            //ApplicationAccessCode = _amazonAccessCode;
            //RefreshToken = _amazonRefreshToken;
        }

        private void Setup()
        {
            AuthCode[0] = ConfigurationManager.AppSettings["AmazonAuthCode"];
            for (var i = 1; i < NumAlts; i++)
            {
                AltAccountIDs[i] = PlaceLeadingAndTrailingCommas(ConfigurationManager.AppSettings["Amazon_Alt" + i]);
                AuthCode[i] = ConfigurationManager.AppSettings["AmazonAuthCode_Alt" + i];
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
                if (AltAccountIDs[i] != null && AltAccountIDs[i].Contains(',' + accountId + ','))
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

        /// Only for Sponsored Product
        public List<AmazonKeyword> GetKeywords(string profileId, IEnumerable<long> campaignIds)
        {
            var parameters = new Dictionary<string, string>();
            AddParameter(parameters, "campaignIdFilter", campaignIds);
            return GetEntities<AmazonKeyword>(EntitesType.Keywords, CampaignType.SponsoredProducts, parameters, profileId);
        }

        public List<AmazonDailySummary> ReportCampaigns(CampaignType campaignType, DateTime date, string profileId, bool includeCampaignName)
        {
            var param = CreateBaseAmazonApiReportParams(campaignType, date, includeCampaignName);
            return GetReportInfo<AmazonDailySummary>(EntitesType.Campaigns, campaignType, param, profileId);
        }
        
        public List<AmazonAdGroupSummary> ReportAdGroups(CampaignType campaignType, DateTime date, string profileId, bool includeCampaignName)
        {
            var param = CreateBaseAmazonApiReportParams(campaignType, date, includeCampaignName);
            param.metrics += ",campaignId,adGroupName";
            return GetReportInfo<AmazonAdGroupSummary>(EntitesType.AdGroups, campaignType, param, profileId);
        }

        /// Only for Sponsored Product
        /// sku metric - is not available
        public List<AmazonAdDailySummary> ReportProductAds(DateTime date, string profileId, bool includeCampaignName)
        {
            const CampaignType campaignType = CampaignType.SponsoredProducts;
            var param = CreateBaseAmazonApiReportParams(campaignType, date, includeCampaignName);
            param.metrics += ",adGroupId,adGroupName,asin";
            return GetReportInfo<AmazonAdDailySummary>(EntitesType.ProductAds, campaignType, param, profileId);
        }

        public List<AmazonKeywordDailySummary> ReportKeywords(CampaignType campaignType, DateTime date, string profileId, bool includeCampaignName)
        {
            var param = CreateBaseAmazonApiReportParams(campaignType, date, includeCampaignName);
            param.metrics += ",keywordText,campaignId";
            return GetReportInfo<AmazonKeywordDailySummary>(EntitesType.Keywords, campaignType, param, profileId);
        }

        public List<AmazonSearchTermDailySummary> ReportSearchTerms(CampaignType campaignType, DateTime date, string profileId, bool includeCampaignName)
        {
            var param = CreateBaseAmazonApiReportParams(campaignType, date, includeCampaignName);
            param.segment = "query";
            param.metrics += ",keywordText,campaignId";
            return GetReportInfo<AmazonSearchTermDailySummary>(EntitesType.Keywords, campaignType, param, profileId);
        }
        
        private void AddParameter<T>(Dictionary<string, string> parameters, string paramName, IEnumerable<T> values)
        {
            var paramValue = string.Join(",", values);
            parameters.Add(paramName, paramValue);
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

        private AmazonApiReportParams CreateBaseAmazonApiReportParams(CampaignType campaignType, DateTime date, bool includeCampaignName)
        {
            var allMetrics = GetReportMetrics(campaignType, includeCampaignName);
            var reportParams = new AmazonApiReportParams
            {
                reportDate = date.ToString("yyyyMMdd"),
                metrics = allMetrics
            };
            return reportParams;
        }

        private string GetReportMetrics(CampaignType campaignType, bool includeCampaignName)
        {
            var metrics = "cost,impressions,clicks,";
            metrics += campaignType == CampaignType.SponsoredBrands
                ? "attributedSales14d,attributedSales14dSameSKU,attributedConversions14d,attributedConversions14dSameSKU"
                : "attributedConversions1d,attributedConversions7d,attributedConversions14d,attributedConversions30d," +
                  "attributedConversions1dSameSKU,attributedConversions7dSameSKU,attributedConversions14dSameSKU,attributedConversions30dSameSKU," +
                  "attributedUnitsOrdered1d,attributedUnitsOrdered7d,attributedUnitsOrdered14d,attributedUnitsOrdered30d," +
                  "attributedSales1d,attributedSales7d,attributedSales14d,attributedSales30d," +
                  "attributedSales1dSameSKU,attributedSales7dSameSKU,attributedSales14dSameSKU,attributedSales30dSameSKU";
            metrics += includeCampaignName ? ",campaignName" : "";
            return metrics;
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
            var campaignTypePath = campaignType == CampaignType.Empty ? "" : campaignTypeNames[campaignType] + "/";
            var resourcePath = $"v2/{campaignTypePath}{entitiesTypeName[entitiesType]}";
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
            var campaignTypePath = campaignType == CampaignType.Empty ? "" : campaignTypeNames[campaignType] + "/";
            var resourcePath = $"v2/{campaignTypePath}{entitiesTypeName[entitiesType]}/{dataType}";
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
                var request = new RestRequest($"v2/{dataType}/{dataId}");
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
                var exePath = AppDomain.CurrentDomain.BaseDirectory;
                var filePath = Path.Combine(exePath, "download.gzip");
                lock (FileLock)
                {
                    using (Stream s = File.Create(filePath))
                    {
                        responseStream.CopyTo(s);
                    }
                    var fileToDecompress = new FileInfo(filePath);
                    Decompress(fileToDecompress);
                }

                var jsonFile = Path.Combine(exePath, "download.json");
                lock (FileLock)
                {
                    using (var r = new StreamReader(jsonFile))
                    {
                        var json = r.ReadToEnd();
                        return json;
                    }
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

        private void Decompress(FileInfo fileToDecompress)
        {
            var currentFileName = fileToDecompress.FullName;
            var newFileName = currentFileName.Remove(currentFileName.Length - fileToDecompress.Extension.Length);
            using (FileStream originalFileStream = fileToDecompress.OpenRead(), decompressedFileStream = File.Create(newFileName + ".json"))
            {
                using (var decompressionStream = new GZipStream(originalFileStream, CompressionMode.Decompress))
                {
                    decompressionStream.CopyTo(decompressedFileStream);
                }
            }
        }

        private IRestResponse<T> ProcessRequest<T>(RestRequest restRequest, bool postNotGet = false)
            where T : new()
        {
            lock (RequestLock)
            {
                var restClient = new RestClient(_amazonApiEndpointUrl);

                if (String.IsNullOrEmpty(AccessToken[WhichAlt]))
                {
                    GetAccessToken();
                }
                AddAuthorizationHeader(restRequest);
                restRequest.OnBeforeDeserialization = resp => { resp.ContentType = "application/json"; };
                restRequest.AddHeader("Amazon-Advertising-API-ClientId", _amazonClientId);

                bool done = false;
                int tries = 0;
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
                        done = true; //TODO: distinguish between success and failure of ProcessRequest
                }

                if (!String.IsNullOrWhiteSpace(response.ErrorMessage))
                    LogError(response.ErrorMessage);

                return response;
            }
        }

        private void AddAuthorizationHeader(RestRequest request)
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
