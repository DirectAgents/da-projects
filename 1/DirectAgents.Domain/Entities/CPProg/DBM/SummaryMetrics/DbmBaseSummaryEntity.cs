using System;

namespace DirectAgents.Domain.Entities.CPProg.DBM.SummaryMetrics
{
    public class DbmBaseSummaryEntity : DbmBaseEntity
    {
        public DateTime Date { get; set; }

        public int? EntityId { get; set; }

        public decimal Cost { get; set; }

        public decimal Impressions { get; set; }

        public decimal Clicks { get; set; }

        public decimal PostClickConv { get; set; }

        public decimal PostViewConv { get; set; }

        public decimal CMPostClickRevenue { get; set; }

        public decimal CMPostViewRevenue { get; set; }
    }
}