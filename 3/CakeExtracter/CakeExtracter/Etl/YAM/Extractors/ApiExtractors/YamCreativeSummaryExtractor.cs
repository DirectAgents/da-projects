using System;
using CakeExtracter.Common;
using CakeExtracter.Etl.YAM.Extractors.CsvExtractors.RowModels;
using DirectAgents.Domain.Entities.CPProg;
using DirectAgents.Domain.Entities.CPProg.YAM.Summaries;
using Yahoo;
using Yahoo.Models;

namespace CakeExtracter.Etl.YAM.Extractors.ApiExtractors
{
    internal class YamCreativeSummaryExtractor : BaseYamApiExtractor<YamCreativeSummary>
    {
        protected override string SummariesDisplayName => "YamCreativeSummaries";

        protected override Func<YamRow, object> GroupedRowsWithUniqueEntitiesFunction =>
            x => new { x.Date, x.CreativeName, x.CreativeId };

        public YamCreativeSummaryExtractor(YamUtility yamUtility, DateRange dateRange, ExtAccount account, bool byPixelParameter)
            : base(yamUtility, dateRange, account, byPixelParameter)
        { }

        protected override ReportSettings GetReportSettings()
        {
            var reportSettings = base.GetReportSettings();
            reportSettings.ByCreative = true;
            return reportSettings;
        }
    }
}