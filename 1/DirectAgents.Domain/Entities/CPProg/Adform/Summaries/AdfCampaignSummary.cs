using System.ComponentModel.DataAnnotations.Schema;

namespace DirectAgents.Domain.Entities.CPProg.Adform.Summaries
{
    /// <inheritdoc />
    /// <summary>
    /// Adform Campaign summary database entity.
    /// </summary>
    public class AdfCampaignSummary : AdfBaseSummary
    {
        /// <summary>
        /// Gets or sets a campaign of the summary.
        /// </summary>
        [ForeignKey("EntityId")]
        public virtual AdfCampaign Campaign { get; set; }
    }
}
