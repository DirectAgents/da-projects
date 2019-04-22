using System;
using System.ComponentModel.DataAnnotations;

namespace DirectAgents.Domain.Entities.CPProg.YAM.Summaries
{
    public abstract class BaseYamSummary
    {
        [Key]
        public int EntityId { get; set; }

        [Key]
        public DateTime Date { get; set; }

        public int Impressions { get; set; }

        public int Clicks { get; set; }

        public int ClickThroughConversion { get; set; }

        public int ViewThroughConversion { get; set; }

        public decimal ConversionValue { get; set; }

        public decimal AdvertiserSpending	 { get; set; }
    }
}
