using System;
using SeleniumDataBrowser.VCD.Enums;

namespace SeleniumDataBrowser.VCD.Helpers.ReportDownloading.Models
{
    public class VcdReportParameters : VcdBaseReportParameters
    {
        public DateTime ReportStartDate { get; set; }

        public DateTime ReportEndDate { get; set; }

        public PeriodType Period { get; set; }
    }
}