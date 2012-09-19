using System;
using System.Collections.Generic;

namespace Common
{
    public class DateRange
    {
        public DateRange(DateTime fromDate, DateTime toDate)
        {
            FromDate = fromDate;
            ToDate = toDate;
        }

        public DateRange(DateTime fromDate, int numDays)
        {
            FromDate = fromDate;
            ToDate = fromDate.AddDays(numDays);
        }

        public IEnumerable<DateRange> Days
        {
            get
            {
                for (DateTime i = FromDate; i <= ToDate ; i = i.AddDays(1))
                    yield return new DateRange(i, 1);
            }
        }

        public DateTime FromDate { get; set; }

        public DateTime ToDate { get; set; }
    }
}
