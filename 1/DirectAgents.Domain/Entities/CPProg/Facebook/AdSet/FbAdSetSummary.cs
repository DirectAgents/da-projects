using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace DirectAgents.Domain.Entities.CPProg.Facebook.AdSet
{
    public class FbAdSetSummary
    {
        public DateTime Date { get; set; }

        public int AdSetId { get; set; }

        [ForeignKey("AdSetId")]
        public virtual FbAdSet AdSet { get; set; }

        public int Impressions { get; set; }

        public int Clicks { get; set; }

        public int AllClicks { get; set; }

        public int PostClickConv { get; set; }

        public int PostViewConv { get; set; }

        public decimal Cost { get; set; }
    }
}
