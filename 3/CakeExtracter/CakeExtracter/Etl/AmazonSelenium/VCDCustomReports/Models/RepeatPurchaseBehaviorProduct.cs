using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CakeExtracter.Etl.AmazonSelenium.VCDCustomReports.Models
{
    public class RepeatPurchaseBehaviorProduct
    {
        public string Asin
        {
            get;
            set;
        }

        public string Name
        {
            get;
            set;
        }

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
