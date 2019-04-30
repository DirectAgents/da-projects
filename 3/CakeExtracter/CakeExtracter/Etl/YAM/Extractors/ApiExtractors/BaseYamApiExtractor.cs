using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using AutoMapper;
using CakeExtracter.Common;
using CakeExtracter.Etl.YAM.Extractors.CsvExtractors;
using CakeExtracter.Etl.YAM.Extractors.CsvExtractors.RowMaps;
using CakeExtracter.Etl.YAM.Extractors.CsvExtractors.RowModels;
using CakeExtracter.Helpers;
using DirectAgents.Domain.Entities.CPProg;
using DirectAgents.Domain.Entities.CPProg.YAM.Summaries;
using Yahoo;
using Yahoo.Models;

namespace CakeExtracter.Etl.YAM.Extractors.ApiExtractors
{
    internal abstract class BaseYamApiExtractor<T> : Extracter<T>
        where T: BaseYamSummary, new()
    {
        protected const string ConversionValuePixelQueryPattern = @"gv=(\d*\.?\d*)";

        protected readonly YAMUtility YamUtility;
        protected readonly DateRange DateRange;
        protected readonly int YamAdvertiserId;

        private readonly bool byPixelParameter;
        private readonly int accountId;

        protected abstract string SummariesDisplayName { get; }

        protected abstract Func<YamRow, object> GroupedRowsWithUniqueEntitiesFunction { get; }

        protected BaseYamApiExtractor(YAMUtility yamUtility, DateRange dateRange, ExtAccount account, bool byPixelParameter)
        {
            YamUtility = yamUtility;
            DateRange = dateRange;
            YamAdvertiserId = int.Parse(account.ExternalId);
            this.byPixelParameter = byPixelParameter;
            accountId = account.Id;
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
            var items = ExtractItems();
            var summaries = TransformItems(items);
            Add(summaries);
            End();
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

            Logger.Error(accountId, new Exception($"The report URL is empty."));
            return new List<YamRow>();
        }

        private IEnumerable<YamRow> ExtractRawDataFromCsv(string reportUrl, string summariesName)
        {
            using (var streamReader = RequestHelper.CreateStreamReaderFromUrl(reportUrl))
            {
                var csvExtractor = new YamCsvExtractor<YamRow, YamRowMap>(accountId, summariesName, streamReader);
                return csvExtractor.EnumerateRows();
            }
        }

        private IEnumerable<T> TransformItems(IEnumerable<YamRow> items)
        {
            var sums = items.GroupBy(GroupedRowsWithUniqueEntitiesFunction).Select(CreateSummary);
            return sums.ToList();
        }

        private T CreateSummary(IEnumerable<YamRow> items)
        {
            var sums = items.Select(Mapper.Map<T>).ToList();
            var sum = sums.FirstOrDefault();
            sum.SetStats(sums);
            SetStatsByPixelQuery(sum, items);
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