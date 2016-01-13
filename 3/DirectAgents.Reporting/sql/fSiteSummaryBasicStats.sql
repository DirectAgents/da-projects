alter FUNCTION [td].[fSiteSummaryBasicStats](
@AdvertiserId int
, @StartDate datetime
, @EndDate datetime
) RETURNS TABLE AS RETURN
/*
declare @AdvertiserId int, @StartDate datetime, @EndDate datetime
select @AdvertiserId = 3, @StartDate = '1/1/2016', @EndDate = getdate();
--*/
WITH revenue AS
(
	SELECT SiteSummary.Date
	, Site.Name AS SiteName
	, Site.Id AS SiteId
	, SiteSummary.Impressions
	, SiteSummary.Clicks
	, SiteSummary.PostClickConv + SiteSummary.PostViewConv AS Conversions
	, SiteSummary.Cost
	, CASE	WHEN budgetInfo.MarginPct = 100 THEN SiteSummary.Cost * (1 + (budgetInfo.MgmtFeePct / 100))
			ELSE SiteSummary.Cost / (1 - (budgetInfo.MarginPct / 100))
	  END AS Revenue
	, budgetInfo.MediaSpend AS BIMediaSpend
	, budgetInfo.MgmtFeePct AS BIMgmtFeePct
	, budgetInfo.MarginPct AS BIMarginPct
	FROM fBudgetInfo(@AdvertiserId, default, default) budgetInfo
	INNER JOIN td.SiteSummary ON td.SiteSummary.AccountId = budgetInfo.AccountId
	INNER JOIN td.Site ON td.Site.Id = td.SiteSummary.SiteId
	WHERE (td.SiteSummary.Date BETWEEN @StartDate AND @EndDate)
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
SELECT Date
, SiteName
, SiteId
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