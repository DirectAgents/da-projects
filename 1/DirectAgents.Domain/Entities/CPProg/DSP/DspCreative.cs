using System.ComponentModel.DataAnnotations.Schema;

namespace DirectAgents.Domain.Entities.CPProg.DSP
{
    public class DspCreative : DspBaseItem
    {
        public int? LineItemId { get; set; }

        [ForeignKey("LineItemId")]
        public virtual DspLineItem LineItem { get; set; }
    }
}
