using System.ComponentModel.DataAnnotations.Schema;

namespace DirectAgents.Domain.Entities.CPProg.DSP
{
    /// <summary>Base DSP database entity.</summary>
    public class DspDbEntity
    {
        public int Id { get; set; }

        public string ReportId { get; set; }

        public string Name { get; set; }

        public int? AccountId { get; set; }

        [ForeignKey("AccountId")]
        public virtual ExtAccount ExtAccount { get; set; }
    }
}
