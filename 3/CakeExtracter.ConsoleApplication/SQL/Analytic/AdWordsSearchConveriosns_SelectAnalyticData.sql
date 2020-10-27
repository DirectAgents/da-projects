DECLARE @startDate DATETIME = '@@param_0',
        @endDate DATETIME = '@@param_1';

SELECT
    sa.Name AS [SearchAccountName],
    sc.SearchCampaignName,
    scs.Date,
    sct.Name AS [ConversionTypeName],
    scs.Network,
    scs.Device,
    scs.Conversions,
    scs.ConVal
FROM dbo.SearchConvSummary scs
INNER JOIN dbo.SearchCampaign sc ON sc.SearchCampaignId = scs.SearchCampaignId
INNER JOIN dbo.SearchConvType sct ON scs.SearchConvTypeId = sct.SearchConvTypeId
INNER JOIN dbo.SearchAccount sa ON sa.SearchAccountId = sc.SearchAccountId
WHERE   scs.Date >= @startDate
    AND scs.Date <= @endDate
ORDER BY [SearchAccountName], [Date], [SearchCampaignName]

