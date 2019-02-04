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
    /// <summary>Line itemes entities and metrics data loader.</summary>
    internal class LineItemsDataLoader : BaseDspItemLoader<ReportLineItem, DspLineItem, DspLineDailyMetricValues>
    {
        protected override DbSet<DspLineDailyMetricValues> GetMetricValuesDbSet(ClientPortalProgContext dbContext)
        {
            return dbContext.DspLineItemsMetricValues;
        }

        /// <summary>Gets the DSP entity database set.</summary>
        /// <param name="dbContext">The database context.</param>
        /// <returns>Dsp entities dbset</returns>
        protected override DbSet<DspLineItem> GetDspEntityDbSet(ClientPortalProgContext dbContext)
        {
            return dbContext.DspLineItems;
        }

        /// <summary>Gets the unique report entities. Removes duplicated report entites.</summary>
        /// <param name="allReportEntities">All report entities.</param>
        /// <returns>Unique report entites</returns>
        protected override List<ReportLineItem> GetUniqueReportEntities(List<ReportLineItem> allReportEntities)
        {
            return allReportEntities.GroupBy(item => new { item.ReportId, item.OrderReportId })
                  .Select(gr => gr.First()).ToList();
        }

        /// <summary>Gets the entity mapping predicate.</summary>
        /// <param name="reportEntity">The report entity.</param>
        /// <param name="extAccount">The ext account.</param>
        /// <returns>Predicate function for finding corresponding db entity by report entity</returns>
        protected override Func<DspLineItem, bool> GetEntityMappingPredicate(ReportLineItem reportEntity, ExtAccount extAccount)
        {
            return (creative => creative.ReportId == reportEntity.ReportId && creative.OrderReportId == reportEntity.OrderReportId);
        }

        /// <summary>Maps the report entity to database entity.</summary>
        /// <param name="reportEntity">The report entity.</param>
        /// <param name="extAccount">The ext account.</param>
        /// <returns>Initialised db entity based on report entity.</returns>
        protected override DspLineItem MapReportEntityToDbEntity(ReportLineItem reportEntity, ExtAccount extAccount)
        {
            return new DspLineItem
            {
                AccountId = extAccount.Id,
                Name = reportEntity.Name,
                ReportId = reportEntity.ReportId,
                AdvertiserName = reportEntity.Advertiser,
                AdvertiserReportId = reportEntity.AdvertiserReportId,
                OrderName = reportEntity.Order,
                OrderReportId = reportEntity.OrderReportId
            };
        }
    }
}
