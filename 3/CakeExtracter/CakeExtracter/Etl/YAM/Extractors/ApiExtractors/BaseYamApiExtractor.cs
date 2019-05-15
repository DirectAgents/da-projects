using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using AutoMapper;
using CakeExtracter.Common;
using CakeExtracter.Common.Extractors.CsvExtractors.Contracts;
using CakeExtracter.Common.JobExecutionManagement;
using CakeExtracter.Etl.YAM.Extractors.CsvExtractors.RowModels;
using CakeExtracter.Helpers;
using DirectAgents.Domain.Entities.CPProg;
using DirectAgents.Domain.Entities.CPProg.YAM.Summaries;
using Yahoo;
using Yahoo.Exceptions;
using Yahoo.Models;

namespace CakeExtracter.Etl.YAM.Extractors.ApiExtractors
{
    public abstract class BaseYamApiExtractor<T> : Extracter<T>
        where T: BaseYamSummary, new()
    {
        protected const string ConversionValuePixelQueryPattern = @"gv=(\d*\.?\d*)";

        protected readonly ICsvExtractor<YamRow> CsvExtractor;
        protected readonly YamUtility YamUtility;
        protected readonly DateRange DateRange;
        protected readonly int YamAdvertiserId;
        protected readonly bool ByPixelParameter;
        protected readonly int AccountId;

        public event Action<FailedReportGenerationException> ProcessFailedExtraction;

        public virtual string SummariesDisplayName { get; } = "Yam Summaries";

        protected abstract Func<YamRow, object> GroupedRowsWithUniqueEntitiesFunction { get; }

        protected BaseYamApiExtractor(ICsvExtractor<YamRow> csvExtractor, YamUtility yamUtility, DateRange dateRange,
            ExtAccount account, bool byPixelParameter)
        {
            CsvExtractor = csvExtractor;
            CsvExtractor.ItemsName = SummariesDisplayName;
            YamUtility = yamUtility;
            DateRange = dateRange;
            YamAdvertiserId = int.Parse(account.ExternalId);
            ByPixelParameter = byPixelParameter;
            AccountId = account.Id;
            ProcessFailedExtraction += exception => Logger.Error(account.Id, exception);
        }

        public IEnumerable<T> GetItemsFromApi()
        {
            try
            {
                var items = ExtractItems();
                var summaries = TransformItems(items);
                return summaries;
            }
            catch (Exception exc)
            {
                var sourceReportSettings = GetReportSettings();
                var exception = new FailedReportGenerationException(sourceReportSettings, exc);
                ProcessFailedExtraction?.Invoke(exception);
                return new List<T>();
            }
        }

        protected virtual ReportSettings GetReportSettings()
        {
            return new ReportSettings
            {
                FromDate = DateRange.FromDate,
                ToDate = DateRange.ToDate,
                AccountId = YamAdvertiserId
            };
        }

        protected override void Extract()
        {
            LogStartOfItemsExtracting();
            var summaries = GetItemsFromApi();
            Add(summaries);
            End();
            LogEndOfItemsExtracting();
        }

        private void LogStartOfItemsExtracting()
        {
            CommandExecutionContext.Current?.AppendJobExecutionStateInHistory($"{SummariesDisplayName} - Extraction", AccountId);
            var startExtractionMessage = $"Extracting {SummariesDisplayName} from YAM API for ({AccountId} - {YamAdvertiserId}) " +
                                         $"from {DateRange.FromDate:d} to {DateRange.ToDate:d}";
            Logger.Info(AccountId, startExtractionMessage);
        }

        private void LogEndOfItemsExtracting()
        {
            CommandExecutionContext.Current?.AppendJobExecutionStateInHistory($"{SummariesDisplayName} - Extraction finished", AccountId);
        }

        private IEnumerable<YamRow> ExtractItems()
        {
            var reportSettings = GetReportSettings();
            if (ByPixelParameter)
            {
                reportSettings.ByPixelParameter = true;
            }

            var items = ExtractData(reportSettings);
            return items;
        }

        private IEnumerable<YamRow> ExtractData(ReportSettings reportSettings)
        {
            var reportUrl = YamUtility.TryGenerateReport(reportSettings);
            if (!string.IsNullOrWhiteSpace(reportUrl))
            {
                return CsvExtractor.EnumerateRows(reportUrl);
            }

            Logger.Warn(AccountId, "The report URL is empty.");
            return new List<YamRow>();
        }

        private IEnumerable<T> TransformItems(IEnumerable<YamRow> items)
        {
            var sums = items.GroupBy(GroupedRowsWithUniqueEntitiesFunction).Select(CreateSummary);
            var notEmptySums = sums.Where(x => !x.IsEmpty()).ToList();
            return notEmptySums;
        }

        private T CreateSummary(IEnumerable<YamRow> items)
        {
            var sums = items.Select(Mapper.Map<T>).ToList();
            var sum = sums.FirstOrDefault();
            sum.SetBaseStats(sums);
            if (ByPixelParameter)
            {
                SetStatsByPixelQuery(sum, items);
            }

            return sum;
        }

        private void SetStatsByPixelQuery(T sum, IEnumerable<YamRow> items)
        {
            sum.ClickConversionValueByPixelQuery = items.Where(x => x.ClickThroughConversion > 0)
                .Sum(x => GetConversionValueFromPixelQuery(x.PixelParameter));
            sum.ViewConversionValueByPixelQuery = items.Where(x => x.ViewThroughConversion > 0)
                .Sum(x => GetConversionValueFromPixelQuery(x.PixelParameter));
        }

        private decimal GetConversionValueFromPixelQuery(string pixelParameter)
        {
            if (pixelParameter == null)
            {
                return 0;
            }

            var match = Regex.Match(pixelParameter, ConversionValuePixelQueryPattern);
            if (!match.Success)
            {
                return 0;
            }

            return decimal.TryParse(match.Groups[1].Value, out var conVal) ? conVal : 0;
        }
    }
}