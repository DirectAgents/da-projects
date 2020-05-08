using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DirectAgents.Domain.Entities.CPSearch
{
    [Table("SearchVideoDailySummary")]
    public class SearchVideoDailySummary
    {
        [Key]
        [Column(Order = 0)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int SearchCampaignId { get; set; }

        [Key]
        [Column(Order = 1)]
        public DateTime Date { get; set; }

        public double VideoPlayedTo25 { get; set; }

        public double VideoPlayedTo50 { get; set; }

        public double VideoPlayedTo75 { get; set; }

        public double VideoPlayedTo100 { get; set; }

        public int Views { get; set; }

        public int ActiveViewImpressions { get; set; }

        [Key]
        [Column(Order = 2)]
        [StringLength(1)]
        public string Network { get; set; }

        [Key]
        [Column(Order = 3)]
        [StringLength(1)]
        public string Device { get; set; }

        public virtual SearchCampaign SearchCampaign { get; set; }
    }
}