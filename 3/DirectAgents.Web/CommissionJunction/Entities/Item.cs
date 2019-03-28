namespace CommissionJunction.Entities
{
    /// <summary>
    /// A record of an item that was purchased as part of a commissionable event.
    /// </summary>
    public class Item
    {
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
