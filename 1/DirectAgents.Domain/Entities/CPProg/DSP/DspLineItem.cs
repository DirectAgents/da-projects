using System.ComponentModel.DataAnnotations.Schema;

namespace DirectAgents.Domain.Entities.CPProg.DSP
{
    public class DspLineItem : DspBaseItem
    {
        public int? OrderId { get; set; }

        [ForeignKey("OrderId")]
        public virtual DspOrder Order { get; set; }
    }
}
