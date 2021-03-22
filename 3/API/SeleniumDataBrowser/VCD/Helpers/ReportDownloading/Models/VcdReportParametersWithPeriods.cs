using SeleniumDataBrowser.VCD.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SeleniumDataBrowser.VCD.Helpers.ReportDownloading.Models
{
    public class VcdReportParametersWithPeriods
    {
        public ReportType ReportType { get; set; }

        public string RequestId { get; set; }

        public string ReportId { get; set; }

        public string ReportLevel { get; set; }

        public int PageIndex { get; set; }

        public string Period { get; set; }
    }
}
