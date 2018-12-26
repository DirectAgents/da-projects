using Amazon.Entities.HelperEntities;
using Amazon.Enums;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Amazon.Helpers
{
    public static class AmazonApiHelper
    {
        private const string ApiVersion = "v2";

        private const string CostMetric = "cost";
        private const string ImpressionsMetric = "impressions";
        private const string ClicksMetric = "clicks";
        private const string CampaignIdMetric = "campaignId";
        private const string CampaignNameMetric = "campaignName";
        private const string AdGroupIdMetric = "adGroupId";
        private const string AdGroupNameMetric = "adGroupName";
        private const string AsinMetric = "asin";
        private const string KeywordTextMetric = "keywordText";
        private const string TargetTextMetric = "targetingText";

        private static readonly Dictionary<CampaignType, string> CampaignTypeNames =
            new Dictionary<CampaignType, string>
            {
                {CampaignType.SponsoredProducts, "sp"},
                {CampaignType.SponsoredBrands, "hsa"},
                {CampaignType.Empty, string.Empty}
            };

        private static readonly Dictionary<CampaignType, string> CampaignTypeReadableNames =
            new Dictionary<CampaignType, string>
            {
                {CampaignType.SponsoredProducts, CampaignType.SponsoredProducts.ToString()},
                {CampaignType.SponsoredBrands, CampaignType.SponsoredBrands.ToString()},
                {CampaignType.ProductDisplay, CampaignType.ProductDisplay.ToString()},
            };

        private static readonly Dictionary<EntitesType, string> EntitiesTypeNames = new Dictionary<EntitesType, string>
        {
            {EntitesType.Campaigns, "campaigns"},
            {EntitesType.AdGroups, "adGroups"},
            {EntitesType.Keywords, "keywords"},
            {EntitesType.SearchTerm, "keywords"},
            {EntitesType.ProductAds, "productAds"},
            {EntitesType.Asins, "asins"},
            {EntitesType.Profiles, "profiles"},
            {EntitesType.TargetSearchTerm, "targets"},
            {EntitesType.TargetKeywords, "targets"}
        };

        private static readonly Dictionary<AttributedMetricType, Dictionary<AttributedMetricDaysInterval, string>>
            AttributedMetrics =
                new Dictionary<AttributedMetricType, Dictionary<AttributedMetricDaysInterval, string>>
                {
                    {
                        AttributedMetricType.attributedConversions, new Dictionary<AttributedMetricDaysInterval, string>
                        {
                            {AttributedMetricDaysInterval.Days1, "attributedConversions1d"},
                            {AttributedMetricDaysInterval.Days7, "attributedConversions7d"},
                            {AttributedMetricDaysInterval.Days14, "attributedConversions14d"},
                            {AttributedMetricDaysInterval.Days30, "attributedConversions30d"},
                        }
                    },
                    {
                        AttributedMetricType.attributedConversionsSameSKU,
                        new Dictionary<AttributedMetricDaysInterval, string>
                        {
                            {AttributedMetricDaysInterval.Days1, "attributedConversions1dSameSKU"},
                            {AttributedMetricDaysInterval.Days7, "attributedConversions7dSameSKU"},
                            {AttributedMetricDaysInterval.Days14, "attributedConversions14dSameSKU"},
                            {AttributedMetricDaysInterval.Days30, "attributedConversions30dSameSKU"},
                        }
                    },
                    {
                        AttributedMetricType.attributedUnitsOrdered,
                        new Dictionary<AttributedMetricDaysInterval, string>
                        {
                            {AttributedMetricDaysInterval.Days1, "attributedUnitsOrdered1d"},
                            {AttributedMetricDaysInterval.Days7, "attributedUnitsOrdered7d"},
                            {AttributedMetricDaysInterval.Days14, "attributedUnitsOrdered14d"},
                            {AttributedMetricDaysInterval.Days30, "attributedUnitsOrdered30d"},
                        }
                    },
                    {
                        AttributedMetricType.attributedSales, new Dictionary<AttributedMetricDaysInterval, string>
                        {
                            {AttributedMetricDaysInterval.Days1, "attributedSales1d"},
                            {AttributedMetricDaysInterval.Days7, "attributedSales7d"},
                            {AttributedMetricDaysInterval.Days14, "attributedSales14d"},
                            {AttributedMetricDaysInterval.Days30, "attributedSales30d"},
                        }
                    },
                    {
                        AttributedMetricType.attributedSalesSameSKU,
                        new Dictionary<AttributedMetricDaysInterval, string>
                        {
                            {AttributedMetricDaysInterval.Days1, "attributedSales1dSameSKU"},
                            {AttributedMetricDaysInterval.Days7, "attributedSales7dSameSKU"},
                            {AttributedMetricDaysInterval.Days14, "attributedSales14dSameSKU"},
                            {AttributedMetricDaysInterval.Days30, "attributedSales30dSameSKU"},
                        }
                    },
                };

        public static string GetCampaignTypeName(CampaignType type)
        {
            return CampaignTypeReadableNames[type];
        }

        public static string GetEntityListRelativePath(EntitesType entitiesType, CampaignType campaignType)
        {
            return GetBaseEntitiesPath(entitiesType, campaignType);
        }

        public static string GetDataRequestRelativePath(EntitesType entitiesType, CampaignType campaignType,
            string dataType)
        {
            var basePath = GetBaseEntitiesPath(entitiesType, campaignType);
            var resourcePath = $"{basePath}/{dataType}";
            return resourcePath;
        }

        public static string GetPreparedDataRelativePath(string dataType, string dataId)
        {
            return $"{ApiVersion}/{dataType}/{dataId}";
        }

        public static AmazonApiReportParams CreateReportParams(EntitesType entitiesType, CampaignType campaignType,
            DateTime date, bool includeCampaignName)
        {
            var allMetrics = GetReportMetrics(entitiesType, campaignType, includeCampaignName);
            var reportParams = new AmazonApiReportParams
            {
                reportDate = date.ToString("yyyyMMdd"),
                metrics = allMetrics
            };
            if (entitiesType == EntitesType.SearchTerm || entitiesType == EntitesType.TargetSearchTerm)
            {
                reportParams.segment = "query";
            }

            return reportParams;
        }

        public static AmazonApiSnapshotParams CreateSnapshotParams()
        {
            var snapshotParams = new AmazonApiSnapshotParams
            {
                stateFilter = "enabled,paused,archived"
            };
            return snapshotParams;
        }

        private static string GetBaseEntitiesPath(EntitesType entitiesType, CampaignType campaignType)
        {
            var campaignTypePath = campaignType == CampaignType.Empty ? "" : CampaignTypeNames[campaignType] + "/";
            var resourcePath = $"{ApiVersion}/{campaignTypePath}{EntitiesTypeNames[entitiesType]}";
            return resourcePath;
        }

        private static string GetReportMetrics(EntitesType entitiesType, CampaignType campaignType,
            bool includeCampaignName)
        {
            var metrics = new List<string> {CostMetric, ImpressionsMetric, ClicksMetric};
            if (includeCampaignName)
            {
                metrics.Add(CampaignNameMetric);
            }

            var dependentCampaignMetrics = GetDependentCampaignReportMetrics(campaignType);
            metrics.AddRange(dependentCampaignMetrics);
            var dependentEntityMetrics = GetDependentEntityReportMetrics(entitiesType);
            metrics.AddRange(dependentEntityMetrics);
            var joinedMetrics = string.Join(",", metrics);
            return joinedMetrics;
        }

        private static IEnumerable<string> GetDependentCampaignReportMetrics(CampaignType campaignType)
        {
            var dependentCampaignMetrics = (campaignType == CampaignType.SponsoredBrands)
                ? new[]
                {
                    AttributedMetrics[AttributedMetricType.attributedSales][AttributedMetricDaysInterval.Days14],
                    AttributedMetrics[AttributedMetricType.attributedSalesSameSKU][AttributedMetricDaysInterval.Days14],
                    AttributedMetrics[AttributedMetricType.attributedConversions][AttributedMetricDaysInterval.Days14],
                    AttributedMetrics[AttributedMetricType.attributedConversionsSameSKU][
                        AttributedMetricDaysInterval.Days14],
                }
                : AttributedMetrics.SelectMany(x => x.Value.Select(y => y.Value));
            return dependentCampaignMetrics;
        }

        private static IEnumerable<string> GetDependentEntityReportMetrics(EntitesType entitiesType)
        {
            switch (entitiesType)
            {
                case EntitesType.AdGroups:
                    return new[] {CampaignIdMetric, AdGroupNameMetric};
                case EntitesType.ProductAds:
                    return new[] {AdGroupIdMetric, AdGroupNameMetric, AsinMetric};
                case EntitesType.Keywords:
                case EntitesType.SearchTerm:
                    return new[] {CampaignIdMetric, AdGroupIdMetric, AdGroupNameMetric, KeywordTextMetric};
                case EntitesType.TargetKeywords:
                case EntitesType.TargetSearchTerm:
                    return new[] {CampaignIdMetric, AdGroupIdMetric, AdGroupNameMetric, TargetTextMetric};
                case EntitesType.Campaigns:
                    break;
                case EntitesType.Asins:
                    break;
                case EntitesType.Profiles:
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(entitiesType), entitiesType, null);
            }

            return new string[0];
        }
    }
}
