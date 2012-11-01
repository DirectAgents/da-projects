using System;
using System.Collections.Generic;

namespace Common
{
    public class DateRange
    {
        private Func<DateTime, DateTime> step = x => x.AddDays(1);

        public DateRange(DateTime fromDate, DateTime toDate)
        {
            FromDate = fromDate;
            ToDate = toDate;
        }

        public DateRange(DateTime fromDate, DateTime toDate, Func<DateTime, DateTime> step)
        {
            FromDate = fromDate;
            ToDate = toDate;
            this.step = step;
        }

        public DateRange(int fromYear, int fromMonth, int fromDay, int toYear, int toMonth, int toDay)
        {
            FromDate = new DateTime(fromYear, fromMonth, fromDay);
            ToDate = new DateTime(toYear, toMonth, toDay);
        }

        public DateRange(DateTime fromDate, int numDays)
        {
            FromDate = fromDate;
            ToDate = fromDate.AddDays(numDays);
        }

        public DateRange(int fromYear, int fromMonth, int fromDay, Func<DateTime, DateTime> toDate)
        {
            FromDate = new DateTime(fromYear, fromMonth, fromDay);
            ToDate = toDate(FromDate);
        }

        public IEnumerable<DateRange> Days
        {
            get
            {
                for (DateTime i = FromDate; i <= ToDate ; i = step(i))
                    yield return new DateRange(i, 1);
            }
        }

        public IEnumerable<DateTime> Dates
        {
            get
            {
                for (DateTime i = FromDate; i <= ToDate; i = step(i))
                    yield return i;
            }
        }

        public DateTime FromDate { get; set; }

        public DateTime ToDate { get; set; }
    }
}
