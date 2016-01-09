ALTER FUNCTION [td].[fCreativeSummaryBasicStats](
@AdvertiserId int
, @StartDate datetime
, @EndDate datetime
) RETURNS TABLE AS RETURN
WITH revenue AS
(
	SELECT AdSummary.Date
	, Ad.Name AS AdName
	, Ad.Id AS AdId
	, AdSummary.Impressions
	, AdSummary.Clicks
	, AdSummary.PostClickConv + AdSummary.PostViewConv AS Conversions
	, CASE WHEN AdSummary.Impressions = 0 THEN 0 ELSE AdSummary.Clicks / CAST(AdSummary.Impressions AS float) END AS CTR
	, AdSummary.Cost
	, CASE	WHEN budgetInfo.MarginPct = 100 THEN AdSummary.Cost * (1 + (budgetInfo.MgmtFeePct / 100))
			ELSE AdSummary.Cost / (1 - (budgetInfo.MarginPct / 100))
	  END AS Revenue
	, budgetInfo.MediaSpend AS BIMediaSpend
	, budgetInfo.MgmtFeePct AS BIMgmtFeePct
	, budgetInfo.MarginPct AS BIMarginPct
	FROM fBudgetInfo(@AdvertiserId, @StartDate, @EndDate) budgetInfo
	INNER JOIN td.Ad ON td.Ad.AccountId = budgetInfo.AccountId
	INNER JOIN td.AdSummary ON td.AdSummary.TDadId = Ad.Id
	WHERE (td.AdSummary.Date BETWEEN @StartDate AND @EndDate)
)
, revenueTopTen AS
(
	SELECT TOP 10 AdId
	, SumCTR
	FROM
		(
		SELECT AdId
		, SUM(Impressions) AS SumImpressions
		, CASE WHEN SUM(Impressions) = 0 THEN 0 ELSE SUM(Clicks) / CAST(SUM(Impressions) AS float) END AS SumCTR
		FROM revenue
		GROUP BY AdId
		) x
	ORDER BY CASE WHEN SumImpressions > 5000 THEN 1 ELSE 0 END DESC, SumCTR DESC
)
, mediaSpend AS
(
	SELECT revenue.*
	, CASE	WHEN revenue.BIMediaSpend = 100 THEN revenue.Cost
			ELSE revenue.Revenue / (1 + (revenue.BIMgmtFeePct / 100))
	  END AS MediaSpend
	, revenueTopTen.SumCTR
	FROM revenue
	INNER JOIN revenueTopTen ON revenueTopTen.adId = revenue.AdId
)
SELECT Date
, AdName
, AdId
, Impressions
, Clicks
, CTR
, SumCTR
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