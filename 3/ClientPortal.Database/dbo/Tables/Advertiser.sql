CREATE TABLE [dbo].[Advertiser] (
    [AdvertiserId]            INT             NOT NULL,
    [AdvertiserName]          NVARCHAR (MAX)  NULL,
    [Logo]                    VARBINARY (MAX) NULL,
    [Culture]                 NVARCHAR (MAX)  NULL,
    [ShowCPMRep]              BIT             CONSTRAINT [DF_Advertiser_ShowCPMRep] DEFAULT ((0)) NOT NULL,
    [ShowConversionData]      BIT             CONSTRAINT [DF_Advertiser_ShowConversionData] DEFAULT ((0)) NOT NULL,
    [ConversionValueName]     NVARCHAR (MAX)  NULL,
    [ConversionValueIsNumber] BIT             CONSTRAINT [DF_Advertiser_ConversionValueIsNumber] DEFAULT ((0)) NOT NULL,
    [HasSearch]               BIT             CONSTRAINT [DF_Advertiser_HasSearch] DEFAULT ((0)) NOT NULL,
    CONSTRAINT [PK_dbo.Advertiser] PRIMARY KEY CLUSTERED ([AdvertiserId] ASC)
);

