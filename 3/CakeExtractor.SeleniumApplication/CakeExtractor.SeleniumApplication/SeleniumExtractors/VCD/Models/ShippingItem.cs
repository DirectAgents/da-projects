using System.Collections.Generic;
using System.Linq;

namespace CakeExtractor.SeleniumApplication.SeleniumExtractors.VCD.Models
{
    internal class ShippingItem
    {
        public string Name
        {
            get;
            set;
        }

        public decimal ShippedRevenue
        {
            get;
            set;
        }

        public int ShippedUnits
        {
            get;
            set;
        }

        public int OrderedUnits
        {
            get;
            set;
        }

        public decimal ShippedCogs
        {
            get;
            set;
        }

        public int CustomerReturns
        {
            get;
            set;
        }

        public int FreeReplacements
        {
            get;
            set;
        }

        public decimal OrderedRevenue
        {
            get;
            set;
        }

        public void SetMetrics<T>(IEnumerable<T> items)
            where T : ShippingItem
        {
            OrderedUnits = items.Sum(p => p.OrderedUnits);
            ShippedUnits = items.Sum(p => p.ShippedUnits);
            ShippedRevenue = items.Sum(p => p.ShippedRevenue);
            CustomerReturns = items.Sum(p => p.CustomerReturns);
            FreeReplacements = items.Sum(p => p.FreeReplacements);
            ShippedCogs = items.Sum(p => p.ShippedCogs);
            OrderedRevenue = items.Sum(p => p.OrderedRevenue);
        }
    }
}
