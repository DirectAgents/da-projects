using System.ComponentModel.DataAnnotations.Schema;

namespace DirectAgents.Domain.Entities.CPProg.YAM.Summaries
{
    /// <inheritdoc />
    /// <summary>
    /// Yahoo Daily Summary Database entity.
    /// </summary>
    public class YamDailySummary : BaseYamSummary
    {
        /// <summary>
        /// Gets or sets an account of the summary.
        /// </summary>
        /// <value>
        /// The account.
        /// </value>
        [ForeignKey("EntityId")]
        public virtual ExtAccount Account { get; set; }
    }
}
