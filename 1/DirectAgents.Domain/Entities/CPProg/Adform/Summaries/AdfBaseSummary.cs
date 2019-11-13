using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace DirectAgents.Domain.Entities.CPProg.Adform.Summaries
{
    /// <summary>
    /// Adform base summary metric entity.
    /// </summary>
    public abstract class AdfBaseSummary
    {
        /// <summary>
        /// Gets or sets the identifier.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets the identifier of parent entity.
        /// </summary>
        public int EntityId { get; set; }

        /// <summary>
        /// Gets or sets the identifier of media type.
        /// </summary>
        public int MediaTypeId { get; set; }

        /// <summary>
        /// Gets or sets the date.
        /// </summary>
        public DateTime Date { get; set; }

        /// <summary>
        /// Gets or sets the media type.
        /// </summary>
        [ForeignKey("MediaTypeId")]
        public virtual AdfMediaType MediaType { get; set; }

        /// <summary>
        /// Gets or sets the impressions metric value.
        /// </summary>
        public int Impressions { get; set; }

        /// <summary>
        /// Gets or sets the unique impressions metric value.
        /// </summary>
        public int UniqueImpressions { get; set; }

        /// <summary>
        /// Gets or sets the clicks metric value.
        /// </summary>
        public int Clicks { get; set; }

        /// <summary>
        /// Gets or sets all clicks metric value.
        /// </summary>
        public int AllClicks { get; set; }

        /// <summary>
        /// Gets or sets the post click conversion metric value.
        /// </summary>
        public int PostClickConv { get; set; }

        /// <summary>
        /// Gets or sets the post view conversion metric value.
        /// </summary>
        public int PostViewConv { get; set; }

        /// <summary>
        /// Gets or sets the post click revenue metric value.
        /// </summary>
        public decimal PostClickRev { get; set; }

        /// <summary>
        /// Gets or sets the post view revenue metric value.
        /// </summary>
        public decimal PostViewRev { get; set; }

        /// <summary>
        /// Gets or sets the cost metric value.
        /// </summary>
        public decimal Cost { get; set; }

        /// <summary>
        /// Gets or sets the conversions metric value for all conversion types.
        /// </summary>
        public int ConversionsAll { get; set; }

        /// <summary>
        /// Gets or sets the conversions metric value for conversion type 1.
        /// </summary>
        public int ConversionsConvType1 { get; set; }

        /// <summary>
        /// Gets or sets the conversions metric value for conversion type 2.
        /// </summary>
        public int ConversionsConvType2 { get; set; }

        /// <summary>
        /// Gets or sets the conversions metric value for conversion type 3.
        /// </summary>
        public int ConversionsConvType3 { get; set; }

        /// <summary>
        /// Gets or sets the sales metric value for all conversion types.
        /// </summary>
        public decimal SalesAll { get; set; }

        /// <summary>
        /// Gets or sets the sales metric value for conversion type 1.
        /// </summary>
        public decimal SalesConvType1 { get; set; }

        /// <summary>
        /// Gets or sets the sales metric value for conversion type 2.
        /// </summary>
        public decimal SalesConvType2 { get; set; }

        /// <summary>
        /// Gets or sets the sales metric value for conversion type 3.
        /// </summary>
        public decimal SalesConvType3 { get; set; }
    }
}
