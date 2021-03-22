using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CakeExtracter.Etl.AmazonSelenium.VCDCustomReports.Models
{
    public class NetPpmProduct
    {
        public string Asin
        {
            get;
            set;
        }

        public string Subcategory
        {
            get;
            set;
        }

        public string Name
        {
            get;
            set;
        }

        public decimal NetPpm
        {
            get;
            set;
        }

        public decimal NetPpmPriorYearPercentChange
        {
            get;
            set;
        }

        public void SetMetrics(IEnumerable<NetPpmProduct> items)
        {
            var shippingItems = items.ToList();
            NetPpm = shippingItems.Sum(p => p.NetPpm);
            NetPpmPriorYearPercentChange = shippingItems.Sum(p => p.NetPpmPriorYearPercentChange);
        }
    }
}
