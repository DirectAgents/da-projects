using CakeExtracter.Etl.TradingDesk.LoadersDA.Interfaces;
using CakeExtracter.Helpers;
using DirectAgents.Domain.Contexts;
using DirectAgents.Domain.Entities.CPProg;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

namespace CakeExtracter.Etl.TradingDesk.LoadersDA.Adders
{
    internal class SummaryMetricAdder : Loader<SummaryMetric>, ISummaryMetricLoader
    {
        private readonly SummaryMetricLoader metricLoader;

        public SummaryMetricAdder()
        {
            metricLoader = new SummaryMetricLoader();
        }

        protected override int Load(List<SummaryMetric> items)
        {
            AddDependentMetricTypes(items);
            AssignMetricTypeIdToItems(items);
            var numChanges = SafeContextWrapper.SaveChangedContext<ClientPortalProgContext>(
                SafeContextWrapper.MetricTypeLocker, db => { UpsertSummaryMetrics<SummaryMetric>(db, items); }
            );
            return numChanges;
        }

        public void AddDependentMetricTypes(IEnumerable<SummaryMetric> items)
        {
            metricLoader.AddDependentMetricTypes(items);
        }

        public void AssignMetricTypeIdToItems(IEnumerable<SummaryMetric> items)
        {
            metricLoader.AssignMetricTypeIdToItems(items);
        }

        public int UpsertSummaryMetrics<TSummaryMetric>(ClientPortalProgContext db, IEnumerable<SummaryMetric> items)
            where TSummaryMetric : SummaryMetric, new()
        {
            foreach (var item in items)
            {
                var target = db.Set<TSummaryMetric>().Find(item.Date, item.EntityId, item.MetricTypeId);
                if (target == null)
                {
                    metricLoader.AddMetric<TSummaryMetric>(db, item);
                }
                else
                {
                    UpdateMetric(db, item, target);
                }
            }

            return items.Count();
        }

        public void RemoveMetrics<TSummaryMetric>(ClientPortalProgContext db, IEnumerable<TSummaryMetric> items)
            where TSummaryMetric : SummaryMetric, new()
        {
        }

        private void UpdateMetric<TSummaryMetric>(ClientPortalProgContext db, SummaryMetric item, TSummaryMetric target)
            where TSummaryMetric : SummaryMetric, new()
        {
            var entry = db.Entry(target);
            entry.State = EntityState.Detached;
            target.AddMetric(item);
            entry.State = EntityState.Modified;
        }
    }
}
