using System.ComponentModel.DataAnnotations.Schema;

namespace DirectAgents.Domain.Entities.CPProg.YAM.Summaries
{
    public class YamPixelSummary : BaseYamSummary
    {
        [ForeignKey("EntityId")]
        public virtual YamPixel Pixel { get; set; }
    }
}
