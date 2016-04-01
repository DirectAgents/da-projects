alter FUNCTION [td].[fSiteSummaryBasicStats](
@AdvertiserId int
, @StartDate datetime
, @EndDate datetime
) RETURNS @ret TABLE
	(
	SiteName	nvarchar(max)
	, SiteId	int
	, Impressions	int
	, Clicks	int
	, CTR	float(53)
	, Conversions	int
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
		FROM td.Campaign
		INNER JOIN td.Account ON Account.CampaignId = Campaign.Id
		INNER JOIN td.SiteSummary dt
		   ON (dt.AccountId = Account.Id)
		  AND (dt.Date BETWEEN @StartDate AND @EndDate)
		INNER JOIN td.Site ON Site.Id = dt.SiteId
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
		SELECT SiteSummary.Date
		, Site.Name AS SiteName
		, Site.Id AS SiteId
		, SiteSummary.Impressions
		, SiteSummary.Clicks
		, SiteSummary.PostClickConv + SiteSummary.PostViewConv AS Conversions
		, SiteSummary.Cost
		, CASE	WHEN dtData.MarginPct = 100 THEN SiteSummary.Cost * (1 + (dtData.MgmtFeePct / 100))
				ELSE SiteSummary.Cost / (1 - (dtData.MarginPct / 100))
		  END AS Revenue
		, dtData.MgmtFeePct
		FROM td.Site
		INNER JOIN td.SiteSummary ON Site.Id = SiteSummary.SiteId
		INNER JOIN dtData
		  ON (dtData.AccountId = SiteSummary.AccountId)
		 AND (dtData.Date = SiteSummary.Date)
		LEFT OUTER JOIN td.Account ON Account.Id = SiteSummary.AccountId
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
		, CASE	WHEN @Budget = 100 THEN revenue.Cost
				ELSE revenue.Revenue / (1 + (revenue.MgmtFeePct / 100))
		  END AS MediaSpend
		FROM revenue
		INNER JOIN revenueTop50 ON revenueTop50.SiteId = revenue.SiteId
	)
	, mgmtFee AS
	(
		SELECT *
		, CASE	WHEN MgmtFeePct = 100 THEN MediaSpend * MgmtFeePct
					ELSE Revenue - MediaSpend
		  END AS MgmtFee
		FROM mediaSpend
	)
	INSERT INTO @ret
	SELECT SiteName, SiteId
	, SUM(Impressions) AS Impressions
	, SUM(Clicks) AS Clicks
	, CASE WHEN SUM(Impressions) = 0 THEN 0 ELSE SUM(Clicks) / CAST(SUM(Impressions) AS float) END AS CTR
	, SUM(Conversions) AS Conversions
	FROM mgmtFee
	GROUP BY SiteName, SiteId

	RETURN
END