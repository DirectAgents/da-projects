using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

using DirectAgents.Domain.Entities.CPProg.Facebook.Campaign;

namespace DirectAgents.Domain.Entities.CPProg.Facebook
{
    /// <summary>
    /// Facebook Reach metric entity for Campaign and Monthly level.
    /// </summary>
    public class FbCampaignReachMetric
    {
        /// <summary>
        /// Gets or sets the period for which reach metrics are extracted.
        /// </summary>
        [Key]
        public string Period { get; set; }

        /// <summary>
        /// Gets or sets the Campaign identifier.
        /// </summary>
        /// <value>
        /// The campaign identifier.
        /// </value>
        [Key]
        public int CampaignId { get; set; }

        /// <summary>
        /// Gets or sets the related Campaign entity.
        /// </summary>
        /// <value>
        /// The campaign.
        /// </value>
        [ForeignKey("CampaignId")]
        public virtual FbCampaign Campaign { get; set; }

        /// <summary>
        /// Gets or sets the reach metric value.
        /// The number of people who saw your ads at least once.
        /// </summary>
        public int Reach { get; set; }

        /// <summary>
        /// Gets or sets the frequency metric value.
        /// The average number of times each person saw your ad.
        /// </summary>
        public double Frequency { get; set; }
    }
}