using System;
using CakeExtracter.Common;
using CakeExtracter.Etl;

namespace CakeExtracter.Helpers
{
    internal class CommandHelper
    {
        /// <summary>
        /// The method returns a calculated date range for command execution.
        /// </summary>
        /// <param name="startDate">Start date from which statistics will be extracted (default is 'daysAgo')</param>
        /// <param name="endDate">End date to which statistics will be extracted (default is yesterday)</param>
        /// <param name="daysAgoToStart"> The number of days ago to calculate the start date from which statistics will be retrieved, used if StartDate not specified (default = defaultDaysAgo)</param>
        /// <param name="defaultDaysAgo"></param>
        /// <returns></returns>
        public static DateRange GetDateRange(DateTime? startDate, DateTime? endDate, int? daysAgoToStart, int defaultDaysAgo)
        {
            var today = DateTime.Today;
            var daysAgo = daysAgoToStart ?? defaultDaysAgo;
            var startDateInRange = startDate ?? today.AddDays(-daysAgo);
            var endDateInRange = endDate ?? today.AddDays(-1);
            var dateRange = new DateRange(startDateInRange, endDateInRange);
            return dateRange;
        }

        public static void DoEtl<T>(Extracter<T> extractor, Loader<T> loader)
        {
            var extractorThread = extractor.Start();
            var loaderThread = loader.Start(extractor);
            extractorThread.Join();
            loaderThread.Join();
        }
    }
}
