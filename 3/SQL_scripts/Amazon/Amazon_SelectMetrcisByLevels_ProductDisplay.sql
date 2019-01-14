/*
This script helps to extract all statistics for Product Display campaigns.
NOTE: The Metric types IDs used in this script may vary depending on the target database.
*/

DECLARE @CampaignTypePd NVARCHAR(MAX) = 'ProductDisplay';
DECLARE @campaignType NVARCHAR(MAX) = @CampaignTypePd;

-- CAMPAIGNS --
select
  strategySummary.Date,
  account.Name                as "Brand",
  strategy.Name               as "Campaign",
  strategySummary.Impressions as "Impressions",
  strategySummary.Clicks      as "Clicks",
  strategySummary.Cost        as "Spend",
  summaryMetrics.[11]         as "14 days total sales",
  summaryMetrics.[41]         as "Detail Page Views",
  summaryMetrics.[42]         as "14 days units ordered",
  summaryMetrics.[43]         as "Orders"
from (select
        strategySummaryMetric.Date,
        strategySummaryMetric.StrategyId,
        strategySummaryMetric.MetricTypeId,
        strategySummaryMetric.Value
      from td.StrategySummaryMetric strategySummaryMetric) as metrics
     pivot (
       avg(metrics.Value)
     for metrics.MetricTypeId
     in ([11], [41], [42], [43])) as summaryMetrics
  join td.StrategySummary strategySummary on summaryMetrics.Date = strategySummary.Date and summaryMetrics.StrategyId = strategySummary.StrategyId
  inner join td.Strategy strategy on strategySummary.StrategyId = strategy.Id
  inner join td.Account account on strategy.AccountId = account.Id
where strategy.TypeId = (select Id from td.Type where Name = @campaignType)
order by strategySummary.Date desc;