using System.ComponentModel.DataAnnotations.Schema;
using DirectAgents.Domain.Entities.CPProg.DBM.Entities;

namespace DirectAgents.Domain.Entities.CPProg.DBM.SummaryMetrics
{
    public class DbmLineItemSummary : DbmBaseSummaryEntity
    {
        [ForeignKey("EntityId")]
        public virtual DbmLineItem LineItem { get; set; }
    }
}