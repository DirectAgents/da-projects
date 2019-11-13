using System.ComponentModel.DataAnnotations.Schema;

namespace DirectAgents.Domain.Entities.CPProg.Adform.Summaries
{
    public class AdfLineItemSummary : AdfBaseSummary
    {
        [ForeignKey("EntityId")]
        public virtual AdfLineItem LineItem { get; set; }
    }
}
