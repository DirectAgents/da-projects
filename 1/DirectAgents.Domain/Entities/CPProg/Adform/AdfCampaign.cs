namespace DirectAgents.Domain.Entities.CPProg.Adform
{
    /// <inheritdoc />
    /// <summary>
    /// Adform Campaign database entity.
    /// </summary>
    public class AdfCampaign : AdfBaseEntity
    {
        /// <summary>
        /// Gets or sets the database ID of a parent account.
        /// </summary>
        public int AccountId { get; set; }

        /// <summary>
        /// Gets or sets the parent account.
        /// </summary>
        public virtual ExtAccount Account { get; set; }
    }
}
