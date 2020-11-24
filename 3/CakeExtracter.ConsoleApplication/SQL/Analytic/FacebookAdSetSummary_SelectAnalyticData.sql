DECLARE @AccountId      INT         = @@param_0,
        @StartDate      DATETIME    = '@@param_1',
        @EndDate        DATETIME    = '@@param_2';

DECLARE @PlatformId     INT,
        @PlatformName   NVARCHAR(20),
        @PlatformCode   NVARCHAR(10),
        @AdvertiserId   INT;

SELECT TOP 1
    @PlatformId = Id,
    @PlatformName = Name,
    @PlatformCode = Code
FROM td.platform
WHERE code = 'fb';

SELECT TOP 1
    @AdvertiserId = c.AdvertiserId
FROM td.Campaign c
WHERE c.Id = (SELECT ac.CampaignId FROM td.Account ac
              WHERE ac.Id = @AccountId);

DECLARE @DesiredAdIds TABLE
(
    Id INT,
    INDEX IX_Id CLUSTERED (Id)
);

DECLARE @AdSetSummaries TABLE
(
    Date                    DATETIME,
    AdSetId                 INT,
    AdSetName               NVARCHAR(300),
    Impressions             INT,
    Clicks                  INT,
    AllClicks               INT,
    PostClickConv           INT,
    PostViewConv            INT,
    Conversions             INT,
    MediaSpend              DECIMAL(38,8),
    MgmtFee                 DECIMAL(38,6),
    NetworkName             NVARCHAR(50),
    PostClickConVal         DECIMAL(18,6),
    PostViewConVal          DECIMAL(18,6),
    ConVal                  DECIMAL(18,6),
    INDEX IX_AdSetId NONCLUSTERED (AdSetId),
    INDEX IX_AdIdAndDate CLUSTERED (AdSetId, Date)
);

DECLARE @AdSetActions TABLE
(
    Date                    DATETIME,
    AdSetId                 INT,
    ActionTypeId            INT,
    PostClick               INT,
    PostView                INT,
    PostClickVal            DECIMAL(18, 6),
    PostViewVal             DECIMAL(18, 6)
    INDEX IX_DateAndAdId NONCLUSTERED (AdSetId, Date)
);

DECLARE @ActionStats TABLE
(
    Date    DATETIME,
    AdSetId INT,
    Col     NVARCHAR(50),
    Value   DECIMAL,
    INDEX IX_ActionStats NONCLUSTERED (Date, AdSetId)
);

DECLARE @ActionStatsPvt TABLE
(
    [Date]      DATETIME,
    [AdSetId]   INT,
    [11-pc]     DECIMAL(18,6),
    [11-pv]     DECIMAL(18,6),
    [11-pcv]    DECIMAL(18,6),
    [11-pvv]    DECIMAL(18,6),
    [30-pc]     DECIMAL(18,6),
    [30-pv]     DECIMAL(18,6),
    [550-pc]    DECIMAL(18,6),
    [550-pv]    DECIMAL(18,6),
    INDEX IX_ActionStatsPvt CLUSTERED
    (
        [Date],
        [AdSetId]
    )
);

INSERT INTO @DesiredAdIds
SELECT Id FROM td.FbAdSet
WHERE AccountId = @AccountId;

INSERT INTO @AdSetSummaries
SELECT
    Date,
    AdSetId,
    AdSetName,
    Impressions,
    Clicks,
    AllClicks,
    PostClickConv,
    PostViewConv,
    Conversions,
    MediaSpend,
    MgmtFee,
    NetworkName,
    PostClickConVal,
    PostViewConVal,
    ConVal
FROM [td].[fFbAdSetSummaryBasicStats] (@AdvertiserId, @StartDate, @EndDate);

INSERT INTO @AdSetActions
SELECT
    [Date],
    [AdSetId],
    [ActionTypeId],
    [PostClick],
    [PostView],
    [PostClickVal],
    [PostViewVal]
FROM td.FbAdSetAction
WHERE   AdSetId IN (SELECT Id FROM @DesiredAdIds)
    AND [Date] >= @StartDate
    AND [Date] <= @EndDate;

INSERT INTO @ActionStats
SELECT
    [date],
    [AdSetId],
    [Col],
    [Value]
FROM @AdSetActions
CROSS APPLY
(
    SELECT CONCAT(ActionTypeId, '-pc'), PostClick UNION ALL
    SELECT CONCAT(ActionTypeId, '-pv'), PostView UNION ALL
    SELECT CONCAT(ActionTypeId, '-pcv'), PostClickVal UNION ALL
    SELECT CONCAT(ActionTypeId, '-pvv'), PostViewVal
) c([Col], [Value]);

INSERT INTO @ActionStatsPvt
SELECT
    [date],
    [AdSetId],
    ISNULL([11-pc], 0) AS [11-pc],
    ISNULL([11-pv],0) AS [11-pv],
    ISNULL([11-pcv],0) AS [11-pcv],
    ISNULL([11-pvv],0) AS [11-pvv],
    ISNULL([30-pc],0) AS [30-pc],
    ISNULL([30-pv],0) AS [30-pv],
    ISNULL([550-pc],0) AS [550-pc],
    ISNULL([550-pv],0) AS [550-pv]
FROM @ActionStats
PIVOT
(
    SUM (value) FOR col IN
    (
        [11-pc],
        [11-pv],
        [11-pcv],
        [11-pvv],
        [30-pc],
        [30-pv],
        [550-pc],
        [550-pv]
    )
) AS Pvt;

SELECT
    @PlatformCode                                   AS [PlatformCode],
    @PlatformName                                   AS [Platform],
    asm.NetworkName                                 AS [Network],
    adset.[AccountId]                               AS [AccountId],
    asm.[Date]                                      AS [Date],
    asm.[Date] - (DATEPART(dw, asm.[Date]) + 5) % 7 AS [StartDayOfWeek],
    camp.Id                                         AS [StrategyId],
    camp.Name                                       AS [CampaignName],
    asm.adsetid                                     AS [AdSetId],
    asm.adsetname                                   AS [AdSetName],
    asm.impressions                                 AS [Impressions],
    asm.clicks                                      AS [Clicks],
    asm.allclicks                                   AS [AllClicks],
    asm.postclickconv                               AS [PostClickConv],
    asm.postviewconv                                AS [PostViewConv],
    asm.conversions                                 AS [Conversions],
    asm.mediaspend                                  AS [MediaSpend],
    asm.mgmtfee                                     AS [MgmtFee],
    asm.postclickconval                             AS [PostClickConVal],
    asm.postviewconval                              AS [PostViewConVal],
    asm.conval                                      AS [ConVal],
    ap.[11-pc]                                      AS [11-pc],
    ap.[11-pv]                                      AS [11-pv],
    ap.[11-pcv]                                     AS [11-pcv],
    ap.[11-pvv]                                     AS [11-pvv],
    ap.[30-pc]                                      AS [30-pc],
    ap.[30-pv]                                      AS [30-pv],
    ap.[550-pc]                                     AS [550-pc],
    ap.[550-pv]                                     AS [550-pv]
FROM @AdSetSummaries asm
INNER JOIN td.FbAdSet adset ON asm.AdSetId = adset.Id
INNER JOIN td.FbCampaign camp ON adset.CampaignId = camp.Id
LEFT JOIN @ActionStatsPvt ap ON asm.AdSetId = ap.AdSetId
                            AND asm.Date=ap.Date
WHERE asm.AdSetId IN (SELECT Id FROM @DesiredAdIds)
ORDER BY    [Date],
            [Platform],
            [AccountId],
            [AdSetName];