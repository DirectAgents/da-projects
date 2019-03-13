using System.ComponentModel.DataAnnotations.Schema;

namespace DirectAgents.Domain.Entities.CPProg.CJ
{
    /// <summary>
    /// Commision Junction item db related entity 
    /// </summary>
    public class CjCommissionItem
    {
        /// <summary>
        /// Gets or sets the db identifier.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets the commision identifier. Foregn key to AdvertiserCommissions Table
        /// </summary>
        public int? CommissionId { get; set; }

        /// <summary>
        /// Commision entity from AdvertiserCommissions Table mapped by CommissionId 
        /// </summary>
        [ForeignKey("CommissionId")]
        public virtual CjAdvertiserCommission Commission { get; set; }

        /// <summary>
        /// Item discount in USD
        /// </summary>
        public string DiscountUsd { get; set; }

        /// <summary>
        /// Unique id of the item list which applies to this item record
        /// </summary>
        public string ItemListId { get; set; }

        /// <summary>
        /// Per item sale amount in USD
        /// </summary>
        public decimal PerItemSaleAmountUsd { get; set; }

        /// <summary>
        /// Item quantity
        /// </summary>
        public int Quantity { get; set; }

        /// <summary>
        /// Advertiser's unique identifier for the item
        /// </summary>
        public string Sku { get; set; }

        /// <summary>
        /// Total commission for the specified quantity of the item in USD
        /// </summary>
        public decimal TotalCommissionUsd { get; set; }
    }
}
