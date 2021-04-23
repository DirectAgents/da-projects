namespace CakeExtracter.Common
{
    public class AmazonCustomReportType
    {
        public const string AllArg = "ALL";
        public const string GeoSalesArg = "GEOSALES";
        public const string NetPpmArg = "NETPPM";
        public const string RepeatPurchaseArg = "REPEATPURCHASE";
        public const string MarketBasketArg = "MARKETBASKET";
        public const string AltPurchaseArg = "ALTPURCHASE";
        public const string ItemCompArg = "ITEMCOMP";

        public bool GeoSales { get; set; }

        public bool NetPpm { get; set; }

        public bool RepeatPurchase { get; set; }

        public bool MarketBasket { get; set; }

        public bool AlternativePurchase { get; set; }

        public bool ItemComparison { get; set; }

        public bool All => GeoSales && NetPpm && RepeatPurchase && MarketBasket && AlternativePurchase && ItemComparison;

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
            else if (statsTypeUpper.StartsWith(MarketBasketArg))
            {
                MarketBasket = true;
            }
            else if (statsTypeUpper.StartsWith(AltPurchaseArg))
            {
                AlternativePurchase = true;
            }
            else if (statsTypeUpper.StartsWith(ItemCompArg))
            {
                ItemComparison = true;
            }
        }

        public void SetAllTrue()
        {
            GeoSales = true;
            NetPpm = true;
            RepeatPurchase = true;
            MarketBasket = true;
            AlternativePurchase = true;
            ItemComparison = true;
        }

        public string GetStatsTypeString()
        {
            return GeoSales
                ? GeoSalesArg
                : NetPpm
                    ? NetPpmArg
                    : RepeatPurchase
                        ? RepeatPurchaseArg
                        : MarketBasket
                            ? MarketBasketArg
                            : AlternativePurchase
                                ? AltPurchaseArg
                                : ItemComparison
                                    ? ItemCompArg
                                    : AllArg;
        }
    }
}
