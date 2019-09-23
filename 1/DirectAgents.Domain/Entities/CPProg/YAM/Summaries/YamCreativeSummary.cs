using System.ComponentModel.DataAnnotations.Schema;

namespace DirectAgents.Domain.Entities.CPProg.YAM.Summaries
{
    /// <inheritdoc />
    /// <summary>
    /// Yahoo Creative Summary Database entity.
    /// </summary>
    public class YamCreativeSummary : BaseYamSummary
    {
        /// <summary>
        /// Gets or sets a creative of the summary.
        /// </summary>
        /// <value>
        /// The creative.
        /// </value>
        [ForeignKey("EntityId")]
        public virtual YamCreative Creative { get; set; }
    }
}
