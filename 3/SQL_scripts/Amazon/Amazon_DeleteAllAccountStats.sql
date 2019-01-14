/*
This script helps to remove all account statistics for the corresponding date range.
Before execution, please set the following values:
- accId (ID of the target account in the database)
- date (statistics start date for deletion)
- endDate (statistics end date for deletion)
*/

declare @accId int = 1435;
declare @date nvarchar(max) = '12/1/2018'
declare @endDate nvarchar(max) = '12/19/2018'

---------------------------------------------

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
   