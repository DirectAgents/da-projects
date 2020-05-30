using System.ComponentModel.DataAnnotations;

namespace CakeExtracter.Common.MatchingPortal.Models
{
    /// <summary>
    /// A model for buyma product.
    /// </summary>
    public class Product
    {
        /// <summary>
        /// Gets or sets product unique identifier.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets product image link.
        /// </summary>
        [Display(Name = "Product Image Link:")]
        public string ProductImageLink { get; set; }

        /// <summary>
        /// Gets or sets product page link.
        /// </summary>
        [Display(Name = "Product Page Link:")]
        public string ProductPageLink { get; set; }

        /// <summary>
        /// Gets or sets value for the original product title.
        /// </summary>
        [Display(Name = "Original Product Title:")]
        public string OriginalProductTitle { get; set; }

        /// <summary>
        /// Gets or sets value for the new product title.
        /// </summary>
        [Display(Name = "New Product Title:")]
        public string NewProductTitle { get; set; }

        /// <summary>
        /// Gets or sets value for the product description.
        /// </summary>
        [Display(Name = "Product Description:")]
        public string ProductDescription { get; set; }

        /// <summary>
        /// Gets or sets product identifier(buyma_id).
        /// </summary>
        public string ProductId { get; set; }

        /// <summary>
        /// Gets or sets identifier of the matched result.
        /// </summary>
        public int? MatchedResultId { get; set; }
    }
}