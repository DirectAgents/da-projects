alter FUNCTION [td].[fAdSetSummaryBasicStats](
@AdvertiserId int
, @StartDate datetime
, @EndDate datetime
) RETURNS @ret TABLE
	(
	Date	datetime
	, StrategyId	int
	, AdSetId	int
	, AdSetName	nvarchar(max)
	, Impressions	int
	, Clicks	int
	, AllClicks	int
	, CTR	float(53)
	, PostClickConv	int
	, PostViewConv	int
	, ShowClickAndViewConv	bit
	, Conversions	int
	, CR	float(53)
	, MediaSpend	decimal(38,8)
	, MgmtFee	decimal(38,6)
	, Budget	float(53)
	, Pacing	float(53)
	, eCPC	float(53)
	, eCPA	float(53)
	, PlatformAlias nvarchar(max)
	, NetworkName nvarchar(max)
	, PostClickConVal decimal(18,6)
	, PostViewConVal decimal(18,6)
	, ConVal decimal(18,6)
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
		, AdSet.Id As AdSetId
		, Account.PlatformId
		, Account.NetworkId
		FROM td.Campaign
		INNER JOIN td.Account ON Account.CampaignId = Campaign.Id
		INNER JOIN td.AdSet ON AdSet.AccountId = Account.Id
		INNER JOIN td.AdSetSummary dt
		   ON (dt.AdSetId = AdSet.Id)
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
		WHERE (td.Campaign.AdvertiserId = @AdvertiserId)
	)
	, revenue AS
	(
		SELECT AdSetSummary.Date
		, CASE	WHEN Platform.Code = 'fb' THEN 'Facebook'
				ELSE 'DA' + CAST(Platform.Id AS varchar)
		END AS PlatformAlias
		, Network.Name AS NetworkName
		, AdSet.Name AS AdSetName
		, AdSet.Id AS AdSetId
		, AdSet.StrategyId
		, AdSetSummary.Impressions
		, AdSetSummary.Clicks
		, AdSetSummary.AllClicks
		, AdSetSummary.PostClickConv
		, AdSetSummary.PostViewConv
		, CAST(CASE WHEN Platform.Name = 'Facebook' THEN 0 ELSE 1 END AS bit) AS ShowClickAndViewConv
		, AdSetSummary.PostClickConv + AdSetSummary.PostViewConv AS Conversions
		, AdSetSummary.PostClickRev AS PostClickConVal
		, AdSetSummary.PostViewRev AS PostViewConVal
		, AdSetSummary.PostClickRev + AdSetSummary.PostViewRev AS ConVal
		, AdSetSummary.Cost
		, CASE	WHEN dtData.MarginPct = 100 THEN AdSetSummary.Cost * (1 + (dtData.MgmtFeePct / 100))
				ELSE AdSetSummary.Cost / (1 - (dtData.MarginPct / 100))
		  END AS Revenue
		, dtData.MgmtFeePct
		FROM td.AdSet
		INNER JOIN td.AdSetSummary ON AdSetSummary.AdSetId = AdSet.Id
		INNER JOIN dtData
		   ON (dtData.AccountId = AdSet.AccountId)
		  AND (dtData.AdSetId = AdSet.Id)
		  AND (dtData.Date = AdSetSummary.Date)
		LEFT OUTER JOIN td.Platform ON Platform.Id = dtData.PlatformId
		LEFT OUTER JOIN td.Network ON Network.Id = dtData.NetworkId
		WHERE (AdSetSummary.Date BETWEEN @StartDate AND @EndDate)
	)
	, mediaSpend AS
	(
		SELECT *
		, CASE	WHEN @Budget = 100 THEN revenue.Cost
				ELSE revenue.Revenue / (1 + (revenue.MgmtFeePct / 100))
		  END AS MediaSpend
		FROM revenue
	)
	INSERT INTO @ret
	SELECT Date
	, StrategyId
	, AdSetId
	--, ISNULL(PlatformAlias + ' ' + AdSetName, AdSetName)
	, AdSetName
	, Impressions
	, Clicks
	, AllClicks
	, CASE WHEN Impressions = 0 THEN 0 ELSE Clicks / CAST(Impressions AS float) END AS CTR
	, PostClickConv
	, PostViewConv
	, ShowClickAndViewConv
	, Conversions AS Conversions
	, CASE WHEN Clicks = 0 THEN 0 ELSE Conversions / CAST(Clicks as float) END AS CR
	, MediaSpend
	, CASE	WHEN MgmtFeePct = 100 THEN MediaSpend * MgmtFeePct
				ELSE Revenue - MediaSpend
	  END AS MgmtFee
	, @Budget AS Budget
	, CASE WHEN @Budget = 0 THEN 0 ELSE MediaSpend / CAST(@Budget as float) END AS Pacing
	, CASE WHEN Clicks = 0 THEN 0 ELSE MediaSpend / CAST(Clicks as float) END AS eCPC
	, CASE WHEN Conversions = 0 THEN 0 ELSE MediaSpend / CAST(Conversions as float) END AS eCPA
	, PlatformAlias
	, NetworkName
	, PostClickConVal
	, PostViewConVal
	, ConVal
	FROM mediaSpend

	RETURN
END