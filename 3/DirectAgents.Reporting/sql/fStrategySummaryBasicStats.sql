alter FUNCTION [td].[fStrategySummaryBasicStats](
@AdvertiserId int
, @StartDate datetime
, @EndDate datetime
) RETURNS TABLE AS RETURN
/*
declare @StartDate datetime, @EndDate datetime, @AdvertiserId int
select @StartDate = '12/1/2015', @EndDate = getdate(), @AdvertiserId = 21

select DATEDIFF(day, @StartDate, Date) / 7 AS WeekNum
, DATEPART(weekday, Date) AS DayOfWeek
, (DATEDIFF(day, @StartDate, Date) % 7) + 1 AS DayOfWeek
, * from td.fDailySummaryBasicStats(4, @StartDate, DATEADD(day, 13, @StartDate))

select DatePart(weekday, Date) AS Weekday, * from td.fDailySummaryBasicStats(4, @StartDate, @EndDate)



select * from td.fStrategySummaryBasicStats(3, '11/1/2015', getdate())
select AdvertiserId
FROM td.Campaign
INNER JOIN td.Account ON td.Account.CampaignId = td.Campaign.Id
where td.ACcount.Id = 21
*/
WITH budgetInfo AS
(
	SELECT ISNULL(td.PlatformBudgetInfo.MediaSpend, ISNULL(td.budgetInfo.MediaSpend, td.Campaign.MediaSpend)) AS MediaSpend
		, ISNULL(td.PlatformBudgetInfo.MgmtFeePct, ISNULL(td.budgetInfo.MgmtFeePct, td.Campaign.MgmtFeePct)) AS MgmtFeePct
		, ISNULL(td.PlatformBudgetInfo.MarginPct, ISNULL(td.budgetInfo.MarginPct, td.Campaign.MarginPct)) AS MarginPct
		, td.Account.Id AS AccountId
	FROM td.Campaign
	INNER JOIN td.Account ON td.Account.CampaignId = td.Campaign.Id
	LEFT OUTER JOIN td.budgetInfo
	   ON (td.budgetInfo.CampaignId = td.Campaign.Id)
	  AND (td.budgetInfo.Date BETWEEN @StartDate AND @EndDate)
	LEFT OUTER JOIN td.PlatformBudgetInfo
	   ON (td.PlatformBudgetInfo.CampaignId = td.Campaign.Id)
	  AND (td.PlatformBudgetInfo.Date BETWEEN @StartDate AND @EndDate)
	WHERE (td.Campaign.AdvertiserId = @AdvertiserId)
)
, revenue AS
(
	SELECT StrategySummary.Date
	, Strategy.Name AS StrategyName
	, Strategy.Id AS StrategyId
	, StrategySummary.Impressions
	, StrategySummary.Clicks
	, StrategySummary.PostClickConv + StrategySummary.PostViewConv AS Conversions
	, StrategySummary.Cost
	, CASE	WHEN budgetInfo.MarginPct = 100 THEN StrategySummary.Cost * (1 + (budgetInfo.MgmtFeePct / 100))
			ELSE StrategySummary.Cost / (1 - (budgetInfo.MarginPct / 100))
	  END AS Revenue
	, budgetInfo.MediaSpend AS BIMediaSpend
	, budgetInfo.MgmtFeePct AS BIMgmtFeePct
	, budgetInfo.MarginPct AS BIMarginPct
	FROM budgetInfo
	INNER JOIN td.Strategy ON td.Strategy.AccountId = budgetInfo.AccountId
	INNER JOIN td.StrategySummary ON td.StrategySummary.StrategyId = td.Strategy.Id
	WHERE (td.StrategySummary.Date BETWEEN @StartDate AND @EndDate)
)
, mediaSpend AS
(
	SELECT *
	, CASE	WHEN revenue.BIMediaSpend = 100 THEN revenue.Cost
			ELSE revenue.Revenue / (1 + (revenue.BIMgmtFeePct / 100))
	  END AS MediaSpend
	FROM revenue
)
SELECT Date
, StrategyName
, StrategyId
, Impressions
, Clicks
, CASE WHEN Impressions = 0 THEN 0 ELSE Clicks / CAST(Impressions AS float) END AS CTR
, Conversions AS Conversions
, CASE WHEN Clicks = 0 THEN 0 ELSE Conversions / CAST(Clicks as float) END AS CR
, MediaSpend
, CASE	WHEN BIMgmtFeePct = 100 THEN MediaSpend * BIMgmtFeePct
			ELSE Revenue - MediaSpend
  END AS MgmtFee
, BIMediaSpend AS Budget
, CASE WHEN BIMediaSpend = 0 THEN 0 ELSE MediaSpend / CAST(BIMediaSpend as float) END AS Pacing
, CASE WHEN Clicks = 0 THEN 0 ELSE MediaSpend / CAST(Clicks as float) END AS eCPC
, CASE WHEN Conversions = 0 THEN 0 ELSE MediaSpend / CAST(Conversions as float) END AS eCPA
-- debugging
, Cost
, Revenue
, BIMediaSpend, BIMgmtFeePct, BIMarginPct
FROM mediaSpend
