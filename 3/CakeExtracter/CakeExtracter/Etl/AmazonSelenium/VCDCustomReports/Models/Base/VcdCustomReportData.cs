using System.Collections.Generic;
using CakeExtracter.Common;
using SeleniumDataBrowser.VCD.Enums;

namespace CakeExtracter.Etl.AmazonSelenium.VCDCustomReports.Models.Base
{
    public class VcdCustomReportData<TProduct>
        where TProduct : VcdCustomProduct
    {
        public PeriodType PeriodType { get; set; }

        public ReportType ReportType { get; set; }

        public DateRange ReportDateRange { get; set; }

        public List<TProduct> Products { get; set; }
    }
}