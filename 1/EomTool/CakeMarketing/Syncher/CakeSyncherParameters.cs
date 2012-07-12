using System;

namespace CakeMarketing.Syncher
{
    /// <summary>
    /// Parameters for the conversion extraction and Item creation.
    /// </summary>
    internal struct CakeSyncherParameters
    {
        /// <summary>
        /// The ID of the EOM campaign that receives the Items which are created from the extracted conversions.
        /// </summary>
        public int CampaignId { get; set; }

        /// <summary>
        /// The ID of the corresponding Offer in Cake which serves as the source of the conversions.
        /// </summary>
        public int ExternalId { get; set; }

        /// <summary>
        /// The year to extract conversions from.
        /// </summary>
        public int Year { get; set; }

        /// <summary>
        /// The month to extract conversions from.
        /// </summary>
        public int Month { get; set; }

        /// <summary>
        /// The starting day to extract conversions.
        /// </summary>
        public int FromDay { get; set; }

        /// <summary>
        /// The ending day to extract conversions.
        /// </summary>
        public int ToDay { get; set; }

        /// <summary>
        /// Gets Year, Month and FromDay as a DateTime.
        /// </summary>
        public DateTime FromDate { get { return new DateTime(this.Year, this.Month, this.FromDay); } }

        /// <summary>
        /// Gets Year, Month and ToDay as a DateTime.
        /// </summary>
        public DateTime ToDate { get { return new DateTime(this.Year, this.Month, this.ToDay); } }
    }
}
