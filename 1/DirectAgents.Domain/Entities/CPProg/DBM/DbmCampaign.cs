using System.ComponentModel.DataAnnotations.Schema;

namespace DirectAgents.Domain.Entities.CPProg.DBM
{
    public class DbmCampaign : DbmBaseEntity
    {
        /// <summary>
        /// Gets or sets the account identifier. Foreign key to Accounts db table.
        /// </summary>
        public int? AccountId { get; set; }

        /// <summary>
        /// Account entity from Accounts table. Mapped by AccountId property.
        /// </summary>
        [ForeignKey("AccountId")]
        public virtual ExtAccount Account { get; set; }
    }
}
