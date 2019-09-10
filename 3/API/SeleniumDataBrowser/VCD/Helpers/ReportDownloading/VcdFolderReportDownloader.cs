using System.IO;
using SeleniumDataBrowser.Helpers;

namespace SeleniumDataBrowser.VCD.Helpers.ReportDownloading
{
    public class VcdFolderReportDownloader
    {
        private const string ReportsFolderName = "VcdReports";

        private readonly string reportFolderPath = PathToFileDirectoryHelper.GetAssemblyRelativePath(ReportsFolderName);

        public VcdFolderReportDownloader()
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