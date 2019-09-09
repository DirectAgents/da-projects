using System;
using SeleniumDataBrowser.VCD.Helpers;
using SeleniumDataBrowser.VCD.Helpers.ReportDownloading;

namespace SeleniumDataBrowser.VCD.DataProviders
{
    /// <summary>
    /// Data provider for Vendor Central.
    /// </summary>
    public class VcdDataProvider : IVcdDataProvider
    {
        private static VcdDataProvider vcdDataProviderInstance;

        private readonly VcdLoginManager loginProcessManager;
        private VcdReportDownloader currentReportDownloader;

        private VcdDataProvider(VcdLoginManager loginProcessManager)
        {
            this.loginProcessManager = loginProcessManager;
        }

        /// <summary>
        /// Gets single instance (new or current) of the VCD data provider.
        /// </summary>
        /// <param name="loginProcessManager">Manager of login process.</param>
        /// <returns>Single instance (new or current) of the VCD data provider.</returns>
        public static VcdDataProvider GetVcdDataProviderInstance(VcdLoginManager loginProcessManager)
        {
            if (vcdDataProviderInstance == null)
            {
                vcdDataProviderInstance = new VcdDataProvider(loginProcessManager);
            }
            return vcdDataProviderInstance;
        }

        /// <summary>
        /// Sets the specified report downloader current for the VCD data provider instance.
        /// </summary>
        /// <param name="reportDownloader">Downloader of reports.</param>
        public void SetReportDownloaderCurrentForDataProvider(VcdReportDownloader reportDownloader)
        {
            currentReportDownloader = reportDownloader;
        }

        /// <summary>
        /// Login to the Amazon Advertiser Portal and saving cookies.
        /// </summary>
        public void LoginToPortal()
        {
            try
            {
                loginProcessManager.LoginToAmazonPortal();
            }
            catch (Exception e)
            {
                throw new Exception("Failed to login to Amazon Advertiser Portal.", e);
            }
        }

        /// <summary>
        /// Downloads the CSV Shipped Revenue report.
        /// </summary>
        /// <param name="reportDay">Day for report.</param>
        /// <returns>Text of report content.</returns>
        public string DownloadShippedRevenueCsvReport(DateTime reportDay)
        {
            return currentReportDownloader.DownloadShippedRevenueCsvReport(reportDay);
        }

        /// <summary>
        /// Downloads the CSV Shipped COGS report.
        /// </summary>
        /// <param name="reportDay">Day for report.</param>
        /// <returns>Text of report content.</returns>
        public string DownloadShippedCogsCsvReport(DateTime reportDay)
        {
            return currentReportDownloader.DownloadShippedCogsCsvReport(reportDay);
        }

        /// <summary>
        /// Downloads the CSV Ordered Revenue report.
        /// </summary>
        /// <param name="reportDay">Day for report.</param>
        /// <returns>Text of report content.</returns>
        public string DownloadOrderedRevenueCsvReport(DateTime reportDay)
        {
            return currentReportDownloader.DownloadOrderedRevenueCsvReport(reportDay);
        }
    }
}