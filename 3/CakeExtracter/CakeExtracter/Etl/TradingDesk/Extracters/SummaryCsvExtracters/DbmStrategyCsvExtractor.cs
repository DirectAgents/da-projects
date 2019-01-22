using System;
using System.IO;
using DirectAgents.Domain.Entities.CPProg;

namespace CakeExtracter.Etl.TradingDesk.Extracters.SummaryCsvExtracters
{
    internal class DbmStrategyCsvExtractor : TDStrategySummaryExtracter
    {
        private const string FirstColumnHeaderName = "Date";

        public DbmStrategyCsvExtractor(ColumnMapping columnMapping, StreamReader streamReader = null, string csvFilePath = null) : base(columnMapping, streamReader, csvFilePath)
        {
        }

        protected override bool ShouldReaderSkipRecord(string[] fields)
        {
            var isSkipRecord = base.ShouldReaderSkipRecord(fields);
            var isFirstColumnIsNotDateTime = ShouldReaderSkipRecordNotDateTime(fields[0]);

            return isSkipRecord || isFirstColumnIsNotDateTime;
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
