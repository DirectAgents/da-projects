alter FUNCTION [td].[fCreativeProgressBasicStats](
@AdvertiserId int
, @StartDate datetime
, @EndDate datetime
, @KPI varchar(max)
) RETURNS @ret TABLE 
	(
	AdName	nvarchar(max)
	, AdId	int
	, ShowClickAndViewConv	bit
	, MediaSpend	decimal(38,8)
	, Impressions	int
	, Clicks	int
	, CTR	float(53)
	, eCPA	float(53)
	, PostClickConv	int
	, PostViewConv	int
	, Conversions	int
	, eCPC	float(53)
	, SumKPI	float(53)
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
		, Account.PlatformId
		, Ad.Id AS AdId
		FROM td.Campaign
		INNER JOIN td.Account ON Account.CampaignId = Campaign.Id
		INNER JOIN td.Ad ON Ad.AccountId = Account.Id
		INNER JOIN td.AdSummary dt
		   ON (dt.TDadId = Ad.Id)
		  AND (dt.Date BETWEEN @StartDate AND @EndDate)
		LEFT OUTER JOIN td.BudgetInfo ON BudgetInfo.Id =
			(
			SELECT TOP 1 x.Id
			FROM td.BudgetInfo x
			WHERE (x.CampaignId = Account.CampaignId)
			  AND (x.Date = CAST(CAST(YEAR(dt.Date) as varchar) + '-' + CAST(MONTH(dt.Date) AS varchar) + '-01' AS datetime))
			ORDER BY x.Date DESC
			)
		LEFT OUTER JOIN td.PlatformBudgetInfo ON PlatformBudgetInfo.Id =
			(
			SELECT TOP 1 x.Id
			FROM td.PlatformBudgetInfo x
			WHERE (x.CampaignId = Campaign.Id)
			  AND (x.PlatformId = Account.PlatformId)
			  AND (x.Date = CAST(CAST(YEAR(dt.Date) as varchar) + '-' + CAST(MONTH(dt.Date) AS varchar) + '-01' AS datetime))
			ORDER BY x.Date DESC
			)
		WHERE (Campaign.AdvertiserId = @AdvertiserId)
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
		, CAST(CASE WHEN Platform.Name = 'Facebook' THEN 0 ELSE 1 END AS bit) AS ShowClickAndViewConv
		, AdSummary.PostClickConv + AdSummary.PostViewConv AS Conversions
		, AdSummary.Cost
		, CASE	WHEN dtData.MarginPct = 100 THEN AdSummary.Cost * (1 + (dtData.MgmtFeePct / 100))
				ELSE AdSummary.Cost / (1 - (dtData.MarginPct / 100))
		  END AS Revenue
		, dtData.MgmtFeePct
		FROM td.Ad
		INNER JOIN td.AdSummary ON AdSummary.TDadId = Ad.Id
		INNER JOIN dtData
		   ON (dtData.AccountId = Ad.AccountId)
		  AND (dtData.AdId = Ad.Id)
		  AND (dtData.Date = AdSummary.Date)
		LEFT OUTER JOIN td.Platform ON Platform.Id = dtData.PlatformId
		WHERE (td.AdSummary.Date BETWEEN @StartDate AND @EndDate)
	)
	, mediaSpend AS
	(
		SELECT revenue.*
		, CASE	WHEN @Budget = 100 THEN revenue.Cost
				ELSE revenue.Revenue / (1 + (revenue.MgmtFeePct / 100))
		  END AS MediaSpend
		FROM revenue
	)
	INSERT INTO @ret
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

	RETURN
END