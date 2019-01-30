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

        public void UpdateAccountSummaryMetricsDataForDate(List<TReportEntity> reportShippingEntities,
            List<TDbEntity> dbVendorEntities, DateTime date, ExtAccount account)
        {
            using (var dbContext = new ClientPortalProgContext())
            {
                var accountRelatedEntityIds = dbVendorEntities.Select(e => e.Id).ToList();
                var dbSet = GetSummaryMetricDbSet(dbContext);
                var existingAccountDailySummaries = dbSet.Where(csm => csm.Date == date && accountRelatedEntityIds.Contains(csm.EntityId)).ToList();
                var actualAccountDailySummaries = GetActualDailySummariesFromReportEntities(reportShippingEntities, dbVendorEntities, account, date);
                MergeDailySummariesAndUpdateInDataBase(dbSet, dbContext, existingAccountDailySummaries, actualAccountDailySummaries);
            }
        }

        protected virtual Func<TDbEntity, bool> GetEntityMappingPredicate(TReportEntity reportEntity, ExtAccount extAccount)
        {
            return dbEntity => dbEntity.Name == reportEntity.Name && dbEntity.AccountId == extAccount.Id;
        }

        protected abstract TDbEntity MapReportEntityToDbEntity(TReportEntity reportEntity, ExtAccount extAccount);

        protected abstract DbSet<TDbEntity> GetVendorDbSet(ClientPortalProgContext dbContext);

        protected abstract DbSet<TSummaryMetricEntity> GetSummaryMetricDbSet(ClientPortalProgContext dbContext);

        private void MergeDailySummariesAndUpdateInDataBase(DbSet<TSummaryMetricEntity> dbSet, ClientPortalProgContext dbContext,
            List<TSummaryMetricEntity> existingAccountDailySummaries, List<TSummaryMetricEntity> actualAccountDailySummaries)
        {
            var dailySummariesToInsert = new List<TSummaryMetricEntity>();
            var dailySummariesToLeaveUntouched = new List<TSummaryMetricEntity>();
            actualAccountDailySummaries.ForEach(actualMetric =>
            {
                var alreadyExistingMetricValue = existingAccountDailySummaries.FirstOrDefault(mS => mS.EntityId == actualMetric.EntityId && mS.MetricTypeId == actualMetric.MetricTypeId);
                if (alreadyExistingMetricValue != null)
                {
                    if (alreadyExistingMetricValue.Value == actualMetric.Value)
                    {
                        dailySummariesToLeaveUntouched.Add(alreadyExistingMetricValue);
                    }
                    else
                    {
                        dailySummariesToInsert.Add(actualMetric);
                    }
                }
                else
                {
                    dailySummariesToInsert.Add(actualMetric);
                }
            });
            var dailySummariesToBeRemoved = existingAccountDailySummaries.Except(dailySummariesToLeaveUntouched).ToList();
            dbSet.RemoveRange(dailySummariesToBeRemoved);
            dbContext.SaveChanges();
            Logger.Info("Amazon VCD, Deleted {0}", dailySummariesToBeRemoved.Count);
            dbSet.AddRange(dailySummariesToInsert);
            dbContext.SaveChanges();
            Logger.Info("Amazon VCD, Inserted {0}", dailySummariesToInsert.Count);
        }

        private List<TSummaryMetricEntity> GetActualDailySummariesFromReportEntities(List<TReportEntity> reportShippingEntities, List<TDbEntity> dbVendorEntities, ExtAccount account, DateTime date)
        {
            var actualAccountDailySummaries = reportShippingEntities.SelectMany(reportEntity =>
            {
                var dbEntity = dbVendorEntities.FirstOrDefault(GetEntityMappingPredicate(reportEntity, account));
                return GetSummaryMetricEntities(reportEntity, dbEntity, date);
            }).ToList();
            return actualAccountDailySummaries;
        }

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
