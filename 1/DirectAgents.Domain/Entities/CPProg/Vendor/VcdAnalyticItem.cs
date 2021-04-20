using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace DirectAgents.Domain.Entities.CPProg.Vendor
{
    /// <summary>
    /// VCD analytic item DB entity.
    /// </summary>
    public class VcdAnalyticItem
    {
        public int Id { get; set; }

        public DateTime Date { get; set; }

        public string Asin { get; set; }

        public string CategoryName { get; set; }

        public string SubcategoryName { get; set; }

        public string ParentProductAsin { get; set; }

        public string BrandName { get; set; }

        public string Ean { get; set; }

        public string Upc { get; set; }

        public string ApparelSize { get; set; }

        public string ApparelSizeWidth { get; set; }

        public string Binding { get; set; }

        public string Color { get; set; }

        public string ModelStyleNumber { get; set; }

        public DateTime ReleaseDate { get; set; }

        public string Name { get; set; }

        public int? AccountId { get; set; }

        [NotMapped]
        public ExtAccount Account { get; set; }

        public decimal? ShippedRevenue { get; set; }

        public decimal? ShippedUnits { get; set; }

        public decimal? OrderedUnits { get; set; }

        public decimal? ShippedCOGS { get; set; }

        public decimal? FreeReplacements { get; set; }

        public decimal? CustomerReturns { get; set; }

        public decimal? OrderedRevenue { get; set; }

        public decimal? LBB { get; set; }

        public decimal? RepOos { get; set; }

        public decimal? RepOosPercentOfTotal { get; set; }

        public decimal? RepOosPriorPeriodPercentChange { get; set; }

        public decimal? GlanceViews { get; set; }

        public decimal? SalesRank { get; set; }

        public decimal? AverageSalesPrice { get; set; }

        public decimal? SellableOnHandUnits { get; set; }

        public decimal? NumberOfCustomerReviews { get; set; }

        public decimal? NumberOfCustomerReviewsLifeToDate { get; set; }

        public decimal? AverageCustomerRating { get; set; }

        public decimal? FiveStars { get; set; }

        public decimal? FourStars { get; set; }

        public decimal? ThreeStars { get; set; }

        public decimal? TwoStars { get; set; }

        public decimal? OneStar { get; set; }

        public decimal? SellThroughRate { get; set; }

        public decimal? OpenPurchaseOrderQuantity { get; set; }
    }
}
