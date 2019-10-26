using System.Collections.Generic;
using System.Linq;
using Adform.Constants;
using Adform.Entities;
using Adform.Entities.ReportEntities.ReportParameters;
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
        public const string SpecsAdUniqueness = "adUniqueness";

        public const string SpecsValueConversionTypeAll = "allConversionTypes";
        public const string SpecsValueConversionType1 = "conversionType1";
        public const string SpecsValueConversionType2 = "conversionType2";
        public const string SpecsValueConversionType3 = "conversionType3";
        public const string SpecsValueCampaignUnique = "campaignUnique";
        public const string SpecsValueAll = "all";

        private const string DateFormat = "yyyy'-'M'-'d";

        public static readonly Dictionary<Dimension, string> Dimensions = new Dictionary<Dimension, string>
        {
            { Dimension.Date, "date" },
            { Dimension.Campaign, "campaign" },
            { Dimension.Order, "order" },
            { Dimension.LineItem, "lineItem" },
            { Dimension.Banner, "banner" },
            { Dimension.Media, "media" },
            { Dimension.AdInteractionType, "adInteractionType" }, // Click, Impression, etc.
        };

        private static readonly string[] BasicMetrics = { CostMetric, ImpressionsMetric, ClicksMetric };

        private static readonly string[] ConversionMetrics = { ConversionsMetric, SalesMetric };

        private static readonly string[] ConversionTypes =
        {
            SpecsValueConversionTypeAll, SpecsValueConversionType1, SpecsValueConversionType2, SpecsValueConversionType3,
        };

        public static ReportFilter GetFilters(ReportSettings settings)
        {
            var filter = settings.TrackingIds.Any(x => !string.IsNullOrEmpty(x))
                ? new ReportFilterWithTracking
                {
                    Tracking = settings.TrackingIds,
                }
                : new ReportFilter();
            InitializeReportFilter(filter, settings);
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
                var uniqueImpressionsMetric = GetUniqueImpressionsMetric();
                metrics.Add(uniqueImpressionsMetric);
            }
            return metrics.ToArray();
        }

        public static string[] GetDimensions(ReportSettings settings)
        {
            var defaultDimensions = new[] { Dimension.Date };
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
                Metric = metricName,
            };
        }

        private static void InitializeReportFilter(ReportFilter filter, ReportSettings settings)
        {
            filter.Client = new[] { settings.ClientId };
            filter.Date = new Dates
            {
                From = settings.StartDate.ToString(DateFormat),
                To = settings.EndDate.ToString(DateFormat),
            };
            filter.Media = settings.RtbMediaOnly
                ? GetRtbMedia()
                : GetMultipleMedia();
        }

        private static IEnumerable<MetricMetadata> GetConversionMetrics(string metricName)
        {
            return ConversionTypes.Select(convType => new MetricMetadata
            {
                Metric = metricName,
                Specs = new
                {
                    conversionType = convType,
                },
            });
        }

        private static MetricMetadata GetUniqueImpressionsMetric()
        {
            return new MetricMetadata
            {
                Metric = ImpressionsMetric,
                Specs = new
                {
                    adUniqueness = SpecsValueCampaignUnique,
                },
            };
        }

        private static Media GetRtbMedia()
        {
            return new Media { Name = new[] { MediaName.RtbMediaName } };
        }

        private static Media GetMultipleMedia()
        {
            return new Media
            {
                Name = new[] { MediaName.RtbMediaName, MediaName.DbmMediaName, MediaName.TtdMediaName, MediaName.YamMediaName },
            };
        }
    }
}