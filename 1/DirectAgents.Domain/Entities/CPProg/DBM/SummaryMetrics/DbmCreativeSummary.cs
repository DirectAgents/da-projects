using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace DirectAgents.Domain.Entities.CPProg.DBM
{
    public class DbmCreativeSummary : DbmBaseSummaryEntity
    {
        [Index("IX_EntityIdAndDate", 1, IsUnique = true)]
        public int? CreativeId { get; set; }

        [Index("IX_EntityIdAndDate", 2, IsUnique = true)]
        public DateTime Date { get; set; }

        [ForeignKey("CreativeId")]
        public virtual DbmCreative Creative { get; set; }
    }
}
