using AutoMapper;
using CakeExtracter.Helpers;
using DirectAgents.Domain.Contexts;
using DirectAgents.Domain.Entities.CPProg;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using CakeExtracter.Etl.TradingDesk.LoadersDA.Interfaces;

namespace CakeExtracter.Etl.TradingDesk.LoadersDA
{
    internal class SummaryMetricLoader : Loader<SummaryMetric>, ISummaryMetricLoader
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
            var numChanges = SafeContextWrapper.SaveChangedContext<ClientPortalProgContext>(
                SafeContextWrapper.MetricTypeLocker, db =>
                {
                    UpsertSummaryMetrics<SummaryMetric>(db, items);
                }
            );
            return numChanges;
        }

        public void AssignMetricTypeIdToItems(IEnumerable<SummaryMetric> items)
        {
            var itemsWithUnassignedMetricTypes = items.Where(x => x.MetricTypeId == default(int)).ToList();
            foreach (var item in itemsWithUnassignedMetricTypes)
            {
                item.MetricTypeId = MetricTypeStorage.GetEntityIdFromStorage(item.MetricType);
                item.MetricType = null;
            }
        }

        public void AddDependentMetricTypes(IEnumerable<SummaryMetric> items)
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

        public void AddMetric<TSummaryMetric>(ClientPortalProgContext db, SummaryMetric item)
            where TSummaryMetric : SummaryMetric, new()
        {
            var target = new TSummaryMetric();
            Mapper.Map(item, target);
            db.Set<TSummaryMetric>().Add(target);
        }

        public void UpdateMetric<TSummaryMetric>(ClientPortalProgContext db, SummaryMetric item, TSummaryMetric target)
            where TSummaryMetric : SummaryMetric, new()
        {
            var entry = db.Entry(target);
            entry.State = EntityState.Detached;
            Mapper.Map(item, target);
            entry.State = EntityState.Modified;
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
