using System.Collections.Generic;
using System.Linq;
using CakeExtracter.Etl.AmazonSelenium.VCDCustomReports.Models.Base;
using CakeExtracter.Etl.AmazonSelenium.VCDCustomReports.Models.Interface;

namespace CakeExtracter.Etl.AmazonSelenium.VCDCustomReports.Models.Products
{
    public class AlternativePurchaseProduct : VcdCustomProduct, ISumMetrics<AlternativePurchaseProduct>
    {
        public string No1PurchasedAsin
        {
            get;
            set;
        }

        public string No1PurchasedProductTitle
        {
            get;
            set;
        }

        public decimal No1PurchasedPercent
        {
            get;
            set;
        }

        public string No2PurchasedAsin
        {
            get;
            set;
        }

        public string No2PurchasedProductTitle
        {
            get;
            set;
        }

        public decimal No2PurchasedPercent
        {
            get;
            set;
        }

        public string No3PurchasedAsin
        {
            get;
            set;
        }

        public string No3PurchasedProductTitle
        {
            get;
            set;
        }

        public decimal No3PurchasedPercent
        {
            get;
            set;
        }

        public string No4PurchasedAsin
        {
            get;
            set;
        }

        public string No4PurchasedProductTitle
        {
            get;
            set;
        }

        public decimal No4PurchasedPercent
        {
            get;
            set;
        }

        public string No5PurchasedAsin
        {
            get;
            set;
        }

        public string No5PurchasedProductTitle
        {
            get;
            set;
        }

        public decimal No5PurchasedPercent
        {
            get;
            set;
        }

        public void SetMetrics(IEnumerable<AlternativePurchaseProduct> items)
        {
            var shippingItems = items.ToList();
            No1PurchasedPercent = shippingItems.Sum(p => p.No1PurchasedPercent);
            No2PurchasedPercent = shippingItems.Sum(p => p.No2PurchasedPercent);
            No3PurchasedPercent = shippingItems.Sum(p => p.No3PurchasedPercent);
            No4PurchasedPercent = shippingItems.Sum(p => p.No4PurchasedPercent);
            No5PurchasedPercent = shippingItems.Sum(p => p.No5PurchasedPercent);
        }
    }
}
