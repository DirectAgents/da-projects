ALTER FUNCTION [td].[fBudgetInfo](
@AdvertiserId int
, @StartDate datetime = NULL
, @EndDate datetime = NULL
) RETURNS TABLE AS RETURN
SELECT ISNULL(PlatformBudgetInfo.MediaSpend, ISNULL(BudgetInfo.MediaSpend, Campaign.MediaSpend)) AS MediaSpend
	, ISNULL(PlatformBudgetInfo.MgmtFeePct, ISNULL(BudgetInfo.MgmtFeePct, Campaign.MgmtFeePct)) AS MgmtFeePct
	, ISNULL(PlatformBudgetInfo.MarginPct, ISNULL(BudgetInfo.MarginPct, Campaign.MarginPct)) AS MarginPct
	, CAST(CASE WHEN Platform.Name = 'Facebook' THEN 0 ELSE 1 END AS bit) AS ShowClickAndViewConv
	, ISNULL(CASE WHEN PlatformBudgetInfo.Date <= @StartDate THEN PlatformBudgetInfo.Date END,
	  ISNULL(CASE WHEN BudgetInfo.Date <= @StartDate THEN BudgetInfo.Date END,
	  ISNULL(@StartDate, '1/1/1970'))) AS AsOfDate
	, Platform.Name AS PlatformName
	, BudgetInfo.Date AS BudgetInfoDate
	, PlatformBudgetInfo.Date AS PlatformBudgetInfoDate
	, Campaign.Id AS CampaignId
	, Campaign.AdvertiserId
	, Account.Id AS AccountId
	, Account.PlatformId
	, CAST(Campaign.Id AS varchar) + ISNULL('-' + CONVERT(varchar, BudgetInfo.Date, 112), '') + ISNULL('-' + CAST(Account.Id AS varchar), '') + ISNULL('-' + CAST(Platform.Id AS varchar), '') + ISNULL('-' + CONVERT(varchar, PlatformBudgetInfo.Date, 112), '') AS UniqueId
FROM td.Campaign
LEFT OUTER JOIN td.BudgetInfo
   ON (BudgetInfo.CampaignId = Campaign.Id)
  AND ((@StartDate IS NULL) OR (BudgetInfo.Date >= @StartDate))
  AND ((@EndDate IS NULL) OR (BudgetInfo.Date <= @EndDate))
LEFT OUTER JOIN td.Account
   ON (Account.CampaignId = Campaign.Id)
LEFT OUTER JOIN td.PlatformBudgetInfo
   ON (PlatformBudgetInfo.CampaignId = Campaign.Id)
  AND (PlatformBudgetInfo.PlatformId = Account.PlatformId)
  AND ((@StartDate IS NULL) OR (PlatformBudgetInfo.Date >= @StartDate))
  AND ((@EndDate IS NULL) OR (PlatformBudgetInfo.Date <= @EndDate))
LEFT OUTER JOIN td.Platform ON Platform.Id = Account.PlatformId
WHERE (td.Campaign.AdvertiserId = @AdvertiserId)
