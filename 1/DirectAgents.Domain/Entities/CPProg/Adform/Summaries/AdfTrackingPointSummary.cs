using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DirectAgents.Domain.Entities.CPProg.Adform.Summaries
{
    /// <inheritdoc />
    /// <summary>
    /// Adform TrackingPoint summary database entity.
    /// </summary>
    public class AdfTrackingPointSummary : AdfBaseSummary
    {
        /// <summary>
        /// Gets or sets a tracking point of the summary.
        /// </summary>
        [ForeignKey("EntityId")]
        public virtual AdfTrackingPoint TrackingPoint { get; set; }

        /// <summary>
        /// Gets or sets the database ID of the related Line Item.
        /// </summary>
        [Key]
        public int LineItemId { get; set; }

        /// <summary>
        /// Gets or sets the related Line Item.
        /// </summary>
        public virtual AdfLineItem LineItem { get; set; }
    }
}
