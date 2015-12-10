using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using CsvHelper;
using CsvHelper.Configuration;
using DirectAgents.Domain.Entities.TD;

namespace CakeExtracter.Etl.TradingDesk.Extracters
{
    public class TDDailySummaryExtracter : Extracter<DailySummary>
    {
        // if streamReader is not null, use it. otherwise use csvFilePath.
        private readonly StreamReader streamReader;
        private readonly string csvFilePath;

        private readonly ColumnMapping columnMapping;

        public TDDailySummaryExtracter(ColumnMapping columnMapping, StreamReader streamReader = null, string csvFilePath = null)
        {
            this.columnMapping = columnMapping;
            this.streamReader = streamReader;
            this.csvFilePath = csvFilePath;
        }

        protected override void Extract()
        {
            Logger.Info("Extracting DailySummaries from {0}", csvFilePath ?? "StreamReader");
            var items = EnumerateRows();
            Add(items);
            End();
        }

        private IEnumerable<DailySummary> EnumerateRows()
        {
            if (this.streamReader != null)
            {
                foreach (var row in EnumerateRowsInner(this.streamReader))
                    yield return row;
            }
            else if (File.Exists(this.csvFilePath))
            {
                using (StreamReader reader = File.OpenText(this.csvFilePath))
                {
                    foreach (var row in EnumerateRowsInner(reader))
                        yield return row;
                }
            }
        }

        private IEnumerable<DailySummary> EnumerateRowsInner(StreamReader reader)
        {
            using (CsvReader csv = new CsvReader(reader))
            {
                csv.Configuration.SkipEmptyRecords = true;

                var classMap = CreateCsvClassMap(this.columnMapping);
                csv.Configuration.RegisterClassMap(classMap);

                List<DailySummary> csvRows = null;
                try
                {
                    csvRows = csv.GetRecords<DailySummary>().ToList();
                }
                catch (CsvHelperException ex)
                {
                    Logger.Error(ex);
                }
                if (csvRows != null)
                {
                    for (int i = 0; i < csvRows.Count; i++)
                    {
                        yield return csvRows[i];
                    }
                }
            }
        }

        private CsvClassMap CreateCsvClassMap(ColumnMapping colMap)
        {
            var classMap = new DefaultCsvClassMap<DailySummary>();
            CheckAddPropertyMap(classMap, "Date", colMap.Date);
            CheckAddPropertyMap(classMap, "Cost", colMap.Cost);
            CheckAddPropertyMap(classMap, "Impressions", colMap.Impressions);
            CheckAddPropertyMap(classMap, "Clicks", colMap.Clicks);
            CheckAddPropertyMap(classMap, "PostClickConv", colMap.PostClickConv);
            CheckAddPropertyMap(classMap, "PostViewConv", colMap.PostViewConv);
            return classMap;
        }
        private void CheckAddPropertyMap(CsvClassMap classMap, string propName, string colName)
        {
            if (!string.IsNullOrWhiteSpace(colName))
                classMap.PropertyMaps.Add(CreatePropertyMap(propName, colName));
        }
        private CsvPropertyMap CreatePropertyMap(string propName, string colName)
        {
            var propertyInfo = typeof(DailySummary).GetProperty(propName);
            var propMap = new CsvPropertyMap(propertyInfo);
            propMap.Name(colName);
            if (propertyInfo.PropertyType == typeof(int) ||
                propertyInfo.PropertyType == typeof(decimal))
            {
                propMap.TypeConverterOption(NumberStyles.Currency);
            }
            return propMap;
        }
    }
}
