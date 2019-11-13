using System.Collections.ObjectModel;

namespace DirectAgents.Domain.Entities.CPProg.Adform
{
    /// <inheritdoc />
    /// <summary>
    /// Adform campaign database entity.
    /// </summary>
    public class AdfCampaign : AdfBaseEntity
    {
        /// <summary>
        /// Gets or sets a database ID of a parent account.
        /// </summary>
        public int AccountId { get; set; }

        /// <summary>
        /// Gets or sets an account of the entity.
        /// </summary>
        public virtual ExtAccount Account { get; set; }

        public virtual Collection<AdfLineItem> LineItems { get; set; }
    }
}
