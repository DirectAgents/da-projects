using System.ComponentModel.DataAnnotations.Schema;

namespace DirectAgents.Domain.Entities.CPProg.Adform.Summaries
{
    public class AdfDailySummary : AdfBaseSummary
    {
        /// <summary>
        /// Gets or sets the account entity.
        /// </summary>
        [ForeignKey("EntityId")]
        public virtual ExtAccount Account { get; set; }
    }
}
