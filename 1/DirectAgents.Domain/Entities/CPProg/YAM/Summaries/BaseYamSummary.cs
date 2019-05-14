using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace DirectAgents.Domain.Entities.CPProg.YAM.Summaries
{
    /// <summary>
    /// Yahoo Base Summary Database entity.
    /// </summary>
    public abstract class BaseYamSummary
    {
        /// <summary>
        /// Gets or sets a database ID of a parent entity.
        /// </summary>
        /// <value>
        /// The entity ID.
        /// </value>
        [Key]
        public int EntityId { get; set; }

        /// <summary>
        /// Gets or sets a date of the summary.
        /// </summary>
        /// <value>
        /// The date.
        /// </value>
        [Key]
        public DateTime Date { get; set; }
        
        /// <summary>
        /// Gets or sets impressions.
        /// </summary>
        /// <value>
        /// The impressions.
        /// </value>
        public int Impressions { get; set; }

        /// <summary>
        /// Gets or sets clicks.
        /// </summary>
        /// <value>
        /// The clicks.
        /// </value>
        public int Clicks { get; set; }

        /// <summary>
        /// Gets or sets click-through conversions.
        /// </summary>
        /// <value>
        /// The click-through conversions.
        /// </value>
        public int ClickThroughConversion { get; set; }

        /// <summary>
        /// Gets or sets view-through conversions.
        /// </summary>
        /// <value>
        /// The view-through conversions.
        /// </value>
        public int ViewThroughConversion { get; set; }

        /// <summary>
        /// Gets or sets conversion value.
        /// </summary>
        /// <value>
        /// The conversion value.
        /// </value>
        public decimal ConversionValue { get; set; }

        /// <summary>
        /// Gets or sets advertiser spending.
        /// </summary>
        /// <value>
        /// The advertiser spending.
        /// </value>
        public decimal AdvertiserSpending { get; set; }

        /// <summary>
        /// Gets or sets conversion value from query by clicks.
        /// </summary>
        /// <value>
        /// The conversion value from query by clicks.
        /// </value>
        public decimal? ClickConversionValueByPixelQuery { get; set; }

        /// <summary>
        /// Gets or sets conversion value from query by views.
        /// </summary>
        /// <value>
        /// The conversion value from query by views.
        /// </value>
        public decimal? ViewConversionValueByPixelQuery { get; set; }

        /// <summary>
        /// Sets the base properties (not from a query) as the sum of the properties of the collection.
        /// </summary>
        /// <param name="stats">The collection of stats.</param>
        public virtual void SetBaseStats(IEnumerable<BaseYamSummary> stats)
        {
            Impressions = stats.Sum(x => x.Impressions);
            Clicks = stats.Sum(x => x.Clicks);
            ClickThroughConversion = stats.Sum(x => x.ClickThroughConversion);
            ViewThroughConversion = stats.Sum(x => x.ViewThroughConversion);
            ConversionValue = stats.Sum(x => x.ConversionValue);
            AdvertiserSpending = stats.Sum(x => x.AdvertiserSpending);
        }

        /// <summary>
        /// Returns true if the object does not contain real metric values.
        /// </summary>
        /// <returns>Result if the object is empty.</returns>
        public virtual bool IsEmpty()
        {
            const decimal emptyValue = default(decimal);
            return Impressions == emptyValue
                   && Clicks == emptyValue
                   && ClickThroughConversion == emptyValue
                   && ViewThroughConversion == emptyValue
                   && ConversionValue == emptyValue
                   && AdvertiserSpending == emptyValue
                   && (!ClickConversionValueByPixelQuery.HasValue || ClickConversionValueByPixelQuery == emptyValue)
                   && (!ViewConversionValueByPixelQuery.HasValue || ViewConversionValueByPixelQuery == emptyValue);
        }
    }
}
