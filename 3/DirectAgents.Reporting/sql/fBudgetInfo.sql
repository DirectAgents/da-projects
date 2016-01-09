alter FUNCTION fBudgetInfo(
@AdvertiserId int
, @StartDate datetime
, @EndDate datetime
) RETURNS TABLE AS RETURN
SELECT ISNULL(td.PlatformBudgetInfo.MediaSpend, ISNULL(td.budgetInfo.MediaSpend, td.Campaign.MediaSpend)) AS MediaSpend
	, ISNULL(td.PlatformBudgetInfo.MgmtFeePct, ISNULL(td.budgetInfo.MgmtFeePct, td.Campaign.MgmtFeePct)) AS MgmtFeePct
	, ISNULL(td.PlatformBudgetInfo.MarginPct, ISNULL(td.budgetInfo.MarginPct, td.Campaign.MarginPct)) AS MarginPct
	, td.Campaign.Id AS CampaignId
	, td.Campaign.AdvertiserId
	, td.Account.Id AS AccountId
FROM td.Campaign
INNER JOIN td.Account ON td.Account.CampaignId = td.Campaign.Id
LEFT OUTER JOIN td.budgetInfo
	ON (td.budgetInfo.CampaignId = td.Campaign.Id)
	AND (td.budgetInfo.Date BETWEEN @StartDate AND @EndDate)
LEFT OUTER JOIN td.PlatformBudgetInfo
	ON (td.PlatformBudgetInfo.CampaignId = td.Campaign.Id)
	AND (td.PlatformBudgetInfo.Date BETWEEN @StartDate AND @EndDate)
WHERE (td.Campaign.AdvertiserId = @AdvertiserId)
