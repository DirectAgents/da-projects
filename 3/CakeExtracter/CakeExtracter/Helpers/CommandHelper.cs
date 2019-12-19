using System;
using CakeExtracter.Common;
using CakeExtracter.Etl;

namespace CakeExtracter.Helpers
{
    public class CommandHelper
    {
        /// <summary>
        /// The method returns a calculated date range for command execution based on nullable parameters
        /// (null parameters are implied as not set).
        /// </summary>
        /// <param name="startDate">Start date from which statistics will be extracted (default is 'daysAgo').</param>
        /// <param name="endDate">End date to which statistics will be extracted
        /// (default is yesterday or today, depending on 'willDateRangeToToday').</param>
        /// <param name="daysAgoToStart">The number of days ago to calculate the start date from which statistics will be retrieved,
        /// used if 'StartDate' not specified (default = defaultDaysAgo).</param>
        /// <param name="defaultDaysAgo">The number of days ago by default, used if 'daysAgoToStart' is not specified.</param>
        /// <param name="willDateRangeToToday">A value indicating whether the date range will be to today (default - to yesterday).</param>
        /// <returns>Date range.</returns>
        public static DateRange GetDateRange(DateTime? startDate, DateTime? endDate, int? daysAgoToStart, int defaultDaysAgo, bool willDateRangeToToday = false)
        {
            var today = DateTime.Today;
            var daysAgo = daysAgoToStart ?? defaultDaysAgo;
            var startDateInRange = startDate ?? today.AddDays(-daysAgo);
            var endDateInRange = GetEndDateInRange(endDate, willDateRangeToToday);
            var dateRange = new DateRange(startDateInRange, endDateInRange);
            return dateRange;
        }

        /// <summary>
        /// The method returns a calculated date range for command execution based on not nullable parameters
        /// (parameters with a default value for their type are implied as not set).
        /// </summary>
        /// <param name="startDate">Start date from which statistics will be extracted (default is 'daysAgo').</param>
        /// <param name="endDate">End date to which statistics will be extracted (default is yesterday)</param>
        /// <param name="daysAgoToStart"> The number of days ago to calculate the start date from which statistics will be retrieved, used if StartDate not specified (default = defaultDaysAgo)</param>
        /// <param name="defaultDaysAgo">The number of days ago by default, used if 'daysAgoToStart' is not specified.</param>
        /// <returns></returns>
        public static DateRange GetDateRange(DateTime startDate, DateTime endDate, int daysAgoToStart, int defaultDaysAgo)
        {
            var today = DateTime.Today;
            var daysAgo = daysAgoToStart == 0 ? defaultDaysAgo : daysAgoToStart;
            var startDateInRange = startDate == DateTime.MinValue ? today.AddDays(-daysAgo) : startDate;
            var endDateInRange = endDate == DateTime.MinValue ? today.AddDays(-1) : endDate;
            var dateRange = new DateRange(startDateInRange, endDateInRange);
            return dateRange;
        }

        /// <summary>
        /// Load some items using the "extractor + loader" retrieving mechanism. 
        /// </summary>
        /// <typeparam name="T">Type of loaded items</typeparam>
        /// <param name="extractor">Instance of an appropriate extractor</param>
        /// <param name="loader">Instance of an appropriate loader</param>
        public static void DoEtl<T>(Extracter<T> extractor, Loader<T> loader)
        {
            var extractorThread = extractor.Start();
            var loaderThread = loader.Start(extractor);
            extractorThread.Join();
            loaderThread.Join();
        }

        private static DateTime GetEndDateInRange(DateTime? endDate, bool willDateRangeToToday)
        {
            var today = DateTime.Today;
            if (endDate.HasValue)
            {
                return endDate.Value;
            }
            else
            {
                return willDateRangeToToday ? today : today.AddDays(-1);
            }
        }
    }
}
