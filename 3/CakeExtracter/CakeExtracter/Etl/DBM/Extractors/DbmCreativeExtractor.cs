using System.Collections.Generic;
using CakeExtracter.Common;
using CakeExtracter.Etl.DBM.Extractors.Parser;
using CakeExtracter.Etl.DBM.Extractors.Parser.ParsingConverters;
using DBM.Parser.Models;
using DirectAgents.Domain.Entities.CPProg.DBM.SummaryMetrics;

namespace CakeExtracter.Etl.DBM.Extractors
{
    internal class DbmCreativeExtractor: Extracter<DbmCreativeSummary>
    {
        private readonly DbmReportCsvParser<DbmCreativeReportRow> _reportsParser;

        public DbmCreativeExtractor(DateRange dateRange)
        {
            var path = "Reports\\TEST_NEW_Creative.csv";

            var rowMap = new DbmCreativeReportEntityRowMap();
            _reportsParser = new DbmReportCsvParser<DbmCreativeReportRow>(dateRange, rowMap, path);
        }
        
        protected override void Extract()
        {
            var reportRows = _reportsParser.EnumerateRows();


            var items = new List<DbmCreativeSummary>();
            Add(items);
            End();
        }
    }
}
