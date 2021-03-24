namespace CakeExtracter.Common
{
    public class AmazonCustomReportType
    {
        public const string AllArg = "ALL";
        public const string GeoSalesArg = "GEOSALES";
        public const string NetPpmArg = "NETPPM";
        public const string RepeatPurchaseArg = "REPEATPURCHASE";

        public bool GeoSales { get; set; }

        public bool NetPpm { get; set; }

        public bool RepeatPurchase { get; set; }

        public bool All => GeoSales && NetPpm && RepeatPurchase;

        public AmazonCustomReportType()
        {
        }

        public AmazonCustomReportType(string reportType)
        {
            var statsTypeUpper = (reportType == null) ? "" : reportType.ToUpper();
            if (string.IsNullOrWhiteSpace(statsTypeUpper) || statsTypeUpper == AllArg)
            {
                SetAllTrue();
            }
            else if (statsTypeUpper.StartsWith(GeoSalesArg))
            {
                GeoSales = true;
            }
            else if (statsTypeUpper.StartsWith(NetPpmArg))
            {
                NetPpm = true;
            }
            else if (statsTypeUpper.StartsWith(RepeatPurchaseArg))
            {
                RepeatPurchase = true;
            }
        }

        public void SetAllTrue()
        {
            GeoSales = true;
            NetPpm = true;
            RepeatPurchase = true;
        }

        public string GetStatsTypeString()
        {
            return GeoSales
                ? GeoSalesArg
                : NetPpm
                    ? NetPpmArg
                    : RepeatPurchase
                        ? RepeatPurchaseArg
                        : AllArg;
        }
    }
}