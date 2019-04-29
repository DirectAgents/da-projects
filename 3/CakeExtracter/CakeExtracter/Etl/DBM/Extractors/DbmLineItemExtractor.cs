using System;
using CakeExtracter.Etl.DBM.Extractors.Parsers.ParsingConverters;
using CakeExtracter.Etl.DBM.Models;
using DirectAgents.Domain.Entities.CPProg.DBM.SummaryMetrics;

namespace CakeExtracter.Etl.DBM.Extractors
{
    /// <summary>
    /// Extractor of DBM line item summaries
    /// </summary>
    internal class DbmLineItemExtractor : Extracter<DbmLineItemSummary>
    {
        private readonly DbmReportDataConverter converter;
        private readonly DbmAccountLineItemReportData reportData;

        public DbmLineItemExtractor(DbmAccountLineItemReportData reportData)
        {
            converter = new DbmReportDataConverter();
            this.reportData = reportData;
        }

        protected override void Extract()
        {
            try
            {
                var lineItemSummaries = converter.ConvertLineItemReportDataToSummaries(reportData);
                Add(lineItemSummaries);
            }
            catch (Exception e)
            {
                Logger.Error(e);
            }
            End();
        }
    }
}
