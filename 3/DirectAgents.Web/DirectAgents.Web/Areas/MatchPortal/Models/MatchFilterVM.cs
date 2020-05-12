using System.ComponentModel.DataAnnotations;

namespace DirectAgents.Web.Areas.MatchPortal.Models
{
    public class MatchFilterVM
    {
        [Display(Name = "Number of products to match:")]
        public int? NumberOfProductsToMatch { get; set; }

        [Display(Name = "Categories:")]
        public string[] Categories { get; set; }

        [Display(Name = "Brands:")]
        public string[] Brands { get; set; }

        [Display(Name = "Product ID:")]
        public int? ProductId { get; set; }

        [Display(Name = "Product title:")]
        public string ProductTitle { get; set; }

        [Display(Name = "Brand Matched:")]
        public string IsBrandMatched { get; set; }

        public bool IsFilterApplied { get; set; }
    }
}
