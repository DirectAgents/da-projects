using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DirectAgents.Domain.Entities.CPSearch
{
    [Table("SearchVideoDailySummary")]
    public class SearchVideoDailySummary : DailySummaryBase
    {
        public double VideoPlayedTo25 { get; set; }

        public double VideoPlayedTo50 { get; set; }

        public double VideoPlayedTo75 { get; set; }

        public double VideoPlayedTo100 { get; set; }

        public int Views { get; set; }

        public int ActiveViewImpressions { get; set; }

        public virtual SearchCampaign SearchCampaign { get; set; }
    }
}