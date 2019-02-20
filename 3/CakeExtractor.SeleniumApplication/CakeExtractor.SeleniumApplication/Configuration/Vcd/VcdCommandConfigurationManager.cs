using CakeExtractor.SeleniumApplication.Properties;
using CakeExtracter.Common;
using CakeExtracter.Helpers;

namespace CakeExtractor.SeleniumApplication.Configuration.Vcd
{
    internal class VcdCommandConfigurationManager
    {
        private const int DefaultDaysIntervalToProcess = 30;

        public DateRange GetDaysToProcess()
        {
            var startDate = VcdSettings.Default.StartDate;
            var endDate = VcdSettings.Default.EndDate;
            return CommandHelper.GetDateRange(startDate, endDate, VcdSettings.Default.DaysInterval, DefaultDaysIntervalToProcess);
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
