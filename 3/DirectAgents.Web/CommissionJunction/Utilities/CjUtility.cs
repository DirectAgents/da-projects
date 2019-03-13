using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Threading;
using CommissionJunction.Entities.RequestEntities;
using CommissionJunction.Entities.ResponseEntities;
using Polly;
using RestSharp;

namespace CommissionJunction.Utilities
{
    public class CjUtility
    {
        private const string GraphQlApiUrl = "https://commissions.api.cj.com/query";
        private const string DateFormat = "yyyy-MM-ddTHH:mm:ssZ"; // The API expects ISO 8601 datetime
        private const int MaxDaysNumberForSingleReport = 31;
        private const int MaxCommissionsNumberForSingleReport = 10000;
        private const int MaxConcurrentConnectionsNumber = 120;

        private const int NumAlts = 10; // including the default (0)

        private const string AuthorizationHeader = "Authorization";
        private const string JsonContentType = "application/json";
        private const string HttpPostMethod = "POST";
        private const string HttpGetMethod = "GET";
        private const int RequestTimeoutInMilliseconds = 600000; // 10 min = 1000 ms * 60 s * 10 min

        private static readonly Semaphore RequestLocker = new Semaphore(MaxConcurrentConnectionsNumber, MaxConcurrentConnectionsNumber);
        private static readonly string[] AccessToken = new string[NumAlts];
        private static readonly string[] AltAccountIDs = new string[NumAlts];

        private readonly CjLogger logger;

        public int WhichAlt { get; set; } // default: 0

        static CjUtility()
        {
            Setup();
        }

        public CjUtility(Action<string> logInfo, Action<string> logError)
        {
            logger = new CjLogger(logInfo, logError);
        }

        // for alternative credentials...
        public void SetWhichAlt(string accountId)
        {
            WhichAlt = 0; //default
            for (var i = 1; i < NumAlts; i++)
            {
                if (AltAccountIDs[i] != null && AltAccountIDs[i].Contains(GetStringWithCommas(accountId)))
                {
                    WhichAlt = i;
                    break;
                }
            }
        }

        public IEnumerable<AdvertiserCommission> GetAdvertiserCommissions(DateTime dateTime, string accountId)
        {
            var query = GetAdvertiserCommissionsQueryForOneDay(dateTime, accountId);
            var items = GetQueryData<AdvertiserCommission>(query);
            return items;
        }

        private static void Setup()
        {
            AccessToken[0] = ConfigurationManager.AppSettings["CJAccessToken"];
            for (var i = 1; i < NumAlts; i++)
            {
                AltAccountIDs[i] = PlaceLeadingAndTrailingCommas(ConfigurationManager.AppSettings["CJ_Alt" + i]);
                AccessToken[i] = ConfigurationManager.AppSettings["CJAccessToken_Alt" + i];
            }
        }

        private static string PlaceLeadingAndTrailingCommas(string idString)
        {
            return string.IsNullOrEmpty(idString)
                ? idString
                : GetStringWithCommas(idString);
        }

        private static string GetStringWithCommas(string strValue)
        {
            return GetCommaOrEmptyString(strValue.First()) + strValue + GetCommaOrEmptyString(strValue.Last());
        }

        private static string GetCommaOrEmptyString(char symbol)
        {
            return symbol == ',' ? string.Empty : ",";
        }

        private string GetAdvertiserCommissionsQueryForOneDay(DateTime dateTime, string accountId)
        {
            var startDayTime = dateTime.ToString(DateFormat);
            var endDayTime = dateTime.AddMinutes(10)/*.AddDays(1).AddSeconds(-1)*/.ToString(DateFormat);
            var query = CjQueries.GetAdvertiserCommissionsQuery(accountId, startDayTime, endDayTime);
            return query;
        }

        private List<T> GetQueryData<T>(string query)
        {
            var request = CreateQueryRequest(query);
            var response = GetQueryResponseData<T>(request);
            var data = response.Records.ToList();
            return data;
        }

        private IRestRequest CreateQueryRequest(string query)
        {
            var request = CreateRestRequest();
            var requestBody = new CjRequestParams { Query = query };
            request.AddJsonBody(requestBody);
            return request;
        }

        private CjQueryResponse<T> GetQueryResponseData<T>(IRestRequest request)
        {
            try
            {
                var response = ProcessRequestManyTimes<CjResponse<CjQueryResponse<T>>>(request, true);
                return response.Data.Data;
            }
            catch (Exception e)
            {
                logger.LogError(e.Message);
                return null;
            }
        }

        private IRestRequest CreateRestRequest()
        {
            var request = new RestRequest();
            AddAuthorizationHeader(request);
            request.OnBeforeDeserialization = resp => { resp.ContentType = JsonContentType; };
            return request;
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
            return $"bearer {AccessToken[WhichAlt]}";
        }

        private IRestResponse<T> ProcessRequestManyTimes<T>(IRestRequest restRequest, bool isPostMethod)
            where T: new()
        {
            var response = Policy
                .Handle<Exception>()
                .OrResult<IRestResponse<T>>(resp => !resp.IsSuccessful)
                .WaitAndRetry(5, retryNumber => TimeSpan.FromMinutes(1), (exception, timeSpan, retryCount, context) =>
                    logger.LogWaiting("Retry one more time, wait {0} seconds.", timeSpan, retryCount, exception))
                .Execute(() => ProcessRequest<T>(restRequest, isPostMethod));
            return response;
        }

        private IRestResponse<T> ProcessRequest<T>(IRestRequest restRequest, bool isPostMethod)
            where T : new()
        {
            var restClient = new RestClient(GraphQlApiUrl) {Timeout = RequestTimeoutInMilliseconds};
            RequestLocker.WaitOne();
            var response = GetRestResponse<T>(restClient, restRequest, isPostMethod);
            RequestLocker.Release();
            return response;
        }

        private IRestResponse<T> GetRestResponse<T>(IRestClient restClient, IRestRequest restRequest, bool isPostMethod)
            where T : new()
        {
            var response = isPostMethod
                ? restClient.ExecuteAsPost<T>(restRequest, HttpPostMethod)
                : restClient.ExecuteAsGet<T>(restRequest, HttpGetMethod);
            return response;
        }
    }
}
