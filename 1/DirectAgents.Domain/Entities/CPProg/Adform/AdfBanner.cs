namespace DirectAgents.Domain.Entities.CPProg.Adform
{
    /// <inheritdoc />
    /// <summary>
    /// Adform Campaign database entity.
    /// </summary>
    public class AdfBanner : AdfBaseEntity
    {
        /// <summary>
        /// Gets or sets the database ID of parent line item.
        /// </summary>
        public int LineItemId { get; set; }

        /// <summary>
        /// Gets or sets the parent line item.
        /// </summary>
        public virtual AdfLineItem LineItem { get; set; }
    }
}
