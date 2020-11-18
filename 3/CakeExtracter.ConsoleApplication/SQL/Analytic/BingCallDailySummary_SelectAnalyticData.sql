DECLARE @startDate DATETIME = '@@param_0',
        @endDate DATETIME = '@@param_1';

SELECT 
	sa.Name AS [SearchAccountName],
	sc.SearchCampaignName,
	cds.Date,
	cds.Calls
FROM dbo.CallDailySummary cds
INNER JOIN dbo.SearchCampaign sc ON sc.SearchCampaignId = cds.SearchCampaignId
INNER JOIN dbo.SearchAccount sa ON sa.SearchAccountId = sc.SearchAccountId
WHERE cds.Date >= @startDate
	AND cds.Date <= @endDate
	AND sa.Channel = 'bing'
ORDER BY [SearchAccountName], [Date], [SearchCampaignName]