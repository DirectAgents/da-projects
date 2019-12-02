using System;

namespace FacebookAPI.Entities
{
    /// <summary>
    /// Facebook Reach metric row entity.
    /// </summary>
    public class FbReachRow
    {
        /// <summary>
        /// Gets or sets the start date of period for which the Reach value is calculated.
        /// </summary>
        public DateTime StartDate { get; set; }

        /// <summary>
        /// Gets or sets the end date of period for which the Reach value is calculated.
        /// </summary>
        public DateTime EndDate { get; set; }

        /// <summary>
        /// Gets or sets the reach metric value.
        /// The number of people who saw your ads at least once.
        /// </summary>
        public int Reach { get; set; }
    }
}
