using System;
using CakeExtracter.Common;
using CakeExtracter.Common.Extractors.CsvExtractors.Contracts;
using CakeExtracter.Etl.YAM.Extractors.CsvExtractors.RowModels;
using DirectAgents.Domain.Entities.CPProg;
using DirectAgents.Domain.Entities.CPProg.YAM.Summaries;
using Yahoo;

namespace CakeExtracter.Etl.YAM.Extractors.ApiExtractors
{
    public class YamDailySummaryExtractor : BaseYamApiExtractor<YamDailySummary>
    {
        public override string SummariesDisplayName => "YamDailySummaries";

        protected override Func<YamRow, object> GroupedRowsWithUniqueEntitiesFunction => 
            x => x.Date;

        public YamDailySummaryExtractor(ICsvExtractor<YamRow> csvExtractor, YamUtility yamUtility, DateRange dateRange,
            ExtAccount account, bool byPixelParameter)
            : base(csvExtractor, yamUtility, dateRange, account, byPixelParameter)
        {
        }
    }
}