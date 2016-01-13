alter FUNCTION fBudgetInfo(
@AdvertiserId int
, @StartDate datetime = NULL
, @EndDate datetime = NULL
) RETURNS TABLE AS RETURN
SELECT ISNULL(td.PlatformBudgetInfo.MediaSpend, ISNULL(td.BudgetInfo.MediaSpend, td.Campaign.MediaSpend)) AS MediaSpend
	, ISNULL(td.PlatformBudgetInfo.MgmtFeePct, ISNULL(td.BudgetInfo.MgmtFeePct, td.Campaign.MgmtFeePct)) AS MgmtFeePct
	, ISNULL(td.PlatformBudgetInfo.MarginPct, ISNULL(td.BudgetInfo.MarginPct, td.Campaign.MarginPct)) AS MarginPct
	, CAST(CASE WHEN Platform.Name = 'Facebook' THEN 0 ELSE 1 END AS bit) AS ShowClickAndViewConv
	, td.Campaign.Id AS CampaignId
	, td.Campaign.AdvertiserId
	, td.Account.Id AS AccountId
	, td.Account.PlatformId
FROM td.Campaign
LEFT OUTER JOIN td.BudgetInfo
	ON (BudgetInfo.CampaignId = Campaign.Id)
	AND ((@StartDate IS NULL) OR (BudgetInfo.Date >= @StartDate))
	AND ((@EndDate IS NULL) OR (BudgetInfo.Date <= @EndDate))
LEFT OUTER JOIN td.PlatformBudgetInfo
	ON (PlatformBudgetInfo.CampaignId = Campaign.Id)
	AND ((@StartDate IS NULL) OR (PlatformBudgetInfo.Date >= @StartDate))
	AND ((@EndDate IS NULL) OR (PlatformBudgetInfo.Date <= @EndDate))
LEFT OUTER JOIN td.Platform ON Platform.Id = PlatformBudgetInfo.PlatformId
LEFT OUTER JOIN td.Account
  ON ((Account.CampaignId = Campaign.Id)  OR (Account.PlatformId = Platform.Id))
WHERE (td.Campaign.AdvertiserId = @AdvertiserId)
