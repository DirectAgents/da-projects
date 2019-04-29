using System;
using CakeExtracter.Etl.DBM.Extractors.Parsers.ParsingConverters;
using CakeExtracter.Etl.DBM.Models;
using DirectAgents.Domain.Entities.CPProg.DBM.SummaryMetrics;

namespace CakeExtracter.Etl.DBM.Extractors
{
    /// <summary>
    /// Extractor of DBM creative summaries
    /// </summary>
    internal class DbmCreativeExtractor: Extracter<DbmCreativeSummary>
    {
        private readonly DbmReportDataConverter converter;
        private readonly DbmAccountCreativeReportData reportData;

        public DbmCreativeExtractor(DbmAccountCreativeReportData reportData) 
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
