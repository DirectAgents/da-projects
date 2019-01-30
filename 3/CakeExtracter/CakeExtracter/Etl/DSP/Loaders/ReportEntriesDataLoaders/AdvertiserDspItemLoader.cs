using CakeExtracter.Etl.DSP.Models;
using DirectAgents.Domain.Contexts;
using DirectAgents.Domain.Entities.CPProg;
using DirectAgents.Domain.Entities.CPProg.DSP;
using DirectAgents.Domain.Entities.CPProg.DSP.SummaryMetrics;
using System;
using System.Collections.Generic;
using System.Data.Entity;

namespace CakeExtracter.Etl.DSP.Loaders.ReportEntriesDataLoaders
{
    internal class AdvertiserDspItemLoader : BaseDspItemLoader<ReportAdvertiser, DspAdvertiser, DspAdvertiserSummaryMetric>
    {
        public AdvertiserDspItemLoader(Dictionary<string, int> metricTypes) : base(metricTypes)
        {
        }

        protected override DbSet<DspAdvertiserSummaryMetric> GetSummaryMetricDbSet(ClientPortalProgContext dbContext)
        {
            return dbContext.DspAdvertisersSummaryMetrics;
        }

        protected override DbSet<DspAdvertiser> GetVendorDbSet(ClientPortalProgContext dbContext)
        {
            return dbContext.DspAdvertisers;
        }

        protected override DspAdvertiser MapReportEntityToDbEntity(ReportAdvertiser reportEntity, ExtAccount extAccount)
        {
            throw new NotImplementedException();
        }
    }
}
