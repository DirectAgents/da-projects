using System.Collections.Generic;
using System.Linq;
using CakeExtracter.Etl.AmazonSelenium.VCDCustomReports.Models.Base;
using CakeExtracter.Etl.AmazonSelenium.VCDCustomReports.Models.Interface;

namespace CakeExtracter.Etl.AmazonSelenium.VCDCustomReports.Models.Products
{
    public class ItemComparisonProduct : VcdCustomProduct, ISumMetrics<ItemComparisonProduct>
    {
        public string No1ComparedAsin
        {
            get;
            set;
        }

        public string No1ComparedProductTitle
        {
            get;
            set;
        }

        public decimal No1ComparedPercent
        {
            get;
            set;
        }

        public string No2ComparedAsin
        {
            get;
            set;
        }

        public string No2ComparedProductTitle
        {
            get;
            set;
        }

        public decimal No2ComparedPercent
        {
            get;
            set;
        }

        public string No3ComparedAsin
        {
            get;
            set;
        }

        public string No3ComparedProductTitle
        {
            get;
            set;
        }

        public decimal No3ComparedPercent
        {
            get;
            set;
        }

        public string No4ComparedAsin
        {
            get;
            set;
        }

        public string No4ComparedProductTitle
        {
            get;
            set;
        }

        public decimal No4ComparedPercent
        {
            get;
            set;
        }

        public string No5ComparedAsin
        {
            get;
            set;
        }

        public string No5ComparedProductTitle
        {
            get;
            set;
        }

        public decimal No5ComparedPercent
        {
            get;
            set;
        }

        public void SetMetrics(IEnumerable<ItemComparisonProduct> items)
        {
            var shippingItems = items.ToList();
            No1ComparedPercent = shippingItems.Sum(p => p.No1ComparedPercent);
            No2ComparedPercent = shippingItems.Sum(p => p.No2ComparedPercent);
            No3ComparedPercent = shippingItems.Sum(p => p.No3ComparedPercent);
            No4ComparedPercent = shippingItems.Sum(p => p.No4ComparedPercent);
            No5ComparedPercent = shippingItems.Sum(p => p.No5ComparedPercent);
        }
    }
}
