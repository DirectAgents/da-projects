using System;

namespace DirectAgents.Domain.Entities.CPProg.DBM.SummaryMetrics
{
    /// <summary>
    /// DBM base summary DB entity.
    /// </summary>
    public class DbmBaseSummaryEntity
    {
        /// <summary>
        /// Gets or sets the identifier.
        /// </summary>
        public long Id { get; set; }

        /// <summary>
        /// Gets or sets the date.
        /// </summary>
        public DateTime Date { get; set; }

        /// <summary>
        /// Gets or sets the country.
        /// </summary>
        public string Country { get; set; }

        /// <summary>
        /// Gets or sets the identifier of entity.
        /// </summary>
        public int? EntityId { get; set; }

        /// <summary>
        /// Gets or sets the revenue.
        /// </summary>
        public decimal Revenue { get; set; }

        /// <summary>
        /// Gets or sets the impressions.
        /// </summary>
        public int Impressions { get; set; }

        /// <summary>
        /// Gets or sets the clicks.
        /// </summary>
        public int Clicks { get; set; }

        /// <summary>
        /// Gets or sets the post click conversions.
        /// </summary>
        public int PostClickConversions { get; set; }

        /// <summary>
        /// Gets or sets the post view conversions.
        /// </summary>
        public int PostViewConversions { get; set; }

        /// <summary>
        /// Gets or sets the CM post click revenue.
        /// </summary>
        public decimal CMPostClickRevenue { get; set; }

        /// <summary>
        /// Gets or sets the CM post view revenue.
        /// </summary>
        public decimal CMPostViewRevenue { get; set; }
    }
}