using System.IO;
using SeleniumDataBrowser.Helpers;

namespace SeleniumDataBrowser.VCD.Helpers.ReportDownloading
{
    public class VcdFolderReportDownloader
    {
        public VcdFolderReportDownloader(string reportFolderPath)
        {
            PathToFileDirectoryHelper.CreateDirectoryIfNotExist(reportFolderPath);
        }

        public string GetReportContent(string reportFilePath)
        {
            var reportContent = File.ReadAllText(reportFilePath);
            return reportContent;
        }
    }
}