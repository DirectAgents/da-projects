using System.Collections.Generic;

namespace CakeExtracter.Etl.DSP.Loaders.Constants
{
    internal class DspMetricConstants
    {
        public const string TotalCostMetricName = "dspTotalCost";

        public const string ImpressionsMetricName = "dspImpressions";

        public const string ClickThroughsMetricName = "dspClickThroughs";

        public const string TotalPixelEventsMetricName = "dspTotalPixelEvents";

        public const string TotalPixelEventsViewsMetricName = "dspTotalPixelEventsViews";

        public const string TotalPixelEventsClicksMetricName = "dspTotalPixelEventsClicks";

        public const string DPVMetricName = "dspDPV";

        public const string ATCMetricName = "dspATC";

        public const string PurchaseMetricName = "dspPurchase";

        public const string PurchaseViewsMetricName = "dspPurchaseViews";

        public const string PurchaseClicksMetricName = "dspPurchaseClicks";

        public const int VendorMetricsDaysInterval = 1;

        public static readonly List<string> MetricTypeNames = new List<string>()
        {
            TotalCostMetricName,
            ImpressionsMetricName,
            ClickThroughsMetricName,
            TotalPixelEventsMetricName,
            TotalPixelEventsViewsMetricName,
            TotalPixelEventsClicksMetricName,
            DPVMetricName,
            ATCMetricName,
            PurchaseMetricName,
            PurchaseViewsMetricName,
            PurchaseClicksMetricName
        };
    }
}
