using System.ComponentModel.DataAnnotations.Schema;
using DirectAgents.Domain.Entities.CPProg.DBM.Entities;

namespace DirectAgents.Domain.Entities.CPProg.DBM.SummaryMetrics
{
    /// <inheritdoc />
    /// <summary>
    /// DBM Line item summary DB entity.
    /// </summary>
    public class DbmLineItemSummary : DbmBaseSummaryEntity
    {
        /// <summary>
        /// Gets or sets the line item.
        /// </summary>
        [ForeignKey("EntityId")]
        public virtual DbmLineItem LineItem { get; set; }

        /// <summary>
        /// Gets or sets the name of floodlight activity.
        /// </summary>
        public string FloodlightActivityName { get; set; }
    }
}