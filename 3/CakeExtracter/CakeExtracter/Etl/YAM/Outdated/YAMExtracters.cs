using System;
using System.Configuration;
using CakeExtracter.Common;
using CakeExtracter.Etl.TradingDesk.Extracters.SummaryCsvExtracters;
using CakeExtracter.Helpers;
using CsvHelper;
using DirectAgents.Domain.Entities.CPProg;
using Yahoo;
using Yahoo.Models;

namespace CakeExtracter.Etl.YAM.Outdated
{
    [Obsolete]
    public abstract class YAMApiExtracter<T> : Extracter<T>
    {
        protected readonly YAMUtility _yamUtility;
        protected readonly DateRange dateRange;
        protected readonly ColumnMapping columnMapping;
        protected readonly int yamAdvertiserId;

        private const string ErrorMessageIfReportIsEmpty = "No header record was found.";

        [Obsolete]
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

        protected void ExtractData(SummaryCsvExtracter<T> extractor)
        {
            try
            {
                var items = extractor.EnumerateRows();
                Add(items);
            }
            catch (Exception exception)
            {
                if (exception is CsvHelperException && exception.Message == ErrorMessageIfReportIsEmpty)
                {
                    Logger.Warn($"There are no statistics in the report: {exception.Message}");
                }
                else
                {
                    Logger.Error(exception);
                }
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

    [Obsolete]
    public class YAMDailySummaryExtracter : YAMApiExtracter<DailySummary>
    {
        [Obsolete]
        public YAMDailySummaryExtracter(YAMUtility yamUtility, DateRange dateRange, ExtAccount account)
            : base(yamUtility, dateRange, account)
        { }

        protected override void Extract()
        {
            var reportUrl = _yamUtility.TryGenerateReport(new ReportSettings
            {
                FromDate = dateRange.FromDate,
                ToDate = dateRange.ToDate,
                AccountId = this.yamAdvertiserId,
                IsOutdated = true
            });
            if (!string.IsNullOrWhiteSpace(reportUrl))
            {
                var streamReader = RequestHelper.CreateStreamReaderFromUrl(reportUrl);
                var tdExtractor = new TDDailySummaryExtracter(columnMapping, streamReader);
                ExtractData(tdExtractor);
            }
            End();
        }
    }

    //By Line
    [Obsolete]
    public class YAMStrategySummaryExtracter : YAMApiExtracter<StrategySummary>
    {
        [Obsolete]
        public YAMStrategySummaryExtracter(YAMUtility yamUtility, DateRange dateRange, ExtAccount account)
            : base(yamUtility, dateRange, account)
        { }

        protected override void Extract()
        {
            var reportUrl = _yamUtility.TryGenerateReport(new ReportSettings
            {
                FromDate = dateRange.FromDate,
                ToDate = dateRange.ToDate,
                AccountId = this.yamAdvertiserId,
                ByLine = true,
                IsOutdated = true
            });

            if (!string.IsNullOrWhiteSpace(reportUrl))
            {
                var streamReader = RequestHelper.CreateStreamReaderFromUrl(reportUrl);
                var tdExtractor = new TDStrategySummaryExtracter(columnMapping, streamReader);
                ExtractData(tdExtractor);
            }

            End();
        }
    }

    //By Campaign
    [Obsolete]
    public class YAMAdSetSummaryExtracter : YAMApiExtracter<AdSetSummary>
    {
        [Obsolete]
        public YAMAdSetSummaryExtracter(YAMUtility yamUtility, DateRange dateRange, ExtAccount account)
            : base(yamUtility, dateRange, account)
        { }

        protected override void Extract()
        {
            var reportUrl = _yamUtility.TryGenerateReport(new ReportSettings
            {
                FromDate = dateRange.FromDate,
                ToDate = dateRange.ToDate,
                AccountId = this.yamAdvertiserId,
                ByCampaign = true,
                ByLine = true,
                IsOutdated = true
            });

            if (!string.IsNullOrWhiteSpace(reportUrl))
            {
                var streamReader = RequestHelper.CreateStreamReaderFromUrl(reportUrl);
                var tdExtractor = new TDAdSetSummaryExtracter(columnMapping, streamReader);
                ExtractData(tdExtractor);
            }

            End();
        }
    }

    //By Ad
    [Obsolete]
    public class YAMTDadSummaryExtracter : YAMApiExtracter<TDadSummary>
    {
        [Obsolete]
        public YAMTDadSummaryExtracter(YAMUtility yamUtility, DateRange dateRange, ExtAccount account)
            : base(yamUtility, dateRange, account)
        { }

        protected override void Extract()
        {
            var reportUrl = _yamUtility.TryGenerateReport(new ReportSettings
            {
                FromDate = dateRange.FromDate,
                ToDate = dateRange.ToDate,
                AccountId = this.yamAdvertiserId,
                ByCampaign = true,
                ByAd = true,
                IsOutdated = true
            });

            if (!string.IsNullOrWhiteSpace(reportUrl))
            {
                var streamReader = RequestHelper.CreateStreamReaderFromUrl(reportUrl);
                var tdExtractor = new TDadSummaryExtracter(columnMapping, streamReader);
                ExtractData(tdExtractor);
            }

            End();
        }
    }

    //By Creative
    [Obsolete]
    public class YAMKeywordSummaryExtracter : YAMApiExtracter<KeywordSummary>
    {
        [Obsolete]
        public YAMKeywordSummaryExtracter(YAMUtility yamUtility, DateRange dateRange, ExtAccount account)
            : base(yamUtility, dateRange, account)
        { }

        protected override void Extract()
        {
            var reportUrl = _yamUtility.TryGenerateReport(new ReportSettings
            {
                FromDate = dateRange.FromDate,
                ToDate = dateRange.ToDate,
                AccountId = this.yamAdvertiserId,
                ByCampaign = true,
                ByCreative = true,
                ByLine = true,
                IsOutdated = true
            });

            if (!string.IsNullOrWhiteSpace(reportUrl))
            {
                var streamReader = RequestHelper.CreateStreamReaderFromUrl(reportUrl);
                var tdExtractor = new KeywordSummaryExtracter(columnMapping, streamReader);
                ExtractData(tdExtractor);
            }

            End();
        }
    }

    //By Pixel
    [Obsolete]
    public class YAMSearchTermSummaryExtracter : YAMApiExtracter<SearchTermSummary>
    {
        [Obsolete]
        public YAMSearchTermSummaryExtracter(YAMUtility yamUtility, DateRange dateRange, ExtAccount account)
            : base(yamUtility, dateRange, account)
        { }

        protected override void Extract()
        {
            var reportUrl = _yamUtility.TryGenerateReport(new ReportSettings
            {
                FromDate = dateRange.FromDate,
                ToDate = dateRange.ToDate,
                AccountId = this.yamAdvertiserId,
                ByPixel = true,
                IsOutdated = true
            });

            if (!string.IsNullOrWhiteSpace(reportUrl))
            {
                var streamReader = RequestHelper.CreateStreamReaderFromUrl(reportUrl);
                var tdExtractor = new SearchTermSummaryExtracter(columnMapping, streamReader);
                ExtractData(tdExtractor);
            }

            End();
        }
    }
}
