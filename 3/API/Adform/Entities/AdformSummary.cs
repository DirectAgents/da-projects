using System;

namespace Adform
{
    /// <summary>
    /// Adform summary entity for all levels.
    /// </summary>
    public class AdformSummary
    {
        /// <summary>
        /// Gets or sets the date.
        /// </summary>
        public DateTime Date { get; set; }

        /// <summary>
        /// Gets or sets the "Impressions" metric without uniqueness level.
        /// </summary>
        public int Impressions { get; set; }

        /// <summary>
        /// Gets or sets the "Clicks" metric.
        /// </summary>
        public int Clicks { get; set; }

        /// <summary>
        /// Gets or sets the "Cost" metric. Calculation method is "Max cost".
        /// </summary>
        public decimal Cost { get; set; }

        /// <summary>
        /// Gets or sets the Order.
        /// </summary>
        public string Order { get; set; }

        /// <summary>
        /// Gets or sets the Campaign.
        /// </summary>
        public string Campaign { get; set; }

        /// <summary>
        /// Gets or sets the LineItem.
        /// </summary>
        public string LineItem { get; set; }

        /// <summary>
        /// Gets or sets the Banner.
        /// </summary>
        public string Banner { get; set; }

        /// <summary>
        /// Gets or sets the ad interaction type.
        /// </summary>
        public string AdInteractionType { get; set; }

        /// <summary>
        /// Gets or sets the "Conversions" metric for all conversion types.
        /// </summary>
        public int ConversionsAll { get; set; }

        /// <summary>
        /// Gets or sets the "Conversions" metric for Conversion type 1.
        /// </summary>
        public int ConversionsConvType1 { get; set; }

        /// <summary>
        /// Gets or sets the "Conversions" metric for Conversion type 2.
        /// </summary>
        public int ConversionsConvType2 { get; set; }

        /// <summary>
        /// Gets or sets the "Conversions" metric for Conversion type 3.
        /// </summary>
        public int ConversionsConvType3 { get; set; }

        /// <summary>
        /// Gets or sets the "Sales" metric for all conversion types.
        /// </summary>
        public decimal SalesAll { get; set; }

        /// <summary>
        /// Gets or sets the "Sales" metric for Conversion type 1.
        /// </summary>
        public decimal SalesConvType1 { get; set; }

        /// <summary>
        /// Gets or sets the "Sales" metric for Conversion type 2.
        /// </summary>
        public decimal SalesConvType2 { get; set; }

        /// <summary>
        /// Gets or sets the "Sales" metric for Conversion type 3.
        /// </summary>
        public decimal SalesConvType3 { get; set; }

        /// <summary>
        /// Gets or sets the "Impressions" metric for the "Campaign unique" level.
        /// </summary>
        public int UniqueImpressions { get; set; }
    }
}
