using System;
using System.Collections.Generic;
using System.Globalization;
using FacebookAPI.Enums;
using FacebookAPI.Utils;

namespace FacebookAPI.Helpers
{
    public static class FacebookReachPeriodHelper
    {
        public static string GetPeriodName(DateTime startPeriodDate)
        {
            var currentPeriod = GetCurrentPeriod(startPeriodDate);
            var currentMonthName = GetNameOfMonth(startPeriodDate);
            switch (currentPeriod)
            {
                case ReachPeriod.LastMonth:
                    return $"Last month ({currentMonthName})";
                case ReachPeriod.FirstPartOfCurrentMonth:
                    return $"First part of {currentMonthName}";
                case ReachPeriod.SecondPartOfCurrentMonth:
                    return $"Second part of {currentMonthName}";
                default:
                    throw new NotSupportedException("Reach period not found");
            }
        }

        public static object[] GetPeriodsForRequest()
        {
            var requestPeriods = new List<object>();

            var today = DateTime.Today;

            var beginOfCurrentMonth = new DateTime(today.Year, today.Month, 1);

            var middleOfCurrentMonth = new DateTime(today.Year, today.Month, 15);
            var endOfFirstPart = today.Day < middleOfCurrentMonth.Day ? today : middleOfCurrentMonth;
            var firstPartOfMonthPeriod = new { since = FacebookRequestUtils.GetDateString(beginOfCurrentMonth), until = FacebookRequestUtils.GetDateString(endOfFirstPart) };
            requestPeriods.Add(firstPartOfMonthPeriod);

            var beginOfPreviousMonth = beginOfCurrentMonth.AddMonths(-1);
            var endOfPreviousMonth = beginOfCurrentMonth.AddDays(-1);
            var lastMonthPeriod = new
            {
                since = FacebookRequestUtils.GetDateString(beginOfPreviousMonth),
                until = FacebookRequestUtils.GetDateString(endOfPreviousMonth),
            };
            requestPeriods.Add(lastMonthPeriod);

            if (today.Day > middleOfCurrentMonth.Day)
            {
                var nextDayAfterMiddleOfCurrentMonth = middleOfCurrentMonth.AddDays(1);
                var secondPartOfMonthPeriod = new
                {
                    since = FacebookRequestUtils.GetDateString(nextDayAfterMiddleOfCurrentMonth),
                    until = FacebookRequestUtils.GetDateString(today),
                };
                requestPeriods.Add(secondPartOfMonthPeriod);
            }
            return requestPeriods.ToArray();
        }

        private static ReachPeriod GetCurrentPeriod(DateTime startPeriodDate)
        {
            ReachPeriod currentPeriod;
            var currentMonth = DateTime.Today.Month;
            var periodMonth = startPeriodDate.Month;
            if (periodMonth == currentMonth)
            {
                var beginOfCurrentMonth = new DateTime(DateTime.Today.Year, DateTime.Today.Month, 1);
                currentPeriod = startPeriodDate.Day == beginOfCurrentMonth.Day
                    ? ReachPeriod.FirstPartOfCurrentMonth
                    : ReachPeriod.SecondPartOfCurrentMonth;
            }
            else
            {
                currentPeriod = ReachPeriod.LastMonth;
            }
            return currentPeriod;
        }

        private static string GetNameOfMonth(DateTime date)
        {
            return date.ToString("MMMM", CultureInfo.InvariantCulture);
        }
    }
}
