namespace DirectAgents.Domain.Entities.CPProg.Adform
{
    /// <summary>
    /// Adform base DB entity.
    /// </summary>
    public class AdfBaseEntity
    {
        /// <summary>
        /// Gets or sets the identifier.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets the external identifier from API.
        /// </summary>
        public string ExternalId { get; set; }

        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        public string Name { get; set; }
    }
}
