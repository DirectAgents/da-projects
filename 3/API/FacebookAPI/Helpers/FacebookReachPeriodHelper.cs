using System;
using System.Collections.Generic;
using System.Globalization;
using FacebookAPI.Enums;
using FacebookAPI.Utils;

namespace FacebookAPI.Helpers
{
    /// <summary>
    /// Facebook helper for periods of Reach metric.
    /// </summary>
    public static class FacebookReachPeriodHelper
    {
        /// <summary>
        /// Gets the key-name of reach period by start date of period.
        /// </summary>
        /// <param name="startDateOfPeriod">Start date of period.</param>
        /// <returns>Key-name of reach period.</returns>
        public static string GetPeriodName(DateTime startDateOfPeriod)
        {
            var period = GetReachPeriod(startDateOfPeriod);
            return GetPeriodKeyName(period, startDateOfPeriod);
        }

        /// <summary>
        /// Gets the key-name of reach month period by start date of period.
        /// </summary>
        /// <param name="startDateOfPeriod">Start date of period.</param>
        /// <returns>Key-name of reach period.</returns>
        public static string GetMonthPeriodName(DateTime startDateOfPeriod)
        {
            return $"{startDateOfPeriod.Year}. {GetFullNameOfMonth(startDateOfPeriod)}";
        }

        /// <summary>
        /// Gets array of periods for the API request of the Reach metric.
        /// </summary>
        /// <returns>Array of periods.</returns>
        public static object[] GetPeriodsForApiRequest()
        {
            var requestedPeriods = new List<object>();
            var firstPartOfMonthPeriod = GetPeriodOfFirstPartOfCurrentMonth();
            requestedPeriods.Add(firstPartOfMonthPeriod);
            var lastMonthPeriod = GetPeriodOfLastMonth();
            requestedPeriods.Add(lastMonthPeriod);
            if (IsSecondPartOfMonthPeriodNeeded())
            {
                var secondPartOfMonthPeriod = GetPeriodOfSecondPartOfCurrentMonth();
                requestedPeriods.Add(secondPartOfMonthPeriod);
            }

            return requestedPeriods.ToArray();
        }

        /// <summary>
        /// Gets array of monthly periods for the API request of the Reach metric.
        /// </summary>
        /// <returns>Array of periods.</returns>
        public static object[] GetMonthlyPeriodsForApiRequest()
        {
            return new[]
            {
                GetPeriodOfCurrentMonth(),
                GetPeriodOfLastMonth(),
            };
        }

        private static object GetPeriodOfFirstPartOfCurrentMonth()
        {
            var today = DateTime.Today;
            var middleOfCurrentMonth = GetMiddleOfCurrentMonth();
            var endOfFirstPart = today.Day < middleOfCurrentMonth.Day
                ? today
                : middleOfCurrentMonth;
            var startOfFirstPart = GetBeginOfCurrentMonth();
            return new
            {
                since = FacebookRequestUtils.GetDateString(startOfFirstPart),
                until = FacebookRequestUtils.GetDateString(endOfFirstPart),
            };
        }

        private static object GetPeriodOfSecondPartOfCurrentMonth()
        {
            var middleOfCurrentMonth = GetMiddleOfCurrentMonth();
            var nextDayAfterMiddleOfCurrentMonth = middleOfCurrentMonth.AddDays(1);
            var startOfSecondPart = nextDayAfterMiddleOfCurrentMonth;
            var endOfSecondPart = DateTime.Today;
            return new
            {
                since = FacebookRequestUtils.GetDateString(startOfSecondPart),
                until = FacebookRequestUtils.GetDateString(endOfSecondPart),
            };
        }

        private static object GetPeriodOfCurrentMonth()
        {
            return new
            {
                since = FacebookRequestUtils.GetDateString(GetBeginOfCurrentMonth()),
                until = FacebookRequestUtils.GetDateString(DateTime.Today),
            };
        }

        private static object GetPeriodOfLastMonth()
        {
            var beginOfCurrentMonth = GetBeginOfCurrentMonth();
            var beginOfPreviousMonth = beginOfCurrentMonth.AddMonths(-1);
            var endOfPreviousMonth = beginOfCurrentMonth.AddDays(-1);
            return new
            {
                since = FacebookRequestUtils.GetDateString(beginOfPreviousMonth),
                until = FacebookRequestUtils.GetDateString(endOfPreviousMonth),
            };
        }

        private static bool IsSecondPartOfMonthPeriodNeeded()
        {
            var today = DateTime.Today;
            var middleOfCurrentMonth = GetMiddleOfCurrentMonth();
            return today.Day > middleOfCurrentMonth.Day;
        }

        private static ReachPeriod GetReachPeriod(DateTime startPeriodDate)
        {
            return DoesPeriodBelongCurrentMonth(startPeriodDate)
                ? GetPartOfCurrentMonth(startPeriodDate)
                : ReachPeriod.FullPreviousMonth;
        }

        private static ReachPeriod GetPartOfCurrentMonth(DateTime periodDate)
        {
            var beginOfCurrentMonth = GetBeginOfCurrentMonth();
            return periodDate.Day == beginOfCurrentMonth.Day
                ? ReachPeriod.FirstPartOfCurrentMonth
                : ReachPeriod.SecondPartOfCurrentMonth;
        }

        private static bool DoesPeriodBelongCurrentMonth(DateTime periodDate)
        {
            var currentMonth = DateTime.Today.Month;
            var periodMonth = periodDate.Month;
            return periodMonth == currentMonth;
        }

        private static string GetPeriodKeyName(ReachPeriod period, DateTime startDateOfPeriod)
        {
            var periodYear = startDateOfPeriod.Year;
            var periodMonthName = GetFullNameOfMonth(startDateOfPeriod);
            switch (period)
            {
                case ReachPeriod.FullPreviousMonth:
                    return $"{periodYear}. {periodMonthName}";
                case ReachPeriod.FirstPartOfCurrentMonth:
                    return $"{periodYear}. {periodMonthName}. First part";
                case ReachPeriod.SecondPartOfCurrentMonth:
                    return $"{periodYear}. {periodMonthName}. Second part";
                default:
                    throw new NotSupportedException("Reach period not found");
            }
        }

        private static string GetFullNameOfMonth(DateTime date)
        {
            return date.ToString("MMMM", CultureInfo.InvariantCulture);
        }

        private static DateTime GetBeginOfCurrentMonth()
        {
            return new DateTime(DateTime.Today.Year, DateTime.Today.Month, 1);
        }

        private static DateTime GetMiddleOfCurrentMonth()
        {
            return new DateTime(DateTime.Today.Year, DateTime.Today.Month, 15);
        }
    }
}
