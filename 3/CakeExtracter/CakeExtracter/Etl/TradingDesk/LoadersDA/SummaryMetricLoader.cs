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
        private readonly EntityIdStorage<MetricType> metricTypeStorage;

        public SummaryMetricLoader()
        {
            metricTypeStorage = new EntityIdStorage<MetricType>(x => x.Id, x => $"{x.Name} {x.DaysInterval}");
        }

        protected override int Load(List<SummaryMetric> items)
        {
            throw new System.NotImplementedException();
        }

        public void UpsertSummaryMetrics<TSummaryMetric>(ClientPortalProgContext db, IEnumerable<SummaryMetric> items)
            where TSummaryMetric : SummaryMetric, new()
        {
            if (items == null || !items.Any())
            {
                return;
            }

            LoadMetricTypes(db, items);
            foreach (var item in items)
            {
                LoadMetric<TSummaryMetric>(db, item);
            }
        }

        private void LoadMetricTypes(ClientPortalProgContext db, IEnumerable<SummaryMetric> items)
        {
            var notStoredMetricTypes = items
                .Select(x => x.MetricType)
                .Where(x => !metricTypeStorage.IsEntityInStorage(x)).ToList();
            var newMetricTypes = GetNonexistentMetricTypes(db, notStoredMetricTypes);
            if (!newMetricTypes.Any())
            {
                return;
            }
            db.MetricTypes.AddRange(newMetricTypes);
            db.SaveChanges();
            newMetricTypes.ForEach(metricTypeStorage.AddEntityIdToStorage);
        }

        private List<MetricType> GetNonexistentMetricTypes(ClientPortalProgContext db, IEnumerable<MetricType> metricTypes)
        {
            var newMetricTypes = new List<MetricType>();
            foreach (var metricType in metricTypes)
            {
                var metricTypeInDB = db.MetricTypes.FirstOrDefault(x =>
                    metricType.Name == x.Name && metricType.DaysInterval == x.DaysInterval);
                if (metricTypeInDB != null)
                {
                    metricTypeStorage.AddEntityIdToStorage(metricTypeInDB);
                }
                else
                {
                    newMetricTypes.Add(metricType);
                }
            }

            return newMetricTypes;
        }

        private void LoadMetric<TSummaryMetric>(ClientPortalProgContext db, SummaryMetric item)
            where TSummaryMetric : SummaryMetric, new()
        {
            PrepareMetric(item);
            var target = db.Set<TSummaryMetric>().Find(item.Date, item.EntityId, item.MetricTypeId);
            if (target == null)
            {
                AddMetric<TSummaryMetric>(db, item);
                return;
            }
            UpdateMetric(db, item, target);
        }

        private void PrepareMetric(SummaryMetric item)
        {
            item.MetricTypeId = metricTypeStorage.GetEntityIdFromStorage(item.MetricType);
            item.MetricType = null;
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
