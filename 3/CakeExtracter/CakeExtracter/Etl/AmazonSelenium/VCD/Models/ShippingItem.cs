using System.Collections.Generic;
using System.Linq;

namespace CakeExtracter.Etl.AmazonSelenium.VCD.Models
{
    internal class ShippingItem
    {
        public string Name
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

        public int OpenPurchaseOrderQuantity
        {
            get;
            set;
        }

        public decimal SellThroughRate
        {
            get;
            set;
        }

        public void SetMetrics<T>(IEnumerable<T> items)
            where T : ShippingItem
        {
            var shippingItems = items.ToList();
            OrderedUnits = shippingItems.Sum(p => p.OrderedUnits);
            ShippedUnits = shippingItems.Sum(p => p.ShippedUnits);
            ShippedRevenue = shippingItems.Sum(p => p.ShippedRevenue);
            CustomerReturns = shippingItems.Sum(p => p.CustomerReturns);
            FreeReplacements = shippingItems.Sum(p => p.FreeReplacements);
            ShippedCogs = shippingItems.Sum(p => p.ShippedCogs);
            OrderedRevenue = shippingItems.Sum(p => p.OrderedRevenue);
            LostBuyBox = shippingItems.Sum(p => p.LostBuyBox);
            RepOos = shippingItems.Sum(p => p.RepOos);
            RepOosPercentOfTotal = shippingItems.Sum(p => p.RepOosPercentOfTotal);
            RepOosPriorPeriodPercentChange = shippingItems.Sum(p => p.RepOosPriorPeriodPercentChange);
            GlanceViews = shippingItems.Sum(p => p.GlanceViews);
            AverageSalesPrice = shippingItems.Sum(p => p.AverageSalesPrice);
            SalesRank = shippingItems.Sum(p => p.SalesRank);
            SellableOnHandUnits = shippingItems.Sum(p => p.SellableOnHandUnits);
            NumberOfCustomerReviews = shippingItems.Sum(p => p.NumberOfCustomerReviews);
            NumberOfCustomerReviewsLifeToDate = shippingItems.Sum(p => p.NumberOfCustomerReviewsLifeToDate);
            AverageCustomerRating = shippingItems.Sum(p => p.AverageCustomerRating);
            FiveStars = shippingItems.Sum(p => p.FiveStars);
            FourStars = shippingItems.Sum(p => p.FourStars);
            ThreeStars = shippingItems.Sum(p => p.ThreeStars);
            TwoStars = shippingItems.Sum(p => p.TwoStars);
            OneStar = shippingItems.Sum(p => p.OneStar);
            OpenPurchaseOrderQuantity = shippingItems.Sum(p => p.OpenPurchaseOrderQuantity);
            SellThroughRate = shippingItems.Sum(p => p.SellThroughRate);
        }
    }
}