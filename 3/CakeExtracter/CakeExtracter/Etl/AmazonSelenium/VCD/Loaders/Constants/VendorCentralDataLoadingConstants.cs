using System.Collections.Generic;

namespace CakeExtracter.Etl.AmazonSelenium.VCD.Loaders.Constants
{
    internal class VendorCentralDataLoadingConstants
    {
        public const string ShippedRevenueMetricName = "vendorShippedRevenue";

        public const string ShippedUnitsMetricName = "vendorShippedUnits";

        public const string OrderedUnitsMetricName = "vendorOrderedUnits";

        public const string ShippedCogsMetricName = "vendorShippedCogs";

        public const string FreeReplacementMetricName = "vendorFreeReplacement";

        public const string CustomerReturnsMetricName = "vendorCustomerReturns";

        public const string OrderedRevenueMetricName = "vendorOrderedRevenue";

        public const int VendorMetricsDaysInterval = 1;

        public static readonly List<string> VendorMetricTypeNames = new List<string>
        {
            ShippedRevenueMetricName,
            OrderedUnitsMetricName,
            ShippedUnitsMetricName,
            ShippedCogsMetricName,
            FreeReplacementMetricName,
            CustomerReturnsMetricName,
            OrderedRevenueMetricName,
        };
    }
}
