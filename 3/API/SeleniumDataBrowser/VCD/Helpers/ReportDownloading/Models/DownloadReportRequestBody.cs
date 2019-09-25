using System.Collections.Generic;

namespace SeleniumDataBrowser.VCD.Helpers.ReportDownloading.Models
{
    //auto generated class from json value from amazon sales diagnostic page
    internal class SalesDiagnosticDetail
    {
        public string requestId { get; set; }

        public List<ReportParameter> reportParameters { get; set; }

        public ReportPaginationWithOrderParameter reportPaginationWithOrderParameter { get; set; }
    }

    internal class ReportPaginationWithOrderParameter
    {
        public List<ReportOrder> reportOrders { get; set; }

        public ReportPagination reportPagination { get; set; }
    }

    internal class ReportOrder
    {
        public string columnId { get; set; }

        public bool ascending { get; set; }
    }

    internal class ReportPagination
    {
        public int pageIndex { get; set; }

        public int pageSize { get; set; }
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