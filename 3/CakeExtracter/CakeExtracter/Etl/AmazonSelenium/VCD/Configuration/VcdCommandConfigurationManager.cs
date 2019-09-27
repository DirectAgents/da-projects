using System.Configuration;
using CakeExtracter.Etl.AmazonSelenium.Configuration;
using CakeExtracter.Helpers;
using SeleniumDataBrowser.Helpers;

namespace CakeExtracter.Etl.AmazonSelenium.VCD.Configuration
{
    /// <inheritdoc />
    /// <summary>
    /// Vcd configuration helper. Prepare config values for using in application.
    /// </summary>
    internal class VcdCommandConfigurationManager : SeleniumCommandConfigurationManager
    {
        private const int DefaultIntervalBetweenUnsuccessfulAndNewRequestsInMinutes = 30;
        private const int DefaultPageSizeValue = 100;
        private const string ReportDownloadingStartedDelayConfigurationKey = "VCD_ReportDownloadingStartedDelayInSeconds";
        private const string MinDelayBetweenReportDownloadingConfigurationKey = "VCD_MinDelayBetweenReportDownloadingInSeconds";
        private const string MaxDelayBetweenReportDownloadingConfigurationKey = "VCD_MaxDelayBetweenReportDownloadingInSeconds";
        private const string ReportDownloadingAttemptCountConfigurationKey = "VCD_ReportDownloadingAttemptCount";
        private const string ExtractDailyDataAttemptCountConfigurationKey = "VCD_ExtractDailyDataAttemptCount";
        private const string IntervalBetweenUnsuccessfulAndNewRequestsConfigurationKey = "VCD_IntervalBetweenUnsuccessfulAndNewRequestsInMinutes";
        private const string MaxPageSizeForReportConfigurationKey = "VCD_MaxPageSizeForReport";
        private const string SyncScriptPathConfigurationKey = "VCD_SyncScriptPath";
        private const string SyncScriptNameConfigurationKey = "VCD_SyncScriptName";

        /// <summary>
        /// Gets the full path to sync SQL script.
        /// </summary>
        /// <returns>Full path to sync SQL script.</returns>
        public static string GetSyncScriptPath()
        {
            var path = ConfigurationManager.AppSettings[SyncScriptPathConfigurationKey];
            var name = ConfigurationManager.AppSettings[SyncScriptNameConfigurationKey];
            return PathToFileDirectoryHelper.GetAssemblyRelativePath(PathToFileDirectoryHelper.CombinePath(path, name));
        }

        /// <summary>
        /// Gets the number of minutes of interval between unsuccessful and new job execution requests
        /// from the config application setting.
        /// </summary>
        /// <returns>Number minutes of interval between unsuccessful and new job execution requests.</returns>
        public static int GetIntervalBetweenUnsuccessfulAndNewRequest()
        {
            return ConfigurationHelper.GetIntConfigurationValue(
                IntervalBetweenUnsuccessfulAndNewRequestsConfigurationKey,
                DefaultIntervalBetweenUnsuccessfulAndNewRequestsInMinutes);
        }

        /// <summary>
        /// Gets the number of seconds for delay before start downloading a report.
        /// </summary>
        /// <returns>Number of seconds for delay before start downloading report.</returns>
        public static int GetReportDownloadingStartedDelay()
        {
            return ConfigurationHelper.GetIntConfigurationValue(ReportDownloadingStartedDelayConfigurationKey);
        }

        /// <summary>
        /// Gets the number of seconds for minimum delay between downloading reports.
        /// </summary>
        /// <returns>Number of seconds for minimum delay between downloading reports.</returns>
        public static int GetMinDelayBetweenReportDownloading()
        {
            return ConfigurationHelper.GetIntConfigurationValue(MinDelayBetweenReportDownloadingConfigurationKey);
        }

        /// <summary>
        /// Gets the number of seconds for maximum delay between downloading reports.
        /// </summary>
        /// <returns>Number of seconds for maximum delay between downloading reports.</returns>
        public static int GetMaxDelayBetweenReportDownloading()
        {
            return ConfigurationHelper.GetIntConfigurationValue(MaxDelayBetweenReportDownloadingConfigurationKey);
        }

        /// <summary>
        /// Gets the count of attempts downloading a report.
        /// </summary>
        /// <returns>Count of attempts downloading a report.</returns>
        public static int GetReportDownloadingAttemptCount()
        {
            return ConfigurationHelper.GetIntConfigurationValue(ReportDownloadingAttemptCountConfigurationKey);
        }

        /// <summary>
        /// Gets the count of attempts extracting daily data.
        /// </summary>
        /// <returns>Count of attempts extracting daily data.</returns>
        public static int GetExtractDailyDataAttemptCount()
        {
            return ConfigurationHelper.GetIntConfigurationValue(ExtractDailyDataAttemptCountConfigurationKey);
        }

        /// <summary>
        /// Gets the number of report rows that will be return in one response.
        /// </summary>
        /// <returns>Number of report rows that will be return in one response.</returns>
        public static int GetMaxPageSizeForReport()
        {
            return ConfigurationHelper.GetIntConfigurationValue(MaxPageSizeForReportConfigurationKey, DefaultPageSizeValue);
        }
    }
}