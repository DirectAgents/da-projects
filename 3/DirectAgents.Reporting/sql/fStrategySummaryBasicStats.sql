ALTER FUNCTION [td].[fStrategySummaryBasicStats](
@AdvertiserId int
, @StartDate datetime
, @EndDate datetime
) RETURNS TABLE AS RETURN
WITH fBI AS
(
	SELECT * FROM td.fBudgetInfo(@AdvertiserId, default, default)
)
, revenue AS
(
	SELECT StrategySummary.Date
	, ISNULL('DA' + CAST(Platform.Id AS varchar) + ' ', '') + Strategy.Name AS StrategyName
	, Strategy.Id AS StrategyId
	, StrategySummary.Impressions
	, StrategySummary.Clicks
	, StrategySummary.PostClickConv
	, StrategySummary.PostViewConv
	, fBI.ShowClickAndViewConv
	, StrategySummary.PostClickConv + StrategySummary.PostViewConv AS Conversions
	, StrategySummary.Cost
	, CASE	WHEN fBI.MarginPct = 100 THEN StrategySummary.Cost * (1 + (fBI.MgmtFeePct / 100))
			ELSE StrategySummary.Cost / (1 - (fBI.MarginPct / 100))
	  END AS Revenue
	, fBI.MediaSpend AS BIMediaSpend
	, fBI.MgmtFeePct AS BIMgmtFeePct
	, fBI.MarginPct AS BIMarginPct
	FROM fBI
	INNER JOIN td.Strategy ON Strategy.AccountId = fBI.AccountId
	INNER JOIN td.StrategySummary ON StrategySummary.StrategyId = Strategy.Id
	LEFT OUTER JOIN td.Account ON Account.Id = Strategy.AccountId
	LEFT OUTER JOIN td.Platform ON Platform.Id = fBI.PlatformId
	WHERE (StrategySummary.Date BETWEEN @StartDate AND @EndDate)
	  AND (fBI.AsOfDate =
			(
			SELECT TOP 1 x.AsOfDate
			FROM fBI x
			WHERE (x.AccountId = Strategy.AccountId)
			  AND ((Account.PlatformId IS NULL) OR (x.PlatformId = Account.PlatformId))
			  AND (x.AsOfDate <= StrategySummary.Date)
			))
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
, PostClickConv
, PostViewConv
, ShowClickAndViewConv
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