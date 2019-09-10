using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using Microsoft.Practices.EnterpriseLibrary.Common.Utility;
using SeleniumDataBrowser.Helpers;
using SeleniumDataBrowser.VCD.Helpers;
using SeleniumDataBrowser.VCD.Helpers.ReportDownloading;

namespace SeleniumDataBrowser.VCD.DataProviders
{
    public class VcdManualDataProvider : IVcdDataProvider
    {
        private const string ShippedRevenueReportName = "ShippedRevenue";
        private const string ShippedCogsReportName = "ShippedCogs";
        private const string OrderedRevenueReportName = "OrderedRevenue";

        private VcdFolderReportDownloader currentReportDownloader;
        private readonly Dictionary<DateTime, DirectoryInfo> reportDaysFolder = new Dictionary<DateTime, DirectoryInfo>();
        private readonly SeleniumLogger logger;

        public VcdManualDataProvider(SeleniumLogger logger)
        {
            this.logger = logger;
        }

        public void SetReportDownloaderCurrentForDataProvider(
            VcdFolderReportDownloader reportDownloader,
            IEnumerable<DirectoryInfo> daysFoldersForAccount)
        {
            currentReportDownloader = reportDownloader;
            daysFoldersForAccount.ForEach(dayFolder =>
            {
                if (DateTime.TryParseExact(dayFolder.Name, "MMddyyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out var reportFolderDay))
                {
                    reportDaysFolder.Add(reportFolderDay.Date, dayFolder);
                }
            });
        }

        public string DownloadShippedRevenueCsvReport(DateTime reportDay)
        {
            logger.LogInfo("Amazon VCD, Attempt to download shipped revenue report.");
            return reportDaysFolder.ContainsKey(reportDay)
                ? GetReportContent(reportDay, ShippedRevenueReportName)
                : string.Empty;
        }

        public string DownloadShippedCogsCsvReport(DateTime reportDay)
        {
            logger.LogInfo("Amazon VCD, Attempt to download shipped cogs report.");
            return reportDaysFolder.ContainsKey(reportDay)
                ? GetReportContent(reportDay, ShippedCogsReportName)
                : string.Empty;
        }

        public string DownloadOrderedRevenueCsvReport(DateTime reportDay)
        {
            logger.LogInfo("Amazon VCD, Attempt to download ordered revenue report.");
            return reportDaysFolder.ContainsKey(reportDay)
                ? GetReportContent(reportDay, OrderedRevenueReportName)
                : string.Empty;
        }

        private string GetReportContent(DateTime reportDay, string reportName)
        {
            var fullPathToReport = GetFullPathToRelatedReport(reportDay, reportName);
            return currentReportDownloader.GetReportContent(fullPathToReport);
        }

        private string GetFullPathToRelatedReport(DateTime reportDay, string reportName)
        {
            var relatedDayFolder = reportDaysFolder[reportDay].FullName;
            var relatedReports = VcdReportFolderHelper.GetFilesFromDirectory(relatedDayFolder);
            var fullPathToReport = relatedReports.FirstOrDefault(r => r.Contains(reportName));
            return fullPathToReport;
        }
    }
}