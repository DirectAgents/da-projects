alter FUNCTION fBudgetInfo(
@AdvertiserId int
, @StartDate datetime = NULL
, @EndDate datetime = NULL
) RETURNS TABLE AS RETURN
SELECT ISNULL(PlatformBudgetInfo.MediaSpend, ISNULL(BudgetInfo.MediaSpend, Campaign.MediaSpend)) AS MediaSpend
	, ISNULL(PlatformBudgetInfo.MgmtFeePct, ISNULL(BudgetInfo.MgmtFeePct, Campaign.MgmtFeePct)) AS MgmtFeePct
	, ISNULL(PlatformBudgetInfo.MarginPct, ISNULL(BudgetInfo.MarginPct, Campaign.MarginPct)) AS MarginPct
	, CAST(CASE WHEN Platform.Name = 'Facebook' THEN 0 ELSE 1 END AS bit) AS ShowClickAndViewConv
	, Campaign.Id AS CampaignId
	, Campaign.AdvertiserId
	, Account.Id AS AccountId
	, Account.PlatformId
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
