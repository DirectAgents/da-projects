namespace CommissionJunction.Entities.QueryParams
{
    /// <summary>
    /// The model that contains common properties for query filters.
    /// </summary>
    internal class BaseQueryParams
    {
        /// <summary>
        /// A commission's unique id.
        /// If present, filters results, keeping commissions whose commission id is greater than the supplied argument
        /// and whose posting date is greater than or equal to the posting date of the commission corresponding to the supplied argument.
        /// </summary>
        public string SinceCommissionId { get; set; }
    }
}
