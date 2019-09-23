using System;

namespace DirectAgents.Domain.Entities.CPProg.DBM.SummaryMetrics
{
    public class DbmBaseSummaryEntity
    {
        public DateTime Date { get; set; }

        public string Country { get; set; }

        public int? EntityId { get; set; }

        public decimal Revenue { get; set; }

        public int Impressions { get; set; }

        public int Clicks { get; set; }

        public int PostClickConversions { get; set; }

        public int PostViewConversions { get; set; }

        public decimal CMPostClickRevenue { get; set; }

        public decimal CMPostViewRevenue { get; set; }
    }
}