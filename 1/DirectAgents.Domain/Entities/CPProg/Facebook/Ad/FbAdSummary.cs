using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace DirectAgents.Domain.Entities.CPProg.Facebook.Ad
{
    /// <summary>
    /// Facebook Ad Summary
    /// </summary>
    public class FbAdSummary : FbBaseSummary
    {
        public int AdId { get; set; }

        [ForeignKey("AdId")]
        public virtual FbAd Ad { get; set; }

        [NotMapped]
        public List<FbAdAction> Actions { get; set; }
    }
}
