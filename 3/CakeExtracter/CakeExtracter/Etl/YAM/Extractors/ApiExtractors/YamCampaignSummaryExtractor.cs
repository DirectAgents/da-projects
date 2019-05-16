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
    public class YamCampaignSummaryExtractor : BaseYamApiExtractor<YamCampaignSummary>
    {
        public override string SummariesDisplayName => "YamCampaignSummaries";

        protected override Func<YamRow, object> GroupedRowsWithUniqueEntitiesFunction =>
            x => new {x.Date, x.CampaignName, x.CampaignId};

        public YamCampaignSummaryExtractor(ICsvExtractor<YamRow> csvExtractor, YamUtility yamUtility,
            DateRange dateRange, ExtAccount account, bool byPixelParameter)
            : base(csvExtractor, yamUtility, dateRange, account, byPixelParameter)
        {
        }

        protected override ReportSettings GetReportSettings()
        {
            var reportSettings = base.GetReportSettings();
            reportSettings.ByCampaign = true;
            return reportSettings;
        }
    }
}