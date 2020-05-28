using System;

namespace CakeExtracter.Common.MatchingPortal.Models
{
    public class MatchResult
    {
        public string ProductImage { get; set; }

        public string OldProductTitle { get; set; }

        public string NewProductTitle { get; set; }

        public string ProductDescription { get; set; }

        public string ProductId { get; set; }

        public string Brand { get; set; }

        public string Category { get; set; }

        public string MatchedStatus { get; set; }

        public DateTime MatchingDate { get; set; }

        public string Url { get; set; }

        public int RedirectId { get; set; }
    }
}