using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using CakeExtracter.Common.Extractors.CsvExtractors.Contracts;
using CakeExtracter.Etl;
using CsvHelper;
using CsvHelper.Configuration;

namespace CakeExtracter.Common.Extractors.CsvExtractors
{
    internal class CsvAccountExtractor<T, TRowMap> : Extracter<T>, ICsvExtractor<T>
        where TRowMap : CsvClassMap<T>
    {
        public event Action<Exception> ReadingExceptionCallback;

        private readonly int accountId;
        private readonly string itemsName;
        private readonly StreamReader streamReader;

        public CsvAccountExtractor(int accountId, string itemsName, StreamReader streamReader)
        {
            this.accountId = accountId;
            this.itemsName = itemsName;
            this.streamReader = streamReader;
        }

        public List<T> EnumerateRows()
        {
            try
            {
                return EnumerateRowsInner(streamReader);
            }
            catch (Exception exception)
            {
                ReadingExceptionCallback?.Invoke(exception);
                Logger.Error(accountId, exception);
                return new List<T>();
            }
        }

        protected override void Extract()
        {
            Logger.Info(accountId, $"Extracting {itemsName} using stream reader.");
            var items = EnumerateRows();
            Add(items);
            End();
        }

        protected virtual void SetupCsvReaderConfig(ICsvReader csvReader)
        {
            csvReader.Configuration.SkipEmptyRecords = true;
            csvReader.Configuration.ShouldSkipRecord = ShouldSkipRecord;
            csvReader.Configuration.WillThrowOnMissingField = false;
            csvReader.Configuration.IgnoreReadingExceptions = true;
            csvReader.Configuration.ReadingExceptionCallback = ProcessReadingException;
            csvReader.Configuration.IsHeaderCaseSensitive = false;
            csvReader.Configuration.RegisterClassMap<TRowMap>();
        }

        protected virtual bool ShouldSkipRecord(string[] fields)
        {
            return false;
        }

        protected virtual void ProcessReadingException(Exception exception, ICsvReader row)
        {
            LogReadingException(exception, row);
        }

        private void LogReadingException(Exception exception, ICsvReader row)
        {
            var rowFields = row.FieldHeaders.Select((x, i) => $"{x} = {row.CurrentRecord[i]}").ToList();
            var message = $"The wrong record: {string.Join(", ", rowFields)}";
            Logger.Error(accountId, new Exception(message, exception));
        }

        private List<T> EnumerateRowsInner(TextReader reader)
        {
            using (var csvReader = new CsvReader(reader))
            {
                SetupCsvReaderConfig(csvReader);
                var csvRows = csvReader.GetRecords<T>().ToList();
                return csvRows;
            }
        }
    }
}
