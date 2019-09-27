using System.ComponentModel.DataAnnotations.Schema;

namespace DirectAgents.Domain.Entities.CPProg.DBM.Entities
{
    /// <inheritdoc />
    /// <summary>
    /// DBM Insertion order DB entity.
    /// </summary>
    public class DbmInsertionOrder : DbmEntity
    {
        /// <summary>
        /// Gets or sets the identifier of campaign.
        /// </summary>
        public int? CampaignId { get; set; }

        /// <summary>
        /// Gets or sets the campaign.
        /// </summary>
        [ForeignKey("CampaignId")]
        public virtual DbmCampaign Campaign { get; set; }
    }
}