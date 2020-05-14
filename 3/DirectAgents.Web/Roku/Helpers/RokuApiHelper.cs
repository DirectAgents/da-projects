using System;
using System.Collections.Generic;
using System.Web.Configuration;

using RokuAPI.Constants;
using RokuAPI.Models;

namespace RokuAPI.Helpers
{
    public static class RokuApiHelper
    {
        private const string DataFormat = "yyyy-MM-dd";

        public static string GetRokuAuthKey => WebConfigurationManager.AppSettings["RokuAuthKey"];

        public static string GetOrderListUrl(DateTime starDate, DateTime endDate)
        {
            return string.Format(RokuConstants.OrderListUrlFormat, starDate.ToString(DataFormat), endDate.ToString(DataFormat));
        }

        public static string GetOrderStatsUrl(string orderIds, DateTime starDate, DateTime endDate)
        {
            return string.Format(RokuConstants.OrdersStatsUrlFormat, orderIds, starDate.ToString(DataFormat), endDate.ToString(DataFormat));
        }

        public static string GetReferHeader(DateTime starDate, DateTime endDate)
        {
            return string.Format(RokuConstants.ReferHeaderFormat, starDate.ToString(DataFormat), endDate.ToString(DataFormat));
        }

        public static IReadOnlyCollection<OrderItem> GetOrderItemsFromResponse(dynamic response)
        {
            var orders = new List<OrderItem>();

            foreach (var item in response)
            {
                var order = new OrderItem
                {
                    Id = item["order_id"].ToString(),
                    OrderName = item["order"]["name"],
                    Type = GetCompanyType(item["order"]["name"]),
                    FlightDates =
                        $"{FormatDate(item["scheduled_start"])} to {FormatDate(item["scheduled_end"])}",
                    Budget = item["budget"],
                    OrderDate = FormatDate(item["order"]["created_at"])
                };
                orders.Add(order);
            }

            return orders;
        }

        public static IReadOnlyCollection<OrderStats> GetOrderStatsFromResponse(dynamic response, string[] orderIds)
        {
            var statsList = new List<OrderStats>();

            foreach (var id in orderIds)
            {
                var stats = response[id];
                if (stats.Count < 1)
                    continue;
                var orderStats = new OrderStats { Id = id, Spend = stats["spend"].ToString(), Stats = GetStats(stats) };
                statsList.Add(orderStats);
            }

            return statsList;
        }

        private static string GetCompanyType(string companyName)
        {
            return companyName.Contains(RokuConstants.CpiCompanyType)
                       ? RokuConstants.CpiCompanyType
                       : companyName.Contains(RokuConstants.CpmCompanyType)
                           ? RokuConstants.CpmCompanyType
                           : RokuConstants.CpcCompanyType;
        }

        private static string FormatDate(string date)
        {
            const int DatePartSize = 10;
            return date.Substring(0, DatePartSize);
        }

        private static string GetStats(dynamic stats)
        {
            var result = string.Empty;
            if (stats.ContainsKey("convs"))
            {
                result += $"Installs: {stats["convs"]}; ";
            }

            if (stats.ContainsKey("impressions"))
            {
                result += $"Imps: {stats["impressions"]}; ";
            }

            if (stats.ContainsKey("clicks"))
            {
                result += $"Clicks: {stats["clicks"]}; ";
            }

            return result;
        }
    }
}