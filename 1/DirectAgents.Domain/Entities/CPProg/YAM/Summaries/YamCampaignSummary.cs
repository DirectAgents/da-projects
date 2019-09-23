using System.ComponentModel.DataAnnotations.Schema;

namespace DirectAgents.Domain.Entities.CPProg.YAM.Summaries
{
    /// <inheritdoc />
    /// <summary>
    /// Yahoo Campaign Summary Database entity.
    /// </summary>
    public class YamCampaignSummary : BaseYamSummary
    {
        /// <summary>
        /// Gets or sets a campaign of the summary.
        /// </summary>
        /// <value>
        /// The campaign.
        /// </value>
        [ForeignKey("EntityId")]
        public virtual YamCampaign Campaign { get; set; }
    }
}
