using System;
using CakeExtracter.Common;
using CakeExtracter.Etl.YAM.Extractors.CsvExtractors.RowModels;
using DirectAgents.Domain.Entities.CPProg;
using DirectAgents.Domain.Entities.CPProg.YAM.Summaries;
using Yahoo;
using Yahoo.Models;

namespace CakeExtracter.Etl.YAM.Extractors.ApiExtractors
{
    internal class YamPixelSummaryExtractor : BaseYamApiExtractor<YamPixelSummary>
    {
        protected override string SummariesDisplayName => "YamPixelSummaries";

        protected override Func<YamRow, object> GroupedRowsWithUniqueEntitiesFunction =>
            x => new { x.Date, x.PixelName, x.PixelId };

        public YamPixelSummaryExtractor(YamUtility yamUtility, DateRange dateRange, ExtAccount account, bool byPixelParameter)
            : base(yamUtility, dateRange, account, byPixelParameter)
        { }

        protected override ReportSettings GetReportSettings()
        {
            var reportSettings = base.GetReportSettings();
            reportSettings.ByPixel = true;
            return reportSettings;
        }
    }
}
