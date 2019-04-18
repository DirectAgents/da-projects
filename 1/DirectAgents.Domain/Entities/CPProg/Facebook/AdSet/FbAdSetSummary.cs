using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace DirectAgents.Domain.Entities.CPProg.Facebook.AdSet
{
    public class FbAdSetSummary : FbBaseSummary
    {
        public int AdSetId { get; set; }

        [ForeignKey("AdSetId")]
        public virtual FbAdSet AdSet { get; set; }

        [NotMapped]
        public List<FbAdSetAction> Actions { get; set; }
    }
}
