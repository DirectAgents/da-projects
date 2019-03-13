namespace CommissionJunction.Entities.RequestEntities
{
    /// <summary>
    /// This class is a wrapper for query to API. More info - https://developers.cj.com/graphql/reference/Commission%20Detail
    /// </summary>
    internal class CjRequestParams
    {
        /// <summary>
        /// GraphQL query
        /// </summary>
        public string query { get; set; }
    }
}
