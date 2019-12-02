using System;

namespace DirectAgents.Domain.Entities.CPProg.Facebook
{
    /// <summary>
    /// Facebook base summary metric entity.
    /// <see cref="https://developers.facebook.com/docs/marketing-api/insights/parameters/v4.0#parameters-and-fields"/>.
    /// </summary>
    public class FbBaseSummary
    {
        /// <summary>
        /// Gets or sets the date.
        /// </summary>
        /// <value>
        /// The date.
        /// </value>
        public DateTime Date { get; set; }

        /// <summary>
        /// Gets or sets the impressions metric value.
        /// The number of times your ads were on screen.
        /// </summary>
        /// <value>
        /// The impressions.
        /// </value>
        public int Impressions { get; set; }

        /// <summary>
        /// Gets or sets the clicks metric value.
        /// </summary>
        /// <value>
        /// The clicks.
        /// </value>
        public int Clicks { get; set; }

        /// <summary>
        /// Gets or sets all clicks metric value.
        /// </summary>
        /// <value>
        /// All clicks.
        /// </value>
        public int AllClicks { get; set; }

        /// <summary>
        /// Gets or sets the post click conversion metric value.
        /// </summary>
        /// <value>
        /// The post click conv.
        /// </value>
        public int PostClickConv { get; set; }

        /// <summary>
        /// Gets or sets the post view conversion metric value.
        /// </summary>
        /// <value>
        /// The post view conv.
        /// </value>
        public int PostViewConv { get; set; }

        /// <summary>
        /// Gets or sets the post click revenue metric value.
        /// </summary>
        /// <value>
        /// The post click rev.
        /// </value>
        public decimal PostClickRev { get; set; }

        /// <summary>
        /// Gets or sets the post view revenue metric value.
        /// </summary>
        /// <value>
        /// The post view rev.
        /// </value>
        public decimal PostViewRev { get; set; }

        /// <summary>
        /// Gets or sets the cost metric value.
        /// </summary>
        /// <value>
        /// The cost.
        /// </value>
        public decimal Cost { get; set; }
    }
}
