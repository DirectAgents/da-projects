using System;
using SeleniumDataBrowser.VCD.Enums;

namespace SeleniumDataBrowser.VCD.Helpers.ReportDownloading.Models
{
    public class VcdReportParameters : VcdBaseReportParameters
    {
        public DateTime ReportDate { get; set; }
    }
}