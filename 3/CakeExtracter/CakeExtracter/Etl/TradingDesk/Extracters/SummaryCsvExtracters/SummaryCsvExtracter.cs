using CsvHelper;
using CsvHelper.Configuration;
using DirectAgents.Domain.Entities.CPProg;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;

namespace CakeExtracter.Etl.TradingDesk.Extracters.SummaryCsvExtracters
{
    public class SummaryCsvExtracter<T> : Extracter<T>
    {
        // if streamReader is not null, use it. otherwise use csvFilePath.
        private readonly StreamReader streamReader;
        private readonly string csvFilePath;

        private readonly ColumnMapping columnMapping;
        private readonly string summariesName;

        protected SummaryCsvExtracter(string summariesName, ColumnMapping columnMapping, StreamReader streamReader, string csvFilePath)
        {
            this.columnMapping = columnMapping;
            this.streamReader = streamReader;
            this.csvFilePath = csvFilePath;
            this.summariesName = summariesName;
        }

        public IEnumerable<T> EnumerateRows()
        {
            if (this.streamReader != null)
            {
                return EnumerateRowsInner(this.streamReader);
            }

            if (File.Exists(this.csvFilePath))
            {
                using (var reader = File.OpenText(this.csvFilePath))
                {
                    return EnumerateRowsInner(reader);
                }
            }

            return new List<T>();
        }

        protected override void Extract()
        {
            Logger.Info($"Extracting {summariesName} from {csvFilePath ?? "StreamReader"}");
            var items = EnumerateRows();
            Add(items);
            End();
        }

        protected virtual void SetupCsvReader(CsvReader csvReader)
        {
            SetupCsvReaderConfig(csvReader);
            var classMap = CreateCsvClassMap(this.columnMapping);
            csvReader.Configuration.RegisterClassMap(classMap);
        }

        protected virtual void AddPropertiesMaps(CsvClassMap classMap, Type classType, ColumnMapping colMap)
        {
            CheckAddPropertyMap(classMap, classType, "Date", colMap.Date);
            CheckAddPropertyMap(classMap, classType, "Cost", colMap.Cost);
            CheckAddPropertyMap(classMap, classType, "Impressions", colMap.Impressions);
            CheckAddPropertyMap(classMap, classType, "Clicks", colMap.Clicks);
            CheckAddPropertyMap(classMap, classType, "PostClickConv", colMap.PostClickConv);
            CheckAddPropertyMap(classMap, classType, "PostViewConv", colMap.PostViewConv);
            CheckAddPropertyMap(classMap, classType, "PostClickRev", colMap.PostClickRev);
            CheckAddPropertyMap(classMap, classType, "PostViewRev", colMap.PostViewRev);
        }

        protected virtual IEnumerable<T> GroupAndEnumerate(List<T> csvRows)
        {
            return csvRows;
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

        protected void CheckAddPropertyMap(CsvClassMap classMap, Type classType, string propName, string colName)
        {
            if (string.IsNullOrWhiteSpace(colName))
            {
                return;
            }
            var csvPropertyMap = CreatePropertyMap(classType, propName, colName);
            if (csvPropertyMap != null)
            {
                classMap.PropertyMaps.Add(csvPropertyMap);
            }
        }

        protected string GetIdIfSame(IEnumerable<T> stats, Func<T, string> getIdFunc)
        {
            var ids = stats.Select(getIdFunc).Distinct().ToList();
            return ids.Count == 1 ? ids.First() : null;
        }

        private IEnumerable<T> EnumerateRowsInner(TextReader reader)
        {
            using (var csvReader = new CsvReader(reader))
            {
                SetupCsvReader(csvReader);
                var csvRows = new List<T>();
                try
                {
                    csvRows = csvReader.GetRecords<T>().ToList();
                }
                catch (CsvHelperException ex)
                {
                    Logger.Warn(ex.Message);
                    throw ex;
                }
                return GroupAndEnumerate(csvRows);
            }
        }

        protected virtual bool ShouldReaderSkipRecord(string[] fields)
        { 
            // assume column 0 is a required field (e.g. the date field)
            var firstColumn = fields[0];
            return string.IsNullOrWhiteSpace(firstColumn) || firstColumn.LastOrDefault() == ':';
        }

        private CsvClassMap CreateCsvClassMap(ColumnMapping colMap)
        {
            var classMap = new DefaultCsvClassMap<T>();
            var classType = typeof(T);
            AddPropertiesMaps(classMap, classType, colMap);
            return classMap;
        }

        private CsvPropertyMap CreatePropertyMap(Type classType, string propName, string colName)
        {
            var propertyInfo = classType.GetProperty(propName);
            if (propertyInfo == null) // the property doesn't exist
            {
                return null;
            }

            var propMap = new CsvPropertyMap(propertyInfo);
            propMap.Name(colName);
            if (propertyInfo.PropertyType == typeof(int) || propertyInfo.PropertyType == typeof(decimal))
            {
                propMap.TypeConverterOption(NumberStyles.Currency | NumberStyles.AllowExponent);
                propMap.Default(0);
            }
            return propMap;
        }
    }
}
