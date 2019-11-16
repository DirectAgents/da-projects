namespace DirectAgents.Domain.Entities.CPProg.Adform
{
    /// <inheritdoc />
    /// <summary>
    /// Adform Line Item database entity.
    /// </summary>
    public class AdfLineItem : AdfBaseEntity
    {
        /// <summary>
        /// Gets or sets the database ID of parent campaign.
        /// </summary>
        public int CampaignId { get; set; }

        /// <summary>
        /// Gets or sets the parent campaign.
        /// </summary>
        public virtual AdfCampaign Campaign { get; set; }
    }
}
