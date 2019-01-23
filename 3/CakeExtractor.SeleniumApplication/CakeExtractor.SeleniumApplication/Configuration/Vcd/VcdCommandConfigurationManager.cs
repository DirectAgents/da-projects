using CakeExtractor.SeleniumApplication.Properties;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CakeExtractor.SeleniumApplication.Configuration.Vcd
{
    internal class VcdCommandConfigurationManager
    {
        private const int defaultDaysIntervalToProcess = 30;

        public List<DateTime> GetDaysToProcess()
        {
            var startDate = VcdSettings.Default.StartDate;
            var endDate = VcdSettings.Default.EndDate;
            // If date not specified in config date value has DateTime.MinValue
            if (startDate != DateTime.MinValue && endDate != DateTime.MinValue)
            {
                return GetDaysBetweenTwoDates(startDate, endDate).ToList();
            }
            else
            {
                var daysInterval = VcdSettings.Default.DaysInterval != 0 ? VcdSettings.Default.DaysInterval : defaultDaysIntervalToProcess;
                var intervalEndDate = DateTime.Today.AddDays(-1);
                var intervalStartDate = DateTime.Today.AddDays(-daysInterval);
                return GetDaysBetweenTwoDates(intervalStartDate, intervalEndDate).ToList();
            }
        }

        public int GetAccountId()
        {
            try
            {
                return VcdSettings.Default.AccountId;
            }
            catch
            {
                return 0;
            }
        }

        public string GetCookiesDirectoryPath()
        {
            return VcdSettings.Default.CookiesDirectory;
        }

        private IEnumerable<DateTime> GetDaysBetweenTwoDates(DateTime from, DateTime thru)
        {
            for (var day = from.Date; day.Date <= thru.Date; day = day.AddDays(1))
                yield return day;
        }
    }
}
