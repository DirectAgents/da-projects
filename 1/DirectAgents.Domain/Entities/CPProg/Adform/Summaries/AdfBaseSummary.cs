using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DirectAgents.Domain.Entities.CPProg.Adform.Summaries
{
    /// <summary>
    /// Adform base summary entity.
    /// </summary>
    public abstract class AdfBaseSummary
    {
        /// <summary>
        /// Gets or sets the identifier of parent entity.
        /// </summary>
        [Key]
        public int EntityId { get; set; }

        /// <summary>
        /// Gets or sets the identifier of media type.
        /// </summary>
        [Key]
        public int MediaTypeId { get; set; }

        /// <summary>
        /// Gets or sets the date.
        /// </summary>
        [Key]
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
        /// Gets or sets the clicks metric value.
        /// </summary>
        public int Clicks { get; set; }

        /// <summary>
        /// Gets or sets the cost metric value.
        /// </summary>
        public decimal Cost { get; set; }

        /// <summary>
        /// Gets or sets the post click conversions metric value for all conversion types.
        /// </summary>
        public int ClickConversionsConvTypeAll { get; set; }

        /// <summary>
        /// Gets or sets the post click conversions metric value for conversion type 1.
        /// </summary>
        public int ClickConversionsConvType1 { get; set; }

        /// <summary>
        /// Gets or sets the post click conversions metric value for conversion type 2.
        /// </summary>
        public int ClickConversionsConvType2 { get; set; }

        /// <summary>
        /// Gets or sets the post click conversions metric value for conversion type 3.
        /// </summary>
        public int ClickConversionsConvType3 { get; set; }

        /// <summary>
        /// Gets or sets the post impression conversions metric value for all conversion types.
        /// </summary>
        public int ImpressionConversionsConvTypeAll { get; set; }

        /// <summary>
        /// Gets or sets the post impression conversions metric value for conversion type 1.
        /// </summary>
        public int ImpressionConversionsConvType1 { get; set; }

        /// <summary>
        /// Gets or sets the post impression conversions metric value for conversion type 2.
        /// </summary>
        public int ImpressionConversionsConvType2 { get; set; }

        /// <summary>
        /// Gets or sets the post impression conversions metric value for conversion type 3.
        /// </summary>
        public int ImpressionConversionsConvType3 { get; set; }

        /// <summary>
        /// Gets or sets the post click sales metric value for all conversion types.
        /// </summary>
        public decimal ClickSalesConvTypeAll { get; set; }

        /// <summary>
        /// Gets or sets the post click sales metric value for conversion type 1.
        /// </summary>
        public decimal ClickSalesConvType1 { get; set; }

        /// <summary>
        /// Gets or sets the post click sales metric value for conversion type 2.
        /// </summary>
        public decimal ClickSalesConvType2 { get; set; }

        /// <summary>
        /// Gets or sets the post click sales metric value for conversion type 3.
        /// </summary>
        public decimal ClickSalesConvType3 { get; set; }

        /// <summary>
        /// Gets or sets the post impression sales metric value for all conversion types.
        /// </summary>
        public decimal ImpressionSalesConvTypeAll { get; set; }

        /// <summary>
        /// Gets or sets the post impression sales metric value for conversion type 1.
        /// </summary>
        public decimal ImpressionSalesConvType1 { get; set; }

        /// <summary>
        /// Gets or sets the post impression sales metric value for conversion type 2.
        /// </summary>
        public decimal ImpressionSalesConvType2 { get; set; }

        /// <summary>
        /// Gets or sets the post impression sales metric value for conversion type 3.
        /// </summary>
        public decimal ImpressionSalesConvType3 { get; set; }

        /// <summary>
        /// Gets or sets the impressions metric value for campaign unique.
        /// </summary>
        public int UniqueImpressions { get; set; }

        /// <summary>
        /// Returns true if the object does not contain real metric values.
        /// </summary>
        /// <returns>Result if the object is empty.</returns>
        public bool IsEmpty()
        {
            const decimal emptyValue = default(decimal);
            return Impressions == emptyValue
                   && UniqueImpressions == emptyValue
                   && Clicks == emptyValue
                   && Cost == emptyValue
                   && ClickConversionsConvTypeAll == emptyValue
                   && ClickConversionsConvType1 == emptyValue
                   && ClickConversionsConvType2 == emptyValue
                   && ClickConversionsConvType3 == emptyValue
                   && ImpressionConversionsConvTypeAll == emptyValue
                   && ImpressionConversionsConvType1 == emptyValue
                   && ImpressionConversionsConvType2 == emptyValue
                   && ImpressionConversionsConvType3 == emptyValue
                   && ClickSalesConvTypeAll == emptyValue
                   && ClickSalesConvType1 == emptyValue
                   && ClickSalesConvType2 == emptyValue
                   && ClickSalesConvType3 == emptyValue
                   && ImpressionSalesConvTypeAll == emptyValue
                   && ImpressionSalesConvType1 == emptyValue
                   && ImpressionSalesConvType2 == emptyValue
                   && ImpressionSalesConvType3 == emptyValue;
        }
    }
}
