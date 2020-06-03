using System;
using System.Collections.Generic;

using RestSharp;
using RokuAPI.Constants;
using RokuAPI.Helpers;
using RokuAPI.Models;

namespace RokuAPI
{
    public class RokuUtility
    {
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
            request.AddHeader("Sec-Fetch-Site", "cross-site");
            request.AddHeader("Sec-Fetch-Mode", "cors");
            request.AddHeader("Sec-Fetch-Dest", "empty");
            request.AddHeader("Authorization", RokuApiHelper.GetRokuAuthKey);
            request.AddHeader("Origin", "https://selfserve.roku.com");
            request.AddHeader("Refer", RokuApiHelper.GetReferHeader(date));
            request.AddHeader("accept", "*/*");
            request.AddHeader("accept-encoding", "gzip, deflate, br");
            request.AddHeader("accept-language", "ru-RU,ru;q=0.9,en-US;q=0.8,en;q=0.7");
            return restClient.Execute<dynamic>(request);
        }
    }
}