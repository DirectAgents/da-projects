using CakeExtracter.Common;
using CakeExtracter.Etl.AmazonSelenium.VCD.Extractors.VcdExtractionHelpers.ReportDataComposer;
using CakeExtracter.Etl.AmazonSelenium.VCDCustomReports.Extractors.VcdCustomReportsExtractionHelpers.ReportParsing;
using CakeExtracter.Etl.AmazonSelenium.VCDCustomReports.Models.Base;
using DirectAgents.Domain.Entities.CPProg;
using SeleniumDataBrowser.VCD;
using SeleniumDataBrowser.VCD.Enums;
using SeleniumDataBrowser.VCD.Models;

namespace CakeExtracter.Etl.AmazonSelenium.VCDCustomReports.Models
{
    internal class VcdCustomReportExtractorParameters<TProduct>
        where TProduct : VcdCustomProduct
    {
        public VcdDataProvider DataProvider { get; set; }

        public VcdCustomReportParser<TProduct> ReportParser { get; set; }

        public VcdReportComposer ReportComposer { get; set; }

        public ExtAccount Account { get; set; }

        public VcdAccountInfo AccountInfo { get; set; }

        public PeriodType Period { get; set; }

        public ReportType ReportType { get; set; }

        public DateRange DateRange { get; set; }
    }
}