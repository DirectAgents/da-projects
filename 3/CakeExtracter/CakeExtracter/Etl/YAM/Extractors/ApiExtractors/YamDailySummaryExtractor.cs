using System;
using System.Collections.Generic;
using System.Linq;
using CakeExtracter.Common;
using CakeExtracter.Etl.YAM.Extractors.CsvExtractors.RowModels;
using DirectAgents.Domain.Entities.CPProg;
using DirectAgents.Domain.Entities.CPProg.YAM.Summaries;
using Yahoo;

namespace CakeExtracter.Etl.YAM.Extractors.ApiExtractors
{
    internal class YamDailySummaryExtractor : BaseYamApiExtractor<YamDailySummary>
    {
        protected override string SummariesDisplayName => "YamDailySummaries";

        protected override Func<YamRow, object> GroupedRowsWithUniqueEntitiesFunction => 
            x => x.Date;

        public YamDailySummaryExtractor(YAMUtility yamUtility, DateRange dateRange, ExtAccount account, bool byPixelParameter)
            : base(yamUtility, dateRange, account, byPixelParameter)
        { }
    }
}