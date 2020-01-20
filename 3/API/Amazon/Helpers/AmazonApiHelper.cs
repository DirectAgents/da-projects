using System;
using System.Collections.Generic;
using Amazon.Constants;
using Amazon.Entities.HelperEntities;
using Amazon.Enums;

namespace Amazon.Helpers
{
    public static class AmazonApiHelper
    {
        private const string ApiVersion = "v2";
        private const string DateFormat = "yyyyMMdd";
        private const string ItemsSeparator = ",";

        private const string CostMetric = "cost";
        private const string ImpressionsMetric = "impressions";
        private const string ClicksMetric = "clicks";
        private const string CampaignIdMetric = "campaignId";
        private const string CampaignNameMetric = "campaignName";
        private const string AdGroupIdMetric = "adGroupId";
        private const string AdGroupNameMetric = "adGroupName";
        private const string KeywordTextMetric = "keywordText";
        private const string TargetTextMetric = "targetingText";
        private const string AsinMetric = "asin";
        private const string OtherAsinMetric = "otherAsin";
        //only for non-vendor profiles
        private const string SkuMetric = "sku";

        private const string SegmentDimensionalQuery = "query";
        //Tactic parametr, Choose one of them 
        private const string TacticDimensionalQuery = "remarketing";
       // private const string TacticDimensionalQuery = "T00001";

        private const string SponsoredProductsCampaignType = "sponsoredProducts";
        private const string CampaignEnabledState = "enabled";
        private const string CampaignPausedState = "paused";
        private const string CampaignArchivedState = "archived";

        private static readonly Dictionary<CampaignType, string> CampaignTypeNames =
            new Dictionary<CampaignType, string>
            {
                {CampaignType.SponsoredProducts, "sp"},
                {CampaignType.SponsoredBrands, "hsa"},
                {CampaignType.ProductDisplay, "sd"},
                {CampaignType.Empty, string.Empty},
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
                        AttributedMetricType.attributedUnitsOrderedOtherSKU,
                        new Dictionary<AttributedMetricDaysInterval, string>
                        {
                            {AttributedMetricDaysInterval.Days1, "attributedUnitsOrdered1dOtherSKU"},
                            {AttributedMetricDaysInterval.Days7, "attributedUnitsOrdered7dOtherSKU"},
                            {AttributedMetricDaysInterval.Days14, "attributedUnitsOrdered14dOtherSKU"},
                            {AttributedMetricDaysInterval.Days30, "attributedUnitsOrdered30dOtherSKU"},
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
                    {
                        AttributedMetricType.attributedSalesOtherSKU,
                        new Dictionary<AttributedMetricDaysInterval, string>
                        {
                            {AttributedMetricDaysInterval.Days1, "attributedSales1dOtherSKU"},
                            {AttributedMetricDaysInterval.Days7, "attributedSales7dOtherSKU"},
                            {AttributedMetricDaysInterval.Days14, "attributedSales14dOtherSKU"},
                            {AttributedMetricDaysInterval.Days30, "attributedSales30dOtherSKU"},
                        }
                    },
                    {
                        AttributedMetricType.attributedUnitsSold,
                        new Dictionary<AttributedMetricDaysInterval, string>
                        {
                            {AttributedMetricDaysInterval.Days14, "attributedUnitsSold14d"},
                        }
                    },
                    {
                        AttributedMetricType.attributedDPV,
                        new Dictionary<AttributedMetricDaysInterval, string>
                        {
                            {AttributedMetricDaysInterval.Days14, "attributedDPV14d"},
                        }
                    },
                };

        private static readonly IEnumerable<string> CommonMetrics = new[] {CostMetric, ImpressionsMetric, ClicksMetric};

        private static readonly Dictionary<CampaignType, IEnumerable<string>> DependentCampaignTypeMetrics =
            new Dictionary<CampaignType, IEnumerable<string>>
            {
                {
                    CampaignType.SponsoredBrands, new[]
                    {
                        AttributedMetrics[AttributedMetricType.attributedSales][AttributedMetricDaysInterval.Days14],
                        AttributedMetrics[AttributedMetricType.attributedSalesSameSKU][AttributedMetricDaysInterval.Days14],
                        AttributedMetrics[AttributedMetricType.attributedConversions][AttributedMetricDaysInterval.Days14],
                        AttributedMetrics[AttributedMetricType.attributedConversionsSameSKU][AttributedMetricDaysInterval.Days14],
                    }
                },
                {
                    CampaignType.SponsoredProducts, new[]
                    {
                        AttributedMetrics[AttributedMetricType.attributedSales][AttributedMetricDaysInterval.Days1],
                        AttributedMetrics[AttributedMetricType.attributedSales][AttributedMetricDaysInterval.Days7],
                        AttributedMetrics[AttributedMetricType.attributedSales][AttributedMetricDaysInterval.Days14],
                        AttributedMetrics[AttributedMetricType.attributedSales][AttributedMetricDaysInterval.Days30],
                        AttributedMetrics[AttributedMetricType.attributedSalesSameSKU][AttributedMetricDaysInterval.Days1],
                        AttributedMetrics[AttributedMetricType.attributedSalesSameSKU][AttributedMetricDaysInterval.Days7],
                        AttributedMetrics[AttributedMetricType.attributedSalesSameSKU][AttributedMetricDaysInterval.Days14],
                        AttributedMetrics[AttributedMetricType.attributedSalesSameSKU][AttributedMetricDaysInterval.Days30],
                        AttributedMetrics[AttributedMetricType.attributedConversions][AttributedMetricDaysInterval.Days1],
                        AttributedMetrics[AttributedMetricType.attributedConversions][AttributedMetricDaysInterval.Days7],
                        AttributedMetrics[AttributedMetricType.attributedConversions][AttributedMetricDaysInterval.Days14],
                        AttributedMetrics[AttributedMetricType.attributedConversions][AttributedMetricDaysInterval.Days30],
                        AttributedMetrics[AttributedMetricType.attributedConversionsSameSKU][AttributedMetricDaysInterval.Days1],
                        AttributedMetrics[AttributedMetricType.attributedConversionsSameSKU][AttributedMetricDaysInterval.Days7],
                        AttributedMetrics[AttributedMetricType.attributedConversionsSameSKU][AttributedMetricDaysInterval.Days14],
                        AttributedMetrics[AttributedMetricType.attributedConversionsSameSKU][AttributedMetricDaysInterval.Days30],
                        AttributedMetrics[AttributedMetricType.attributedUnitsOrdered][AttributedMetricDaysInterval.Days1],
                        AttributedMetrics[AttributedMetricType.attributedUnitsOrdered][AttributedMetricDaysInterval.Days7],
                        AttributedMetrics[AttributedMetricType.attributedUnitsOrdered][AttributedMetricDaysInterval.Days14],
                        AttributedMetrics[AttributedMetricType.attributedUnitsOrdered][AttributedMetricDaysInterval.Days30],
                    }
                },
                {
                    CampaignType.ProductDisplay, new[]
                    {
                      /* AttributedMetrics[AttributedMetricType.attributedDPV][AttributedMetricDaysInterval.Days14],
                       AttributedMetrics[AttributedMetricType.attributedUnitsSold][AttributedMetricDaysInterval.Days14],
                       AttributedMetrics[AttributedMetricType.attributedSales][AttributedMetricDaysInterval.Days14],*/
                       AttributedMetrics[AttributedMetricType.attributedUnitsOrdered][AttributedMetricDaysInterval.Days1],
                       AttributedMetrics[AttributedMetricType.attributedUnitsOrdered][AttributedMetricDaysInterval.Days7],
                       AttributedMetrics[AttributedMetricType.attributedUnitsOrdered][AttributedMetricDaysInterval.Days14],
                       AttributedMetrics[AttributedMetricType.attributedUnitsOrdered][AttributedMetricDaysInterval.Days30],
                    }
                },
            };

        private static readonly Dictionary<EntitesType, IEnumerable<string>> DependentEntityTypeMetrics =
            new Dictionary<EntitesType, IEnumerable<string>>
            {
                {
                    EntitesType.Campaigns, Array.Empty<string>()
                },
                {
                    EntitesType.AdGroups, new[] {CampaignIdMetric, AdGroupNameMetric}
                },
                {
                    EntitesType.ProductAds, new[] { CampaignIdMetric, AdGroupIdMetric, AdGroupNameMetric, AsinMetric}
                },
                {
                    EntitesType.Keywords, new[] {CampaignIdMetric, AdGroupIdMetric, AdGroupNameMetric, KeywordTextMetric}
                },
                {
                    EntitesType.SearchTerm, new[] {CampaignIdMetric, AdGroupIdMetric, AdGroupNameMetric, KeywordTextMetric}
                },
                {
                    EntitesType.TargetKeywords, new[] {CampaignIdMetric, AdGroupIdMetric, AdGroupNameMetric, TargetTextMetric}
                },
                {
                    EntitesType.TargetSearchTerm, new[] {CampaignIdMetric, AdGroupIdMetric, AdGroupNameMetric, TargetTextMetric}
                },
                {
                    EntitesType.Asins, new[]
                    {
                        AdGroupIdMetric, AsinMetric,
                        AttributedMetrics[AttributedMetricType.attributedUnitsOrderedOtherSKU][AttributedMetricDaysInterval.Days1],
                        AttributedMetrics[AttributedMetricType.attributedUnitsOrderedOtherSKU][AttributedMetricDaysInterval.Days7],
                        AttributedMetrics[AttributedMetricType.attributedUnitsOrderedOtherSKU][AttributedMetricDaysInterval.Days14],
                        AttributedMetrics[AttributedMetricType.attributedUnitsOrderedOtherSKU][AttributedMetricDaysInterval.Days30],
                        AttributedMetrics[AttributedMetricType.attributedSalesOtherSKU][AttributedMetricDaysInterval.Days1],
                        AttributedMetrics[AttributedMetricType.attributedSalesOtherSKU][AttributedMetricDaysInterval.Days7],
                        AttributedMetrics[AttributedMetricType.attributedSalesOtherSKU][AttributedMetricDaysInterval.Days14],
                        AttributedMetrics[AttributedMetricType.attributedSalesOtherSKU][AttributedMetricDaysInterval.Days30],
                    }
                },
            };

        private static readonly Dictionary<string, string> DependentCountryCodeApiEndpointUrls =
            new Dictionary<string, string>
            {
                { "US", ApiEndpointUrl.NorthAmerica },
                { "CA", ApiEndpointUrl.NorthAmerica },
                { "JP", ApiEndpointUrl.FarEast },
                { "AU", ApiEndpointUrl.FarEast },
                { "FR", ApiEndpointUrl.Europe },
                { "DE", ApiEndpointUrl.Europe },
                { "IT", ApiEndpointUrl.Europe },
                { "ES", ApiEndpointUrl.Europe },
                { "UK", ApiEndpointUrl.Europe },
            };

        public static string GetCampaignTypeName(CampaignType type)
        {
            return CampaignTypeReadableNames[type];
        }

        public static string GetEntityListRelativePath(EntitesType entitiesType, CampaignType campaignType)
        {
            return GetBaseEntitiesPath(entitiesType, campaignType);
        }

        public static string GetDataRequestRelativePath(EntitesType entitiesType, CampaignType campaignType, string dataType)
        {
            var basePath = GetBaseEntitiesPath(entitiesType, campaignType);
            var resourcePath = $"{basePath}/{dataType}";
            return resourcePath;
        }

        public static string GetPreparedDataRelativePath(string dataType, string dataId)
        {
            return $"{ApiVersion}/{dataType}/{dataId}";
        }

        public static AmazonApiReportSbAndSpParams CreateReportSbAndSpParams(EntitesType entitiesType, CampaignType campaignType,
            DateTime date, bool includeCampaignName)
        {
            var metrics = GetReportMetrics(entitiesType, campaignType, includeCampaignName);
            var reportParams = CreateReportSbAndSpParams(entitiesType, date, metrics);
            return reportParams;
        }

        public static AmazonApiReportSbAndSpParams CreateAsinReportSbAndSpParams(DateTime date)
        {
            var metrics = GetAsinReportMetrics();
            var reportParams = CreateReportSbAndSpParams(EntitesType.Asins, date, metrics);
            // The Amazon team has promised that it will be exploring a change to not require a campaign type for ASIN reports.
            reportParams.campaignType = SponsoredProductsCampaignType;
            return reportParams;
        }

        public static AmazonApiReportSdParams CreateReportSdParams(EntitesType entitiesType, CampaignType campaignType,
            DateTime date, bool includeCampaignName)
        {
            var metrics = GetReportMetrics(entitiesType, campaignType, includeCampaignName);
            var reportParams = CreateReportSdParams(entitiesType, date, metrics);
            return reportParams;
        }

        public static AmazonApiReportSdParams CreateAsinReportSdParams(DateTime date)
        {
            var metrics = GetAsinReportMetrics();
            var reportParams = CreateReportSdParams(EntitesType.Asins, date, metrics);
            // The Amazon team has promised that it will be exploring a change to not require a campaign type for ASIN reports.
            return reportParams;
        }

        public static AmazonApiSnapshotParams CreateSnapshotParams()
        {
            var states = new[] {CampaignArchivedState, CampaignEnabledState, CampaignPausedState};
            var snapshotParams = new AmazonApiSnapshotParams
            {
                stateFilter = TransformItemsForRequest(states)
            };
            return snapshotParams;
        }

        /// <summary>
        /// Gets appropriate API endpoint URL by the specified country code.
        /// </summary>
        /// <param name="countryCode">Code of country.</param>
        /// <returns>URL of the API endpoint.</returns>
        public static string GetAppropriateApiEndpointUrlByCountryCode(string countryCode)
        {
            DependentCountryCodeApiEndpointUrls.TryGetValue(countryCode, out var apiEndpointUrl);
            return apiEndpointUrl ?? ApiEndpointUrl.NorthAmerica;
        }

        private static string GetBaseEntitiesPath(EntitesType entitiesType, CampaignType campaignType)
        {
            var campaignTypePath = campaignType == CampaignType.Empty || entitiesType == EntitesType.Asins
                ? string.Empty
                : CampaignTypeNames[campaignType] + "/";
            var resourcePath = campaignType == CampaignType.ProductDisplay
               ? $"{campaignTypePath}{EntitiesTypeNames[entitiesType]}"
               : $"{ApiVersion}/{campaignTypePath}{EntitiesTypeNames[entitiesType]}";
            return resourcePath;
        }

        private static AmazonApiReportSbAndSpParams CreateReportSbAndSpParams(EntitesType entitiesType, DateTime date, IEnumerable<string> metrics)
        {
            var reportParams = new AmazonApiReportSbAndSpParams
            {
                reportDate = date.ToString(DateFormat),
                metrics = TransformItemsForRequest(metrics),
                segment = GetSegmentQuery(entitiesType),
            };
            return reportParams;
        }

        private static AmazonApiReportSdParams CreateReportSdParams(EntitesType entitiesType, DateTime date, IEnumerable<string> metrics)
        {
            var reportParams = new AmazonApiReportSdParams
            {
                reportDate = date.ToString(DateFormat),
                metrics = TransformItemsForRequest(metrics),
                tactic = GetTacticQuery(entitiesType),
            };
            return reportParams;
        }

        private static IEnumerable<string> GetReportMetrics(EntitesType entitiesType, CampaignType campaignType, bool includeCampaignName)
        {
            var metrics = new List<string>(CommonMetrics);
            if (includeCampaignName)
            {
                metrics.Add(CampaignNameMetric);
            }

            var dependentCampaignMetrics = DependentCampaignTypeMetrics[campaignType];
            var dependentEntityMetrics = DependentEntityTypeMetrics[entitiesType];
            metrics.AddRange(dependentCampaignMetrics);
            metrics.AddRange(dependentEntityMetrics);
            return metrics;
        }

        private static IEnumerable<string> GetAsinReportMetrics()
        {
            return DependentEntityTypeMetrics[EntitesType.Asins];
        }

        private static string TransformItemsForRequest(IEnumerable<string> items)
        {
            var joinedItems = string.Join(ItemsSeparator, items);
            return joinedItems;
        }

        private static string GetSegmentQuery(EntitesType entitiesType)
        {
            var segment = entitiesType == EntitesType.SearchTerm || entitiesType == EntitesType.TargetSearchTerm
                ? SegmentDimensionalQuery
                : default(string);
            return segment;
        }

        private static string GetTacticQuery(EntitesType entitiesType)
        {
            var tactic = entitiesType == EntitesType.Campaigns
                ? TacticDimensionalQuery
                : default(string);
            return tactic;
        }
    }
}
