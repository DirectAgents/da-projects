using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace DirectAgents.Domain.Entities.CPProg.Facebook.AdSet
{
    /// <summary>
    /// Facebook Adset Summary entity
    /// </summary>
    /// <seealso cref="DirectAgents.Domain.Entities.CPProg.Facebook.FbBaseSummary" />
    public class FbAdSetSummary : FbBaseSummary
    {
        public int AdSetId { get; set; }

        [ForeignKey("AdSetId")]
        public virtual FbAdSet AdSet { get; set; }

        [NotMapped]
        public List<FbAdSetAction> Actions { get; set; }
    }
}
