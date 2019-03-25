using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using Adform.Entities;
using Adform.Entities.ReportEntities;
using Adform.Enums;

namespace Adform.Helpers
{
    internal class AdformApiHelper
    {
        public const string CostMetric = "cost";
        public const string ImpressionsMetric = "impressions";
        public const string ClicksMetric = "clicks";
        public const string ConversionsMetric = "conversions";
        public const string SalesMetric = "sales";

        public const string SpecsConversionType = "conversionType";

        public const string ConversionTypeAll = "allConversionTypes";
        public const string ConversionType1 = "conversionType1";
        public const string ConversionType2 = "conversionType2";
        public const string ConversionType3 = "conversionType3";

        private const string DateFormat = "yyyy'-'M'-'d";
        private const string RtbName = "Real Time Bidding";

        public static readonly Dictionary<Dimension, string> Dimensions = new Dictionary<Dimension, string>
        {
            {Dimension.Date, "date"},
            {Dimension.Campaign, "campaign"},
            {Dimension.Order, "order"},
            {Dimension.LineItem, "lineItem"},
            {Dimension.Banner, "banner"},
            {Dimension.Media, "media"},
            {Dimension.AdInteractionType, "adInteractionType"} // Click, Impression, etc.
        };

        private static readonly string[] BasicMetrics = {CostMetric, ImpressionsMetric, ClicksMetric};

        private static readonly string[] ConversionMetrics = {ConversionsMetric, SalesMetric };

        private static readonly string[] ConversionTypes = {ConversionTypeAll, ConversionType1, ConversionType2, ConversionType3};

        public static ExpandoObject GetFilters(ReportSettings settings)
        {
            dynamic filter = new ExpandoObject();
            filter.client = new[] {settings.ClientId};
            if (settings.TrackingId != null)
            {
                filter.tracking = settings.TrackingId;
            }

            filter.date = new Dates
            {
                from = settings.StartDate.ToString(DateFormat),
                to = settings.EndDate.ToString(DateFormat)
            };
            if (settings.RtbOnly)
            {
                filter.media = new {name = new[] {RtbName}};
            }

            return filter;
        }

        public static MetricMetadata[] GetMetrics(ReportSettings settings)
        {
            var metrics = new List<MetricMetadata>();
            if (settings.BasicMetrics)
            {
                var basicMetrics = BasicMetrics.Select(GetBasicMetric);
                metrics.AddRange(basicMetrics);
            }

            if (settings.ConvMetrics)
            {
                var convMetrics = ConversionMetrics.SelectMany(GetConversionMetrics);
                metrics.AddRange(convMetrics);
            }

            return metrics.ToArray();
        }

        public static string[] GetDimensions(ReportSettings settings)
        {
            var defaultDimensions = new[] {Dimension.Date};
            var dimensions = settings.Dimensions == null
                ? defaultDimensions
                : settings.Dimensions.Concat(defaultDimensions);
            var strDimensions = dimensions.Select(x => Dimensions[x]).ToArray();
            return strDimensions;
        }

        private static MetricMetadata GetBasicMetric(string metricName)
        {
            return new MetricMetadata
            {
                metric = metricName
            };
        }

        private static IEnumerable<MetricMetadata> GetConversionMetrics(string metricName)
        {
            return ConversionTypes.Select(convType => new MetricMetadata
            {
                metric = metricName,
                specs = new
                {
                    conversionType = convType
                }
            });
        }
    }
}
