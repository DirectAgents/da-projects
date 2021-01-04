DECLARE @AccountId      INT         = @@param_0,
        @StartDate      DATETIME    = '@@param_1',
        @EndDate        DATETIME    = '@@param_2';

DECLARE @PlatformName   NVARCHAR(20),
        @PlatformCode   NVARCHAR(10),
        @AdvertiserId   INT;

SELECT TOP 1
    @PlatformName   = [Name],
    @PlatformCode   = [Code]
FROM td.platform WHERE code = 'adf';

SELECT TOP 1
    @AdvertiserId = c.AdvertiserId 
FROM td.Campaign c
INNER JOIN td.Account ac ON c.Id = ac.CampaignId
WHERE ac.Id = @AccountId;

SELECT
    @PlatformCode                                       AS [PlatformCode],
    @PlatformName                                       AS [Platform],
    acc.Id                                              AS [AccountId],
    acc.[Name]                                          AS [Account],
    asm.[Date]                                          AS [Date],
    asm.[Date] - (DATEPART(dw, asm.[Date]) + 5) % 7     AS [StartDayOfWeek],
    asm.AdSetName                                       AS [CampaignName],
    asm.Impressions                                     AS [Impressions],
    asm.Clicks                                          AS [Clicks],
    asm.ClickConversionsConvType1                       AS [ClickConversionsConvType1],
    asm.ClickConversionsConvType2                       AS [ClickConversionsConvType2],
    asm.ClickConversionsConvType3                       AS [ClickConversionsConvType3],
    asm.ImpressionConversionsConvType1                  AS [ImpressionConversionsConvType1],
    asm.ImpressionConversionsConvType2                  AS [ImpressionConversionsConvType2],
    asm.ImpressionConversionsConvType3                  AS [ImpressionConversionsConvType3],
    asm.Conversions                                     AS [Conversions],
    asm.MediaSpend                                      AS [MediaSpend],
    asm.ConVal                                          AS [ConVal]
FROM [td].[fn_AdfAdSetSummaryBasicStats] (@AdvertiserId, @StartDate, @EndDate) asm
INNER JOIN td.AdfLineItem ads ON ads.id = asm.adsetid
INNER JOIN td.AdfCampaign ac ON ads.CampaignId = ac.Id
INNER JOIN td.Account acc ON ac.AccountId = acc.Id
ORDER BY    [Date],
            [platform],
            [CampaignName]