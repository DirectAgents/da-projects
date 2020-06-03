using System;
using System.Collections.Generic;
using System.Linq;

using CakeExtracter.Common;
using DirectAgents.Domain.Entities.CPProg.Roku;
using RokuAPI;
using RokuAPI.Models;

namespace CakeExtracter.Etl.Roku.Extractors
{
    public class RokuApiExtractor : Extracter<RokuSummary>
    {
        private readonly RokuUtility rokuUtility;

        private readonly DateRange dateRange;

        /// <summary>
        /// Initializes a new instance of the <see cref="RokuApiExtractor"/> class.
        /// </summary>
        /// <param name="dateRange">Date range to extract statistic.</param>
        public RokuApiExtractor(DateRange dateRange)
        {
            rokuUtility = new RokuUtility();
            this.dateRange = dateRange;
        }

        protected override void Extract()
        {
            try
            {
                foreach (var date in dateRange.Dates)
                {
                    ExtractDaily(date);
                }
            }
            catch (Exception e)
            {
                Logger.Error(e);
            }

            End();
        }

        private void ExtractDaily(DateTime date)
        {
            try
            {
                var orderList = rokuUtility.GetOrderList(date);
                var rokuSummaries = GetSummariesByOrders(orderList, date);
                Add(rokuSummaries);
            }
            catch (Exception e)
            {
                Logger.Warn(e.Message);
            }
        }

        private IReadOnlyList<RokuSummary> GetSummariesByOrders(IReadOnlyList<OrderItem> orders, DateTime date)
        {
            var stats = rokuUtility.GetOrderStats(orders.Select(x => x.Id).ToArray(), date);

            var result =
                from order in orders
                join stat in stats on order.Id equals stat.Id into orderStats
                from orderStat in orderStats.DefaultIfEmpty()
                select new RokuSummary
                {
                    Id = order.Id,
                    OrderName = order.OrderName,
                    Types = order.Type,
                    NumberOfLineItems = 1,
                    FlightDates = order.FlightDates,
                    Stats = orderStat?.Stats ?? string.Empty,
                    Spend = orderStat?.Spend ?? string.Empty,
                    Budget = order.Budget,
                    OrderDate = order.OrderDate,
                    ExtractingDate = date,
                };

            return result.ToList();
        }
    }
}