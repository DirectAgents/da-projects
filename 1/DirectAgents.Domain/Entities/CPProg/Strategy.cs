using System.ComponentModel.DataAnnotations.Schema;

namespace DirectAgents.Domain.Entities.CPProg
{
    public class Strategy
    {
        public int Id { get; set; }
        public int AccountId { get; set; }

        [ForeignKey("AccountId")]
        public virtual ExtAccount ExtAccount { get; set; }

        public int? TypeId { get; set; }
        public virtual EntityType Type { get; set; }
        public string CampaignType { get; set; }
        public string ExternalId { get; set; }
        public string Name { get; set; }
    }
}