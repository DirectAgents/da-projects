using System;

namespace CakeExtracter.Common.MatchingPortal.Models
{
    /// <summary>
    /// A model for results of the <see cref="ResultFilter"/>.
    /// </summary>
    public class MatchResult
    {
        /// <summary>
        /// Gets or sets url of the original product image.
        /// </summary>
        public string ProductImage { get; set; }

        /// <summary>
        /// Gets or sets value of the old product title.
        /// </summary>
        public string OldProductTitle { get; set; }

        /// <summary>
        /// Gets or sets value of the new product title.
        /// </summary>
        public string NewProductTitle { get; set; }

        /// <summary>
        /// Gets or sets value of the product description.
        /// </summary>
        public string ProductDescription { get; set; }

        /// <summary>
        /// Gets or sets product identifier(buyma_id).
        /// </summary>
        public string ProductId { get; set; }

        /// <summary>
        /// Gets or sets value of the product brand.
        /// </summary>
        public string Brand { get; set; }

        /// <summary>
        /// Gets or sets value of the product category.
        /// </summary>
        public string Category { get; set; }

        /// <summary>
        /// Gets or sets value indicated whether is product matched.
        /// </summary>
        public string MatchedStatus { get; set; }

        /// <summary>
        /// Gets or sets date when the product matched.
        /// </summary>
        public DateTime MatchingDate { get; set; }

        /// <summary>
        /// Gets or sets search result url.
        /// </summary>
        public string Url { get; set; }

        /// <summary>
        /// Gets or sets identifier to redirect on the Product Matching page.
        /// </summary>
        public int RedirectId { get; set; }
    }
}