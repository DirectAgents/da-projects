using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace DirectAgents.Domain.Entities.CPProg.Facebook.Campaign
{
    public class FbCampaignSummary
    {
        public DateTime Date { get; set; }

        public int CampaignId { get; set; }

        [ForeignKey("CampaignId")]
        public virtual FbCampaign Campaign { get; set; }

        public int Impressions { get; set; }

        public int Clicks { get; set; }

        public int AllClicks { get; set; }

        public int PostClickConv { get; set; }

        public int PostViewConv { get; set; }

        public decimal Cost { get; set; }
    }
}
