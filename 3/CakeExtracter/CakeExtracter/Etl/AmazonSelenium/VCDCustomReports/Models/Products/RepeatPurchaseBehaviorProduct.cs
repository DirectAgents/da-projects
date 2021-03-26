using System.Collections.Generic;
using System.Linq;
using CakeExtracter.Etl.AmazonSelenium.VCDCustomReports.Models.Base;
using CakeExtracter.Etl.AmazonSelenium.VCDCustomReports.Models.Interface;

namespace CakeExtracter.Etl.AmazonSelenium.VCDCustomReports.Models.Products
{
    public class RepeatPurchaseBehaviorProduct : VcdCustomProduct, ISumMetrics<RepeatPurchaseBehaviorProduct>
    {
        public int UniqueCustomers
        {
            get;
            set;
        }

        public decimal RepeatPurchaseRevenue
        {
            get;
            set;
        }

        public decimal RepeatPurchaseRevenuePriorPeriod
        {
            get;
            set;
        }

        public void SetMetrics(IEnumerable<RepeatPurchaseBehaviorProduct> items)
        {
            var shippingItems = items.ToList();
            UniqueCustomers = shippingItems.Sum(p => p.UniqueCustomers);
            RepeatPurchaseRevenue = shippingItems.Sum(p => p.RepeatPurchaseRevenue);
            RepeatPurchaseRevenuePriorPeriod = shippingItems.Sum(p => p.RepeatPurchaseRevenuePriorPeriod);
        }
    }
}