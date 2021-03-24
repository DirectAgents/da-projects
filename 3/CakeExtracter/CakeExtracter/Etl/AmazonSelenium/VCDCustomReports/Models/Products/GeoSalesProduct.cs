using System.Collections.Generic;
using System.Linq;
using CakeExtracter.Etl.AmazonSelenium.VCDCustomReports.Models.Base;
using CakeExtracter.Etl.AmazonSelenium.VCDCustomReports.Models.Interface;

namespace CakeExtracter.Etl.AmazonSelenium.VCDCustomReports.Models.Products
{
    public class GeoSalesProduct : VcdCustomProduct, ISumMetrics<GeoSalesProduct>
    {
        public string ParentAsin
        {
            get;
            set;
        }

        public string Category
        {
            get;
            set;
        }

        public string Subcategory
        {
            get;
            set;
        }

        public string Ean
        {
            get;
            set;
        }

        public string Upc
        {
            get;
            set;
        }

        public string Brand
        {
            get;
            set;
        }

        public string ApparelSize
        {
            get;
            set;
        }

        public string ApparelSizeWidth
        {
            get;
            set;
        }

        public string Binding
        {
            get;
            set;
        }

        public string Color
        {
            get;
            set;
        }

        public string ModelStyleNumber
        {
            get;
            set;
        }

        public string Region
        {
            get;
            set;
        }

        public string State
        {
            get;
            set;
        }

        public string City
        {
            get;
            set;
        }

        public string Zip
        {
            get;
            set;
        }

        public string Author
        {
            get;
            set;
        }

        public string Isbn13
        {
            get;
            set;
        }

        public decimal ShippedRevenuePriorPeriodPercentChange
        {
            get;
            set;
        }

        public decimal ShippedRevenuePriorYearPercentChange
        {
            get;
            set;
        }

        public decimal ShippedRevenuePercentOfTotal
        {
            get;
            set;
        }

        public decimal ShippedCogsPriorPeriodPercentChange
        {
            get;
            set;
        }

        public decimal ShippedCogsPriorYearPercentChange
        {
            get;
            set;
        }

        public decimal ShippedCogsPercentOfTotal
        {
            get;
            set;
        }

        public decimal ShippedUnitsPriorPeriodPercentChange
        {
            get;
            set;
        }

        public decimal ShippedUnitsPriorYearPercentChange
        {
            get;
            set;
        }

        public decimal ShippedUnitsPercentOfTotal
        {
            get;
            set;
        }

        public decimal AverageShippedPrice
        {
            get;
            set;
        }

        public decimal AverageShippedPricePriorPeriodPercentChange
        {
            get;
            set;
        }

        public decimal AverageShippedPricePriorYearPercentChange
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

        public decimal LostBuyBox
        {
            get;
            set;
        }

        public decimal RepOos
        {
            get;
            set;
        }

        public decimal RepOosPercentOfTotal
        {
            get;
            set;
        }

        public decimal RepOosPriorPeriodPercentChange
        {
            get;
            set;
        }

        public decimal GlanceViews
        {
            get;
            set;
        }

        public int SalesRank
        {
            get;
            set;
        }

        public decimal AverageSalesPrice
        {
            get;
            set;
        }

        public int SellableOnHandUnits
        {
            get;
            set;
        }

        public int NumberOfCustomerReviews
        {
            get;
            set;
        }

        public int NumberOfCustomerReviewsLifeToDate
        {
            get;
            set;
        }

        public decimal AverageCustomerRating
        {
            get;
            set;
        }

        public int FiveStars
        {
            get;
            set;
        }

        public int FourStars
        {
            get;
            set;
        }

        public int ThreeStars
        {
            get;
            set;
        }

        public int TwoStars
        {
            get;
            set;
        }

        public int OneStar
        {
            get;
            set;
        }

        public void SetMetrics(IEnumerable<GeoSalesProduct> items)
        {
            var shippingItems = items.ToList();
            ShippedRevenuePercentOfTotal = shippingItems.Sum(p => p.ShippedRevenuePercentOfTotal);
            ShippedRevenuePriorPeriodPercentChange = shippingItems.Sum(p => p.ShippedRevenuePriorPeriodPercentChange);
            ShippedRevenuePriorYearPercentChange = shippingItems.Sum(p => p.ShippedRevenuePriorYearPercentChange);
            ShippedUnits = shippingItems.Sum(p => p.ShippedUnits);
            ShippedUnitsPercentOfTotal = shippingItems.Sum(p => p.ShippedUnitsPercentOfTotal);
            ShippedUnitsPriorPeriodPercentChange = shippingItems.Sum(p => p.ShippedUnitsPriorPeriodPercentChange);
            ShippedUnitsPriorYearPercentChange = shippingItems.Sum(p => p.ShippedUnitsPriorYearPercentChange);
            ShippedCogsPercentOfTotal = shippingItems.Sum(p => p.ShippedCogsPercentOfTotal);
            ShippedCogsPriorPeriodPercentChange = shippingItems.Sum(p => p.ShippedCogsPriorPeriodPercentChange);
            ShippedCogsPriorYearPercentChange = shippingItems.Sum(p => p.ShippedCogsPriorYearPercentChange);
            AverageShippedPrice = shippingItems.Sum(p => p.AverageShippedPrice);
            AverageShippedPricePriorPeriodPercentChange = shippingItems.Sum(p => p.AverageShippedPricePriorPeriodPercentChange);
            AverageShippedPricePriorYearPercentChange = shippingItems.Sum(p => p.AverageShippedPricePriorYearPercentChange);
        }
    }
}