alter FUNCTION [td].[fSiteSummaryBasicStats](
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
	SELECT SiteSummary.Date
	, Site.Name AS SiteName
	, Site.Id AS SiteId
	, SiteSummary.Impressions
	, SiteSummary.Clicks
	, SiteSummary.PostClickConv + SiteSummary.PostViewConv AS Conversions
	, SiteSummary.Cost
	, CASE	WHEN fBI.MarginPct = 100 THEN SiteSummary.Cost * (1 + (fBI.MgmtFeePct / 100))
			ELSE SiteSummary.Cost / (1 - (fBI.MarginPct / 100))
	  END AS Revenue
	, fBI.MediaSpend AS BIMediaSpend
	, fBI.MgmtFeePct AS BIMgmtFeePct
	, fBI.MarginPct AS BIMarginPct
	FROM fBI
	INNER JOIN td.SiteSummary ON SiteSummary.AccountId = fBI.AccountId
	INNER JOIN td.Site ON Site.Id = SiteSummary.SiteId
	LEFT OUTER JOIN td.Account ON Account.Id = SiteSummary.AccountId
	WHERE (td.SiteSummary.Date BETWEEN @StartDate AND @EndDate)
	  AND (fBI.AsOfDate =
			(
			SELECT TOP 1 x.AsOfDate
			FROM fBI x
			WHERE (x.AccountId = SiteSummary.AccountId)
			  AND ((Account.PlatformId IS NULL) OR (x.PlatformId = Account.PlatformId))
			  AND (x.AsOfDate <= SiteSummary.Date)
			))
)
, revenueTop50 AS
(
	SELECT TOP 50 SiteId, SiteName
	, SUM(Impressions) AS SumImpressions
	FROM revenue
	GROUP BY SiteId, SiteName
	ORDER BY SumImpressions DESC, SiteName
)
, mediaSpend AS
(
	SELECT revenue.*
	, CASE	WHEN revenue.BIMediaSpend = 100 THEN revenue.Cost
			ELSE revenue.Revenue / (1 + (revenue.BIMgmtFeePct / 100))
	  END AS MediaSpend
	FROM revenue
	INNER JOIN revenueTop50 ON revenueTop50.SiteId = revenue.SiteId
)
, mgmtFee AS
(
	SELECT *
	, CASE	WHEN BIMgmtFeePct = 100 THEN MediaSpend * BIMgmtFeePct
				ELSE Revenue - MediaSpend
	  END AS MgmtFee
	FROM mediaSpend
)
SELECT SiteName, SiteId
, SUM(Impressions) AS Impressions
, SUM(Clicks) AS Clicks
, CASE WHEN SUM(Impressions) = 0 THEN 0 ELSE SUM(Clicks) / CAST(SUM(Impressions) AS float) END AS CTR
, SUM(Conversions) AS Conversions
/*
, CASE WHEN SUM(Clicks) = 0 THEN 0 ELSE SUM(Conversions) / CAST(SUM(Clicks) as float) END AS CR
, SUM(MediaSpend) AS MediaSpend
, SUM(MgmtFee) AS MgmtFee
, SUM(BIMediaSpend) AS Budget
, CASE WHEN SUM(BIMediaSpend) = 0 THEN 0 ELSE SUM(MediaSpend) / CAST(SUM(BIMediaSpend) as float) END AS Pacing
, CASE WHEN SUM(Clicks) = 0 THEN 0 ELSE SUM(MediaSpend) / CAST(SUM(Clicks) as float) END AS eCPC
, CASE WHEN SUM(Conversions) = 0 THEN 0 ELSE SUM(MediaSpend) / CAST(SUM(Conversions) as float) END AS eCPA
*/
FROM mgmtFee
GROUP BY SiteName, SiteId