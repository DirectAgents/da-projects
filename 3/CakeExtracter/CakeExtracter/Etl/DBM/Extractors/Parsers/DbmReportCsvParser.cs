using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using CakeExtracter.Common;
using CakeExtracter.Etl.DBM.Extractors.Parsers.Models;
using CsvHelper;
using CsvHelper.Configuration;

namespace CakeExtracter.Etl.DBM.Extractors.Parsers
{
    /// <summary> DBM reports parser </summary>
    internal class DbmReportCsvParser<T> where T : DbmBaseReportRow
    {
        private const string FirstColumnHeaderName = "Date";

        private readonly DateRange dateRange;
        private readonly StreamReader streamReader;
        private readonly string csvFilePath;
        private readonly CsvClassMap csvClassMap;

        public DbmReportCsvParser(DateRange dateRange, CsvClassMap csvClassMap,
            string csvFilePath = null, StreamReader streamReader = null)
        {
            this.dateRange = dateRange;
            this.csvClassMap = csvClassMap;
            this.csvFilePath = csvFilePath;
            this.streamReader = streamReader;
        }

        public IEnumerable<T> EnumerateRows()
        {
            Logger.Info($"Extracting rows from {csvFilePath ?? "StreamReader"}");

            if (streamReader != null)
            {
                return EnumerateRowsInner(streamReader);
            }

            if (!File.Exists(csvFilePath))
            {
                return new List<T>();
            }

            using (var reader = File.OpenText(csvFilePath))
            {
                return EnumerateRowsInner(reader);
            }
        }

        protected virtual void SetupCsvReader(CsvReader csvReader)
        {
            SetupCsvReaderConfig(csvReader);
            csvReader.Configuration.RegisterClassMap(csvClassMap);
        }

        protected void SetupCsvReaderConfig(CsvReader csvReader)
        {
            csvReader.Configuration.SkipEmptyRecords = true; //ShouldSkipRecord is checked first. if false, will check if it's empty (all fields)
            csvReader.Configuration.ShouldSkipRecord = ShouldReaderSkipRecord;
            csvReader.Configuration.WillThrowOnMissingField = false;
            csvReader.Configuration.IgnoreReadingExceptions = true; // This is at the row level
            csvReader.Configuration.ReadingExceptionCallback = (ex, row) =>
            {
                Logger.Error(ex);
            };
            csvReader.Configuration.IsHeaderCaseSensitive = false;
            //Note: once the CsvHelper package is updated, change the above line to:
            //csvReader.Configuration.PrepareHeaderForMatch = header => header.ToLower();
        }

        protected virtual bool ShouldReaderSkipRecord(string[] row)
        {
            // assume column 0 is a required field (e.g. the date field)
            var firstColumn = row[0];
            var isSkipRecord = string.IsNullOrWhiteSpace(firstColumn) || firstColumn.LastOrDefault() == ':';
            var isFirstColumnIsNotDateTime = ShouldReaderSkipRecordNotDateTime(firstColumn);

            return isSkipRecord || isFirstColumnIsNotDateTime;
        }

        protected virtual IEnumerable<T> GroupAndEnumerate(List<T> allCsvRows)
        {
            Logger.Info($"All rows count [{allCsvRows.Count}]. Rows do not relate date range {dateRange.ToString()} will be removed...");
            var relatedCsvRows = allCsvRows.Where(x => dateRange.IsInRange(x.Date)).ToList();
            return relatedCsvRows;
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
                return GroupAndEnumerate(csvRows);
            }
        }

        /// <summary>
        /// Method skips those records for which the value of the first column isn't Date Time (except for the title)
        /// </summary>
        /// <param name="firstColumn">The first column of a row from a CSV report, the value of the first column must be of a type DateTime</param>
        /// <returns>True - skip the record; False - not skip the record</returns>
        private bool ShouldReaderSkipRecordNotDateTime(string firstColumn)
        {
            if (string.Equals(firstColumn, FirstColumnHeaderName))
            {
                return false;
            }
            return !DateTime.TryParse(firstColumn, out _);
        }
    }
}
