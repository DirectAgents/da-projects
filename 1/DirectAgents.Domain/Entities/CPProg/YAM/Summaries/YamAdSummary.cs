using System.ComponentModel.DataAnnotations.Schema;

namespace DirectAgents.Domain.Entities.CPProg.YAM.Summaries
{
    /// <inheritdoc />
    /// <summary>
    /// Yahoo Ad Summary Database entity.
    /// </summary>
    public class YamAdSummary : BaseYamSummary
    {
        /// <summary>
        /// Gets or sets an ad of the summary.
        /// </summary>
        /// <value>
        /// The ad.
        /// </value>
        [ForeignKey("EntityId")]
        public virtual YamAd Ad { get; set; }
    }
}
