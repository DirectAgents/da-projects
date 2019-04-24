namespace DirectAgents.Domain.Entities.CPProg.DBM.Entities
{
    /// <summary>
    /// Db related entity
    /// </summary>
    public class DbmEntity : DbmBaseEntity
    {
        /// <summary>
        /// Gets or sets the item unique db identifier.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// The item unique identifier from API
        /// </summary>
        public string ExternalId { get; set; }

        /// <summary>
        /// The item name
        /// </summary>
        public string Name { get; set; }
    }
}
