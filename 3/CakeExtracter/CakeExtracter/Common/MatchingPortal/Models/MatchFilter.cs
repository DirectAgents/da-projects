using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace CakeExtracter.Common.MatchingPortal.Models
{
    /// <summary>
    /// A filter model for entry to matching interface page.
    /// </summary>
    public class MatchFilter
    {
        /// <summary>
        /// Gets or sets count of found products.
        /// </summary>
        [Display(Name = "Number of products to match:")]
        public int? NumberOfProductsToMatch { get; set; }

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
        /// Gets or sets yes/no value whether a buyma url matched for selected brand.
        /// </summary>
        [Display(Name = "Brand Matched:")]
        public string IsBrandMatched { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether is filter applied.
        /// </summary>
        public bool IsFilterApplied { get; set; }

        /// <summary>
        /// Gets or sets a collection of the filter results.
        /// </summary>
        public IReadOnlyCollection<int> Results { get; set; }
    }
}