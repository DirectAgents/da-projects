using System;

namespace DirectAgents.Domain.Entities.CPProg.DBM
{
    public class DbmBaseSummaryEntity
    {
        public int? EntityId { get; set; }

        public DateTime Date { get; set; }

        public int Impressions { get; set; }

        public int Clicks { get; set; }

        public int AllClicks { get; set; }

        public int PostClickConv { get; set; }

        public int PostViewConv { get; set; }

        public decimal Cost { get; set; }

        public bool IsNullableSummary()
        {
            return Impressions == 0 && Clicks == 0 && AllClicks == 0
                   && PostClickConv == 0 && PostViewConv == 0 && Cost == 0;
        }

        public bool IsEqualWith(DbmBaseSummaryEntity summary)
        {
            return Impressions == summary.Impressions && Clicks == summary.Clicks && AllClicks == summary.AllClicks
                   && PostClickConv == summary.PostClickConv && PostViewConv == summary.PostViewConv && Cost == summary.Cost;
        }
    }
}