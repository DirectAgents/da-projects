using System.ComponentModel.DataAnnotations.Schema;

namespace DirectAgents.Domain.Entities.CPProg.YAM.Summaries
{
    /// <inheritdoc />
    /// <summary>
    /// Yahoo Line Summary Database entity.
    /// </summary>
    public class YamLineSummary : BaseYamSummary
    {
        /// <summary>
        /// Gets or sets a line of the summary.
        /// </summary>
        /// <value>
        /// The line.
        /// </value>
        [ForeignKey("EntityId")]
        public virtual YamLine Line { get; set; }
    }
}
