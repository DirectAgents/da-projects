using System.Collections.ObjectModel;

namespace DirectAgents.Domain.Entities.CPProg.YAM
{
    /// <inheritdoc />
    /// <summary>
    /// Yahoo Line Database entity.
    /// </summary>
    public class YamLine : BaseYamEntity
    {
        /// <summary>
        /// Gets or sets a database ID of a parent campaign.
        /// </summary>
        /// <value>
        /// The campaign ID.
        /// </value>
        public int CampaignId { get; set; }

        /// <summary>
        /// Gets or sets a campaign of the entity.
        /// </summary>
        /// <value>
        /// The campaign.
        /// </value>
        public virtual YamCampaign Campaign { get; set; }

        /// <summary>
        /// Gets or sets ads of the entity.
        /// </summary>
        /// <value>
        /// The ads.
        /// </value>
        public virtual Collection<YamAd> Ads { get; set; }
    }
}
