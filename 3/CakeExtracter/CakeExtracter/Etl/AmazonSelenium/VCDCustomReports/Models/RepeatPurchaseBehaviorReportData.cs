using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CakeExtracter.Etl.AmazonSelenium.VCDCustomReports.Models
{
    public class RepeatPurchaseBehaviorReportData
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

        public List<RepeatPurchaseBehaviorProduct> Products
        {
            get;
            set;
        }
    }
}
