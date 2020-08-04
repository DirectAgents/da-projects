using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DirectAgents.Domain.Entities.CPSearch
{
    public abstract class DailySummaryBase
    {
        [Key]
        [Column(Order = 0)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int SearchCampaignId { get; set; }

        [Key]
        [Column(Order = 1)]
        public DateTime Date { get; set; }

        [Key]
        [Column(Order = 2)]
        [StringLength(1)]
        public string Network { get; set; }

        [Key]
        [Column(Order = 3)]
        [StringLength(1)]
        public string Device { get; set; }
    }
}
