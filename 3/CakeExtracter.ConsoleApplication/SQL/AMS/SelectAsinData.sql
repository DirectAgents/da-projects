DECLARE @campaignType   NVARCHAR(MAX) = 'SponsoredProducts'; -- Campaign types

TRUNCATE TABLE td.AmazonAsinAnalytic


INSERT INTO td.AmazonAsinAnalytic (Date, ASIN, AccountId, Brand, Campaign, AdGroup,
								   Impressions, Clicks, PostClickConv, PostViewConv, Cost, AllClicks,
								   AttributedConversions1, AttributedConversions7, AttributedConversions14, AttributedConversions30,
								   AttributedConversionsSameSKU1,AttributedConversionsSameSKU7,AttributedConversionsSameSKU14,AttributedConversionsSameSKU30,
								   AttributedSales1,AttributedSales7,AttributedSales14,AttributedSales30,
								   AttributedSalesSameSKU1,AttributedSalesSameSKU7,AttributedSalesSameSKU14,AttributedSalesSameSKU30,
								   AttributedUnitsOrdered1,AttributedUnitsOrdered7,AttributedUnitsOrdered14,AttributedUnitsOrdered30,
								   AttributedSalesOtherSKU1,AttributedSalesOtherSKU7,AttributedSalesOtherSKU14,AttributedSalesOtherSKU30,
								   AttributedUnitsOrderedOtherSKU1,AttributedUnitsOrderedOtherSKU7,AttributedUnitsOrderedOtherSKU14,AttributedUnitsOrderedOtherSKU30
								    )
select
  adSummary.Date,
  adExternalId.ExternalId as "ASIN",
  account.Id              as "AccountId",
  account.Name            as "Brand",
  strategy.Name           as "Campaign",
  adSet.Name              as "Ad Group",
  
  adSummary.Impressions   as "Impressions",
  adSummary.Clicks        as "Clicks",
  adSummary.PostClickConv as "Post Click Conversion",
  adSummary.PostViewConv  as "Post View Conversion",
  adSummary.Cost          as "Spend",
  adSummary.AllClicks     as "AllClicks",

  summaryMetrics.[1]      as "1 days total orders",
  summaryMetrics.[2]      as "7 days total orders",
  summaryMetrics.[3]      as "14 days total orders",
  summaryMetrics.[4]      as "30 days total orders",

  summaryMetrics.[5]      as "1 days total orders same SKU",
  summaryMetrics.[6]      as "7 days total orders same SKU",
  summaryMetrics.[7]      as "14 days total orders same SKU",
  summaryMetrics.[8]      as "30 days total orders same SKU",

  summaryMetrics.[9]      as "1 days total sales",
  summaryMetrics.[10]     as "7 days total sales",
  summaryMetrics.[11]     as "14 days total sales",
  summaryMetrics.[12]     as "30 days total sales",

  summaryMetrics.[13]      as "1 days total sales same SKU",
  summaryMetrics.[14]     as "7 days total sales same SKU",
  summaryMetrics.[15]     as "14 days total sales same SKU",
  summaryMetrics.[16]     as "30 days total sales same SKU",

  summaryMetrics.[17]      as "1 days total units",
  summaryMetrics.[18]     as "7 days total units",
  summaryMetrics.[19]     as "14 days total units",
  summaryMetrics.[20]     as "30 days total units",

  summaryMetrics.[44]      as "1 days total sales other SKU",
  summaryMetrics.[45]     as "7 days total sales other SKU",
  summaryMetrics.[46]     as "14 days total sales other SKU",
  summaryMetrics.[47]     as "30 days total sales other SKU",

  summaryMetrics.[48]      as "1 days total units other SKU",
  summaryMetrics.[49]     as "7 days total units other SKU",
  summaryMetrics.[50]     as "14 days total units other SKU",
  summaryMetrics.[51]     as "30 days total units other SKU"


  --summaryMetrics.[11]         as "14 days total sales",
  --summaryMetrics.[15]         as "14 days total sales same SKU",
  --summaryMetrics.[46]         as "14 days total sales other SKU",
  --summaryMetrics.[3]          as "14 days total orders",
  --summaryMetrics.[7]          as "14 days total orders same SKU",
  --summaryMetrics.[19]         as "14 days total units",
  --summaryMetrics.[50]         as "14 days total units other SKU"
from (select
        adSummaryMetric.Date,
        adSummaryMetric.TDadId,
        adSummaryMetric.MetricTypeId,
        adSummaryMetric.Value
      from td.AdSummaryMetric adSummaryMetric) as metrics
     pivot (
       avg(metrics.Value)
     for metrics.MetricTypeId
     in ([1],[2],[3],[4],[5],[6],[7],[8],
	     [9],[10],[11],[12], [13],[14],[15],[16], 
		 [17],[18], [19], [20], 
		 [44],[45],[46],[47],[48],[49],[50],[51]) ) as summaryMetrics
  right join td.AdSummary adSummary on summaryMetrics.Date = adSummary.Date and summaryMetrics.TDadId = adSummary.TDadId
  inner join td.Ad ad on adSummary.TDadId = ad.Id
  inner join td.AdSet adSet on ad.AdSetId = adSet.Id
  inner join td.Account account on adSet.AccountId = account.Id
  inner join td.Strategy strategy on adSet.StrategyId = strategy.Id
  inner join td.AdExternalId adExternalId on ad.Id = adExternalId.AdId
where adExternalId.TypeId = 2
  and strategy.TypeId = (select Id from td.Type where Name = @campaignType)
order by adSummary.Date desc;
