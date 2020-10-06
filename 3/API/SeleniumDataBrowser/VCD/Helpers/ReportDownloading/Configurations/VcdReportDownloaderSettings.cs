namespace SeleniumDataBrowser.VCD.Helpers.ReportDownloading.Configurations
{
    /// <summary>
    /// Represents configuration settings for <see cref="VcdReportDownloader"/>.
    /// </summary>
    public class VcdReportDownloaderSettings
    {
        /// <summary>
        /// Gets or sets delay in seconds for a started report downloading.
        /// </summary>
        public int ReportDownloadingStartedDelayInSeconds { get; set; }

        /// <summary>
        /// Gets or sets the minimum delay in seconds between report downloads.
        /// </summary>
        public int MinDelayBetweenReportDownloadingInSeconds { get; set; }

        /// <summary>
        /// Gets or sets the maximum delay in seconds between report downloads.
        /// </summary>
        public int MaxDelayBetweenReportDownloadingInSeconds { get; set; }

        /// <summary>
        /// Gets or sets count of report downloading attempts.
        /// </summary>
        public int ReportDownloadingAttemptCount { get; set; }

        /// <summary>
        /// Gets or sets the maximum page size for a report.
        /// </summary>
        public int MaxPageSizeForReport { get; set; }
    }
}