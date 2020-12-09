using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
    }
}
