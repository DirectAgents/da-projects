using System;

namespace Adform.Entities.ResponseEntities
{
    /// <summary>
    /// The result of API response for polling operation.
    /// See more = https://api.adform.com/v1/help/buyer/stats#!/Operations/Operations_Get.
    /// </summary>
    internal class PollingOperationResponse
    {
        /// <summary>
        ///  Gets or sets the unique ID of operation.
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// Gets or sets the operation status.
        /// </summary>
        public string Status { get; set; }

        /// <summary>
        /// Gets or sets the operation creation time.
        /// </summary>
        public DateTime CreatedAt { get; set; }

        /// <summary>
        /// Gets or sets the latest operation status change timestamp.
        /// </summary>
        public DateTime LastActionAt { get; set; }

        /// <summary>
        /// Gets or sets the relative path to the job result once the job is successfully completed.
        /// </summary>
        public string Location { get; set; }
    }
}
