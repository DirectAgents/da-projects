namespace DirectAgents.Domain.Entities.CPProg.YAM
{
    /// <inheritdoc />
    /// <summary>
    /// Yahoo Pixel Database entity.
    /// </summary>
    public class YamPixel : BaseYamEntity
    {
        /// <summary>
        /// Gets or sets a database ID of a parent account.
        /// </summary>
        /// <value>
        /// The account ID.
        /// </value>
        public int AccountId { get; set; }

        /// <summary>
        /// Gets or sets an account of the entity.
        /// </summary>
        /// <value>
        /// The account.
        /// </value>
        public virtual ExtAccount Account { get; set; }
    }
}
