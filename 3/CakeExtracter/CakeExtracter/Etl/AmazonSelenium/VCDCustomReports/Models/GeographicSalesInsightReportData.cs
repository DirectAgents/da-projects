using CakeExtracter.Etl.AmazonSelenium.VCD.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CakeExtracter.Etl.AmazonSelenium.VCDCustomReports.Models
{
    class VcdGeographicSalesInsightsReportData
    {
        public DateTime Date
        {
            get;
            set;
        }

        public List<GeographicSalesInsightsProduct> Products
        {
            get;
            set;
        }
    }
}
