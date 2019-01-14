/*
This script helps to extract all statistics for Sponsored Products campaigns by levels: 
campaigns, product ads, keywords

NOTE: The Metric types IDs used in this script may vary depending on the target database.
*/

DECLARE @CampaignTypeSp NVARCHAR(MAX) = 'SponsoredProducts';
DECLARE @campaignType NVARCHAR(MAX) = @CampaignTypeSp;

-- CAMPAIGNS --
select
  strategySummary.Date,
  account.Name                as "Brand",
  strategy.Name               as "Campaign",
  strategySummary.Impressions as "Impressions",
  strategySummary.Clicks      as "Clicks",
  strategySummary.Cost        as "Spend",
  summaryMetrics.[11]         as "14 days total sales",
  summaryMetrics.[15]         as "14 days total sales same SKU",
  summaryMetrics.[3]          as "14 days total orders",
  summaryMetrics.[7]          as "14 days total orders same SKU",
  summaryMetrics.[19]         as "14 days total units"
from (select
        strategySummaryMetric.Date,
        strategySummaryMetric.StrategyId,
        strategySummaryMetric.MetricTypeId,
        strategySummaryMetric.Value
      from td.StrategySummaryMetric strategySummaryMetric) as metrics
     pivot (
       avg(metrics.Value)
     for metrics.MetricTypeId
     in ([11], [15], [3], [7], [19]) ) as summaryMetrics
  right join td.StrategySummary strategySummary on summaryMetrics.Date = strategySummary.Date and summaryMetrics.StrategyId = strategySummary.StrategyId
  inner join td.Strategy strategy on strategySummary.StrategyId = strategy.Id
  inner join td.Account account on strategy.AccountId = account.Id
where strategy.TypeId = (select Id from td.Type where Name = @campaignType)
order by strategySummary.Date desc;

-- PRODUCT ADS (ASINS) --
select
  adSummary.Date,
  account.Name            as "Brand",
  strategy.Name           as "Campaign",
  adSet.Name              as "Ad Group",
  adExternalId.ExternalId as "ASIN",
  adSummary.Impressions   as "Impressions",
  adSummary.Clicks        as "Clicks",
  adSummary.Cost          as "Spend",
  summaryMetrics.[11]         as "14 days total sales",
  summaryMetrics.[15]         as "14 days total sales same SKU",
  summaryMetrics.[45]         as "14 days total sales other SKU",
  summaryMetrics.[3]          as "14 days total orders",
  summaryMetrics.[7]          as "14 days total orders same SKU",
  summaryMetrics.[19]         as "14 days total units",
  summaryMetrics.[49]         as "14 days total units other SKU"
from (select
        adSummaryMetric.Date,
        adSummaryMetric.TDadId,
        adSummaryMetric.MetricTypeId,
        adSummaryMetric.Value
      from td.AdSummaryMetric adSummaryMetric) as metrics
     pivot (
       avg(metrics.Value)
     for metrics.MetricTypeId
     in ([11], [15], [3], [7], [19], [49], [45]) ) as summaryMetrics
  right join td.AdSummary adSummary on summaryMetrics.Date = adSummary.Date and summaryMetrics.TDadId = adSummary.TDadId
  inner join td.Ad ad on adSummary.TDadId = ad.Id
  inner join td.AdSet adSet on ad.AdSetId = adSet.Id
  inner join td.Account account on adSet.AccountId = account.Id
  inner join td.Strategy strategy on adSet.StrategyId = strategy.Id
  inner join td.AdExternalId adExternalId on ad.Id = adExternalId.AdId
where adExternalId.TypeId = 2
  and strategy.TypeId = (select Id from td.Type where Name = @campaignType)  
order by adSummary.Date desc;

-- KEYWORDS --
select
  keywordSummary.Date,
  account.Name                as "Brand",
  strategy.Name               as "Campaign",
  AdSet.Name                  as "Ad Group",
  keyword.Name                as "Keyword",
  keywordSummary.Impressions  as "Impressions",
  keywordSummary.Clicks       as "Clicks",
  keywordSummary.Cost         as "Spend",
  summaryMetrics.[11]         as "14 days total sales",
  summaryMetrics.[15]         as "14 days total sales same SKU",
  summaryMetrics.[3]          as "14 days total orders",
  summaryMetrics.[7]          as "14 days total orders same SKU",
  summaryMetrics.[19]         as "14 days total units"
from (select
        keywordSummaryMetric.Date,
        keywordSummaryMetric.KeywordId,
        keywordSummaryMetric.MetricTypeId,
        keywordSummaryMetric.Value
      from td.KeywordSummaryMetric keywordSummaryMetric) as metrics
     pivot (
       avg(metrics.Value)
     for metrics.MetricTypeId
     in ([11], [15], [3], [7], [19]) ) as summaryMetrics
  right join td.KeywordSummary keywordSummary on summaryMetrics.Date = keywordSummary.Date and summaryMetrics.KeywordId = keywordSummary.KeywordId
  inner join td.Keyword keyword on keywordSummary.KeywordId = keyword.Id
  inner join td.AdSet adSet on keyword.AdSetId = adSet.Id
  inner join td.Strategy strategy on keyword.StrategyId = strategy.Id
  inner join td.Account account on strategy.AccountId = account.Id
where strategy.TypeId = (select Id from td.Type where Name = @campaignType)
order by keywordSummary.Date desc;