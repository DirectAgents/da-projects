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

        public const string LostBuyBoxMetricName = "vendorLostBuyBox";

        public const string RepOosMetricName = "vendorRepOos";

        public const string RepOosPercentOfTotalMetricName = "vendorRepOosPercentOfTotal";

        public const string RepOosPriorPeriodPercentChangeMetricName = "vendorRepOosPriorPeriodPercentChange";

        public const string GlanceViewsMetricName = "gvcurrenttotal";

        public const string SalesRank = "subcategoryrank";

        public const string AverageSalesPrice = "averagesellingpriceshippedunits";

        public const string SellableOnHandUnits = "sellableonhandunits";

        public const string NumberOfCustomerReviews = "numberofcustomerreviews";

        public const string NumberOfCustomerReviewsLifeToDate = "numberofcustomerreviewslifetodate";

        public const string AverageCustomerRating = "averagecustomerrating";

        public const string FiveStars = "fivestars";

        public const string FourStars = "fourstars";

        public const string ThreeStars = "threestars";

        public const string TwoStars = "twostars";

        public const string OneStar = "onestar";

        public const string SellThroughRate = "sellthroughrate";

        public const string OpenPurchaseOrderQuantity = "openpurchaseorderquantity";

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
            LostBuyBoxMetricName,
            RepOosMetricName,
            RepOosPercentOfTotalMetricName,
            RepOosPriorPeriodPercentChangeMetricName,
            GlanceViewsMetricName,
            SalesRank,
            AverageSalesPrice,
            SellableOnHandUnits,
            NumberOfCustomerReviews,
            NumberOfCustomerReviewsLifeToDate,
            AverageCustomerRating,
            FiveStars,
            FourStars,
            ThreeStars,
            TwoStars,
            OneStar,
            OpenPurchaseOrderQuantity,
            SellThroughRate,
        };

        public static readonly List<string> AllowedZeroValueMetricTypeNames = new List<string>
        {
            LostBuyBoxMetricName,
            RepOosMetricName,
            RepOosPercentOfTotalMetricName,
            RepOosPriorPeriodPercentChangeMetricName,
        };
    }
}