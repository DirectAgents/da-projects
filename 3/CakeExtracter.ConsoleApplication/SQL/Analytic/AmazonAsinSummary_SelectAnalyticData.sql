DECLARE @AccountId      INT         = @@param_0,
        @StartDate      DATETIME    = '@@param_1',
        @EndDate        DATETIME    = '@@param_2';

DECLARE @CampaignType   NVARCHAR(MAX)   = 'SponsoredProducts',
        @AsinTypeId     INT             = 2;

SELECT
  adSummary.Date            AS "Date",
  adExternalId.ExternalId   AS "ASIN",
  account.Name              AS "Brand",
  strategy.Name             AS "Campaign",
  adSet.Name                AS "AdGroup",
  account.Id                AS "AccountId",
  adSummary.Impressions     AS "Impressions",
  adSummary.Clicks          AS "Clicks",
  adSummary.PostClickConv   AS "PostClickConv",
  adSummary.PostViewConv    AS "PostViewConv",
  adSummary.Cost            AS "Cost",
  adSummary.AllClicks       AS "AllClicks",
  summaryMetrics.[1]        AS "AttributedConversions1",
  summaryMetrics.[2]        AS "AttributedConversions7",
  summaryMetrics.[3]        AS "AttributedConversions14",
  summaryMetrics.[4]        AS "AttributedConversions30",
  summaryMetrics.[5]        AS "AttributedConversionsSameSKU1",
  summaryMetrics.[6]        AS "AttributedConversionsSameSKU7",
  summaryMetrics.[7]        AS "AttributedConversionsSameSKU14",
  summaryMetrics.[8]        AS "AttributedConversionsSameSKU30",
  summaryMetrics.[9]        AS "AttributedSales1",
  summaryMetrics.[10]       AS "AttributedSales7",
  summaryMetrics.[11]       AS "AttributedSales14",
  summaryMetrics.[12]       AS "AttributedSales30",
  summaryMetrics.[13]       AS "AttributedSalesSameSKU1",
  summaryMetrics.[14]       AS "AttributedSalesSameSKU7",
  summaryMetrics.[15]       AS "AttributedSalesSameSKU14",
  summaryMetrics.[16]       AS "AttributedSalesSameSKU30",
  summaryMetrics.[17]       AS "AttributedUnitsOrdered1",
  summaryMetrics.[18]       AS "AttributedUnitsOrdered7",
  summaryMetrics.[19]       AS "AttributedUnitsOrdered14",
  summaryMetrics.[20]       AS "AttributedUnitsOrdered30",
  summaryMetrics.[44]       AS "AttributedSalesOtherSKU1",
  summaryMetrics.[45]       AS "AttributedSalesOtherSKU7",
  summaryMetrics.[46]       AS "AttributedSalesOtherSKU14",
  summaryMetrics.[47]       AS "AttributedSalesOtherSKU30",
  summaryMetrics.[48]       AS "AttributedUnitsOrderedOtherSKU1",
  summaryMetrics.[49]       AS "AttributedUnitsOrderedOtherSKU7",
  summaryMetrics.[50]       AS "AttributedUnitsOrderedOtherSKU14",
  summaryMetrics.[51]       AS "AttributedUnitsOrderedOtherSKU30"
FROM
(
    SELECT
        adSummaryMetric.Date,
        adSummaryMetric.TDadId,
        adSummaryMetric.MetricTypeId,
        adSummaryMetric.Value
    FROM td.AdSummaryMetric adSummaryMetric
) AS metrics
PIVOT
(
    AVG(metrics.Value) FOR metrics.MetricTypeId IN 
    (
        [1], [2], [3], [4], [5], [6], [7], [8], [9], [10],
        [11], [12], [13], [14], [15], [16], [17], [18], [19], [20],
        [44], [45], [46], [47], [48], [49], [50], [51]
    )
) AS summaryMetrics
RIGHT JOIN td.AdSummary adSummary ON summaryMetrics.Date = adSummary.Date AND summaryMetrics.TDadId = adSummary.TDadId
INNER JOIN td.Ad ad ON adSummary.TDadId = ad.Id
INNER JOIN td.AdSet adSet ON ad.AdSetId = adSet.Id
INNER JOIN td.Account account ON adSet.AccountId = account.Id
INNER JOIN td.Strategy strategy ON adSet.StrategyId = strategy.Id
INNER JOIN td.AdExternalId adExternalId ON ad.Id = adExternalId.AdId
WHERE adExternalId.TypeId = @AsinTypeId
  AND strategy.TypeId = (SELECT Id FROM td.Type WHERE Name = @CampaignType)
  AND account.Id = @AccountId
  AND adSummary.Date >= @StartDate
  AND adSummary.Date <= @EndDate
ORDER BY adSummary.Date DESC;
