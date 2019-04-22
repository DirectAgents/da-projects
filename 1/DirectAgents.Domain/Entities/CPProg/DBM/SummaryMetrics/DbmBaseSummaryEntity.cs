using System;

namespace DirectAgents.Domain.Entities.CPProg.DBM.SummaryMetrics
{
    public class DbmBaseSummaryEntity
    {
        public DateTime Date { get; set; }

        public int Impressions { get; set; }

        public int Clicks { get; set; }

        public int AllClicks { get; set; }

        public int PostClickConv { get; set; }

        public int PostViewConv { get; set; }

        public decimal Cost { get; set; }

        public decimal CMPostClickRevenue { get; set; }

        public decimal CMPostViewRevenue { get; set; }
    }
}