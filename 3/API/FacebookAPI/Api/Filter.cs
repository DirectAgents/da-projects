namespace FacebookAPI.Api
{
    /// <summary>
    /// Facebook request filter value (Used in facebook request body)
    /// </summary>
    internal class Filter
    {
        /// <summary>
        /// The field
        /// </summary>
        public string field;
        /// <summary>
        /// The operator
        /// </summary>
        public string @operator;

        /// <summary>
        /// The value
        /// </summary>
        public object value;
    }
}
