using System.ComponentModel.DataAnnotations.Schema;

namespace DirectAgents.Domain.Entities.CPProg.DBM
{
    public class DbmInsertionOrder : DbmBaseEntity
    {
        public int? CampaignId { get; set; }

        [ForeignKey("CampaignId")]
        public virtual DbmCampaign Campaign { get; set; }
    }
}
