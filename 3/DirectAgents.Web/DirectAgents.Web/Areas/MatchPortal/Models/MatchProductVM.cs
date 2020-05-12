using System.ComponentModel.DataAnnotations;

namespace DirectAgents.Web.Areas.MatchPortal.Models
{
    public class MatchProductVM
    {
        public int Id { get; set; }

        [Display(Name = "Product Image Link:")]
        public string ProductImageLink { get; set; }

        [Display(Name = "Product Page Link:")]
        public string ProductPageLink { get; set; }

        [Display(Name = "Original Product Title:")]
        public string OriginalProductTitle { get; set; }

        [Display(Name = "New Product Title:")]
        public string NewProductTitle { get; set; }

        [Display(Name = "Product Description:")]
        public string ProductDescription { get; set; }

        public int? MatchedResultId { get; set; }
    }
}