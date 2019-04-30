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

        private List<T> EnumerateRowsInner(TextReader reader)
        {
            using (var csvReader = new CsvReader(reader))
            {
                SetupCsvReaderConfig(csvReader);
                var csvRows = csvReader.GetRecords<T>().ToList();
                return csvRows;
            }
        }

        private void SetupCsvReaderConfig(CsvReader csvReader)
        {
            csvReader.Configuration.SkipEmptyRecords = true;
            csvReader.Configuration.WillThrowOnMissingField = false;
            csvReader.Configuration.IgnoreReadingExceptions = true;
            csvReader.Configuration.ReadingExceptionCallback = (ex, row) => { Logger.Error(ex); };
            csvReader.Configuration.IsHeaderCaseSensitive = false;
            csvReader.Configuration.RegisterClassMap<TRowMap>();
        }
    }
}
