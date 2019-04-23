﻿using CakeExtracter.Common;
using CakeExtracter.Etl.TradingDesk.Extracters.SummaryCsvExtracters;
using DirectAgents.Domain.Entities.CPProg;
using Yahoo;

namespace CakeExtracter.Etl.YAM.Extractors.ApiExtractors
{
    public class YamBeaconSummaryExtractor : BaseYamApiExtractor<SearchTermSummary>
    {
        public YamBeaconSummaryExtractor(YAMUtility yamUtility, DateRange dateRange, ExtAccount account)
            : base(yamUtility, dateRange, account)
        { }

        protected override void Extract()
        {
            var reportUrl = _yamUtility.TryGenerateReport(dateRange.FromDate, dateRange.ToDate, this.yamAdvertiserId,
                byPixel: true);

            if (!string.IsNullOrWhiteSpace(reportUrl))
            {
                var streamReader = TDDailySummaryExtracter.CreateStreamReaderFromUrl(reportUrl);
                var tdExtractor = new SearchTermSummaryExtracter(columnMapping, streamReader);
                ExtractData(tdExtractor);
            }

            End();
        }
    }
}
