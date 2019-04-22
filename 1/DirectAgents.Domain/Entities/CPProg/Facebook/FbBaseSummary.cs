using System;

namespace DirectAgents.Domain.Entities.CPProg.Facebook
{
    /// <summary>
    /// Facebook base summary metric entity.
    /// </summary>
    public class FbBaseSummary
    {
        public DateTime Date { get; set; }

        public int Impressions { get; set; }

        public int Clicks { get; set; }

        public int AllClicks { get; set; }

        public int PostClickConv { get; set; }

        public int PostViewConv { get; set; }

        public decimal PostClickRev { get; set; }

        public decimal PostViewRev { get; set; }

        public decimal Cost { get; set; }

        public bool IsNullableSummary()
        {
            return Impressions == 0 && Clicks == 0 && AllClicks == 0 && PostClickConv == 0 && 
                PostViewConv == 0 && PostViewRev == 0 && PostClickRev == 0 && Cost == 0;
        }
    }
}
