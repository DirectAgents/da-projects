using System;
using CakeExtracter.Etl.DBM.Extractors.Parsers.ParsingConverters;
using CakeExtracter.Etl.DBM.Models;
using DirectAgents.Domain.Entities.CPProg.DBM.SummaryMetrics;

namespace CakeExtracter.Etl.DBM.Extractors
{
    internal class DbmCreativeExtractor: DbmApiExtractor<DbmCreativeSummary>
    {
        private readonly DbmReportDataConverter converter;
        private readonly DbmAccountReportData reportData;

        public DbmCreativeExtractor(DbmAccountReportData reportData) 
           // : base(dbmUtility, dateRange)
        {
            converter = new DbmReportDataConverter();
            this.reportData = reportData;
        }

        protected override void Extract()
        {
            try
            {
                var creativeSummaries = converter.ConvertCreativeReportDataToSummaries(reportData);
                Add(creativeSummaries);
            }
            catch (Exception e)
            {
                Logger.Error(e);
            }
            End();
        }
    }
}
