using CakeExtractor.SeleniumApplication.Loaders.VCD.Constants;
using CakeExtractor.SeleniumApplication.SeleniumExtractors.VCD.Models;
using DirectAgents.Domain.Contexts;
using DirectAgents.Domain.Entities.CPProg;
using DirectAgents.Domain.Entities.CPProg.Vendor;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

namespace CakeExtractor.SeleniumApplication.Loaders.VCD.MetricTypesLoader
{
    internal abstract class BaseVendorItemLoader<TReportEntity, TDbEntity, TSummaryMetricEntity>
        where TReportEntity : ShippingItem
        where TDbEntity : BaseVendorEntity
        where TSummaryMetricEntity : SummaryMetric, new()
    {
        protected Dictionary<string, int> metricTypes;

        public BaseVendorItemLoader(Dictionary<string, int> metricTypes)
        {
            this.metricTypes = metricTypes;
        }

        public List<TDbEntity> EnsureVendorEntitiesInDataBase(List<TReportEntity> reportEntities, ExtAccount extAccount)
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
               InitMetricValue(dbEntity.Id, date, reportEntity.ShippedUnits,
                    metricTypes[VendorCentralDataLoadingConstants.ShippedUnitsMetricName]),
               InitMetricValue(dbEntity.Id, date, reportEntity.OrderedUnits,
                    metricTypes[VendorCentralDataLoadingConstants.OrderedUnitsMetricName]),
               InitMetricValue(dbEntity.Id, date, reportEntity.ShippedRevenue,
                    metricTypes[VendorCentralDataLoadingConstants.ShippedRevenueMetricName]),
               InitMetricValue(dbEntity.Id, date, reportEntity.CustomerReturns,
                    metricTypes[VendorCentralDataLoadingConstants.CustomerReturnsMetricName]),
               InitMetricValue(dbEntity.Id, date, reportEntity.FreeReplacements,
                    metricTypes[VendorCentralDataLoadingConstants.FreeReplacementMetricName]),
               InitMetricValue(dbEntity.Id, date, reportEntity.ShippedCogs,
                    metricTypes[VendorCentralDataLoadingConstants.ShippedCogsMetricName])
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
