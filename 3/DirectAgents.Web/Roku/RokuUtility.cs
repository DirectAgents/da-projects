using System;
using System.Collections.Generic;
using System.Net;
using RestSharp;
using RokuAPI.Constants;
using RokuAPI.Helpers;
using RokuAPI.Models;

namespace RokuAPI
{
    public class RokuUtility
    {
        private readonly Dictionary<string, string> baseHeaders =
            new Dictionary<string, string>()
            {
                { "Sec-Fetch-Site", "cross-site" },
                { "Sec-Fetch-Mode", "cors" },
                { "Sec-Fetch-Dest", "empty" },
                { "Authorization", RokuApiHelper.GetRokuAuthKey },
                { "Origin", "https://selfserve.roku.com" },
                { "accept", "*/*" },
                { "accept-encoding", "gzip, deflate, br" },
                { "accept-language", "ru-RU,ru;q=0.9,en-US;q=0.8,en;q=0.7" },
            };

        private readonly RestClient restClient;

        public RokuUtility()
        {
            restClient = new RestClient(RokuConstants.BaseApiUrl);
        }

        public IReadOnlyList<OrderItem> GetOrderList(DateTime date)
        {
            var request = new RestRequest(RokuApiHelper.GetOrderListUrl(date));
            var result = ExecuteGetRequest(request, date);
            return RokuApiHelper.GetOrderItemsFromResponse(result.Data["lineitems"]);
        }

        public IReadOnlyList<OrderStats> GetOrderStats(string[] orderIds, DateTime date)
        {
            var request = new RestRequest(RokuApiHelper.GetOrderStatsUrl(string.Join(",", orderIds), date));
            var result = ExecuteGetRequest(request, date);
            return RokuApiHelper.GetOrderStatsFromResponse(result.Data["order_stats"], orderIds);
        }

        private dynamic ExecuteGetRequest(IRestRequest request, DateTime date)
        {
            request.Method = Method.GET;
            request.AddHeaders(baseHeaders);
            request.AddHeader("Refer", RokuApiHelper.GetReferHeader(date));
            var response = restClient.Execute<dynamic>(request);
            if(response.StatusCode == HttpStatusCode.Forbidden)
            {
                throw new Exception(response.ErrorMessage);
            }
            return response;
        }
    }
}