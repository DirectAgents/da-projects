using System;
using CakeExtracter.Common;
using CakeExtracter.Common.Extractors.CsvExtractors.Contracts;
using CakeExtracter.Etl.YAM.Extractors.CsvExtractors.RowModels;
using DirectAgents.Domain.Entities.CPProg;
using DirectAgents.Domain.Entities.CPProg.YAM.Summaries;
using Yahoo;
using Yahoo.Models;

namespace CakeExtracter.Etl.YAM.Extractors.ApiExtractors
{
    public class YamAdSummaryExtractor : BaseYamApiExtractor<YamAdSummary>
    {
        public override string SummariesDisplayName => "YamAdSummaries";

        protected override Func<YamRow, object> GroupedRowsWithUniqueEntitiesFunction =>
            x => new { x.Date, x.CampaignName, x.CampaignId, x.LineName, x.LineId, x.CreativeName, x.CreativeId, x.AdName, x.AdId };

        public YamAdSummaryExtractor(ICsvExtractor<YamRow> csvExtractor, YamUtility yamUtility, DateRange dateRange,
            ExtAccount account, bool byPixelParameter)
            : base(csvExtractor, yamUtility, dateRange, account, byPixelParameter)
        {
        }

        protected override ReportSettings GetReportSettings()
        {
            var reportSettings = base.GetReportSettings();
            reportSettings.ByCampaign = true;
            reportSettings.ByLine = true;
            reportSettings.ByCreative = true;
            reportSettings.ByAd = true;
            return reportSettings;
        }
    }
}