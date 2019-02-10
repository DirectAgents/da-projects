using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using DirectAgents.Domain.Contexts;
using DirectAgents.Domain.Entities.CPProg;
using Microsoft.Practices.EnterpriseLibrary.Common.Utility;

namespace CakeExtracter.Etl.TradingDesk.LoadersDA.AmazonLoaders
{
    //Assumed that clean before update done!!!!
    internal class AmazonAdSummaryLoader : BaseAmazonLevelLoader<TDadSummary>
    {
        private readonly TDadSummaryLoader summaryItemsLoader;

        private readonly AmazonSummaryMetricLoader<TDadSummaryMetric> summaryMetricsItemsLoader;

        public AmazonAdSummaryLoader(int accountId)
            : base(accountId)
        {
            summaryItemsLoader = new TDadSummaryLoader(accountId);
            summaryMetricsItemsLoader = new AmazonSummaryMetricLoader<TDadSummaryMetric>();
        }

        protected override int Load(List<TDadSummary> summaryItems)
        {
            Logger.Info(accountId, "Loading {0} Amazon ProductAd / Creative Daily Summaries..", summaryItems.Count);
            EnsureRelatedItems(summaryItems);
            UpsertAdSummaryItems(summaryItems);
            var summaryMetricItems = GetSummaryMetricsToInsert(summaryItems);
            summaryMetricsItemsLoader.UpsertSummaryMetrics(summaryMetricItems);
            return summaryItems.Count;
        }

        private void EnsureRelatedItems(List<TDadSummary> tDadSummaryItems)
        {
            summaryItemsLoader.PrepareData(tDadSummaryItems);
            summaryItemsLoader.AddUpdateDependentAdSets(tDadSummaryItems);
            summaryItemsLoader.AddUpdateDependentTDads(tDadSummaryItems);
            summaryItemsLoader.AssignTDadIdToItems(tDadSummaryItems);
        }

        private void UpsertAdSummaryItems(List<TDadSummary> summaryItems)
        {
            using (var db = new ClientPortalProgContext())
            {
                db.BulkInsert(summaryItems);
            }
        }

        private List<SummaryMetric> GetSummaryMetricsToInsert(List<TDadSummary> adSummaries)
        {
            var summaryMetricsToInsert = new List<SummaryMetric>();
            adSummaries.ForEach(adSummary =>
            {
                var metrics = adSummary.InitialMetrics == null
                ? adSummary.Metrics
                : adSummary.Metrics == null
                    ? adSummary.InitialMetrics
                    : adSummary.InitialMetrics.Concat(adSummary.Metrics);
                metrics.ForEach(metric=> 
                {
                    metric.EntityId = adSummary.TDadId;
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
