using System.ComponentModel.DataAnnotations.Schema;
using DirectAgents.Domain.Entities.CPProg.DBM.Entities;

namespace DirectAgents.Domain.Entities.CPProg.DBM.SummaryMetrics
{
    /// <inheritdoc />
    /// <summary>
    /// DBM Creative summary DB entity.
    /// </summary>
    public class DbmCreativeSummary : DbmBaseSummaryEntity
    {
        /// <summary>
        /// Gets or sets the creative.
        /// </summary>
        [ForeignKey("EntityId")]
        public virtual DbmCreative Creative { get; set; }
    }
}