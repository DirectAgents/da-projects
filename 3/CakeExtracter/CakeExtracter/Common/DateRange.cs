using System;
using System.Collections.Generic;

namespace CakeExtracter.Common
{
    public struct DateRange
    {
        private readonly Func<DateTime, DateTime> step;

        public DateTime FromDate { get; set; }

        public DateTime ToDate { get; set; }

        public IEnumerable<DateTime> Dates
        {
            get
            {
                for (var i = FromDate; i <= ToDate; i = step(i))
                    yield return i;
            }
        }

        public DateRange(DateTime fromDate, DateTime toDate, bool datePartOnly = true)
            : this()
        {
            step = x => x.AddDays(1);
            FromDate = datePartOnly ? fromDate.Date : fromDate;
            ToDate = datePartOnly ? toDate.Date : toDate;
        }

        public override string ToString()
        {
            return $"{FromDate.ToShortDateString()} to {ToDate.ToShortDateString()}";
        }

        public bool IsInRange(DateTime date)
        {
            return date >= FromDate && date <= ToDate;
        }

        public bool IsCrossDateRange(DateRange dateRange)
        {
            return IsInRange(dateRange.FromDate) || IsInRange(dateRange.ToDate);
        }

        public DateRange MergeDateRange(DateRange dateRange)
        {
            var fromDate = FromDate < dateRange.FromDate ? FromDate : dateRange.FromDate;
            var toDate = ToDate > dateRange.ToDate ? ToDate : dateRange.ToDate;
            return new DateRange(fromDate, toDate);
        }

        public IEnumerable<DateRange> GetDaysChunks(int chunkSizeInDays)
        {
            var tempStart = FromDate;
            var tempEnd = FromDate;
            while (tempStart <= ToDate)
            {
                tempEnd = tempStart.AddDays(chunkSizeInDays);
                if (tempEnd > ToDate)
                {
                    tempEnd = ToDate;
                }
                yield return new DateRange(tempStart, tempEnd);
                tempStart = tempEnd.AddDays(1);
            }
        }
    }
}
