using System.Collections.Generic;
using System.Linq;
using CakeExtracter.Etl.AmazonSelenium.VCDCustomReports.Models.Base;
using CakeExtracter.Etl.AmazonSelenium.VCDCustomReports.Models.Interface;

namespace CakeExtracter.Etl.AmazonSelenium.VCDCustomReports.Models.Products
{
    public class MarketBasketAnalysisProduct : VcdCustomProduct, ISumMetrics<MarketBasketAnalysisProduct>
    {
        public string N1PurchasedAsin
        {
            get;
            set;
        }

        public string N1PurchasedTitle
        {
            get;
            set;
        }

        public decimal N1Combination
        {
            get;
            set;
        }

        public string N2PurchasedAsin
        {
            get;
            set;
        }

        public string N2PurchasedTitle
        {
            get;
            set;
        }

        public decimal N2Combination
        {
            get;
            set;
        }

        public string N3PurchasedAsin
        {
            get;
            set;
        }

        public string N3PurchasedTitle
        {
            get;
            set;
        }

        public decimal N3Combination
        {
            get;
            set;
        }

        public void SetMetrics(IEnumerable<MarketBasketAnalysisProduct> items)
        {
            var shippingItems = items.ToList();
            N1Combination = shippingItems.Sum(p => p.N1Combination);
            N2Combination = shippingItems.Sum(p => p.N2Combination);
            N3Combination = shippingItems.Sum(p => p.N3Combination);
        }
    }
}
