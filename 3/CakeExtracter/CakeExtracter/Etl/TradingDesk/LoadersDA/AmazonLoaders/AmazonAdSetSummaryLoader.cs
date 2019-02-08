using System.Collections.Generic;
using System.Linq;
using DirectAgents.Domain.Contexts;
using DirectAgents.Domain.Entities.CPProg;
using Microsoft.Practices.EnterpriseLibrary.Common.Utility;

namespace CakeExtracter.Etl.TradingDesk.LoadersDA.AmazonLoaders
{
    public class AmazonAdSetSummaryLoader : Loader<AdSetSummary>
    {
        private readonly TDAdSetSummaryLoader summaryItemsLoader;

        private readonly AmazonSummaryMetricLoader<AdSetSummaryMetric> summaryMetricsItemsLoader;

        public AmazonAdSetSummaryLoader(int accountId)
        {
            summaryItemsLoader = new TDAdSetSummaryLoader(accountId);
            summaryMetricsItemsLoader = new AmazonSummaryMetricLoader<AdSetSummaryMetric>();
        }

        protected override int Load(List<AdSetSummary> items)
        {
            Logger.Info(accountId, "Loading {0} Amazon AdSet Daily Summaries..", items.Count);
            EnsureRelatedItems(items);
            UpsertSummaryItems(items);
            var summaryMetricItems = GetSummaryMetricsToInsert(items);
            summaryMetricsItemsLoader.UpsertSummaryMetrics(summaryMetricItems);
            return items.Count;
        }

        private void EnsureRelatedItems(List<AdSetSummary> items)
        {
            summaryItemsLoader.PrepareData(items);
            summaryItemsLoader.AddUpdateDependentStrategies(items);
            summaryItemsLoader.AddUpdateDependentAdSets(items);
            summaryItemsLoader.AssignAdSetIdToItems(items);
        }

        private void UpsertSummaryItems(List<AdSetSummary> summaryItems)
        {
            using (var db = new ClientPortalProgContext())
            {
                db.BulkInsert(summaryItems);
            }
        }

        private List<SummaryMetric> GetSummaryMetricsToInsert(List<AdSetSummary> adSummaries)
        {
            var summaryMetricsToInsert = new List<SummaryMetric>();
            adSummaries.ForEach(adSummary =>
            {
                var metrics = adSummary.InitialMetrics == null
                ? adSummary.Metrics
                : adSummary.Metrics == null
                    ? adSummary.InitialMetrics
                    : adSummary.InitialMetrics.Concat(adSummary.Metrics);
                metrics.ForEach(metric =>
                {
                    metric.EntityId = adSummary.AdSetId;
                });
                if (metrics != null)
                {
                    summaryMetricsToInsert.AddRange(metrics);
                }
            });
            return summaryMetricsToInsert;
        }
    }
}
