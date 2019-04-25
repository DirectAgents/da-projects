using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using CakeExtracter.Common;
using CsvHelper;
using CsvHelper.Configuration;
using DBM.Parsers.Models;

namespace CakeExtracter.Etl.DBM.Extractors.Parsers
{
    /// <summary> DBM reports parser </summary>
    internal class DbmReportCsvParser<T> where T : DbmBaseReportRow
    {
        private const string FirstColumnHeaderName = "Date";

        private readonly DateRange _dateRange;
        private readonly StreamReader _streamReader;
        private readonly string _csvFilePath;
        private readonly CsvClassMap _csvClassMap;

        public DbmReportCsvParser(DateRange dateRange, CsvClassMap csvClassMap,
            string csvFilePath = null, StreamReader streamReader = null)
        {
            _dateRange = dateRange;
            _csvClassMap = csvClassMap;
            _csvFilePath = csvFilePath;
            _streamReader = streamReader;
        }

        public IEnumerable<T> EnumerateRows()
        {
            Logger.Info($"Extracting rows from {_csvFilePath ?? "StreamReader"}");

            if (_streamReader != null)
            {
                return EnumerateRowsInner(_streamReader);
            }

            if (!File.Exists(_csvFilePath))
            {
                return new List<T>();
            }

            using (var reader = File.OpenText(_csvFilePath))
            {
                return EnumerateRowsInner(reader);
            }
        }

        protected virtual void SetupCsvReader(CsvReader csvReader)
        {
            SetupCsvReaderConfig(csvReader);
            csvReader.Configuration.RegisterClassMap(_csvClassMap);
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

        protected virtual IEnumerable<T> GroupAndEnumerate(List<T> csvRows)
        {
            return csvRows.Where(x => _dateRange.IsInRange(x.Date)).ToList();
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
