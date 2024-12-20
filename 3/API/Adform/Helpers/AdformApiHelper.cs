﻿using System;
using System.Collections.Generic;
using System.Linq;
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

        public static readonly Dictionary<Dimension, string> Dimensions = new Dictionary<Dimension, string>
        {
            { Dimension.Date, "date" },
            { Dimension.Campaign, "campaign" },
            { Dimension.CampaignId, "campaignID" },
            { Dimension.Order, "order" },
            { Dimension.OrderId, "orderID" },
            { Dimension.LineItem, "lineItem" },
            { Dimension.LineItemId, "lineItemID" },
            { Dimension.TrackingPoint, "page" },
            { Dimension.TrackingPointId, "pageID" },
            { Dimension.Banner, "banner" },
            { Dimension.BannerId, "bannerID" },
            { Dimension.MediaId, "mediaID" },
            { Dimension.Media, "media" },
            { Dimension.AdInteractionType, "adInteractionType" }, // Click, Impression, etc.
        };

        private static readonly string[] BasicMetrics = { CostMetric, ImpressionsMetric, ClicksMetric };

        private static readonly string[] ConversionMetrics = { ConversionsMetric, SalesMetric };

        private static readonly string[] ConversionTypes =
        {
            SpecsValueConversionTypeAll, SpecsValueConversionType1, SpecsValueConversionType2, SpecsValueConversionType3,
        };

        /// <summary>
        /// Creates the filter for request a report by report settings.
        /// </summary>
        /// <param name="settings">Report settings.</param>
        /// <returns>Filter object.</returns>
        public static dynamic CreateRequestFilterBySettings(ReportSettings settings)
        {
            dynamic filter = new System.Dynamic.ExpandoObject();
            filter.Client = new[] { settings.ClientId };
            filter.Date = GetDatesFilter(settings.StartDate, settings.EndDate);
            if (IsTrackingFilterNeed(settings.TrackingIds))
            {
                filter.Tracking = settings.TrackingIds;
            }
            return filter;
        }

        public static MetricMetadata[] GetMetrics(ReportSettings settings)
        {
            var metrics = new List<MetricMetadata>();
            if (settings.BasicMetrics)
            {
                SetBasicMetrics(metrics);
            }
            if (settings.ConvMetrics)
            {
                SetConversionMetrics(metrics);
                SetUniqueImpressionMetric(metrics);
            }
            return metrics.ToArray();
        }

        public static string[] GetDimensions(ReportSettings settings)
        {
            var defaultDimensions = new[]
            {
                Dimension.MediaId,
                Dimension.Date,
            };
            var dimensions = settings.Dimensions == null
                ? defaultDimensions
                : settings.Dimensions.Concat(defaultDimensions);
            var strDimensions = dimensions.Select(x => Dimensions[x]).ToArray();
            return strDimensions;
        }

        private static void SetBasicMetrics(List<MetricMetadata> metrics)
        {
            var basicMetrics = BasicMetrics.Select(GetBasicMetric);
            metrics.AddRange(basicMetrics);
        }

        private static void SetConversionMetrics(List<MetricMetadata> metrics)
        {
            var convMetrics = ConversionMetrics.SelectMany(GetConversionMetrics);
            metrics.AddRange(convMetrics);
        }

        private static void SetUniqueImpressionMetric(List<MetricMetadata> metrics)
        {
            var uniqueImpressionsMetric = GetUniqueImpressionsMetric();
            metrics.Add(uniqueImpressionsMetric);
        }

        private static MetricMetadata GetBasicMetric(string metricName)
        {
            return new MetricMetadata
            {
                Metric = metricName,
            };
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

        private static Dates GetDatesFilter(DateTime startDate, DateTime endDate)
        {
            const string dateFormat = "yyyy'-'M'-'d";
            return new Dates
            {
                From = startDate.ToString(dateFormat),
                To = endDate.ToString(dateFormat),
            };
        }

        private static bool IsTrackingFilterNeed(IEnumerable<string> trackingIds)
        {
            return trackingIds.Any(x => !string.IsNullOrEmpty(x));
        }
    }
}