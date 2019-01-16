/*
This script helps to extract all statistics for Sponsored Brands campaigns
for the corresponding account Id and range of dates
by levels: campaigns, ad groups, keywords

Before execution, please set the following values:
- accId (ID of the target account in the database)
- startDate (statistics start date for extraction)
- endDate (statistics end date for extraction)

NOTE: The Metric types IDs used in this script may vary depending on the target database.
*/

DECLARE @accId          INT           = 1433;              -- Account ID
DECLARE @startDate      NVARCHAR(MAX) = '12/1/2018';       -- Metrics start date
DECLARE @endDate        NVARCHAR(MAX) = '12/31/2018';      -- Metrics end date
DECLARE @campaignType   NVARCHAR(MAX) = 'SponsoredBrands'; -- Campaign types

/* Account IDs:
1362  AvoDerm
1433  Belkin
1437  Carhartt Sportswear - Mens
1438  Carhartt Women's Collection
1434  Linksys Invoices
1439  Living Fresh
1423  Walker & Company
1436  Walker & Company TEST
1435  Wemo Invoice
*/

-----------------------------------------------------------------------------------------------------------------------------

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
  summaryMetrics.[7]          as "14 days total orders same SKU"
from (select
        strategySummaryMetric.Date,
        strategySummaryMetric.StrategyId,
        strategySummaryMetric.MetricTypeId,
        strategySummaryMetric.Value
      from td.StrategySummaryMetric strategySummaryMetric) as metrics
     pivot (
       avg(metrics.Value)
     for metrics.MetricTypeId
     in ([11], [15], [3], [7]) ) as summaryMetrics
  right join td.StrategySummary strategySummary on summaryMetrics.Date = strategySummary.Date and summaryMetrics.StrategyId = strategySummary.StrategyId
  inner join td.Strategy strategy on strategySummary.StrategyId = strategy.Id
  inner join td.Account account on strategy.AccountId = account.Id
where strategy.TypeId = (select Id from td.Type where Name = @campaignType)
  and account.Id = @accId
  and strategySummary.Date between @startDate and @endDate
order by strategySummary.Date desc;

-- AD GROUPS --
select
  adSetSummary.Date,
  account.Name                as "Brand",
  strategy.Name               as "Campaign",
  AdSet.Name                  as "Ad Group",
  adSetSummary.Impressions    as "Impressions",
  adSetSummary.Clicks         as "Clicks",
  adSetSummary.Cost           as "Spend",
  summaryMetrics.[11]         as "14 days total sales",
  summaryMetrics.[15]         as "14 days total sales same SKU",
  summaryMetrics.[3]          as "14 days total orders",
  summaryMetrics.[7]          as "14 days total orders same SKU"
from (select
        adSetSummaryMetric.Date,
        adSetSummaryMetric.AdSetId,
        adSetSummaryMetric.MetricTypeId,
        adSetSummaryMetric.Value
      from td.AdSetSummaryMetric adSetSummaryMetric) as metrics
     pivot (
       avg(metrics.Value)
     for metrics.MetricTypeId
     in ([11], [15], [3], [7]) ) as summaryMetrics
  right join td.AdSetSummary adSetSummary on summaryMetrics.Date = adSetSummary.Date and summaryMetrics.AdSetId = adSetSummary.AdSetId
  inner join td.AdSet adSet on adSetSummary.AdSetId = adSet.Id
  inner join td.Strategy strategy on adSet.StrategyId = strategy.Id
  inner join td.Account account on strategy.AccountId = account.Id
where strategy.TypeId = (select Id from td.Type where Name = @campaignType)
  and account.Id = @accId
  and adSetSummary.Date between @startDate and @endDate
order by adSetSummary.Date desc;

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
  summaryMetrics.[7]          as "14 days total orders same SKU"
from (select
        keywordSummaryMetric.Date,
        keywordSummaryMetric.KeywordId,
        keywordSummaryMetric.MetricTypeId,
        keywordSummaryMetric.Value
      from td.KeywordSummaryMetric keywordSummaryMetric) as metrics
     pivot (
       avg(metrics.Value)
     for metrics.MetricTypeId
     in ([11], [15], [3], [7]) ) as summaryMetrics
  right join td.KeywordSummary keywordSummary on summaryMetrics.Date = keywordSummary.Date and summaryMetrics.KeywordId = keywordSummary.KeywordId
  inner join td.Keyword keyword on keywordSummary.KeywordId = keyword.Id
  inner join td.AdSet adSet on keyword.AdSetId = adSet.Id
  inner join td.Strategy strategy on keyword.StrategyId = strategy.Id
  inner join td.Account account on strategy.AccountId = account.Id
where strategy.TypeId = (select Id from td.Type where Name = @campaignType)
  and account.Id = @accId
  and keywordSummary.Date between @startDate and @endDate
order by keywordSummary.Date desc;