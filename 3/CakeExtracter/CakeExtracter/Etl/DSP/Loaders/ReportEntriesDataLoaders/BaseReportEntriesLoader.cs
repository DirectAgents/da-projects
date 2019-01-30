using CakeExtracter.Etl.DSP.Loaders.Constants;
using CakeExtracter.Etl.DSP.Models;
using DirectAgents.Domain.Contexts;
using DirectAgents.Domain.Entities.CPProg;
using DirectAgents.Domain.Entities.CPProg.DSP;
using DirectAgents.Domain.Entities.CPProg.DSP.SummaryMetrics;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

namespace CakeExtracter.Etl.DSP.Loaders.ReportEntriesDataLoaders
{
    internal abstract class BaseDspItemLoader<TReportEntity, TDbEntity, TSummaryMetricEntity>
        where TReportEntity : DspReportMetricItem
        where TDbEntity : DspBaseItem
        where TSummaryMetricEntity : DspSummaryMetric, new()
    {
        protected Dictionary<string, int> metricTypes;

        public BaseDspItemLoader(Dictionary<string, int> metricTypes)
        {
            this.metricTypes = metricTypes;
        }

        public List<TDbEntity> EnsureDspEntitiesInDataBase(List<TReportEntity> reportEntities, ExtAccount extAccount)
        {
            using (var dbContext = new ClientPortalProgContext())
            {
                var itemsToBeAdded = new List<TDbEntity>();
                var allItems = new List<TDbEntity>();
                var dbSet = GetVendorDbSet(dbContext);
                var allAccountdbEntities = dbSet.Where(db => db.AccountId == extAccount.Id).ToList();
                reportEntities.ForEach(reportEntity =>
                {
                    var correspondingDbEntity = allAccountdbEntities.FirstOrDefault(GetEntityMappingPredicate(reportEntity, extAccount));
                    if (correspondingDbEntity == null)
                    {
                        correspondingDbEntity = MapReportEntityToDbEntity(reportEntity, extAccount);
                        itemsToBeAdded.Add(correspondingDbEntity);
                    }
                    allItems.Add(correspondingDbEntity);
                });
                dbSet.AddRange(itemsToBeAdded);
                dbContext.SaveChanges();
                return allItems;
            }
        }

        public void LoadNewAccountSummaryMetricsDataForDate(List<TReportEntity> reportShippingEntities,
            List<TDbEntity> dbVendorEntities, DateTime date, ExtAccount account)
        {
            using (var dbContext = new ClientPortalProgContext())
            {
                dbContext.Configuration.AutoDetectChangesEnabled = false; // performance purposes (ToDo: Replace with bulk insert)
                var dbSet = GetSummaryMetricDbSet(dbContext);
                var summaryMetricsToAdd = reportShippingEntities.SelectMany(reportEntity =>
                {
                    var dbEntity = dbVendorEntities.FirstOrDefault(GetEntityMappingPredicate(reportEntity, account));
                    return GetSummaryMetricEntities(reportEntity, dbEntity, date);
                }).ToList();
                dbSet.AddRange(summaryMetricsToAdd);
                dbContext.SaveChanges();
                dbContext.Configuration.AutoDetectChangesEnabled = true;
            }
        }

        public void CleanExistingAccountSummaryMetricsDataForDate(DateTime date, ExtAccount extAccount)
        {
            using (var dbContext = new ClientPortalProgContext())
            {
                dbContext.Configuration.AutoDetectChangesEnabled = false; // performance purposes (ToDo: Replace with bulk delete)
                var entitiesDbSet = GetVendorDbSet(dbContext);
                var accountRelatedEntityIds = entitiesDbSet.Where(ent => ent.AccountId == extAccount.Id).Select(e => e.Id).ToList();
                var summaryMetricsDbSet = GetSummaryMetricDbSet(dbContext);
                var itemsToBeRemoved = summaryMetricsDbSet.Where(csm => csm.Date == date && accountRelatedEntityIds.Contains(csm.EntityId)).ToList();
                summaryMetricsDbSet.RemoveRange(itemsToBeRemoved);
                dbContext.SaveChanges();
                dbContext.Configuration.AutoDetectChangesEnabled = true;
            }
        }

        protected virtual Func<TDbEntity, bool> GetEntityMappingPredicate(TReportEntity reportEntity, ExtAccount extAccount)
        {
            return dbEntity => dbEntity.Name == reportEntity.Name && dbEntity.AccountId == extAccount.Id;
        }

        protected abstract TDbEntity MapReportEntityToDbEntity(TReportEntity reportEntity, ExtAccount extAccount);

        protected abstract DbSet<TDbEntity> GetVendorDbSet(ClientPortalProgContext dbContext);

        protected abstract DbSet<TSummaryMetricEntity> GetSummaryMetricDbSet(ClientPortalProgContext dbContext);

        private List<TSummaryMetricEntity> GetSummaryMetricEntities(TReportEntity reportEntity, TDbEntity dbEntity, DateTime date)
        {
            var metricEntities = new List<TSummaryMetricEntity>
            {
               InitMetricValue(dbEntity.Id, date, reportEntity.TotalCost,
                    metricTypes[DspMetricConstants.TotalCostMetricName]),
               InitMetricValue(dbEntity.Id, date, reportEntity.Impressions,
                    metricTypes[DspMetricConstants.ImpressionsMetricName]),
               InitMetricValue(dbEntity.Id, date, reportEntity.ClickThroughs,
                    metricTypes[DspMetricConstants.ClickThroughsMetricName]),
               InitMetricValue(dbEntity.Id, date, reportEntity.TotalPixelEvents,
                    metricTypes[DspMetricConstants.TotalPixelEventsMetricName]),
               InitMetricValue(dbEntity.Id, date, reportEntity.TotalPixelEventsViews,
                    metricTypes[DspMetricConstants.TotalPixelEventsViewsMetricName]),
               InitMetricValue(dbEntity.Id, date, reportEntity.TotalPixelEventsClicks,
                    metricTypes[DspMetricConstants.TotalPixelEventsClicksMetricName]),
               InitMetricValue(dbEntity.Id, date, reportEntity.DPV,
                    metricTypes[DspMetricConstants.DPVMetricName]),
               InitMetricValue(dbEntity.Id, date, reportEntity.ATC,
                    metricTypes[DspMetricConstants.ATCMetricName]),
               InitMetricValue(dbEntity.Id, date, reportEntity.Purchase,
                    metricTypes[DspMetricConstants.PurchaseMetricName]),
               InitMetricValue(dbEntity.Id, date, reportEntity.PurchaseViews,
                    metricTypes[DspMetricConstants.PurchaseViewsMetricName]),
               InitMetricValue(dbEntity.Id, date, reportEntity.PurchaseClicks,
                    metricTypes[DspMetricConstants.PurchaseClicksMetricName]),
            };
            metricEntities.RemoveAll(item => item == null);
            return metricEntities;
        }

        private TSummaryMetricEntity InitMetricValue(int entityId, DateTime date, decimal value, int metricTypeId)
        {
            if (value != 0)
            {
                return new TSummaryMetricEntity
                {
                    EntityId = entityId,
                    MetricTypeId = metricTypeId,
                    Date = date,
                    Value = value
                };
            }
            return null;
        }
    }
}
