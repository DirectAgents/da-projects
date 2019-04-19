using System.ComponentModel.DataAnnotations.Schema;

namespace DirectAgents.Domain.Entities.CPProg.DBM
{
    public class DbmLineItemSummary : DbmBaseSummaryEntity
    {
        [ForeignKey("EntityId")]
        public virtual DbmLineItem LineItem { get; set; }
    }
}
