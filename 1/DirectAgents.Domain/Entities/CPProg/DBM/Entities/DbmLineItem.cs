using System.ComponentModel.DataAnnotations.Schema;

namespace DirectAgents.Domain.Entities.CPProg.DBM.Entities
{
    /// <inheritdoc />
    /// <summary>
    /// DBM Line item DB entity.
    /// </summary>
    public class DbmLineItem : DbmEntity
    {
        /// <summary>
        /// Gets or sets the identifier of insertion order.
        /// </summary>
        public int? InsertionOrderId { get; set; }

        /// <summary>
        /// Gets or sets the insertion order.
        /// </summary>
        [ForeignKey("InsertionOrderId")]
        public virtual DbmInsertionOrder InsertionOrder { get; set; }

        /// <summary>
        /// Gets or sets the type of insertion order.
        /// </summary>
        public string Type { get; set; }

        /// <summary>
        /// Gets or sets the status of insertion order.
        /// </summary>
        public string Status { get; set; }
    }
}