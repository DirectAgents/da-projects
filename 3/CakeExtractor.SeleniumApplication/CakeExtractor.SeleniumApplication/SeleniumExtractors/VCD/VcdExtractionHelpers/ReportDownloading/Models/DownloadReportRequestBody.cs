using System.Collections.Generic;

namespace CakeExtractor.SeleniumApplication.Models.Vcd
{
    internal class DownloadReportRequestBody
    {
        public SalesDiagnosticDetail salesDiagnosticDetail { get; set; }
    }

    internal class SalesDiagnosticDetail
    {
        public string requestId { get; set; }
        public string reportId { get; set; }
        public List<ReportParameter> reportParameters { get; set; }
        public Dictionary<string, List<string>> visibleFilters;
    }

    internal class ReportParameter
    {
        public string parameterId { get; set; }
        public List<Value> values { get; set; }
    }

    internal class Value
    {
        public object val { get; set; }
    }
}
