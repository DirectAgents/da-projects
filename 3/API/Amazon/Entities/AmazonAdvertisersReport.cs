using System.Collections.Generic;

namespace Amazon.Entities
{
    /// <summary>
    /// Represents a report of advertisers.
    /// </summary>
    public class AmazonAdvertisersReport
    {
        /// <summary>
        /// Gets  or sets Amazon Advertisers list.
        /// </summary>
        public List<AmazonAdvertiser> Advertisers { get; set; }
    }
}