using System.Collections.ObjectModel;

namespace DirectAgents.Domain.Entities.CPProg.YAM
{
    /// <inheritdoc />
    /// <summary>
    /// Yahoo Creative Database entity.
    /// </summary>
    public class YamCreative : BaseYamEntity
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
        
        /// <summary>
        /// Gets or sets ads of the entity.
        /// </summary>
        /// <value>
        /// The ads.
        /// </value>
        public virtual Collection<YamAd> Ads { get; set; }
    }
}
