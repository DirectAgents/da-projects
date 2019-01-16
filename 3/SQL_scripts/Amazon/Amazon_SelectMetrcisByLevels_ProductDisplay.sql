/*
This script helps to extract all statistics for Product Display campaigns
for the corresponding account Id and range of dates.

Before execution, please set the following values:
- accId (ID of the target account in the database)
- startDate (statistics start date for extraction)
- endDate (statistics end date for extraction)

NOTE: The Metric types IDs used in this script may vary depending on the target database.
*/

DECLARE @accId          INT           = 1433;             -- Account ID
DECLARE @startDate      NVARCHAR(MAX) = '12/1/2018';      -- Metrics start date
DECLARE @endDate        NVARCHAR(MAX) = '12/31/2018';     -- Metrics end date
DECLARE @campaignType   NVARCHAR(MAX) = 'ProductDisplay'; -- Campaign types

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
  and account.Id = @accId
  and strategySummary.Date between @startDate and @endDate
order by strategySummary.Date desc;