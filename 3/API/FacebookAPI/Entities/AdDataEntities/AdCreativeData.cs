namespace FacebookAPI.Entities.AdDataEntities
{
    /// <summary>
    /// Ad Creative data entity
    /// </summary>
    public class AdCreativeData
    {
        /// <summary>
        /// Gets or sets the identifier.
        /// </summary>
        /// <value>
        /// The identifier.
        /// </value>
        public string Id { get; set; }

        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        /// <value>
        /// The name.
        /// </value>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the status.
        /// </summary>
        /// <value>
        /// The status.
        /// </value>
        public string Status { get; set; }

        /// <summary>
        /// Gets or sets the creative.
        /// </summary>
        /// <value>
        /// The creative.
        /// </value>
        public Creative Creative { get; set; }
    }
}
