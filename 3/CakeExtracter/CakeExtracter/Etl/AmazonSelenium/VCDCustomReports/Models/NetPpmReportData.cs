using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CakeExtracter.Etl.AmazonSelenium.VCDCustomReports.Models
{
    public class NetPpmReportData
    {
        public DateTime StartDate
        {
            get;
            set;
        }

        public DateTime EndDate
        {
            get;
            set;
        }

        public List<NetPpmProduct> Products
        {
            get;
            set;
        }
    }
}
