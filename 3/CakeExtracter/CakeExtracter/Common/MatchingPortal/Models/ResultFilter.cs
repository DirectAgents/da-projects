using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Windows;

namespace CakeExtracter.Common.MatchingPortal.Models
{
    /// <summary>
    /// A filter model for Product Data QA page.
    /// </summary>
    public class ResultFilter
    {

        /// <summary>
        /// Gets or sets count of the found matched results.
        /// </summary>
        [Display(Name = "Number of Results:")]
        public int? NumberOfResults { get; set; }

        /// <summary>
        /// Gets or sets percent of the matched results.
        /// </summary>
        [Display(Name = "Percent of Total Matched:")]
        [DisplayFormat(DataFormatString = "{0:P}")]
        public double? PercentMatched { get; set; }

        /// <summary>
        /// Gets or sets an array of the product categories.
        /// </summary>
        [Display(Name = "Categories:")]
        public string[] Categories { get; set; }

        /// <summary>
        /// Gets or sets an array of the product brands.
        /// </summary>
        [Display(Name = "Brands:")]
        public string[] Brands { get; set; }

        /// <summary>
        /// Gets or sets an array of the matched statuses.
        /// </summary>
        [Display(Name = "Matched Status:")]
        public string[] MatchedStatus { get; set; }

        /// <summary>
        /// Gets or sets product identifier(buyma_id).
        /// </summary>
        [Display(Name = "Product ID:")]
        public string ProductId { get; set; }

        /// <summary>
        /// Gets or sets old product title value.
        /// </summary>
        [Display(Name = "Product title:")]
        public string ProductTitle { get; set; }

        /// <summary>
        /// Gets or sets minimal value for the product matched date.
        /// </summary>
        [Display(Name = "Product Updated Date Range From:")]
        public DateTime? DateRangeFrom { get; set; }

        /// <summary>
        /// Gets or sets maximum value for the product matched date.
        /// </summary>
        [Display(Name = "Product Updated Date Range To:")]
        public DateTime? DateRangeTo { get; set; }

        /// <summary>
        /// Gets or sets a collection of the filter results.
        /// </summary>
        public IReadOnlyCollection<MatchResult> Results { get; set; }
    }
}