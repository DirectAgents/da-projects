using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace DirectAgents.Web.Areas.MatchPortal.Models
{
    public class ResultFilterVM
    {
        [Display(Name = "Number of Results:")]
        public int? NumberOfResults { get; set; }

        [Display(Name = "Percent of Total Matched:")]
        public int? PercentMatched { get; set; }

        [Display(Name = "Categories:")]
        public string[] Categories { get; set; }

        [Display(Name = "Brands:")]
        public string[] Brands { get; set; }

        [Display(Name = "Matched Status:")]
        public string[] MatchedStatus { get; set; }

        [Display(Name = "Product ID:")]
        public int? ProductId { get; set; }

        [Display(Name = "Product title:")]
        public string ProductTitle { get; set; }

        [Display(Name = "Product Updated Date Range From:")]
        public DateTime DateRangeFrom { get; set; }

        [Display(Name = "Product Updated Date Range To:")]
        public DateTime DateRangeTo { get; set; }

        public IReadOnlyCollection<MatchResultVM> Results { get; set; }
    }
}