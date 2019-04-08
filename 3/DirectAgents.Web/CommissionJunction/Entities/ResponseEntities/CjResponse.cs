namespace CommissionJunction.Entities.ResponseEntities
{
    /// <summary>
    /// This class is a wrapper for data returned by API requests.
    /// </summary>
    /// <typeparam name="T">The type of returned data</typeparam>
    internal class CjResponse<T>
    {
        /// <summary>
        /// Response data
        /// </summary>
        public T Data { get; set; }
    }
}
