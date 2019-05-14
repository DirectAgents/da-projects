using System.ComponentModel.DataAnnotations.Schema;

namespace DirectAgents.Domain.Entities.CPProg.YAM.Summaries
{
    /// <inheritdoc />
    /// <summary>
    /// Yahoo Pixel Summary Database entity.
    /// </summary>
    public class YamPixelSummary : BaseYamSummary
    {
        /// <summary>
        /// Gets or sets a pixel of the summary.
        /// </summary>
        /// <value>
        /// The pixel.
        /// </value>
        [ForeignKey("EntityId")]
        public virtual YamPixel Pixel { get; set; }
    }
}
