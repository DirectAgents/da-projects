using CakeExtracter.Common;
using CakeExtracter.Etl.TradingDesk.Extracters.SummaryCsvExtracters;
using DirectAgents.Domain.Entities.CPProg;
using Yahoo;

namespace CakeExtracter.Etl.YAM.Extractors.ApiExtractors
{
    public class YamCampaignSummaryExtractor : BaseYamApiExtractor<AdSetSummary>
    {
        public YamCampaignSummaryExtractor(YAMUtility yamUtility, DateRange dateRange, ExtAccount account)
            : base(yamUtility, dateRange, account)
        { }

        protected override void Extract()
        {
            var reportUrl = _yamUtility.TryGenerateReport(dateRange.FromDate, dateRange.ToDate, this.yamAdvertiserId,
                byCampaign: true, byLine: true);

            if (!string.IsNullOrWhiteSpace(reportUrl))
            {
                var streamReader = TDDailySummaryExtracter.CreateStreamReaderFromUrl(reportUrl);
                var tdExtractor = new TDAdSetSummaryExtracter(columnMapping, streamReader);
                ExtractData(tdExtractor);
            }

            End();
        }
    }
}