using System;

namespace DirectAgents.Web.Areas.MatchPortal.Models
{
    public class MatchResultVM
    {
        public string ProductImage { get; set; }

        public string OldProductTitle { get; set; }
        
        public string NewProductTitle { get; set; }

        public string ProductDescription { get; set; }

        public int ProductId { get; set; }

        public string Brand { get; set; }

        public string MatchedStatus { get; set; }

        public DateTime MatchingDate { get; set; }

        public string Url { get; set; }
    }
}