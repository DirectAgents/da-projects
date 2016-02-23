ALTER FUNCTION [td].[fDailySummaryBasicStats](
@AdvertiserId int
, @StartDate datetime
, @EndDate datetime
) RETURNS TABLE AS RETURN
WITH fBI AS
(
	SELECT * FROM td.fBudgetInfo(@AdvertiserId, @StartDate, @EndDate)
)
, revenue AS
(
	SELECT DailySummary.Date
	, DailySummary.Impressions
	, DailySummary.Clicks
	, DailySummary.PostClickConv + DailySummary.PostViewConv AS Conversions
	, DailySummary.Cost
	, CASE	WHEN fBI.MarginPct = 100 THEN DailySummary.Cost * (1 + (fBI.MgmtFeePct / 100))
			ELSE DailySummary.Cost / (1 - (fBI.MarginPct / 100))
	  END AS Revenue
	, fBI.MediaSpend AS BIMediaSpend
	, fBI.MgmtFeePct AS BIMgmtFeePct
	, fBI.MarginPct AS BIMarginPct
	FROM fBI
	INNER JOIN td.Account
	   ON (Account.Id = fBI.AccountId)
	  AND (Account.PlatformId = fBI.PlatformId)
	INNER JOIN td.DailySummary ON DailySummary.AccountId = Account.Id
	WHERE (td.DailySummary.Date BETWEEN @StartDate AND @EndDate)
	  AND (fBI.AsOfDate =
			(
			SELECT TOP 1 x.AsOfDate
			FROM fBI x
			WHERE (x.AccountId = Account.Id)
			  AND (x.PlatformId = Account.PlatformId)
			  AND (x.AsOfDate <= DailySummary.Date)
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