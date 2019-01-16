/*
This script helps to remove all account statistics for the corresponding date range.

Before execution, please set the following values:
- accId (ID of the target account in the database)
- date (statistics start date for deletion)
- endDate (statistics end date for deletion)
*/

DECLARE @accId   INT           = 1433;         -- Account ID
DECLARE @date    NVARCHAR(MAX) = '12/1/2018';  -- Metrics start date
DECLARE @endDate NVARCHAR(MAX) = '12/31/2018'; -- Metrics end date

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

delete
--select *
FROM [td].[DailySummaryMetric]
where AccountId = @accId
  and Date >= @date
  and Date <= @endDate;

delete
--select *
FROM [td].[DailySummary]
where AccountId = @accId
  and Date >= @date
  and Date <= @endDate;


  delete
  --select *
  from [td].[StrategySummaryMetric]
  where StrategyId in (select s.Id from [td].[Strategy] s where s.AccountId = @accId)
  and Date >= @date
  and Date <= @endDate;

  delete
  --select *
  from [td].[StrategySummary]
  where StrategyId in (select s.Id from [td].[Strategy] s where s.AccountId = @accId)
  and Date >= @date
  and Date <= @endDate;


  delete
  --select *
  from [td].[AdSetSummaryMetric]
  where AdSetId in (select a.Id from [td].[AdSet] a where a.AccountId = @accId)
  and Date >= @date
  and Date <= @endDate;

  delete
  --select *
  from [td].[AdSetSummary]
  where AdSetId in (select a.Id from [td].[AdSet] a where a.AccountId = @accId)
  and Date >= @date
  and Date <= @endDate;


  delete
  --select *
  FROM [td].AdSummaryMetric
  where TDadId in (select Id from td.Ad where AccountId = @accId)
  and Date >= @date
  and Date <= @endDate;

  delete
  --select *
  FROM [td].AdSummary
  where TDadId in (select Id from td.Ad where AccountId = @accId)
  and Date >= @date
  and Date <= @endDate;


  delete
  --select *
  FROM [td].KeywordSummaryMetric
  where KeywordId in (select Id from td.Keyword where AccountId = @accId)
  and Date >= @date
  and Date <= @endDate;

  delete
  --select *
  FROM [td].KeywordSummary
  where KeywordId in (select Id from td.Keyword where AccountId = @accId)
   and Date >= @date
   and Date <= @endDate;


  delete
  --select *
  FROM [td].SearchTermSummaryMetric
  where SearchTermId in (select Id from td.SearchTerm where AccountId = @accId)
   and Date >= @date
   and Date <= @endDate;

  delete
  --select *
  FROM [td].SearchTermSummary
  where SearchTermId in (select Id from td.SearchTerm where AccountId = @accId)
   and Date >= @date
   and Date <= @endDate;
