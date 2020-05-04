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
                var orderList = rokuUtility.GetOrderList(dateRange.FromDate, dateRange.ToDate);
                var rokuSummaries = GetSummariesByOrders(orderList);
                Add(rokuSummaries);
            }
            catch (Exception e)
            {
                Logger.Error(e);
            }

            End();
        }

        private IReadOnlyCollection<RokuSummary> GetSummariesByOrders(IReadOnlyCollection<OrderItem> orders)
        {
            var stats = rokuUtility.GetOrderStats(
                orders.Select(x => x.Id).ToArray(),
                dateRange.FromDate,
                dateRange.ToDate);

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
                };

            return result.ToList();
        }
    }
}