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
    /// <summary>Creatives info data loader. Loaded creatices entities and metric values for creatives.</summary>
    internal class CreativesDataLoader : BaseDspItemLoader<ReportCreative, DspCreative, DspCreativeDailyMetricValues>
    {
        /// <summary>Gets the metric values database set.</summary>
        /// <param name="dbContext">The database context.</param>
        /// <returns>Dsp entites metric values dpbset</returns>
        protected override DbSet<DspCreativeDailyMetricValues> GetMetricValuesDbSet(ClientPortalProgContext dbContext)
        {
            return dbContext.DspCreativesMetricValues;
        }

        /// <summary>Gets the DSP entity database set.</summary>
        /// <param name="dbContext">The database context.</param>
        /// <returns>Dsp entities dbset</returns>
        protected override DbSet<DspCreative> GetDspEntityDbSet(ClientPortalProgContext dbContext)
        {
            return dbContext.DspCreatives;
        }

        /// <summary>Gets the unique report entities. Removes duplicated report entites.</summary>
        /// <param name="allReportEntities">All report entities.</param>
        /// <returns>Unique report entites</returns>
        protected override List<ReportCreative> GetUniqueReportEntities(List<ReportCreative> allReportEntities)
        {
          return allReportEntities.GroupBy(item => new { item.ReportId, item.LineItemReportId })
                .Select(gr => gr.First()).ToList();
        }

        /// <summary>Gets the entity mapping predicate.</summary>
        /// <param name="reportEntity">The report entity.</param>
        /// <param name="extAccount">The ext account.</param>
        /// <returns>Predicate function for finding corresponding db entity by report entity</returns>
        protected override Func<DspCreative, bool> GetEntityMappingPredicate(ReportCreative reportEntity, ExtAccount extAccount)
        {
            return (creative => creative.ReportId == reportEntity.ReportId && creative.LineItemReportId == reportEntity.LineItemReportId);
        }

        /// <summary>Maps the report entity to database entity.</summary>
        /// <param name="reportEntity">The report entity.</param>
        /// <param name="extAccount">The ext account.</param>
        /// <returns>Initialised db entity based on report entity.</returns>
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