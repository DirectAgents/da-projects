alter FUNCTION [td].[fSiteSummaryBasicStats](
@AdvertiserId int
, @StartDate datetime
, @EndDate datetime
) RETURNS TABLE AS RETURN
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
	FROM fBudgetInfo(@AdvertiserId, @StartDate, @EndDate) budgetInfo
	INNER JOIN td.Account ON td.Account.CampaignId = budgetInfo.CampaignId
	INNER JOIN td.SiteSummary ON td.SiteSummary.AccountId = td.Account.Id
	INNER JOIN td.Site ON td.Site.Id = td.SiteSummary.SiteId
	WHERE (td.SiteSummary.Date BETWEEN @StartDate AND @EndDate)
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