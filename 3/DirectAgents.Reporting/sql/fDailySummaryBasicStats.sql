USE [DirectAgentsDev]
GO
/****** Object:  UserDefinedFunction [td].[fDailySummaryBasicStats]    Script Date: 1/5/2016 4:05:22 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
ALTER FUNCTION [td].[fDailySummaryBasicStats](
@AdvertiserId int
, @StartDate datetime
, @EndDate datetime
) RETURNS TABLE AS RETURN
/*
select * from td.fExecutiveSummary(4, '11/1/2015', '11/30/2015')
*/
WITH budgetInfo AS
(
	SELECT ISNULL(td.PlatformBudgetInfo.MediaSpend, ISNULL(td.budgetInfo.MediaSpend, td.Campaign.MediaSpend)) AS MediaSpend
		, ISNULL(td.PlatformBudgetInfo.MgmtFeePct, ISNULL(td.budgetInfo.MgmtFeePct, td.Campaign.MgmtFeePct)) AS MgmtFeePct
		, ISNULL(td.PlatformBudgetInfo.MarginPct, ISNULL(td.budgetInfo.MarginPct, td.Campaign.MarginPct)) AS MarginPct
		, td.Campaign.Id AS CampaignId
		, td.Campaign.AdvertiserId
	FROM td.Campaign
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
	SELECT DailySummary.Date
	, DailySummary.Impressions
	, DailySummary.Clicks
	, DailySummary.PostClickConv + DailySummary.PostViewConv AS Conversions
	, DailySummary.Cost
	, CASE	WHEN budgetInfo.MarginPct = 100 THEN DailySummary.Cost * (1 + (budgetInfo.MgmtFeePct / 100))
			ELSE DailySummary.Cost / (1 - (budgetInfo.MarginPct / 100))
	  END AS Revenue
	, budgetInfo.MediaSpend AS BIMediaSpend
	, budgetInfo.MgmtFeePct AS BIMgmtFeePct
	, budgetInfo.MarginPct AS BIMarginPct
	FROM budgetInfo
	INNER JOIN td.Account ON td.Account.CampaignId = budgetInfo.CampaignId
	INNER JOIN td.DailySummary ON td.DailySummary.AccountId = td.Account.Id
	WHERE (td.DailySummary.Date BETWEEN @StartDate AND @EndDate)
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
