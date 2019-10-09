using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using CakeExtracter.Etl.SearchMarketing.Extracters.BingExtractors.Models;
using CsvHelper;
using CsvHelper.Configuration;

namespace CakeExtracter.Etl.SearchMarketing.Extracters.BingExtractors.Parsers
{
    /// <summary>
    /// Parser for CSV Bing reports.
    /// </summary>
    /// <typeparam name="T">Bing base row.</typeparam>
    public class BingReportCsvParser<T>
        where T : BingBaseRow
    {
        private const string FirstColumnHeaderName = "TimePeriod";

        private readonly string csvFilePath;
        private readonly CsvClassMap csvClassMap;

        /// <summary>
        /// Initializes a new instance of the <see cref="BingReportCsvParser{T}"/> class.
        /// </summary>
        /// <param name="csvClassMap">Row map rules.</param>
        /// <param name="csvFilePath">Path to report file.</param>
        public BingReportCsvParser(CsvClassMap csvClassMap, string csvFilePath)
        {
            this.csvClassMap = csvClassMap;
            this.csvFilePath = csvFilePath;
        }

        /// <summary>
        /// Gets report rows are parsed by CSV reader.
        /// </summary>
        /// <returns>Report rows.</returns>
        public IEnumerable<T> EnumerateRows()
        {
            Logger.Info($"Extracting rows from {csvFilePath}");
            if (!File.Exists(csvFilePath))
            {
                return new List<T>();
            }
            using (var reader = File.OpenText(csvFilePath))
            {
                return EnumerateRowsInner(reader);
            }
        }

        /// <inheritdoc cref="CsvParser"/>
        protected virtual void SetupCsvReader(CsvReader csvReader)
        {
            SetupCsvReaderConfig(csvReader);
            csvReader.Configuration.RegisterClassMap(csvClassMap);
        }

        /// <inheritdoc cref="CsvConfiguration"/>
        protected virtual bool ShouldReaderSkipRecord(string[] row)
        {
            // assume column 0 is a required field (e.g. the date field)
            var firstColumn = row[0];
            var isSkipRecord = string.IsNullOrWhiteSpace(firstColumn) || firstColumn.LastOrDefault() == ':';
            var isFirstColumnIsNotDateTime = ShouldReaderSkipRecordNotDateTime(firstColumn);

            return isSkipRecord || isFirstColumnIsNotDateTime;
        }

        private void SetupCsvReaderConfig(CsvReader csvReader)
        {
            csvReader.Configuration.SkipEmptyRecords = true;
            csvReader.Configuration.ShouldSkipRecord = ShouldReaderSkipRecord;
            csvReader.Configuration.WillThrowOnMissingField = false;
            csvReader.Configuration.IgnoreReadingExceptions = true; // This is at the row level
            csvReader.Configuration.ReadingExceptionCallback = (ex, row) =>
            {
                Logger.Error(ex);
            };
            csvReader.Configuration.IsHeaderCaseSensitive = false;
        }

        /// <summary>
        /// Method skips those records for which the value of the first column isn't Date Time (except for the title).
        /// </summary>
        /// <param name="firstColumn">The first column of a row from a CSV report, the value of the first column must be of a type DateTime.</param>
        /// <returns>True - skip the record; False - not skip the record.</returns>
        private bool ShouldReaderSkipRecordNotDateTime(string firstColumn)
        {
            if (string.Equals(firstColumn, FirstColumnHeaderName, StringComparison.CurrentCulture))
            {
                return false;
            }
            return !DateTime.TryParse(firstColumn, out _);
        }

        private IEnumerable<T> EnumerateRowsInner(TextReader reader)
        {
            using (var csvReader = new CsvReader(reader))
            {
                SetupCsvReader(csvReader);
                List<T> csvRows;
                try
                {
                    csvRows = csvReader.GetRecords<T>().ToList();
                }
                catch (CsvHelperException ex)
                {
                    Logger.Warn(ex.Message);
                    throw;
                }
                return csvRows;
            }
        }
    }
}
