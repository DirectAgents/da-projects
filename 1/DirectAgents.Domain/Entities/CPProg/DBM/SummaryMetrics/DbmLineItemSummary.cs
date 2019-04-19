using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace DirectAgents.Domain.Entities.CPProg.DBM
{
    public class DbmLineItemSummary : DbmBaseSummaryEntity
    {
        [Index("IX_EntityIdAndDate", 1, IsUnique = true)]
        public int? LineItemId { get; set; }

        [Index("IX_EntityIdAndDate", 2, IsUnique = true)]
        public DateTime Date { get; set; }

        [ForeignKey("LineItemId")]
        public virtual DbmLineItem LineItem { get; set; }
    }
}
