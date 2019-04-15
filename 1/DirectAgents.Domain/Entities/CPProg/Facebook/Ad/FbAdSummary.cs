using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace DirectAgents.Domain.Entities.CPProg.Facebook.Ad
{
    public class FbAdSummary
    {
        public DateTime Date { get; set; }

        public int AdId { get; set; }

        [ForeignKey("AdId")]
        public virtual FbAd Ad { get; set; }

        public int Impressions { get; set; }

        public int Clicks { get; set; }

        public int AllClicks { get; set; }

        public int PostClickConv { get; set; }

        public int PostViewConv { get; set; }

        public decimal Cost { get; set; }
    }
}
