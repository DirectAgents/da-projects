using System.ComponentModel.DataAnnotations.Schema;
using DirectAgents.Domain.Entities.CPProg.DBM.Entities;

namespace DirectAgents.Domain.Entities.CPProg.DBM.SummaryMetrics
{
    public class DbmCreativeSummary : DbmBaseSummaryEntity
    {
        [ForeignKey("EntityId")]
        public virtual DbmCreative Creative { get; set; }
    }
}
