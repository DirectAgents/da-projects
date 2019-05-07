using System;
using CakeExtracter.Common;
using CakeExtracter.Etl.YAM.Extractors.CsvExtractors.RowModels;
using DirectAgents.Domain.Entities.CPProg;
using DirectAgents.Domain.Entities.CPProg.YAM.Summaries;
using Yahoo;

namespace CakeExtracter.Etl.YAM.Extractors.ApiExtractors
{
    internal class YamDailySummaryExtractor : BaseYamApiExtractor<YamDailySummary>
    {
        public override string SummariesDisplayName => "YamDailySummaries";

        protected override Func<YamRow, object> GroupedRowsWithUniqueEntitiesFunction => 
            x => x.Date;

        public YamDailySummaryExtractor(YamUtility yamUtility, DateRange dateRange, ExtAccount account, bool byPixelParameter)
            : base(yamUtility, dateRange, account, byPixelParameter)
        { }
    }
}