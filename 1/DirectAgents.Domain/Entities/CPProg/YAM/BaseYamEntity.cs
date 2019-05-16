namespace DirectAgents.Domain.Entities.CPProg.YAM
{
    /// <summary>
    /// Yahoo Base Database entity, contains general properties.
    /// </summary>
    public class BaseYamEntity
    {
        /// <summary>
        /// Gets or sets a database ID of the entity.
        /// </summary>
        /// <value>
        /// The ID of a database.
        /// </value>
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets an API ID of the entity.
        /// </summary>
        /// <value>
        /// The API ID.
        /// </value>
        public int ExternalId { get; set; }

        /// <summary>
        /// Gets or sets a name of the entity.
        /// </summary>
        /// <value>
        /// The name.
        /// </value>
        public string Name { get; set; }
    }
}
