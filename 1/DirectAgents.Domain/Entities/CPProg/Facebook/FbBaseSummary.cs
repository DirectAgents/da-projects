using System;

namespace DirectAgents.Domain.Entities.CPProg.Facebook
{
    public class FbBaseSummary
    {
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
    }
}
