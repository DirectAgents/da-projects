using System.ComponentModel.DataAnnotations.Schema;

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
