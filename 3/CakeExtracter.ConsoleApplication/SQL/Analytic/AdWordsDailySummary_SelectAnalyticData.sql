DECLARE @startDate DATETIME = '@@param_0',
        @endDate DATETIME = '@@param_1';

SELECT
    sa.Name AS [SearchAccountName],
    sc.SearchCampaignName  AS [SearchCampaignName],
    sds.Date AS [Date],
    sds.Date-(datepart(dw,sds.Date)+5)%7 AS [StartDayOfWeek],
    SUM(sds.Cost) AS [Cost],
    SUM(sds.Impressions) AS [Impressions],
    SUM(sds.Clicks) AS [Clicks],
    SUM(sds.ViewThrus) AS [ViewThrus],
    SUM(sds.CassConvs) AS [CassConvs],
    SUM(sds.CassConval) AS [CassConval],
    SUM(svd.VideoPlayedTo25) AS [VideoPlayedTo25],
    SUM(svd.VideoPlayedTo50) AS [VideoPlayedTo50],
    SUM(svd.VideoPlayedTo75) AS [VideoPlayedTo75],
    SUM(svd.VideoPlayedTo100) AS [VideoPlayedTo100],
    SUM(svd.Views) AS [Views],
    SUM(svd.ActiveViewImpressions) AS [ActiveViewImpressions]
FROM SearchDailySummary sds
FULL OUTER JOIN SearchVideoDailySummary svd ON svd.searchcampaignid = sds.searchcampaignid
                                           AND svd.Date = sds.Date
                                           AND svd.Device = sds.Device
                                           AND svd.Network = sds.Network
INNER JOIN SearchCampaign sc ON sc.searchcampaignid=sds.searchcampaignid
LEFT JOIN SearchConvSummary scs ON scs.searchcampaignid = sds.searchcampaignid
                               AND scs.Date = sds.Date
                               AND scs.Device = sds.Device
                               AND scs.Network = sds.Network
INNER JOIN SearchAccount sa ON sa.searchaccountid=sc.searchaccountid
WHERE   sds.Date >= @startDate
    AND sds.Date <= @endDate
    AND sa.Channel = 'Google'
GROUP BY sa.Name, sds.Date, sc.SearchCampaignName
ORDER BY [SearchAccountName], [Date], [SearchCampaignName]