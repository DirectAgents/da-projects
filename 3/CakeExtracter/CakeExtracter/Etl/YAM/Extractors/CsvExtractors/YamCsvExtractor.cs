using System;
using System.Collections.Generic;
using System.Configuration;
using CakeExtracter.Common.Extractors.CsvExtractors;
using CakeExtracter.Etl.YAM.Extractors.CsvExtractors.RowMaps;
using CakeExtracter.Etl.YAM.Extractors.CsvExtractors.RowModels;
using CsvHelper;
using DirectAgents.Domain.Entities.CPProg;
using Polly;

namespace CakeExtracter.Etl.YAM.Extractors.CsvExtractors
{
    /// <inheritdoc />
    /// Yam csv extractor for separated accounts.
    public class YamCsvExtractor : CsvAccountExtractor<YamRow, YamRowMap>
    {
        private const string ErrorMessageIfReportIsEmpty = "No header record was found.";
        private const int NumTriesReportParsingDefault = 3;

        private int NumTriesReportParsing { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="YamCsvExtractor"/> class.
        /// </summary>
        /// <param name="account">Target account.</param>
        public YamCsvExtractor(ExtAccount account)
            : base(account)
        {
            NumTriesReportParsing = int.TryParse(ConfigurationManager.AppSettings["YAM_NumTries_ReportParsing"], out var tmpInt)
                ? tmpInt
                : NumTriesReportParsingDefault;
            ReadingExceptionCallback += exception =>
            {
                if (exception is CsvHelperException && exception.Message == ErrorMessageIfReportIsEmpty)
                {
                    Logger.Warn(AccountId, $"There are no statistics in the report: {exception.Message}");
                }
                else
                {
                    throw exception;
                }
            };
        }

        /// <inheritdoc />
        public override List<YamRow> EnumerateRows(string url)
        {
            var rows = Policy<List<YamRow>>
                .Handle<Exception>()
                .Retry(NumTriesReportParsing, (response, retryNumber, context) =>
                    Logger.Warn(
                        AccountId,
                        $"Try to parse the report again by URL {url}: {response.Exception.Message} (attempt - {retryNumber})"))
                .Execute(() => base.EnumerateRows(url));
            return rows;
        }

        /// <inheritdoc />
        protected override bool ShouldSkipRecord(string[] fields)
        {
            // if the first column is “Totals” the current record is a record with information and should be skipped.
            return fields[0] == "Totals";
        }
    }
}