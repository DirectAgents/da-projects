DECLARE @AccountId      INT         = @@param_0,
        @StartDate      DATETIME    = '@@param_1',
        @EndDate        DATETIME    = '@@param_2';

SELECT
    p.Code                                                  AS [PlatformCode],
    p.[Name]                                                AS [Platform],
    n.[Name]                                                AS [Network],
    acc.Id                                                  AS [AccountId],
    acc.[Name]                                              AS [Account],
    actSum.[Date]                                           AS [Date],
    actSum.[Date] - (DATEPART(dw, actSum.[Date]) + 5) % 7   AS [StartDayOfWeek],
    camp.Id                                                 AS [StrategyId],
    camp.[Name]                                             AS [CampaignName],
    adSet.Id                                                AS [AdSetId],
    adSet.[Name]                                            AS [AdSetName],
    act.Code                                                AS [ActionType],
    actSum.PostClick                                        AS [PostClick],
    actSum.PostView                                         AS [PostView],
    actSum.PostClickVal                                     AS [PostClickVal],
    actSum.PostViewVal                                      AS [PostViewVal],
    actSum.ClickAttrWindow                                  AS [ClickAttrWindow],
    actSum.ViewAttrWindow                                   AS [ViewAttrWindow]
FROM td.FbAdSetAction actSum
INNER JOIN td.FbActionType act ON actSum.ActionTypeId = act.Id
INNER JOIN td.FbAdSet adSet ON actSum.AdSetId = adSet.Id
INNER JOIN td.FbCampaign camp ON adset.CampaignId = camp.Id
INNER JOIN td.Account acc ON camp.AccountId = acc.Id
INNER JOIN td.Platform p ON acc.PlatformId = p.Id
INNER JOIN td.Network n ON acc.NetworkId = n.Id
WHERE   acc.Id = @AccountId
    AND actSum.[Date] >= @StartDate
    AND actSum.[Date] <= @EndDate
ORDER BY    [Date],
            [Platform],
            [AccountId],
            [AdSetName],
            [ActionType];