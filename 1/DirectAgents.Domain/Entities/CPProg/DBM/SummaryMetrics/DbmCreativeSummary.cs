using System.ComponentModel.DataAnnotations.Schema;

namespace DirectAgents.Domain.Entities.CPProg.DBM
{
    public class DbmCreativeSummary : DbmBaseSummaryEntity
    {
        [ForeignKey("EntityId")]
        public virtual DbmCreative Creative { get; set; }
    }
}
