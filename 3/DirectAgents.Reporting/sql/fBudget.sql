alter FUNCTION [td].[fBudget](
@AdvertiserId int
, @StartDate datetime
, @EndDate datetime
) RETURNS float AS
BEGIN
	DECLARE @ret float
	; WITH budgets AS
	(
		SELECT ISNULL(BudgetInfo.MediaSpend, Campaign.MediaSpend) AS BudgetPart
		, CASE WHEN ROW_NUMBER() OVER(PARTITION BY Campaign.Id ORDER BY BudgetInfo.Date DESC) = 1 THEN 1 END AS BudgetRow
		FROM td.Campaign
		LEFT OUTER JOIN td.BudgetInfo ON BudgetInfo.Id =
			(
			SELECT TOP 1 x.Id
			FROM td.BudgetInfo x
			WHERE (x.CampaignId = Campaign.Id)
				AND ((@StartDate IS NULL) OR (x.Date >= @StartDate))
				AND ((@EndDate IS NULL) OR (x.Date <= @EndDate))
			ORDER BY x.Date DESC
			)
		LEFT OUTER JOIN td.Account ON Account.CampaignId = Campaign.Id
		WHERE (td.Campaign.AdvertiserId = @AdvertiserId)
	)
	SELECT @ret = SUM(BudgetPart)
	FROM budgets
	WHERE (BudgetRow = 1)
	
	RETURN @ret
END