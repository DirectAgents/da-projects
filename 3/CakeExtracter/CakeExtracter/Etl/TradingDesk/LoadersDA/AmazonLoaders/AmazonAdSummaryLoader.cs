using System.Collections.Generic;
using System.Linq;
using DirectAgents.Domain.Contexts;
using DirectAgents.Domain.Entities.CPProg;

namespace CakeExtracter.Etl.TradingDesk.LoadersDA.AmazonLoaders
{
    //Assumed that clean before update done!!!!
    public class AmazonAdSummaryLoader : Loader<TDadSummary>
    {
        private readonly TDadSummaryLoader tdAdSummaryLoader;

        public AmazonAdSummaryLoader(int accountId)
        {
            this.tdAdSummaryLoader = new TDadSummaryLoader(accountId);
        }

        protected override int Load(List<TDadSummary> tDadItems)
        {
            Logger.Info(accountId, "Loading {0} Amazon ProductAd / Creative Daily Summaries..", tDadItems.Count);

            tdAdSummaryLoader.PrepareData(tDadItems);
            tdAdSummaryLoader.AddUpdateDependentAdSets(tDadItems);
            tdAdSummaryLoader.AddUpdateDependentTDads(tDadItems);
            tdAdSummaryLoader.AssignTDadIdToItems(tDadItems);

            InsertAdSummaryItems(tDadItems);
            //var count = tdAdSummaryLoader.UpsertDailySummaries(tDadItems);
            return 50;
        }

        private void InsertAdSummaryItems(List<TDadSummary> tDadSummaryItems)
        {
            using (var db = new ClientPortalProgContext())
            {
                var itemTDadIds = tDadSummaryItems.Select(i => i.TDadId).Distinct().ToArray();
                var tdAdIdsInDb = db.TDads.Select(a => a.Id).Where(i => itemTDadIds.Contains(i)).ToArray();
                var summaryItemsToInsert = tDadSummaryItems;
                db.BulkInsert<TDadSummary>(summaryItemsToInsert);
            }
        }

        private void InsertAdSummaryMetricItems()
        {

        }

        private void UpsertSummaryMetrics(ClientPortalProgContext db, TDadSummary item)
        {
            item.InitialMetrics = GetItemMetrics(item);
            if (item.InitialMetrics == null || !item.InitialMetrics.Any())
            {
                return;
            }
            item.InitialMetrics.ForEach(x => x.EntityId = item.TDadId);
            metricLoader.AddDependentMetricTypes(item.InitialMetrics);
            metricLoader.AssignMetricTypeIdToItems(item.InitialMetrics);
            metricLoader.UpsertSummaryMetrics<TDadSummaryMetric>(db, item.InitialMetrics);
        }

        private List<TDadSummaryMetric> GetSummaryMetrics(List<TDadSummary> adSummaries)
        {
            var summaryMetricsToInsert = new List<TDadSummaryMetric>();
            adSummaries.ForEach(adSummary =>
            {
                var metrics = adSummary.InitialMetrics == null
                ? adSummary.Metrics
                : adSummary.Metrics == null
                    ? adSummary.InitialMetrics
                    : adSummary.InitialMetrics.Concat(adSummary.Metrics);

                return metrics?.ToList();
            });
        }

        private TDadSummaryMetric CastSummaryMetricValueToDefinitType(SummaryMetric summaryMetric)
        {
            return new TDadSummaryMetric
            {
                Date = summaryMetric.Date
            }
        }
    }
}
