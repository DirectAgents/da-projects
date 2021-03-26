using SeleniumDataBrowser.VCD.Enums;

namespace SeleniumDataBrowser.VCD.Helpers.ReportDownloading.Models
{
    public abstract class VcdBaseReportParameters
    {
        public ReportType ReportType { get; set; }

        public string RequestId { get; set; }

        public string ReportId { get; set; }

        public string ReportLevel { get; set; }

        public int PageIndex { get; set; }
    }
}
