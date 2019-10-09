using System;
using System.Collections.Generic;
using System.Linq;
using BingAds.Utilities;
using CakeExtracter.Etl.SearchMarketing.Extracters.BingExtractors.Models;
using CakeExtracter.Etl.SearchMarketing.Extracters.BingExtractors.Parsers;
using CsvHelper.Configuration;

namespace CakeExtracter.Etl.SearchMarketing.Extracters.BingExtractors
{
    public abstract class BingExtractorBase : Extracter<Dictionary<string, string>>
    {
        protected readonly long AccountId;
        protected readonly DateTime StartDate;
        protected readonly DateTime EndDate;
        protected readonly BingUtility BingUtility;

        protected BingExtractorBase(BingUtility bingUtility, long accountId, DateTime startDate, DateTime endDate)
        {
            AccountId = accountId;
            StartDate = startDate;
            EndDate = endDate;
            BingUtility = bingUtility;
        }

        protected abstract IEnumerable<Dictionary<string, string>> ExtractAndEnumerateRows();

        protected void ExtractData()
        {
            try
            {
                var items = ExtractAndEnumerateRows();
                Add(items);
            }
            catch (Exception exception)
            {
                Logger.Error(exception);
                throw exception; // Throws an exception to catch it in the general class Extractor for scheduling a new Bing command with initial parameters.
            }

            End();
        }

        protected IEnumerable<Dictionary<string, string>> EnumerateRowsAsDictionaries<T>(IEnumerable<T> rows)
        {
            foreach (var row in rows)
            {
                var dict = new Dictionary<string, string>();

                // Use reflection to add values
                var type = typeof(T);
                var properties = type.GetProperties();
                foreach (var propertyInfo in properties)
                {
                    if (propertyInfo.Name == "AccountId" && String.IsNullOrWhiteSpace((string)propertyInfo.GetValue(row)))
                        dict["AccountId"] = this.AccountId.ToString();
                    else if (propertyInfo.PropertyType == typeof(int))
                        dict[propertyInfo.Name] = ((int)propertyInfo.GetValue(row)).ToString();
                    else if (propertyInfo.PropertyType == typeof(decimal))
                        dict[propertyInfo.Name] = ((decimal)propertyInfo.GetValue(row)).ToString();
                    else
                        dict[propertyInfo.Name] = (string)propertyInfo.GetValue(row);

                    //TODO: Have the extracter return objects with typed properties (and have the loader handle that)
                }
                yield return dict;
            }
        }

        /// <summary>
        /// Parses the report content.
        /// </summary>
        /// <typeparam name="TBingReportRow">Base Bing report row type.</typeparam>
        /// <param name="reportFilePath">Path to report file.</param>
        /// <param name="rowMap">Row map for parsing.</param>
        /// <returns>List of report rows.</returns>
        protected List<TBingReportRow> GetReportRows<TBingReportRow>(string reportFilePath, CsvClassMap rowMap)
            where TBingReportRow : BingBaseRow
        {
            Logger.Info("Parsing a report...");
            var parser = new BingReportCsvParser<TBingReportRow>(rowMap, reportFilePath);
            var reportRows = parser.EnumerateRows().ToList();
            Logger.Info($"The report parsed successfully (row count: {reportRows.Count})");
            return reportRows;
        }
    }
}
