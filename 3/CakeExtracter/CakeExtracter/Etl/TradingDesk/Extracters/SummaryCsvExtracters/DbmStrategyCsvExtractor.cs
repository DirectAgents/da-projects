using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using CakeExtracter.Common;
using DirectAgents.Domain.Entities.CPProg;

namespace CakeExtracter.Etl.TradingDesk.Extracters.SummaryCsvExtracters
{
    internal class DbmStrategyCsvExtractor : TDStrategySummaryExtracter
    {
        private const string FirstColumnHeaderName = "Date";

        private readonly DateRange dateRange;

        public DbmStrategyCsvExtractor(DateRange dateRange, ColumnMapping columnMapping,
            StreamReader streamReader = null, string csvFilePath = null) 
            : base(columnMapping, streamReader, csvFilePath)
        {
            this.dateRange = dateRange;
        }

        protected override bool ShouldReaderSkipRecord(string[] fields)
        {
            var isSkipRecord = base.ShouldReaderSkipRecord(fields);
            var isFirstColumnIsNotDateTime = ShouldReaderSkipRecordNotDateTime(fields[0]);

            return isSkipRecord || isFirstColumnIsNotDateTime;
        }

        protected override IEnumerable<StrategySummary> GroupAndEnumerate(List<StrategySummary> csvRows)
        {
            var validRows = csvRows.Where(x => dateRange.IsInRange(x.Date)).ToList();
            return base.GroupAndEnumerate(validRows);
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
