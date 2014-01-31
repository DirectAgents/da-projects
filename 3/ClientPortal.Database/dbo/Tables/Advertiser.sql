CREATE TABLE [dbo].[Advertiser] (
    [AdvertiserId]                     INT             NOT NULL,
    [AdvertiserName]                   NVARCHAR (MAX)  NULL,
    [Logo]                             VARBINARY (MAX) NULL,
    [Culture]                          NVARCHAR (MAX)  NULL,
    [ShowCPMRep]                       BIT             CONSTRAINT [DF_Advertiser_ShowCPMRep] DEFAULT ((0)) NOT NULL,
    [ShowConversionData]               BIT             CONSTRAINT [DF_Advertiser_ShowConversionData] DEFAULT ((0)) NOT NULL,
    [ConversionValueName]              NVARCHAR (MAX)  NULL,
    [ConversionValueIsNumber]          BIT             CONSTRAINT [DF_Advertiser_ConversionValueIsNumber] DEFAULT ((0)) NOT NULL,
    [HasSearch]                        BIT             CONSTRAINT [DF_Advertiser_HasSearch] DEFAULT ((0)) NOT NULL,
    [AdWordsAccountId]                 NVARCHAR (255)  NULL,
    [BingAdsAccountId]                 NVARCHAR (255)  NULL,
    [AnalyticsProfileId]               NVARCHAR (255)  NULL,
    [AutomatedReportsEnabled]          BIT             CONSTRAINT [DF_Advertiser_AutomatedReportsEnabled_1] DEFAULT ((0)) NOT NULL,
    [AutomatedReportsDestinationEmail] NVARCHAR (MAX)  NULL,
    [AutomatedReportsPeriodDays]       INT             CONSTRAINT [DF_Advertiser_AutomatedReportsPeriodDays] DEFAULT ((7)) NOT NULL,
    [AutomatedReportsNextSendAfter]    DATETIME        NULL,
    [ShowSearchChannels]               BIT             CONSTRAINT [DF_Advertiser_ShowSearchChannels] DEFAULT ((0)) NOT NULL,
    [AccountManagerId]                 INT             NULL,
    CONSTRAINT [PK_dbo.Advertiser] PRIMARY KEY CLUSTERED ([AdvertiserId] ASC),
    CONSTRAINT [FK_Advertiser_CakeContact] FOREIGN KEY ([AccountManagerId]) REFERENCES [dbo].[CakeContact] ([CakeContactId])
);



