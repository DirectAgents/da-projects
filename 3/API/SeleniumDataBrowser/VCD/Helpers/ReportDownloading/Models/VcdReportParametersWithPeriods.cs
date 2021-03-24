using SeleniumDataBrowser.VCD.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SeleniumDataBrowser.VCD.Helpers.ReportDownloading.Models
{
    public class VcdReportParametersWithPeriods : VcdBaseReportParameters
    { 
        public string Period { get; set; }
    }
}
