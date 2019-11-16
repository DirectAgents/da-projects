using System.ComponentModel.DataAnnotations.Schema;

namespace DirectAgents.Domain.Entities.CPProg.Adform.Summaries
{
    /// <inheritdoc />
    /// <summary>
    /// Adform Banner summary database entity.
    /// </summary>
    public class AdfBannerSummary : AdfBaseSummary
    {
        /// <summary>
        /// Gets or sets a banner of the summary.
        /// </summary>
        [ForeignKey("EntityId")]
        public virtual AdfBanner Banner { get; set; }
    }
}
