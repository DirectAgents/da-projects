using System;
using System.IO;
using SeleniumDataBrowser.Helpers;

namespace SeleniumDataBrowser.VCD.DataProviders
{
    public class VcdManualDataProvider : IVcdDataProvider
    {
        private const string ReportsFolderName = "VcdReports";
        private readonly SeleniumLogger logger;
        private readonly string reportFolderPath;

        public VcdManualDataProvider(SeleniumLogger logger)
        {
            this.logger = logger;
            reportFolderPath = PathToFileDirectoryHelper.GetAssemblyRelativePath(ReportsFolderName);
            PathToFileDirectoryHelper.CreateDirectoryIfNotExist(reportFolderPath);
        }

        public string DownloadShippedRevenueCsvReport(DateTime reportDay)
        {
            logger.LogInfo("Amazon VCD, Attempt to download shipped revenue report.");
            return GetReportContent("1444_09012019_ShippedRevenue.csv");
        }

        public string DownloadShippedCogsCsvReport(DateTime reportDay)
        {
            logger.LogInfo("Amazon VCD, Attempt to download shipped cogs report.");
            return GetReportContent("1444_09012019_ShippedCogs.csv");
        }

        public string DownloadOrderedRevenueCsvReport(DateTime reportDay)
        {
            logger.LogInfo("Amazon VCD, Attempt to download ordered revenue report.");
            return GetReportContent("1444_09012019_OrderedRevenue.csv");
        }

        private string GetReportContent(string reportFileName)
        {
            var reportFilePath = PathToFileDirectoryHelper.CombinePath(reportFolderPath, reportFileName);
            var reportContent = File.ReadAllText(reportFilePath);
            return reportContent;
        }
    }
}