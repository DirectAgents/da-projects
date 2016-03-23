﻿alter FUNCTION [td].[fStrategySummaryBasicStats](
@AdvertiserId int
, @StartDate datetime
, @EndDate datetime
) RETURNS @ret TABLE
	(
	Date	datetime
	, StrategyName	nvarchar(max)
	, StrategyId	int
	, Impressions	int
	, Clicks	int
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
		, Strategy.Id As StrategyId
		, Account.PlatformId
		FROM td.Campaign
		INNER JOIN td.Account ON Account.CampaignId = Campaign.Id
		INNER JOIN td.Strategy ON Strategy.AccountId = Account.Id
		INNER JOIN td.StrategySummary dt
		   ON (dt.StrategyId = Strategy.Id)
		  AND (dt.Date BETWEEN @StartDate AND @EndDate)
		LEFT OUTER JOIN td.BudgetInfo ON BudgetInfo.Id =
			(
			SELECT TOP 1 x.Id
			FROM td.BudgetInfo x
			WHERE (x.CampaignId = Account.CampaignId)
			  AND (x.Date <= dt.Date)
			ORDER BY x.Date DESC
			)
		LEFT OUTER JOIN td.PlatformBudgetInfo ON PlatformBudgetInfo.Id =
			(
			SELECT TOP 1 x.Id
			FROM td.PlatformBudgetInfo x
			WHERE (x.CampaignId = Campaign.Id)
			  AND (x.PlatformId = Account.PlatformId)
			  AND (x.Date <= dt.Date)
			ORDER BY x.Date DESC
			)
		WHERE (td.Campaign.AdvertiserId = @AdvertiserId)
	)
	, revenue AS
	(
		SELECT StrategySummary.Date
		, ISNULL('DA' + CAST(Platform.Id AS varchar) + ' ', '') + Strategy.Name AS StrategyName
		, Strategy.Id AS StrategyId
		, StrategySummary.Impressions
		, StrategySummary.Clicks
		, StrategySummary.PostClickConv
		, StrategySummary.PostViewConv
		, CAST(CASE WHEN Platform.Name = 'Facebook' THEN 0 ELSE 1 END AS bit) AS ShowClickAndViewConv
		, StrategySummary.PostClickConv + StrategySummary.PostViewConv AS Conversions
		, StrategySummary.Cost
		, CASE	WHEN dtData.MarginPct = 100 THEN StrategySummary.Cost * (1 + (dtData.MgmtFeePct / 100))
				ELSE StrategySummary.Cost / (1 - (dtData.MarginPct / 100))
		  END AS Revenue
		, dtData.MgmtFeePct
		FROM td.Strategy
		INNER JOIN td.StrategySummary ON StrategySummary.StrategyId = Strategy.Id
		INNER JOIN dtData
		   ON (dtData.AccountId = Strategy.AccountId)
		  AND (dtData.StrategyId = Strategy.Id)
		  AND (dtData.Date = StrategySummary.Date)
		LEFT OUTER JOIN td.Platform ON Platform.Id = dtData.PlatformId
		WHERE (StrategySummary.Date BETWEEN @StartDate AND @EndDate)
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
	, StrategyName
	, StrategyId
	, Impressions
	, Clicks
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
	FROM mediaSpend

	RETURN
END