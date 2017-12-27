using Amazon.Entities;
using Newtonsoft.Json;
using RestSharp;
using RestSharp.Authenticators;
using RestSharp.Deserializers;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Dynamic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web;

namespace Amazon
{
    public class AmazonUtility
    {
        // From Config File
        private readonly string _amazonApiClientId = ConfigurationManager.AppSettings["AmazonClientId"];
        private readonly string _amazonClientSecret = ConfigurationManager.AppSettings["AmazonClientSecret"];
        private readonly string _amazonApiUsername = ConfigurationManager.AppSettings["AmazonAPIUsername"];
        private readonly string _amazonApiPassword = ConfigurationManager.AppSettings["AmazonAPIPassword"];        
        private readonly string _amazonApiEndpointUrl = ConfigurationManager.AppSettings["AmazonAPIEndpointUrl"];
        private readonly string _amazonAuthorizeUrl = ConfigurationManager.AppSettings["AmazonAuthorizeUrl"];
        private readonly string _amazonTokenUrl = ConfigurationManager.AppSettings["AmazonTokenUrl"];
        private readonly string _amazonClientUrl = ConfigurationManager.AppSettings["AmazonClientUrl"];
        private readonly string _amazonAccessCode = ConfigurationManager.AppSettings["AmazonAccessCode"];
        private readonly string _amazonRefreshToken = ConfigurationManager.AppSettings["AmazonRefreshToken"];

        private const string TOKEN_DELIMITER = "|DELIMITER|";
        private long CustomerID { get; set; }
        private string DeveloperToken { get; set; }          
        private string UserName { get; set; }
        private string Password { get; set; }
        private string ClientId { get; set; }
        private string ClientSecret { get; set; }
        
        private string ApiEndpointUrl { get; set; }
        private string AuthorizeUrl { get; set; }
        private string TokenUrl { get; set; }
        private string ClientUrl { get; set; }
        private string ProfileId { get; set; }
        public static string AccessToken { get; set; }
        public static string RefreshToken { get; set; }
        public static string ApplicationAccessCode { get; set; }

        private AmazonAuth AmazonAuth = null;
        public AccessTokens AccessTokens = new AccessTokens();

        #region Logging
        // --- Logging ---
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
        private IEnumerable<string> CreateTokenSets()
        {
            yield return AccessTokens.AccessToken + TOKEN_DELIMITER + AccessTokens.RefreshToken;
        }
        private void ResetCredentials()
        {
            UserName = _amazonApiUsername;
            Password = _amazonApiPassword;
            ClientId = _amazonApiClientId;
            ClientSecret = _amazonClientSecret;
            ApiEndpointUrl = _amazonApiEndpointUrl;
            AuthorizeUrl = _amazonAuthorizeUrl;
            TokenUrl = _amazonTokenUrl;
            ClientUrl = _amazonClientUrl;
            ApplicationAccessCode = _amazonAccessCode;
            RefreshToken = _amazonRefreshToken;
        }
        public string[] TokenSets // each string in the array is a combination of Access + Refresh Token
        {
            get { return CreateTokenSets().ToArray(); }
            set
            {
                for (int i = 0; i < value.Length; i++)
                {
                    var tokenSet = value[i].Split(new string[] { TOKEN_DELIMITER }, StringSplitOptions.None);
                    AccessTokens.access_token = AccessToken = tokenSet[0];

                    if (tokenSet.Length > 1)
                        AccessTokens.refresh_token = RefreshToken = tokenSet[1];
                }
            }
        }
        private void SetCredentials(long accountId)
        {
            ResetCredentials();

            string customerID = ConfigurationManager.AppSettings["BingCustomerID" + accountId];
            if (!String.IsNullOrWhiteSpace(customerID))
                CustomerID = Convert.ToInt64(customerID);
            string token = ConfigurationManager.AppSettings["BingApiToken" + accountId];
            if (!String.IsNullOrWhiteSpace(token))
                DeveloperToken = token;
            string username = ConfigurationManager.AppSettings["BingApiUsername" + accountId];
            if (!String.IsNullOrWhiteSpace(username))
                UserName = username;
            string password = ConfigurationManager.AppSettings["BingApiPassword" + accountId];
            if (!String.IsNullOrWhiteSpace(password))
                Password = password;

            string _clientId = ConfigurationManager.AppSettings["BingClientId" + accountId];
            if (!String.IsNullOrWhiteSpace(_clientId))
                ClientId = _clientId;
            string _clientSecret = ConfigurationManager.AppSettings["BingClientSecret" + accountId];
            if (!String.IsNullOrWhiteSpace(_clientSecret))
                ClientSecret = _clientSecret;
            string _refreshToken = ConfigurationManager.AppSettings["BingRefreshToken" + accountId];
            if (!String.IsNullOrWhiteSpace(_refreshToken))
                RefreshToken = _refreshToken;
        }

        //Get campaigns by profile id
        public string GetCampaings(string profileId)
        {
            try
            {                
                var client = new RestClient(_amazonApiEndpointUrl); //"https://advertising-api.amazon.com"
                client.AddHandler("application/json", new JsonDeserializer());
                var request = new RestRequest("v1/campaigns", Method.GET);
                request.AddHeader("Content-Type", "application/json");
                request.AddHeader("Amazon-Advertising-API-Scope", profileId);
                var restResponse = ProcessRequest<AmazonCampaign>(request, postNotGet: false);
                return restResponse.Content;

            }
            catch (Exception x)
            {
                LogError(x.Message);
            }

            return string.Empty;

        }
        #region NOT USED
        public List<AmazonCampaignSummary> AmazonCampaignDailySummaries(DateTime date, string advertisableEid, string campaignEid = null)
        {
            //var request = new CampaignReportRequest
            //{
            //    start_date = date.ToString("MM-dd-yyyy"),
            //    end_date = date.ToString("MM-dd-yyyy"),
            //    advertisables = advertisableEid,
            //    campaigns = campaignEid, // null for all campaigns
            //};
            //var response = this.CampaignReportClient.CampaignSummaries(request);
            //if (response == null)
            //{
            //    LogInfo("No CampaignSummaries found");
            //    return new List<AmazonCampaignSummary>();
            //}
            //var campaignSummaries = response.results;
            //foreach (var campSum in campaignSummaries)
            //{
            //    campSum.date = date;
            //}
            //return campaignSummaries;
            //;
            return null;
        }
        //private AuthorizationData GetAuthorizationData()
        //{
        //    var authorizationData = new AuthorizationData
        //    {
        //        CustomerId = CustomerID,
        //        //AccountId: not needed?
        //        DeveloperToken = DeveloperToken
        //    };
        //    if (UserName.Contains('@')) // is an email address (Microsoft account); can't use PasswordAuthentication
        //    {
        //        string redirString = ConfigurationManager.AppSettings["BingRedirectionUri"];
        //        var authorization = new OAuthWebAuthCodeGrant(ClientId, ClientSecret, new Uri(redirString));
        //        var task = authorization.RequestAccessAndRefreshTokensAsync(RefreshToken);
        //        task.Wait();
        //        // TODO: see if refreshtoken changed; if so, save the new one

        //        authorizationData.Authentication = authorization;
        //    }
        //    else
        //    {   // old style: BingAds username
        //        authorizationData.Authentication = new PasswordAuthentication(UserName, Password);
        //    }
        //    return authorizationData;
        //}


        #endregion
        #region Constructors
        // --- Constructors ---
        public AmazonUtility()
        {
            ResetCredentials();
            //AmazonAuth = new AmazonAuth(UserName, Password, ClientId, ClientSecret, RefreshToken);
            AmazonAuth = new AmazonAuth(ClientId, ClientSecret, ApplicationAccessCode, RefreshToken);

            AccessTokens = AmazonAuth.GetInitialTokens();
        }
        public AmazonUtility(Action<string> logInfo, Action<string> logError)
        {
            _LogInfo = logInfo;
            _LogError = logError;
            ResetCredentials();
            AmazonAuth = new AmazonAuth(UserName, Password, ClientId, ClientSecret, RefreshToken);
        }
        #endregion

        private IRestResponse<T> ProcessRequest<T>(RestRequest restRequest, bool postNotGet = false)
    where T : new()
        {
            var restClient = new RestClient
            {
                BaseUrl = new Uri(_amazonApiEndpointUrl)
            };
            //restClient.AddHandler("application/json", new JsonDeserializer());

            if (String.IsNullOrEmpty(AccessToken))
                GetAccessToken();

            restRequest.AddHeader("Authorization", "bearer " + AccessToken);
            //restRequest.AddHeader("Content-Type", "application/json");
            restRequest.OnBeforeDeserialization = resp => { resp.ContentType = "application/json"; };

            bool done = false;
            int tries = 0;
            IRestResponse<T> response = null;
            while (!done)
            {
                if (postNotGet)
                    response = restClient.ExecuteAsPost<T>(restRequest, "POST");
                else
                    response = restClient.ExecuteAsGet<T>(restRequest, "GET");

                //var jsonResponse = JsonConvert.DeserializeObject(response.Content);
                tries++;

                if (response.StatusCode == HttpStatusCode.Unauthorized && tries < 2)
                { // Get a new access token and use that.
                    GetAccessToken();
                    var param = restRequest.Parameters.Find(p => p.Type == ParameterType.HttpHeader && p.Name == "Authorization");
                    param.Value = "bearer " + AccessToken;
                }
                else if (response.StatusDescription != null && response.StatusDescription.Contains("API calls quota exceeded") && tries < 5)
                {
                    LogInfo("API calls quota exceeded. Waiting 55 seconds.");
                    Thread.Sleep(55000);
                }
                else
                    done = true; //TODO: distinguish between success and failure of ProcessRequest
            }
            if (!String.IsNullOrWhiteSpace(response.ErrorMessage))
                LogError(response.ErrorMessage);

            return response;
        }

        public string GetKeywords(string profileId)
        {
            try
            {
                var client = new RestClient(_amazonApiEndpointUrl); //"https://advertising-api.amazon.com"
                client.AddHandler("application/json", new JsonDeserializer());
                var request = new RestRequest("v1/keywords", Method.GET);
                request.AddHeader("Content-Type", "application/json");
                request.AddHeader("Amazon-Advertising-API-Scope", profileId);
                var restResponse = ProcessRequest<AmazonKeyword>(request, postNotGet: false);
                return restResponse.Content;
            }
            catch (Exception x)
            {
                LogError(x.Message);
            }
            return string.Empty;
        }

        public string GetProductAds(string profileId)
        {
            try
            {
                var client = new RestClient(_amazonApiEndpointUrl); //"https://advertising-api.amazon.com"
                client.AddHandler("application/json", new JsonDeserializer());
                var request = new RestRequest("v1/productAds", Method.GET);
                request.AddHeader("Content-Type", "application/json");
                request.AddHeader("Amazon-Advertising-API-Scope", profileId);
                var restResponse = ProcessRequest<AmazonProductAds>(request, postNotGet: false);
                return restResponse.Content;
            }
            catch (Exception x)
            {
                LogError(x.Message);
            }
            return string.Empty;
        }

        public void GetAccessToken()
        {
            //var restClient = new RestClient
            //{
            //    BaseUrl = new Uri(AuthorizeUrl),
            //    Authenticator = new HttpBasicAuthenticator(ClientId, ClientSecret)
            //};
            //restClient.AddHandler("application/json", new JsonDeserializer());

            //var request = new RestRequest();
            //request.AddParameter("grant_type", "client_credentials");
            //request.AddParameter("scope", "");

            //var response = restClient.ExecuteAsPost<GetTokenResponse>(request, "POST");
            //if (response.Data != null)
            //    AccessToken = response.Data.access_token;

            AccessTokens = AmazonAuth.GetInitialTokens();

            AccessToken = AccessTokens.AccessToken;
            RefreshToken = AccessTokens.RefreshToken;

        }

        public ReportResponseDownloadInfo RequestReport(string reportId, string profileId)
        {
            try
            {
                var request = new RestRequest("v1/reports/"+reportId);
                request.AddHeader("Amazon-Advertising-API-Scope", profileId);
                
                var restResponse = ProcessRequest<ReportResponseDownloadInfo>(request, postNotGet: false);
                if (restResponse.Data.status == "SUCCESS")
                    return restResponse.Data;
                return null;
               
            }
            catch (Exception x)
            {
                LogError(x.Message);
            }
            return null;
        }
        public ReportRequestResponse SubmitReport(AmazonApiReportParams reportParams, string reportType)
        {            
            var request = new RestRequest("v1/"+ reportType + "/report");
            ProfileId = "2436984122296584";
            request.AddHeader("Amazon-Advertising-API-Scope", ProfileId);
            request.AddJsonBody(reportParams);

            var restResponse = ProcessRequest<ReportRequestResponse>(request, postNotGet: true);
            if (restResponse != null && restResponse.Content != null)
            {

                //ReportResponse reportResponse = restResponse.Data;
                return restResponse.Data;
            }
            return null;

        }

        public AmazonApiReportParams CreateAmazonApiReportParams(string reportDate)
        {
            var reportParams = new AmazonApiReportParams
            {
                campaignType = "sponsoredProducts",
                //segment = segment,
                reportDate = reportDate,
                metrics = "impressions,clicks,cost"
            };
            return reportParams;
        }
        // like a constructor...
        //public ReportParams CreateReportParams(DateTime startDate, DateTime endDate, Int64 clientId, bool basicMetrics = true, 
        //    bool convMetrics = false, bool byCampaign = false, bool byLineItem = false, bool byBanner = false, 
        //    bool byMedia = false, bool byAdInteractionType = false, bool RTBonly = false)
        //{
        //    dynamic filter = new ExpandoObject();
        //    filter.date = new Dates
        //    {
        //        from = startDate.ToString("yyyy'-'M'-'d"),
        //        to = endDate.ToString("yyyy'-'M'-'d")
        //    };
        //    filter.client = new Int64[] { clientId };
        //    if (RTBonly)
        //        filter.media = new { name = new string[] { "Real Time Bidding" } };

        //    var dimensions = new List<string> { "date" };
        //    if (byCampaign)
        //        dimensions.Add("campaign");
        //    if (byLineItem)
        //        dimensions.Add("lineItem");
        //    if (byBanner)
        //        dimensions.Add("banner");
        //    if (byMedia)
        //        dimensions.Add("media");
        //    if (byAdInteractionType)
        //        dimensions.Add("adInteractionType"); // Click, Impression, etc.

        //    var metrics = new List<string>();
        //    if (basicMetrics)
        //        metrics.AddRange(new string[] { "cost", "impressions", "clicks" });
        //    if (convMetrics)
        //        metrics.AddRange(new string[] { "conversions", "sales" });

        //    var reportParams = new ReportParams
        //    {
        //        filter = filter,
        //        dimensions = dimensions.ToArray(),
        //        metrics = metrics.ToArray(),
        //        paging = new Paging
        //        {
        //            offset = 0,
        //            limit = 3000
        //        }
        //    };
        //    return reportParams;
        //}


        public List<Profile> GetProfiles()
        {
            try
            {
                List<Profile> profiles = new List<Profile>();
                var client = new RestClient(_amazonApiEndpointUrl);
                client.AddHandler("application/json", new JsonDeserializer());
                var request = new RestRequest("v1/profiles", Method.GET);
                var restResponse = ProcessRequest<List<Profile>>(request, postNotGet: false);
                return restResponse.Data;
            }
            catch (Exception x)
            {
                LogError(x.Message);
            }
            return null;
        }

        public string GetJsonStringFromDownloadFile(string url)
        {
            try
            {
                var request = (HttpWebRequest)WebRequest.Create(url);
                request.Headers.Add("Authorization", "bearer " + AccessToken);
                request.Headers.Add("Amazon-Advertising-API-Scope", ProfileId);
                var response = (HttpWebResponse)request.GetResponse();
                var responseStream = response.GetResponseStream();
                var streamReader = new StreamReader(responseStream);

                string filePath = @"H:\SourceCode\DirectAgents\da-projects\3\DirectAgents.Web\Amazon\download.gzip";
                using (Stream s = File.Create(filePath))
                {
                    responseStream.CopyTo(s);
                }
                FileInfo fileToDecompress = new FileInfo(filePath);
                Decompress(fileToDecompress);

                using (StreamReader r = new StreamReader(@"H:\SourceCode\DirectAgents\da-projects\3\DirectAgents.Web\Amazon\download.json"))
                {
                    string json = r.ReadToEnd();
                    return json;
                }
            }
            catch (Exception x)
            {
                LogError(x.Message);
            }
            return string.Empty;           
        }

        //public StreamReader CreateStreamReaderFromUrl(string url)
        //{
        //    var request = (HttpWebRequest)WebRequest.Create(url);
        //    request.Headers.Add("Authorization", "bearer " + AccessToken);
        //    request.Headers.Add("Amazon-Advertising-API-Scope", ProfileId);
        //    var response = (HttpWebResponse)request.GetResponse();
        //    var responseStream = response.GetResponseStream();
        //    var streamReader = new StreamReader(responseStream);

        //    return streamReader;

        //}

        public void Decompress(FileInfo fileToDecompress)
        {
            try
            {
                using (FileStream originalFileStream = fileToDecompress.OpenRead())
                {
                    string currentFileName = fileToDecompress.FullName;
                    string newFileName = currentFileName.Remove(currentFileName.Length - fileToDecompress.Extension.Length);

                    using (FileStream decompressedFileStream = File.Create(newFileName + ".json"))
                    {
                        using (GZipStream decompressionStream = new GZipStream(originalFileStream, CompressionMode.Decompress))
                        {
                            decompressionStream.CopyTo(decompressedFileStream);
                            Console.WriteLine("Decompressed: {0}", fileToDecompress.Name);
                        }
                    }
                }
            }
            catch (Exception x)
            {
                LogError(x.Message);
            }
        }
    }
}
