using System;
using System.Collections.Generic;
using CakeExtractor.SeleniumApplication.Properties;
using CakeExtracter.Common;
using CakeExtracter.Helpers;
using Microsoft.Practices.EnterpriseLibrary.Common.Utility;

namespace CakeExtractor.SeleniumApplication.Configuration.Vcd
{
    internal class VcdCommandConfigurationManager
    {
        private const int DefaultDaysIntervalToProcess = 30;

        public IEnumerable<DateRange> GetDaysToProcess()
        {
            var daysInterval = ConfigurationHelper.ExtractNumbersFromConfigValue(VcdSettings.Default.DaysInterval);
            var dateRanges = new List<DateRange>
            {
                CommandHelper.GetDateRange(VcdSettings.Default.EndDate, VcdSettings.Default.StartDate, daysInterval[0], DefaultDaysIntervalToProcess)
            };
            for (var i = 1; i < daysInterval.Count; i++)
            {
                var dateRange = CommandHelper.GetDateRange(default(DateTime), default(DateTime), daysInterval[i], 0);
                dateRanges.Add(dateRange);
            }

            return dateRanges;
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

        public int GetRefreshPageTimeInterval()
        {
            try
            {
                return VcdSettings.Default.RefreshPageMinutesInterval;
            }
            catch
            {
                return 30;
            }
        }
    }
}
