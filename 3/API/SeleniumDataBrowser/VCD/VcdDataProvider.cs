using System;

using SeleniumDataBrowser.Helpers;
using SeleniumDataBrowser.Models;
using SeleniumDataBrowser.VCD.Configuration;
using SeleniumDataBrowser.VCD.Helpers;
using SeleniumDataBrowser.VCD.Helpers.ReportDownloading;
using SeleniumDataBrowser.VCD.Helpers.ReportDownloading.Configurations;
using SeleniumDataBrowser.VCD.Models;
using SeleniumDataBrowser.VCD.PageActions;

namespace SeleniumDataBrowser.VCD
{
    /// <summary>
    /// Data provider for Vendor Central.
    /// </summary>
    public class VcdDataProvider : IDisposable
    {
        private readonly AmazonVcdActionsWithPagesManager pagesManager;

        private readonly VcdLoginManager vcdLoginManager;

        private readonly AuthorizationModel authorizationModel;

        private readonly SeleniumLogger logger;

        private readonly VcdReportDownloaderSettings reportDownloaderSettings;

        private bool disposedValue;

        /// <summary>
        /// Initializes a new instance of the <see cref="VcdDataProvider"/> class.
        /// </summary>
        /// <param name="waitPageTimeOutInMinutes">Waiting time out for a page in minutes.</param>
        /// <param name="isHidingBrowser">Indicates whether to hide the browser window.</param>
        /// <param name="logger">Selenium data browser logger.</param>
        /// <param name="authorizationModel">Authorization settings.</param>
        public VcdDataProvider(int waitPageTimeOutInMinutes, bool isHidingBrowser, SeleniumLogger logger, AuthorizationModel authorizationModel)
        {
            pagesManager = new AmazonVcdActionsWithPagesManager(waitPageTimeOutInMinutes, isHidingBrowser, logger);
            this.logger = logger;
            this.authorizationModel = authorizationModel;
            reportDownloaderSettings = GetReportDownloaderSettings();
            vcdLoginManager = new VcdLoginManager(authorizationModel, pagesManager, logger);
            LoginToPortal();
        }

        /// <summary>
        /// Finalizes an instance of the <see cref="VcdDataProvider"/> class.
        /// </summary>
        ~VcdDataProvider()
        {
            // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
            Dispose(false);
        }

        /// <summary>
        /// Gets or sets Selenium logger with account id.
        /// </summary>
        public SeleniumLogger LoggerWithAccountId { get; set; }

        /// <summary>
        /// Downloads the CSV Shipped Revenue report.
        /// </summary>
        /// <param name="accountInfo">Selected account for a report.</param>
        /// <param name="reportDay">Day for report.</param>
        /// <returns>Text of report content.</returns>
        public string DownloadShippedRevenueCsvReport(VcdAccountInfo accountInfo, DateTime reportDay)
        {
            var reportDownloader = new VcdReportDownloader(accountInfo, pagesManager, authorizationModel, LoggerWithAccountId ?? logger, reportDownloaderSettings);
            return reportDownloader.DownloadShippedRevenueCsvReport(reportDay);
        }

        /// <summary>
        /// Downloads the CSV Shipped COGS report.
        /// </summary>
        /// <param name="accountInfo">Selected account for a report.</param>
        /// <param name="reportDay">Day for report.</param>
        /// <returns>Text of report content.</returns>
        public string DownloadShippedCogsCsvReport(VcdAccountInfo accountInfo, DateTime reportDay)
        {
            var reportDownloader = new VcdReportDownloader(accountInfo, pagesManager, authorizationModel, LoggerWithAccountId ?? logger, reportDownloaderSettings);
            return reportDownloader.DownloadShippedCogsCsvReport(reportDay);
        }

        /// <summary>
        /// Downloads the CSV Ordered Revenue report.
        /// </summary>
        /// <param name="accountInfo">Selected account for a report.</param>
        /// <param name="reportDay">Day for report.</param>
        /// <returns>Text of report content.</returns>
        public string DownloadOrderedRevenueCsvReport(VcdAccountInfo accountInfo, DateTime reportDay)
        {
            var reportDownloader = new VcdReportDownloader(accountInfo, pagesManager, authorizationModel, LoggerWithAccountId ?? logger, reportDownloaderSettings);
            return reportDownloader.DownloadOrderedRevenueCsvReport(reportDay);
        }

        /// <summary>
        /// Returns user info in JSON format.
        /// </summary>
        /// <returns>User info.</returns>
        public string GetUserInfoJson()
        {
            return pagesManager.GetUserInfoJson();
        }

        /// <inheritdoc/>
        public void Dispose()
        {
            // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Dispose managed and unmanaged resources.
        /// </summary>
        /// <param name="disposing">Indicates whether to dispose manages resources.</param>
        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                }

                pagesManager.Dispose();
                disposedValue = true;
            }
        }

        private static VcdReportDownloaderSettings GetReportDownloaderSettings()
        {
            var reportSettings = new VcdReportDownloaderSettings
            {
                MaxDelayBetweenReportDownloadingInSeconds = VcdConfigurationManager.GetMaxDelayBetweenReportDownloading(),
                MaxPageSizeForReport = VcdConfigurationManager.GetMaxPageSizeForReport(),
                MinDelayBetweenReportDownloadingInSeconds = VcdConfigurationManager.GetMinDelayBetweenReportDownloading(),
                ReportDownloadingAttemptCount = VcdConfigurationManager.GetReportDownloadingAttemptCount(),
                ReportDownloadingStartedDelayInSeconds = VcdConfigurationManager.GetReportDownloadingStartedDelay(),
            };
            return reportSettings;
        }

        private void LoginToPortal()
        {
            try
            {
                vcdLoginManager.LoginToAmazonPortal();
            }
            catch (Exception e)
            {
                const string LoginToPortalErrorMessage = "Failed to login to Amazon Advertiser Portal.";
                throw new Exception(LoginToPortalErrorMessage, e);
            }
        }
    }
}