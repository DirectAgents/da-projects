﻿using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using CommissionJunction.Entities;
using CommissionJunction.Entities.QueryParams;
using CommissionJunction.Entities.RequestEntities;
using CommissionJunction.Entities.ResponseEntities;
using CommissionJunction.Enums;
using CommissionJunction.Exceptions;
using Polly;
using RestSharp;

namespace CommissionJunction.Utilities
{
    /// <summary>
    /// Commission Junction utility to send API requests
    /// </summary>
    public class CjUtility
    {
        private const string GraphQlApiUrl = "https://commissions.api.cj.com/query";
        private const string DateFormat = "yyyy-MM-ddTHH:mm:ssZ"; // The API expects ISO 8601 datetime
        private const int MaxDaysNumberForSingleReport = 31;
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

        public event Action<SkippedCommissionsException> ProcessSkippedCommissions;

        /// <summary>
        /// Number of an alt account number to use specific access values ​​for Api (for alternate credentials)
        /// </summary>
        public int WhichAlt { get; set; } // default: 0

        static CjUtility()
        {
            InitializeAccessTokens();
            InitializeVariablesFromConfig();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CjUtility"/> class.
        /// </summary>
        /// <param name="logInfo">Action that logs infos</param>
        /// <param name="logError">Action that logs errors</param>
        /// <param name="logWarning">Action that logs warnings</param>
        public CjUtility(Action<string> logInfo, Action<Exception> logError, Action<string> logWarning)
        {
            logger = new CjLogger(logInfo, logError, logWarning);
            ProcessSkippedCommissions += logger.LogError;
        }

        /// <summary>
        /// Set an alt account number to use specific access values ​​for Api (for alternate credentials)
        /// </summary>
        /// <param name="accountId">Account external id</param>
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

        /// <summary>
        /// Get Advertiser commissions from the API.
        /// </summary>
        /// <param name="dateRangeType">Enum to determine the type of date filters used for API requests.</param>
        /// <param name="sinceDateTime">Since date</param>
        /// <param name="beforeDateTime">Before date</param>
        /// <param name="accountId">Advertiser id</param>
        /// <returns>Advertiser commissions</returns>
        public List<AdvertiserCommission> GetAdvertiserCommissions(DateRangeType dateRangeType,
            DateTime sinceDateTime, DateTime beforeDateTime, string accountId)
        {
            return GetItemsForAllDateRanges(sinceDateTime, beforeDateTime, accountId,
                (startTime, endTime) => GetAdvertiserCommissionsData(dateRangeType, startTime, endTime, accountId));
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

        public List<T> GetItemsForAllDateRanges<T>(DateTime sinceDate, DateTime beforeDate,
            string accountId, Func<DateTime, DateTime, List<T>> getItemsFunc)
        {
            var allItems = new List<T>();
            var tasks = new List<Task>();
            while (sinceDate < beforeDate)
            {
                var nextDateRangeStartTime = sinceDate.AddDays(MaxDaysNumberForSingleReport);
                var endTime = beforeDate < nextDateRangeStartTime ? beforeDate : nextDateRangeStartTime;
                var startTime = sinceDate;
                var task = Task.Factory
                    .StartNew(() => getItemsFunc(startTime, endTime))
                    .ContinueWith(x => allItems.AddRange(x.Result));
                tasks.Add(task);
                sinceDate = endTime;
            }

            Task.WaitAll(tasks.ToArray());
            return allItems;
        }

        private List<AdvertiserCommission> GetAdvertiserCommissionsData(DateRangeType dataRangeType,
            DateTime sinceTime, DateTime beforeTime, string accountId)
        {
            try
            {
                var queryParams = new AdvertiserCommissionQueryParams
                {
                    AdvertiserId = accountId,
                    SinceDateTime = sinceTime.ToString(DateFormat),
                    BeforeDateTime = beforeTime.ToString(DateFormat)
                };
                logger.LogInfo($"Get Advertiser Commissions from API (since {queryParams.SinceDateTime}, before {queryParams.BeforeDateTime})");
                return GetAllCommissions(queryParams, dataRangeType, GetAdvertiserCommissionsData);
            }
            catch (Exception exception)
            {
                var exc = new SkippedCommissionsException(sinceTime, beforeTime, accountId, exception);
                ProcessSkippedCommissions?.Invoke(exc);
                return new List<AdvertiserCommission>();
            }
        }

        private CjQueryResponse<AdvertiserCommission> GetAdvertiserCommissionsData(AdvertiserCommissionQueryParams queryParams, DateRangeType dataRangeType)
        {
            var query = CjQueries.GetAdvertiserCommissionsQuery(queryParams, dataRangeType);
            var request = CreateQueryRequest(query);
            var response = GetResponseData<CjAdvertiserCommissionsResponse>(request);
            var responseData = response.AdvertiserCommissions;
            logger.LogInfo($"Extracted {responseData.Count} items (payloadCompleted = {responseData.PayloadComplete}, limit = {responseData.Limit})");
            return responseData;
        }

        private List<T> GetAllCommissions<T, TQueryParams>(TQueryParams queryParams, DateRangeType dataRangeType,
            Func<TQueryParams, DateRangeType, CjQueryResponse<T>> getCommissionsFunc)
            where TQueryParams : BaseQueryParams
        {
            var items = new List<T>();
            var responseData = getCommissionsFunc(queryParams, dataRangeType);
            items.AddRange(responseData.Records);
            while (!responseData.PayloadComplete)
            {
                queryParams.SinceCommissionId = responseData.MaxCommissionId;
                responseData = getCommissionsFunc(queryParams, dataRangeType);
                items.AddRange(responseData.Records);
            }

            return items;
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
