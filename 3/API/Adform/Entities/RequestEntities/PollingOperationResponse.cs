using System;

namespace Adform.Entities.RequestEntities
{
    /// <summary>
    /// The result of API response for polling operation.
    /// See more = https://api.adform.com/v1/help/buyer/stats#!/Operations/Operations_Get
    /// </summary>
    internal class PollingOperationResponse
    {
        /// <summary>
        /// Unique ID of operation
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// Operation status
        /// </summary>
        public string Status { get; set; }

        /// <summary>
        /// Operation creation time
        /// </summary>
        public DateTime CreatedAt { get; set; }

        /// <summary>
        /// Latest operation status change timestamp
        /// </summary>
        public DateTime LastActionAt { get; set; }

        /// <summary>
        /// Relative path to the job result once the job is successfully completed
        /// </summary>
        public string Location { get; set; }
    }
}
