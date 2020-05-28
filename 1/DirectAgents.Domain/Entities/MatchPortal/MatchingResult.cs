using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DirectAgents.Domain.Entities.MatchPortal
{
    public class MatchingResult
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public string BuymaId { get; set; }

        public string ProductName { get; set; }

        public string ProductDescription { get; set; }

        public DateTime MatchedDate { get; set; }

        public int? SearchResultId { get; set; }

        public MatchingProduct SearchResult { get; set; }
    }
}