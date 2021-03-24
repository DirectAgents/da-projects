using System.Collections.Generic;
using System.Linq;
using CakeExtracter.Etl.AmazonSelenium.VCDCustomReports.Models.Base;
using CakeExtracter.Etl.AmazonSelenium.VCDCustomReports.Models.Interface;

namespace CakeExtracter.Etl.AmazonSelenium.VCDCustomReports.Models.Products
{
    public class NetPpmProduct : VcdCustomProduct, ISumMetrics<NetPpmProduct>
    {
        public string Subcategory
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