using System;
using DirectAgents.Domain.Abstract;

namespace DirectAgents.Domain.Entities.CPProg.Vendor
{
    public class VendorGeographicSalesInsightsProduct : BaseVendorEntity, IVendorProductDate
    {
        public string Asin
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

        public DateTime Date
        {
            get;
            set;
        }
    }
}
