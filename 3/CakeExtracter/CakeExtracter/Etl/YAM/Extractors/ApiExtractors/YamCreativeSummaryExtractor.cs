using CakeExtracter.Common;
using CakeExtracter.Etl.TradingDesk.Extracters.SummaryCsvExtracters;
using DirectAgents.Domain.Entities.CPProg;
using Yahoo;

namespace CakeExtracter.Etl.YAM.Extractors.ApiExtractors
{
    public class YamCreativeSummaryExtractor : BaseYamApiExtractor<KeywordSummary>
    {
        public YamCreativeSummaryExtractor(YAMUtility yamUtility, DateRange dateRange, ExtAccount account)
            : base(yamUtility, dateRange, account)
        { }

        protected override void Extract()
        {
            var reportUrl = _yamUtility.TryGenerateReport(dateRange.FromDate, dateRange.ToDate, this.yamAdvertiserId,
                byCreative: true, byLine: true, byCampaign: true);

            if (!string.IsNullOrWhiteSpace(reportUrl))
            {
                var streamReader = TDDailySummaryExtracter.CreateStreamReaderFromUrl(reportUrl);
                var tdExtractor = new KeywordSummaryExtracter(columnMapping, streamReader);
                ExtractData(tdExtractor);
            }

            End();
        }
    }
}