using System.Collections.Generic;

namespace CommissionJunction.Entities.ResponseEntities
{
    /// <summary>
    /// The result of a query request to the API. More info - https://developers.cj.com/graphql/reference/Commission%20Detail
    /// </summary>
    /// <typeparam name="T">The type of returned commissions data</typeparam>
    internal class CjQueryResponse<T>
    {
        /// <summary>
        /// The number of commissions returned.
        /// </summary>
        public int Count { get; set; }

        /// <summary>
        /// The maximum number of returned commissions supported by the query.
        /// </summary>
        public int Limit { get; set; }

        /// <summary>
        /// The unique id of the final commission returned by this call (useful as a cursor).
        /// </summary>
        public string MaxCommissionId{ get; set; }

        /// <summary>
        /// If false, use maxCommissionId as the sinceCommissionId argument to continue and retrieve the next batch of commissions.
        /// </summary>
        public bool PayloadComplete { get; set; }

        /// <summary>
        /// The commission records returned by the query.
        /// </summary>
        public List<T> Records { get; set; }
    }
}
