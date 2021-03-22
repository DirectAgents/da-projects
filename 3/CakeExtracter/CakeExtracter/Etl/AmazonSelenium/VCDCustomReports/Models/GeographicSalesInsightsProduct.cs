using CakeExtracter.Etl.AmazonSelenium.VCD.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CakeExtracter.Etl.AmazonSelenium.VCDCustomReports.Models
{
    class GeographicSalesInsightsProduct : Product
    {
        public string ProductTitle
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

        public void SetMetrics(IEnumerable<GeographicSalesInsightsProduct> items)
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
