using System.Configuration;
using CakeExtracter.Common;
using CakeExtracter.Etl.TradingDesk.Extracters.SummaryCsvExtracters;
using DirectAgents.Domain.Entities.CPProg;
using Yahoo;

namespace CakeExtracter.Etl.TradingDesk.Extracters
{
    public abstract class YAMApiExtracter<T> : Extracter<T>
    {
        protected readonly YAMUtility _yamUtility;
        protected readonly DateRange dateRange;
        protected readonly ColumnMapping columnMapping;
        protected readonly int yamAdvertiserId;

        public YAMApiExtracter(YAMUtility yamUtility, DateRange dateRange, ExtAccount account)
        {
            this._yamUtility = yamUtility;
            this.dateRange = dateRange;
            this.yamAdvertiserId = int.Parse(account.ExternalId);
            this.columnMapping = account.Platform.PlatColMapping;
            if (columnMapping != null)
            { //These override the colMappings b/c the API has different col headers than the UI-generated reports
                SetupColumnMapping();
            }
        }

        private void SetupColumnMapping()
        {
            columnMapping.PostClickConv = GetMappingPropertyName(columnMapping.PostClickConv, "YAMMap_PostClickConv");
            columnMapping.PostViewConv = GetMappingPropertyName(columnMapping.PostViewConv, "YAMMap_PostViewConv");
            columnMapping.PostClickRev = GetMappingPropertyName(columnMapping.PostClickRev, "YAMMap_PostClickRev");
            columnMapping.PostViewRev = GetMappingPropertyName(columnMapping.PostViewRev, "YAMMap_PostViewRev");
            columnMapping.StrategyName = GetMappingPropertyName(columnMapping.StrategyName, "YAMMap_StrategyName");
            columnMapping.StrategyEid = GetMappingPropertyName(columnMapping.StrategyEid, "YAMMap_StrategyEid");
            columnMapping.TDadName = GetMappingPropertyName(columnMapping.TDadName, "YAMMap_CreativeName");
            columnMapping.TDadEid = GetMappingPropertyName(columnMapping.TDadEid, "YAMMap_CreativeEid");
            columnMapping.AdSetName = GetMappingPropertyName(columnMapping.AdSetName, "YAMMap_AdSetName");
            columnMapping.AdSetEid = GetMappingPropertyName(columnMapping.AdSetEid, "YAMMap_AdSetEid");
            columnMapping.KeywordName = GetMappingPropertyName(columnMapping.KeywordName, "YAMMap_KeywordName");
            columnMapping.KeywordEid = GetMappingPropertyName(columnMapping.KeywordEid, "YAMMap_KeywordEid");
            columnMapping.SearchTermName = GetMappingPropertyName(columnMapping.SearchTermName, "YAMMap_SearchTermName");
        }

        private string GetMappingPropertyName(string sourcePropertyName, string configPropertyName)
        {
            var mapVal = ConfigurationManager.AppSettings[configPropertyName];
            return mapVal ?? sourcePropertyName;
        }
    }

    public class YAMDailySummaryExtracter : YAMApiExtracter<DailySummary>
    {
        public YAMDailySummaryExtracter(YAMUtility yamUtility, DateRange dateRange, ExtAccount account)
            : base(yamUtility, dateRange, account)
        { }

        protected override void Extract()
        {
            var payload = _yamUtility.CreateReportRequestPayload(dateRange.FromDate, dateRange.ToDate, this.yamAdvertiserId);
            var reportUrl = _yamUtility.GenerateReport(payload);

            if (!string.IsNullOrWhiteSpace(reportUrl))
            {
                var streamReader = TDDailySummaryExtracter.CreateStreamReaderFromUrl(reportUrl);
                var tdExtracter = new TDDailySummaryExtracter(this.columnMapping, streamReader: streamReader);
                var items = tdExtracter.EnumerateRows();
                Add(items);
            }
            End();
        }
    }

    //By Line
    public class YAMStrategySummaryExtracter : YAMApiExtracter<StrategySummary>
    {
        public YAMStrategySummaryExtracter(YAMUtility yamUtility, DateRange dateRange, ExtAccount account)
            : base(yamUtility, dateRange, account)
        { }

        protected override void Extract()
        {
            var payload = _yamUtility.CreateReportRequestPayload(dateRange.FromDate, dateRange.ToDate, this.yamAdvertiserId, byLine: true);
            var reportUrl = _yamUtility.GenerateReport(payload);

            if (!string.IsNullOrWhiteSpace(reportUrl))
            {
                var streamReader = TDDailySummaryExtracter.CreateStreamReaderFromUrl(reportUrl);
                var tdExtracter = new TDStrategySummaryExtracter(this.columnMapping, streamReader: streamReader);
                var items = tdExtracter.EnumerateRows();
                Add(items);
            }
            End();
        }
    }

    //By Campaign
    public class YAMAdSetSummaryExtracter : YAMApiExtracter<AdSetSummary>
    {
        public YAMAdSetSummaryExtracter(YAMUtility yamUtility, DateRange dateRange, ExtAccount account)
            : base(yamUtility, dateRange, account)
        { }

        protected override void Extract()
        {
            var payload = _yamUtility.CreateReportRequestPayload(dateRange.FromDate, dateRange.ToDate, this.yamAdvertiserId, byCampaign: true, byLine: true);
            var reportUrl = _yamUtility.GenerateReport(payload);

            if (!string.IsNullOrWhiteSpace(reportUrl))
            {
                var streamReader = TDDailySummaryExtracter.CreateStreamReaderFromUrl(reportUrl);
                var tdExtracter = new TDAdSetSummaryExtracter(this.columnMapping, streamReader: streamReader);
                var items = tdExtracter.EnumerateRows();
                Add(items);
            }
            End();
        }
    }

    //By Ad
    public class YAMTDadSummaryExtracter : YAMApiExtracter<TDadSummary>
    {
        public YAMTDadSummaryExtracter(YAMUtility yamUtility, DateRange dateRange, ExtAccount account)
            : base(yamUtility, dateRange, account)
        { }

        protected override void Extract()
        {
            var payload = _yamUtility.CreateReportRequestPayload(dateRange.FromDate, dateRange.ToDate, this.yamAdvertiserId, byCampaign: true, byAd: true);
            var reportUrl = _yamUtility.GenerateReport(payload);

            if (!string.IsNullOrWhiteSpace(reportUrl))
            {
                var streamReader = TDDailySummaryExtracter.CreateStreamReaderFromUrl(reportUrl);
                var tdExtracter = new TDadSummaryExtracter(this.columnMapping, streamReader: streamReader);
                var items = tdExtracter.EnumerateRows();
                Add(items);
            }
            End();
        }
    }

    //By Creative
    public class YAMKeywordSummaryExtracter : YAMApiExtracter<KeywordSummary>
    {
        public YAMKeywordSummaryExtracter(YAMUtility yamUtility, DateRange dateRange, ExtAccount account)
            : base(yamUtility, dateRange, account)
        { }

        protected override void Extract()
        {
            var payload = _yamUtility.CreateReportRequestPayload(dateRange.FromDate, dateRange.ToDate, this.yamAdvertiserId, byCreative: true, byLine: true, byCampaign: true);
            var reportUrl = _yamUtility.GenerateReport(payload);

            if (!string.IsNullOrWhiteSpace(reportUrl))
            {
                var streamReader = TDDailySummaryExtracter.CreateStreamReaderFromUrl(reportUrl);
                var tdExtracter = new KeywordSummaryExtracter(this.columnMapping, streamReader: streamReader);
                var items = tdExtracter.EnumerateRows();
                Add(items);
            }
            End();
        }
    }

    //By Pixel
    public class YAMSearchTermSummaryExtracter : YAMApiExtracter<SearchTermSummary>
    {
        public YAMSearchTermSummaryExtracter(YAMUtility yamUtility, DateRange dateRange, ExtAccount account)
            : base(yamUtility, dateRange, account)
        { }

        protected override void Extract()
        {
            var payload = _yamUtility.CreateReportRequestPayload(dateRange.FromDate, dateRange.ToDate, this.yamAdvertiserId, byPixel: true);
            var reportUrl = _yamUtility.GenerateReport(payload);

            if (!string.IsNullOrWhiteSpace(reportUrl))
            {
                var streamReader = TDDailySummaryExtracter.CreateStreamReaderFromUrl(reportUrl);
                var tdExtracter = new SearchTermSummaryExtracter(this.columnMapping, streamReader: streamReader);
                var items = tdExtracter.EnumerateRows();
                Add(items);
            }
            End();
        }
    }
}
