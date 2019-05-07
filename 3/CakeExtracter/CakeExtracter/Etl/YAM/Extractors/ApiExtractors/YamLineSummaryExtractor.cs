using System;
using CakeExtracter.Common;
using CakeExtracter.Etl.YAM.Extractors.CsvExtractors.RowModels;
using DirectAgents.Domain.Entities.CPProg;
using DirectAgents.Domain.Entities.CPProg.YAM.Summaries;
using Yahoo;
using Yahoo.Models;

namespace CakeExtracter.Etl.YAM.Extractors.ApiExtractors
{
    internal class YamLineSummaryExtractor : BaseYamApiExtractor<YamLineSummary>
    {
        public override string SummariesDisplayName => "YamLineSummaries";

        protected override Func<YamRow, object> GroupedRowsWithUniqueEntitiesFunction =>
            x => new { x.Date, x.CampaignName, x.CampaignId, x.LineName, x.LineId };

        public YamLineSummaryExtractor(YamUtility yamUtility, DateRange dateRange, ExtAccount account, bool byPixelParameter)
            : base(yamUtility, dateRange, account, byPixelParameter)
        { }

        protected override ReportSettings GetReportSettings()
        {
            var reportSettings = base.GetReportSettings();
            reportSettings.ByCampaign = true;
            reportSettings.ByLine = true;
            return reportSettings;
        }
    }
}