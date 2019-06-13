using System.Collections.Generic;

namespace SeleniumDataBrowser.VCD.Helpers.ReportDownloading.Models
{
    //auto generated class from json value from amazon sales diagnostic page
    internal class DownloadReportRequestBody
    {
        public SalesDiagnosticDetail salesDiagnosticDetail { get; set; }
    }

    //auto generated class from json value from amazon sales diagnostic page
    internal class SalesDiagnosticDetail
    {
        public string requestId { get; set; }
        public string reportId { get; set; }
        public List<ReportParameter> reportParameters { get; set; }
        public Dictionary<string, List<string>> visibleFilters;
    }

    //auto generated class from json value from amazon sales diagnostic page
    internal class ReportParameter
    {
        public string parameterId { get; set; }
        public List<Value> values { get; set; }
    }

    //auto generated class from json value from amazon sales diagnostic page
    internal class Value
    {
        public object val { get; set; }
    }
}
