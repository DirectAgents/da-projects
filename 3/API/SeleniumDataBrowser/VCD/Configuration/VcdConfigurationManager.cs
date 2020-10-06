using System;
using System.Web.Configuration;

namespace SeleniumDataBrowser.VCD.Configuration
{
    /// <summary>
    /// Vcd configuration manager. Prepare config values for using in application.
    /// </summary>
    public static class VcdConfigurationManager
    {
        private const int DefaultPageSizeValue = 100;
        private const int DefaultDelay = 10;
        private const int DefaultAttemptCount = 5;
        private const string ReportDownloadingStartedDelayConfigurationKey = "VCD_ReportDownloadingStartedDelayInSeconds";
        private const string MinDelayBetweenReportDownloadingConfigurationKey = "VCD_MinDelayBetweenReportDownloadingInSeconds";
        private const string MaxDelayBetweenReportDownloadingConfigurationKey = "VCD_MaxDelayBetweenReportDownloadingInSeconds";
        private const string ReportDownloadingAttemptCountConfigurationKey = "VCD_ReportDownloadingAttemptCount";
        private const string MaxPageSizeForReportConfigurationKey = "VCD_MaxPageSizeForReport";

        /// <summary>
        /// Gets the number of seconds for delay before start downloading a report.
        /// </summary>
        /// <returns>Number of seconds for delay before start downloading report.</returns>
        public static int GetReportDownloadingStartedDelay()
        {
            try
            {
                return int.Parse(WebConfigurationManager.AppSettings[ReportDownloadingStartedDelayConfigurationKey]);
            }
            catch
            {
                return DefaultDelay;
            }
        }

        /// <summary>
        /// Gets the number of seconds for minimum delay between downloading reports.
        /// </summary>
        /// <returns>Number of seconds for minimum delay between downloading reports.</returns>
        public static int GetMinDelayBetweenReportDownloading()
        {
            try
            {
                return int.Parse(WebConfigurationManager.AppSettings[MinDelayBetweenReportDownloadingConfigurationKey]);
            }
            catch
            {
                return DefaultDelay;
            }
        }

        /// <summary>
        /// Gets the number of seconds for maximum delay between downloading reports.
        /// </summary>
        /// <returns>Number of seconds for maximum delay between downloading reports.</returns>
        public static int GetMaxDelayBetweenReportDownloading()
        {
            try
            {
                return int.Parse(WebConfigurationManager.AppSettings[MaxDelayBetweenReportDownloadingConfigurationKey]);
            }
            catch
            {
                return DefaultDelay;
            }
        }

        /// <summary>
        /// Gets the count of attempts downloading a report.
        /// </summary>
        /// <returns>Count of attempts downloading a report.</returns>
        public static int GetReportDownloadingAttemptCount()
        {
            try
            {
                return int.Parse(WebConfigurationManager.AppSettings[ReportDownloadingAttemptCountConfigurationKey]);
            }
            catch (Exception e)
            {
                return DefaultAttemptCount;
            }
        }

        /// <summary>
        /// Gets the number of report rows that will be return in one response.
        /// </summary>
        /// <returns>Number of report rows that will be return in one response.</returns>
        public static int GetMaxPageSizeForReport()
        {
            try
            {
                return int.Parse(WebConfigurationManager.AppSettings[MaxPageSizeForReportConfigurationKey]);
            }
            catch
            {
                return DefaultPageSizeValue;
            }
        }
    }
}