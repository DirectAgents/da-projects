using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using AutoMapper;
using CakeExtracter.Etl.TradingDesk.Helpers;
using DirectAgents.Domain.Contexts;
using DirectAgents.Domain.Entities.CPProg;

namespace CakeExtracter.Etl.TradingDesk.LoadersDA
{
    class SummaryMetricLoader : Loader<SummaryMetric>
    {
        public static readonly EntityIdStorage<MetricType> MetricTypeStorage;

        static SummaryMetricLoader()
        {
            MetricTypeStorage = new EntityIdStorage<MetricType>(x => x.Id, x => $"{x.Name} {x.DaysInterval}");
        }

        protected override int Load(List<SummaryMetric> items)
        {
            AddDependentMetricTypes(items);
            AssignMetricTypeIdToItems(items);
            using (var db = new ClientPortalProgContext())
            {
                var numChanges = UpsertSummaryMetrics<SummaryMetric>(db, items);
                db.SaveChanges();
                return numChanges;
            }
        }

        public void AssignMetricTypeIdToItems(IEnumerable<SummaryMetric> items)
        {
            foreach (var item in items)
            {
                item.MetricTypeId = MetricTypeStorage.GetEntityIdFromStorage(item.MetricType);
                item.MetricType = null;
            }
        }

        public void AddDependentMetricTypes(IEnumerable<SummaryMetric> items)
        {
            var notStoredMetricTypes = items
                .GroupBy(x => new { x.MetricType.Name, x.MetricType.DaysInterval })
                .Select(x => x.First().MetricType)
                .Where(x => !MetricTypeStorage.IsEntityInStorage(x))
                .ToList();
            if (notStoredMetricTypes.Any())
            {
                AddDependentMetricTypes(notStoredMetricTypes);
            }
        }

        public int UpsertSummaryMetrics<TSummaryMetric>(ClientPortalProgContext db, IEnumerable<SummaryMetric> items)
            where TSummaryMetric : SummaryMetric, new()
        {
            foreach (var item in items)
            {
                var target = db.Set<TSummaryMetric>().Find(item.Date, item.EntityId, item.MetricTypeId);
                if (target == null)
                {
                    AddMetric<TSummaryMetric>(db, item);
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
            db.Set<TSummaryMetric>().RemoveRange(items);
        }

        private void AddDependentMetricTypes(IEnumerable<MetricType> items)
        {
            using (var db = new ClientPortalProgContext())
            {
                var newMetricTypes = new List<MetricType>();
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
                db.SaveChanges();
                newMetricTypes.ForEach(MetricTypeStorage.AddEntityIdToStorage);
            }
        }

        private void AddMetric<TSummaryMetric>(ClientPortalProgContext db, SummaryMetric item)
            where TSummaryMetric : SummaryMetric, new()
        {
            var target = new TSummaryMetric();
            Mapper.Map(item, target);
            db.Set<TSummaryMetric>().Add(target);
        }

        private void UpdateMetric<TSummaryMetric>(ClientPortalProgContext db, SummaryMetric item, TSummaryMetric target)
            where TSummaryMetric : SummaryMetric, new()
        {
            var entry = db.Entry(target);
            Mapper.Map(item, target);
            entry.State = EntityState.Modified;
        }
    }
}
