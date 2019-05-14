﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using AutoMapper;
using CakeExtracter.Common;
using CakeExtracter.Common.JobExecutionManagement;
using CakeExtracter.Etl.YAM.Extractors.CsvExtractors;
using CakeExtracter.Etl.YAM.Extractors.CsvExtractors.RowModels;
using CakeExtracter.Helpers;
using DirectAgents.Domain.Entities.CPProg;
using DirectAgents.Domain.Entities.CPProg.YAM.Summaries;
using Yahoo;
using Yahoo.Exceptions;
using Yahoo.Models;

namespace CakeExtracter.Etl.YAM.Extractors.ApiExtractors
{
    internal abstract class BaseYamApiExtractor<T> : Extracter<T>
        where T: BaseYamSummary, new()
    {
        protected const string ConversionValuePixelQueryPattern = @"gv=(\d*\.?\d*)";

        protected readonly YamUtility YamUtility;
        protected readonly DateRange DateRange;
        protected readonly int YamAdvertiserId;

        private readonly bool byPixelParameter;
        private readonly int accountId;

        public event Action<FailedReportGenerationException> ProcessFailedExtraction;

        public abstract string SummariesDisplayName { get; }

        protected abstract Func<YamRow, object> GroupedRowsWithUniqueEntitiesFunction { get; }

        protected BaseYamApiExtractor(YamUtility yamUtility, DateRange dateRange, ExtAccount account, bool byPixelParameter)
        {
            YamUtility = yamUtility;
            DateRange = dateRange;
            YamAdvertiserId = int.Parse(account.ExternalId);
            this.byPixelParameter = byPixelParameter;
            accountId = account.Id;
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
            CommandExecutionContext.Current?.AppendJobExecutionStateInHistory($"{SummariesDisplayName} - Extraction", accountId);
            var startExtractionMessage = $"Extracting {SummariesDisplayName} from YAM API for ({accountId} - {YamAdvertiserId}) " +
                                         $"from {DateRange.FromDate:d} to {DateRange.ToDate:d}";
            Logger.Info(accountId, startExtractionMessage);
        }

        private void LogEndOfItemsExtracting()
        {
            CommandExecutionContext.Current?.AppendJobExecutionStateInHistory($"{SummariesDisplayName} - Extraction finished", accountId);
        }

        private IEnumerable<YamRow> ExtractItems()
        {
            var reportSettings = GetReportSettings();
            if (byPixelParameter)
            {
                reportSettings.ByPixelParameter = true;
            }

            var items = ExtractData(reportSettings, SummariesDisplayName);
            return items;
        }

        private IEnumerable<YamRow> ExtractData(ReportSettings reportSettings, string summariesName)
        {
            var reportUrl = YamUtility.TryGenerateReport(reportSettings);
            if (!string.IsNullOrWhiteSpace(reportUrl))
            {
                return ExtractRawDataFromCsv(reportUrl, summariesName);
            }

            Logger.Warn(accountId, $"The report URL is empty.");
            return new List<YamRow>();
        }

        private IEnumerable<YamRow> ExtractRawDataFromCsv(string reportUrl, string summariesName)
        {
            using (var streamReader = RequestHelper.CreateStreamReaderFromUrl(reportUrl))
            {
                var csvExtractor = new YamCsvExtractor(accountId, summariesName, streamReader);
                return csvExtractor.EnumerateRows();
            }
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
            if (byPixelParameter)
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