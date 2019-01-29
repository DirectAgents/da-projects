using System.ComponentModel.DataAnnotations.Schema;

namespace DirectAgents.Domain.Entities.CPProg.DSP
{
    public class DspBaseItem
    {
        public int Id { get; set; }

        public string ExternalId { get; set; }

        public string Name { get; set; }

        public int? AccountId { get; set; }

        [ForeignKey("AccountId")]
        public virtual ExtAccount ExtAccount { get; set; }
    }
}
