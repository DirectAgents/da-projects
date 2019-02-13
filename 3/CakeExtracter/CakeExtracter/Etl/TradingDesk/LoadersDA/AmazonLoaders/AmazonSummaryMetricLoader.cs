﻿using AutoMapper;
using CakeExtracter.Helpers;
using DirectAgents.Domain.Contexts;
using DirectAgents.Domain.Entities.CPProg;
using System.Collections.Generic;
using System.Linq;

namespace CakeExtracter.Etl.TradingDesk.LoadersDA.AmazonLoaders
{
    /// <summary>
    /// Amazon job summary metrics loader
    /// </summary>
    /// <typeparam name="TSummaryMetric">The type of the summary metric.</typeparam>
    internal class AmazonSummaryMetricLoader<TSummaryMetric>
        where TSummaryMetric : SummaryMetric, new()
    {
        /// <summary>
        /// The metric type storage
        /// </summary>
        public static readonly EntityIdStorage<MetricType> MetricTypeStorage;

        /// <summary>
        /// Initializes the <see cref="AmazonSummaryMetricLoader{TSummaryMetric}"/> class.
        /// </summary>
        static AmazonSummaryMetricLoader()
        {
            MetricTypeStorage = new EntityIdStorage<MetricType>(x => x.Id, x => $"{x.Name} {x.DaysInterval}");
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="AmazonSummaryMetricLoader{TSummaryMetric}"/> class.
        /// </summary>
        public AmazonSummaryMetricLoader()
        {
        }

        /// <summary>
        /// Upserts the summary metrics.
        /// </summary>
        /// <param name="summaryMetrics">The summary metrics.</param>
        public void UpsertSummaryMetrics(List<SummaryMetric> summaryMetrics)
        {
            AddDependentMetricTypes(summaryMetrics);
            AssignMetricTypeIdToItems(summaryMetrics);
            var summaryMetricsItemsToInsert = CastSummaryMetricsToChildClass(summaryMetrics);
            using (var dbContext = new ClientPortalProgContext())
            {
                dbContext.BulkInsert<TSummaryMetric>(summaryMetricsItemsToInsert);
            }
        }

        private void AssignMetricTypeIdToItems(IEnumerable<SummaryMetric> items)
        {
            var itemsWithUnassignedMetricTypes = items.Where(x => x.MetricTypeId == default(int)).ToList();
            foreach (var item in itemsWithUnassignedMetricTypes)
            {
                item.MetricTypeId = MetricTypeStorage.GetEntityIdFromStorage(item.MetricType);
                item.MetricType = null;
            }
        }

        private IEnumerable<TSummaryMetric> CastSummaryMetricsToChildClass(IEnumerable<SummaryMetric> sourceSummaryMetrics)
        {
            return sourceSummaryMetrics.Select(sm =>
            {
                var target = new TSummaryMetric();
                Mapper.Map(sm, target);
                return target;
            });
        }

        private void AddDependentMetricTypes(IEnumerable<SummaryMetric> items)
        {
            var notStoredMetricTypes = items
                .Where(x => x.MetricType != null)
                .GroupBy(x => new { x.MetricType.Name, x.MetricType.DaysInterval })
                .Select(x => x.First().MetricType)
                .Where(x => !MetricTypeStorage.IsEntityInStorage(x))
                .ToList();
            if (notStoredMetricTypes.Any())
            {
                AddDependentMetricTypes(notStoredMetricTypes);
            }
        }

        private void AddDependentMetricTypes(IEnumerable<MetricType> items)
        {
            var newMetricTypes = new List<MetricType>();
            SafeContextWrapper.SaveChangedContext<ClientPortalProgContext>(
                SafeContextWrapper.MetricTypeLocker, db =>
                {
                    foreach (var metricType in items)
                    {
                        var metricTypeInDB = db.MetricTypes.FirstOrDefault(x =>
                            metricType.Name == x.Name && metricType.DaysInterval == x.DaysInterval);
                        if (metricTypeInDB == null)
                        {
                            newMetricTypes.Add(metricType);
                        }
                        else
                        {
                            MetricTypeStorage.AddEntityIdToStorage(metricTypeInDB);
                        }
                    }
                    db.MetricTypes.AddRange(newMetricTypes);
                }
            );
            newMetricTypes.ForEach(MetricTypeStorage.AddEntityIdToStorage);
        }
    }
}
