using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using CsvHelper;
using CsvHelper.Configuration;

namespace CakeExtracter.Etl.YAM.Extractors.CsvExtractors
{
    internal class YamCsvExtractor<T, TRowMap> : Extracter<T>
        where TRowMap : CsvClassMap<T>
    {
        private const string ErrorMessageIfReportIsEmpty = "No header record was found.";

        private readonly int accountId;
        private readonly string summariesName;
        private readonly StreamReader streamReader;

        public YamCsvExtractor(int accountId, string summariesName, StreamReader streamReader)
        {
            this.accountId = accountId;
            this.summariesName = summariesName;
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
                if (exception is CsvHelperException && exception.Message == ErrorMessageIfReportIsEmpty)
                {
                    Logger.Warn(accountId, $"There are no statistics in the report: {exception.Message}");
                }
                else
                {
                    Logger.Error(accountId, exception);
                }

                Logger.Error(accountId, exception);
                return new List<T>();
            }
        }

        protected override void Extract()
        {
            Logger.Info(accountId, $"Extracting {summariesName} using stream reader.");
            var items = EnumerateRows();
            Add(items);
            End();
        }

        private List<T> EnumerateRowsInner(TextReader reader)
        {
            using (var csvReader = new CsvReader(reader))
            {
                SetupCsvReaderConfig(csvReader);
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

                return csvRows;
            }
        }

        private void SetupCsvReaderConfig(CsvReader csvReader)
        {
            csvReader.Configuration.SkipEmptyRecords = true;
            csvReader.Configuration.ShouldSkipRecord = ShouldReaderSkipRecord;
            csvReader.Configuration.WillThrowOnMissingField = false;
            csvReader.Configuration.IgnoreReadingExceptions = true;
            csvReader.Configuration.ReadingExceptionCallback = (ex, row) => { Logger.Error(ex); };
            csvReader.Configuration.IsHeaderCaseSensitive = false;
            csvReader.Configuration.RegisterClassMap<TRowMap>();
        }

        private bool ShouldReaderSkipRecord(string[] fields)
        {
            // assume column 0 is a required field (e.g. the date field)
            var firstColumn = fields[0];
            return string.IsNullOrWhiteSpace(firstColumn) || firstColumn.LastOrDefault() == ':';
        }


        //private void SetupCsvReader(CsvReader csvReader)
        //{
        //    SetupCsvReaderConfig(csvReader);
        //    var classMap = CreateCsvClassMap();
        //    csvReader.Configuration.RegisterClassMap(classMap);
        //}

        //private CsvClassMap CreateCsvClassMap()
        //{
        //    var classMap = new DefaultCsvClassMap<T>();
        //    var classType = typeof(T);
        //    AddPropertiesMaps(classMap, classType);
        //    return classMap;
        //}

        //private void AddPropertiesMaps(CsvClassMap classMap, Type classType)
        //{
        //    CheckAddPropertyMap(classMap, classType, "Date", ColumnMapping.Date);
        //    CheckAddPropertyMap(classMap, classType, "Cost", ColumnMapping.Cost);
        //    CheckAddPropertyMap(classMap, classType, "Impressions", ColumnMapping.Impressions);
        //    CheckAddPropertyMap(classMap, classType, "Clicks", ColumnMapping.Clicks);
        //    CheckAddPropertyMap(classMap, classType, "ClickThroughConversion", ColumnMapping.ClickThroughConversion);
        //    CheckAddPropertyMap(classMap, classType, "ViewThroughConversion", ColumnMapping.ViewThroughConversion);
        //    CheckAddPropertyMap(classMap, classType, "ConversionValue", ColumnMapping.ConversionValue);
        //    CheckAddPropertyMap(classMap, classType, "AdvertiserSpending", ColumnMapping.AdvertiserSpending);
        //}

        //private void CheckAddPropertyMap(CsvClassMap classMap, Type classType, string propName, string colName)
        //{
        //    if (string.IsNullOrWhiteSpace(colName))
        //    {
        //        return;
        //    }

        //    var csvPropertyMap = CreatePropertyMap(classType, propName, colName);
        //    if (csvPropertyMap != null)
        //    {
        //        classMap.PropertyMaps.Add(csvPropertyMap);
        //    }
        //}

        //private CsvPropertyMap CreatePropertyMap(Type classType, string propName, string colName)
        //{
        //    var propertyInfo = classType.GetProperty(propName);
        //    if (propertyInfo == null) // the property doesn't exist
        //    {
        //        return null;
        //    }

        //    var propMap = new CsvPropertyMap(propertyInfo);
        //    propMap.Name(colName);
        //    if (propertyInfo.PropertyType != typeof(int) && propertyInfo.PropertyType != typeof(decimal))
        //    {
        //        return propMap;
        //    }

        //    propMap.TypeConverterOption(NumberStyles.Currency | NumberStyles.AllowExponent);
        //    propMap.Default(0);
        //    return propMap;
        //}
    }
}