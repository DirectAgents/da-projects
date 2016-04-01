alter FUNCTION [td].[fDailySummaryBasicStats](
@AdvertiserId int
, @StartDate datetime
, @EndDate datetime
) RETURNS @ret TABLE
	(
	Date	datetime
	, Impressions	int
	, Clicks	int
	, CTR	float(53)
	, Conversions	int
	, CR	float(53)
	, MediaSpend	decimal(38,8)
	, MgmtFee	decimal(38,6)
	, Budget	float(53)
	, Pacing	float(53)
	, eCPC	float(53)
	, eCPA	float(53)
	)
AS
BEGIN
	DECLARE @Budget float
	SET @Budget = td.fBudget(@AdvertiserId, @StartDate, @EndDate)

	; WITH dtData AS
	(
		SELECT dt.Date
		, ISNULL(PlatformBudgetInfo.MgmtFeePct, ISNULL(BudgetInfo.MgmtFeePct, Campaign.MgmtFeePct)) AS MgmtFeePct
		, ISNULL(PlatformBudgetInfo.MarginPct, ISNULL(BudgetInfo.MarginPct, Campaign.MarginPct)) AS MarginPct
		, Account.Id AS AccountId
		FROM td.Campaign
		INNER JOIN td.Account ON Account.CampaignId = Campaign.Id
		INNER JOIN td.DailySummary dt
		   ON (dt.AccountId = Account.Id)
		  AND (dt.Date BETWEEN @StartDate AND @EndDate)
		LEFT OUTER JOIN td.BudgetInfo ON BudgetInfo.Id =
			(
			SELECT TOP 1 x.Id
			FROM td.BudgetInfo x
			WHERE (x.CampaignId = Account.CampaignId)
			  AND (x.Date BETWEEN @StartDate AND @EndDate)
			  AND (x.Date <= dt.Date)
			ORDER BY x.Date DESC
			)
		LEFT OUTER JOIN td.PlatformBudgetInfo ON PlatformBudgetInfo.Id =
			(
			SELECT TOP 1 x.Id
			FROM td.PlatformBudgetInfo x
			WHERE (x.CampaignId = Campaign.Id)
			  AND (x.PlatformId = Account.PlatformId)
			  AND (x.Date BETWEEN @StartDate AND @EndDate)
			  AND (x.Date <= dt.Date)
			ORDER BY x.Date DESC
			)
		WHERE (td.Campaign.AdvertiserId = @AdvertiserId)
	)
	, revenue AS
	(
		SELECT dt.Date
		, dt.Impressions
		, dt.Clicks
		, dt.PostClickConv + dt.PostViewConv AS Conversions
		, dt.Cost
		, CASE	WHEN dtData.MarginPct = 100 THEN dt.Cost * (1 + (dtData.MgmtFeePct / 100))
				ELSE dt.Cost / (1 - (dtData.MarginPct / 100))
		  END AS Revenue
		, dtData.MgmtFeePct AS BIMgmtFeePct
		, dtData.MarginPct AS BIMarginPct
		FROM td.DailySummary dt
		INNER JOIN dtData
		   ON (dtData.AccountId = dt.AccountId)
		  AND (dtData.Date = dt.Date)
	)
	, mediaSpend AS
	(
		SELECT *
		, CASE	WHEN @Budget = 100 THEN revenue.Cost
				ELSE revenue.Revenue / (1 + (revenue.BIMgmtFeePct / 100))
		  END AS MediaSpend
		FROM revenue
	)
	INSERT INTO @ret
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
	, @Budget AS Budget
	, CASE WHEN @Budget = 0 THEN 0 ELSE MediaSpend / CAST(@Budget as float) END AS Pacing
	, CASE WHEN Clicks = 0 THEN 0 ELSE MediaSpend / CAST(Clicks as float) END AS eCPC
	, CASE WHEN Conversions = 0 THEN 0 ELSE MediaSpend / CAST(Conversions as float) END AS eCPA
	FROM mediaSpend

	RETURN
END