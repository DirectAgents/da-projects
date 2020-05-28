using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DirectAgents.Domain.Entities.MatchPortal
{
    [Table("buyma_product")]
    public class MatchingProduct
    {
        [Key]
        public int Id { get; set; }

        public int Level { get; set; }

        public string Brand { get; set; }

        public string BuymaId { get; set; }

        public string BuymaImageUrl { get; set; }

        public string ImageUrl { get; set; }

        public int Index { get; set; }

        public string OldTitle { get; set; }

        public string RelatedSert { get; set; }

        public string SerItemUrl { get; set; }

        public string SerpUrl { get; set; }

        public string SrSubTitle { get; set; }

        public string SrTitle { get; set; }

        public int UId { get; set; }

        public string Category { get; set; }
    }
}