using System.ComponentModel.DataAnnotations.Schema;

namespace DirectAgents.Domain.Entities.CPProg.Adform.Summaries
{
    /// <inheritdoc />
    /// <summary>
    /// Adform LineItem summary database entity.
    /// </summary>
    public class AdfLineItemSummary : AdfBaseSummary
    {
        /// <summary>
        /// Gets or sets a line item of the summary.
        /// </summary>
        [ForeignKey("EntityId")]
        public virtual AdfLineItem LineItem { get; set; }
    }
}
