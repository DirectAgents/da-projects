namespace DirectAgents.Domain.Entities.CPProg.YAM
{
    /// <inheritdoc />
    /// <summary>
    /// Yahoo Ad Database entity.
    /// </summary>
    public class YamAd : BaseYamEntity
    {
        /// <summary>
        /// Gets or sets a database ID of a parent line.
        /// </summary>
        /// <value>
        /// The line ID.
        /// </value>
        public int LineId { get; set; }

        /// <summary>
        /// Gets or sets a line of the entity.
        /// </summary>
        /// <value>
        /// The line.
        /// </value>
        public virtual YamLine Line { get; set; }

        /// <summary>
        /// Gets or sets a database ID of a parent creative.
        /// </summary>
        /// <value>
        /// The creative ID.
        /// </value>
        public int CreativeId { get; set; }

        /// <summary>
        /// Gets or sets a creative of the entity.
        /// </summary>
        /// <value>
        /// The creative.
        /// </value>
        public virtual YamCreative Creative { get; set; }
    }
}
