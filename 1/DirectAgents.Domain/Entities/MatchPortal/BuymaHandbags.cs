using System.ComponentModel.DataAnnotations.Schema;

namespace DirectAgents.Domain.Entities.MatchPortal
{
    [Table("buyma_handbags")]
    public class BuymaHandbag
    {
        [Column("level_0")]
        public long Level { get; set; }

        [Column("brand")]
        public string Brand { get; set; }

        [Column("buyma_id")]
        public string BuymaId { get; set; }

        [Column("buyma_image_url")]
        public string BuymaImageUrl { get; set; }

        [Column("image_url")]
        public string ImageUrl { get; set; }

        [Column("index")]
        public long Index { get; set; }

        [Column("old_title")]
        public string OldTitle { get; set; }

        [Column("p_related_serp")]
        public string RelatedSert { get; set; }

        [Column("SerItemUrl")]
        public string SerItemUrl { get; set; }

        [Column("serp_url")]
        public string SerpUrl { get; set; }

        [Column("sr_sub_title")]
        public string SrSubTitle { get; set; }

        [Column("sr_title")]
        public string SrTitle { get; set; }

        [Column("uid")]
        public double UId { get; set; }

        [Column("category")]
        public string Category { get; set; }
    }
}