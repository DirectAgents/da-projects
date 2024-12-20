﻿using System;
using CakeExtracter.Common;
using CakeExtracter.Common.Extractors.CsvExtractors.Contracts;
using CakeExtracter.Etl.YAM.Extractors.CsvExtractors.RowModels;
using DirectAgents.Domain.Entities.CPProg;
using DirectAgents.Domain.Entities.CPProg.YAM.Summaries;
using Yahoo;
using Yahoo.Models;

namespace CakeExtracter.Etl.YAM.Extractors.ApiExtractors
{
    public class YamPixelSummaryExtractor : BaseYamApiExtractor<YamPixelSummary>
    {
        public override string SummariesDisplayName => "YamPixelSummaries";

        protected override Func<YamRow, object> GroupedRowsWithUniqueEntitiesFunction =>
            x => new { x.Date, x.PixelName, x.PixelId };

        public YamPixelSummaryExtractor(ICsvExtractor<YamRow> csvExtractor, YamUtility yamUtility, DateRange dateRange,
            ExtAccount account, bool byPixelParameter)
            : base(csvExtractor, yamUtility, dateRange, account, byPixelParameter)
        {
        }

        protected override ReportSettings GetReportSettings()
        {
            var reportSettings = base.GetReportSettings();
            reportSettings.ByPixel = true;
            return reportSettings;
        }
    }
}
