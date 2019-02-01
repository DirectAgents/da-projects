using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using CakeExtracter.Etl.DSP.Models;
using DirectAgents.Domain.Contexts;
using DirectAgents.Domain.Entities.CPProg;
using DirectAgents.Domain.Entities.CPProg.DSP;
using DirectAgents.Domain.Entities.CPProg.DSP.SummaryMetrics;

namespace CakeExtracter.Etl.DSP.Loaders.ReportEntriesDataLoaders
{
    internal class CreativesDataLoader : BaseDspItemLoader<ReportCreative, DspCreative, DspCreativeDailyMetricValues>
    {
        protected override DbSet<DspCreativeDailyMetricValues> GetMetricValuesDbSet(ClientPortalProgContext dbContext)
        {
            return dbContext.DspCreativesMetricValues;
        }

        protected override DbSet<DspCreative> GetVendorDbSet(ClientPortalProgContext dbContext)
        {
            return dbContext.DspCreatives;
        }

        protected override List<ReportCreative> GetUniqueReportEntities(List<ReportCreative> allReportEntities)
        {
          return allReportEntities.GroupBy(item => new { item.ReportId, item.LineItemReportId })
                .Select(gr => gr.First()).ToList();
        }

        protected override Func<DspCreative, bool> GetEntityMappingPredicate(ReportCreative reportEntity, ExtAccount extAccount)
        {
            return (creative => creative.ReportId == reportEntity.ReportId && creative.LineItemReportId == reportEntity.LineItemReportId);
        }

        protected override DspCreative MapReportEntityToDbEntity(ReportCreative reportEntity, ExtAccount extAccount)
        {
            return new DspCreative
            {
                AccountId = extAccount.Id,
                Name = reportEntity.Name,
                ReportId = reportEntity.ReportId,
                AdvertiserName = reportEntity.Advertiser,
                AdvertiserReportId = reportEntity.AdvertiserReportId,
                OrderName = reportEntity.Order,
                OrderReportId = reportEntity.OrderReportId,
                LineItemName = reportEntity.LineItem,
                LineItemReportId = reportEntity.LineItemReportId
            };
        }
    }
}