using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using CakeExtracter.Common.Extractors.CsvExtractors.Contracts;
using CakeExtracter.Etl;
using CakeExtracter.Helpers;
using CsvHelper;
using CsvHelper.Configuration;
using DirectAgents.Domain.Entities.CPProg;

namespace CakeExtracter.Common.Extractors.CsvExtractors
{
    public class CsvAccountExtractor<T, TRowMap> : Extracter<T>, ICsvExtractor<T>
        where TRowMap : CsvClassMap<T>
    {
        protected readonly int AccountId;
        protected readonly StreamReader StreamReader;

        public event Action<Exception> ReadingExceptionCallback;

        public string ItemsName { get; set; } = "items";

        public CsvAccountExtractor(ExtAccount account)
        {
            AccountId = account.Id;
        }

        public CsvAccountExtractor(ExtAccount account, string itemsName, StreamReader streamReader) : this(account)
        {
            ItemsName = itemsName;
            StreamReader = streamReader;
        }

        public virtual List<T> EnumerateRows(string url)
        {
            using (var streamReader = RequestHelper.CreateStreamReaderFromUrl(url))
            {
                return EnumerateRows(streamReader);
            }
        }

        public virtual List<T> EnumerateRows(StreamReader streamReader)
        {
            try
            {
                return EnumerateRowsInner(streamReader);
            }
            catch (Exception exception)
            {
                if (ReadingExceptionCallback != null)
                {
                    ReadingExceptionCallback.Invoke(exception);
                }
                else
                {
                    Logger.Error(AccountId, exception);
                }

                return new List<T>();
            }
        }

        public List<T> EnumerateRows()
        {
            return EnumerateRows(StreamReader);
        }

        protected override void Extract()
        {
            Logger.Info(AccountId, $"Extracting {ItemsName} using stream reader.");
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
            Logger.Error(AccountId, new Exception(message, exception));
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
