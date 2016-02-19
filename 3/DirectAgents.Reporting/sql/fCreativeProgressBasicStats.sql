alter FUNCTION [td].[fCreativeProgressBasicStats](
@AdvertiserId int
, @StartDate datetime
, @EndDate datetime
, @KPI varchar(max)
) RETURNS TABLE AS RETURN
WITH fBI AS
(
	SELECT * FROM td.fBudgetInfo(@AdvertiserId, @StartDate, @EndDate)
)
, revenue AS
(
	SELECT AdSummary.Date
	, Ad.Name AS AdName
	, Ad.Id AS AdId
	, AdSummary.Impressions
	, AdSummary.Clicks
	, AdSummary.PostClickConv
	, AdSummary.PostViewConv
	, fBI.ShowClickAndViewConv
	, AdSummary.PostClickConv + AdSummary.PostViewConv AS Conversions
	, AdSummary.Cost
	, CASE	WHEN fBI.MarginPct = 100 THEN AdSummary.Cost * (1 + (fBI.MgmtFeePct / 100))
			ELSE AdSummary.Cost / (1 - (fBI.MarginPct / 100))
	  END AS Revenue
	, fBI.MediaSpend AS BIMediaSpend
	, fBI.MgmtFeePct AS BIMgmtFeePct
	, fBI.MarginPct AS BIMarginPct
	FROM fBI
	INNER JOIN td.Ad ON Ad.AccountId = fBI.AccountId
	INNER JOIN td.AdSummary ON AdSummary.TDadId = Ad.Id
	LEFT OUTER JOIN td.Account
	   ON (Account.Id = fBI.AccountId)
	  AND (Account.PlatformId = fBI.PlatformId)
	WHERE (td.AdSummary.Date BETWEEN @StartDate AND @EndDate)
	  AND (fBI.AsOfDate =
			(
			SELECT TOP 1 x.AsOfDate
			FROM fBI x
			WHERE (x.AccountId = Ad.AccountId)
			  AND ((Account.PlatformId IS NULL) OR (x.PlatformId = Account.PlatformId))
			  AND (x.AsOfDate <= AdSummary.Date)
			)))
, mediaSpend AS
(
	SELECT revenue.*
	, CASE	WHEN revenue.BIMediaSpend = 100 THEN revenue.Cost
			ELSE revenue.Revenue / (1 + (revenue.BIMgmtFeePct / 100))
	  END AS MediaSpend
	FROM revenue
)
SELECT AdName, AdId, ShowClickAndViewConv
, SUM(MediaSpend) AS MediaSpend
, SUM(Impressions) AS Impressions
, SUM(Clicks) AS Clicks
, CASE WHEN SUM(Impressions) = 0 THEN 0 ELSE SUM(Clicks) / CAST(SUM(Impressions) as float) END AS CTR
, CASE WHEN SUM(Conversions) = 0 THEN 0 ELSE SUM(MediaSpend) / CAST(SUM(Conversions) as float) END AS eCPA
, SUM(PostClickConv) AS PostClickConv
, SUM(PostViewConv) AS PostViewConv
, SUM(Conversions) AS Conversions
, CASE WHEN SUM(Clicks) = 0 THEN 0 ELSE SUM(MediaSpend) / CAST(SUM(Clicks) as float) END AS eCPC
, CASE @KPI
		WHEN 'Impressions' THEN SUM(Impressions)
		WHEN 'Clicks' THEN SUM(Clicks)
		WHEN 'CTR' THEN CASE WHEN SUM(Impressions) = 0 THEN 0 ELSE SUM(Clicks) / CAST(SUM(Impressions) AS float) END
		WHEN 'Conversions' THEN SUM(Conversions)
		WHEN 'CR' THEN CASE WHEN SUM(Clicks) = 0 THEN 0 ELSE SUM(Conversions) / CAST(SUM(Clicks) as float) END
		WHEN 'eCPC' THEN CASE WHEN SUM(Clicks) = 0 THEN 0 ELSE SUM(MediaSpend) / CAST(SUM(Clicks) as float) END
		WHEN 'eCPA' THEN CASE WHEN SUM(Conversions) = 0 THEN 0 ELSE SUM(MediaSpend) / CAST(SUM(Conversions) as float) END
	END AS SumKPI
FROM mediaSpend
GROUP BY AdName, AdId, ShowClickAndViewConv