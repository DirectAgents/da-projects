using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Threading;
using CommissionJunction.Entities;
using CommissionJunction.Entities.RequestEntities;
using CommissionJunction.Entities.ResponseEntities;
using CommissionJunction.Exceptions;
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

        private const string AuthorizationHeader = "Authorization";
        private const string JsonContentType = "application/json";
        private const string HttpPostMethod = "POST";
        private const string HttpGetMethod = "GET";

        private const int NumAlts = 10; // including the default (0)

        private static readonly Semaphore RequestLocker = new Semaphore(MaxConcurrentConnectionsNumber, MaxConcurrentConnectionsNumber);
        private static readonly string[] AccessToken = new string[NumAlts];
        private static readonly string[] AltAccountIDs = new string[NumAlts];

        private static int requestTimeoutInMilliseconds;
        private static int waitTimeInSeconds;
        private static int waitAttemptsNumber;

        private readonly CjLogger logger;

        public int WhichAlt { get; set; } // default: 0

        static CjUtility()
        {
            InitializeAccessTokens();
            InitializeVariablesFromConfig();
        }

        public CjUtility(Action<string> logInfo, Action<Exception> logError, Action<string> logWarning)
        {
            logger = new CjLogger(logInfo, logError, logWarning);
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

        public IEnumerable<List<AdvertiserCommission>> GetAdvertiserCommissions(DateTime fromDateTime, DateTime toDateTime, string accountId)
        {
            return GetItemsForAllDateRanges(fromDateTime, toDateTime, accountId,
                (startTime, endTime) => GetAdvertiserCommissionsData(startTime, endTime, accountId));
        }

        private static void InitializeAccessTokens()
        {
            AccessToken[0] = ConfigurationManager.AppSettings["CJAccessToken"];
            for (var i = 1; i < NumAlts; i++)
            {
                AltAccountIDs[i] = PlaceLeadingAndTrailingCommas(ConfigurationManager.AppSettings["CJ_Alt" + i]);
                AccessToken[i] = ConfigurationManager.AppSettings["CJAccessToken_Alt" + i];
            }
        }

        private static void InitializeVariablesFromConfig()
        {
            requestTimeoutInMilliseconds = int.Parse(ConfigurationManager.AppSettings["CJRequestTimeoutInMilliseconds"]);
            waitTimeInSeconds = int.Parse(ConfigurationManager.AppSettings["CJWaitTimeInSeconds"]);
            waitAttemptsNumber = int.Parse(ConfigurationManager.AppSettings["CJWaitAttemptsNumber"]);
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

        public IEnumerable<List<T>> GetItemsForAllDateRanges<T>(DateTime fromDateTime, DateTime toDateTime,
            string accountId, Func<string, string, IEnumerable<List<T>>> getItemsFunc)
        {
            while (fromDateTime < toDateTime)
            {
                var nextDateRangeStartTime = fromDateTime.AddDays(MaxDaysNumberForSingleReport);
                var endTime = toDateTime < nextDateRangeStartTime ? toDateTime : nextDateRangeStartTime;
                var items = GetItems(fromDateTime, endTime.AddSeconds(-1), accountId, getItemsFunc);
                foreach (var itemsEnumerable in items)
                {
                    yield return itemsEnumerable;
                }
                fromDateTime = endTime;
            }
        }

        public IEnumerable<List<T>> GetItems<T>(DateTime fromDateTime, DateTime toDateTime, string accountId,
            Func<string, string, IEnumerable<List<T>>> getItemsFunc)
        {
            try
            {
                return getItemsFunc(fromDateTime.ToString(DateFormat), toDateTime.ToString(DateFormat));
            }
            catch (Exception exception)
            {
                logger.LogError(new SkippedCommissionsException(fromDateTime, toDateTime, accountId, exception));
                return new List<List<T>>();
            }
        }

        private IEnumerable<List<AdvertiserCommission>> GetAdvertiserCommissionsData(string startTime, string endTime, string accountId)
        {
            logger.LogInfo($"Get Advertiser Commissions from API: {startTime} - {endTime}");
            var query = CjQueries.GetAdvertiserCommissionsQuery(accountId, startTime, endTime);
            var responseData = GetAdvertiserCommissionsData(query);
            yield return responseData.Records;

            while (!responseData.PayloadComplete)
            {
                query = CjQueries.GetAdvertiserCommissionsQuery(accountId, startTime, endTime, responseData.MaxCommissionId);
                responseData = GetAdvertiserCommissionsData(query);
                yield return responseData.Records;
            }
        }

        private CjQueryResponse<AdvertiserCommission> GetAdvertiserCommissionsData(string query)
        {
            var request = CreateQueryRequest(query);
            var response = GetResponseData<CjAdvertiserCommissionsResponse>(request);
            return response.AdvertiserCommissions;
        }

        private IRestRequest CreateQueryRequest(string query)
        {
            var request = CreateRestRequest();
            var requestBody = new CjRequestParams { query = query };
            request.AddJsonBody(requestBody);
            return request;
        }

        private T GetResponseData<T>(IRestRequest request)
            where T: class
        {
            try
            {
                var response = ProcessRequestManyTimes<CjResponse<T>>(request, true);
                return response.Data.Data;
            }
            catch (Exception e)
            {
                logger.LogWarning(e.Message);
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
            where T : new()
        {
            var response = Policy
                .Handle<Exception>()
                .OrResult<IRestResponse<T>>(resp => resp.StatusCode != HttpStatusCode.OK)
                .WaitAndRetry(waitAttemptsNumber, retryNumber => TimeSpan.FromSeconds(waitTimeInSeconds), (exception, timeSpan, retryCount, context) =>
                    logger.LogWaiting("Retry one more time, wait {0} seconds.", timeSpan, retryCount, exception))
                .Execute(() => ProcessRequest<T>(restRequest, isPostMethod));
            return response;
        }

        private IRestResponse<T> ProcessRequest<T>(IRestRequest restRequest, bool isPostMethod)
            where T : new()
        {
            var restClient = new RestClient(GraphQlApiUrl) {Timeout = requestTimeoutInMilliseconds};
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
