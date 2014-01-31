CREATE TABLE [dbo].[SearchCampaign] (
    [SearchCampaignId]   INT            IDENTITY (1, 1) NOT NULL,
    [SearchCampaignName] NVARCHAR (255) NOT NULL,
    [AdvertiserId]       INT            NULL,
    [Channel]            NVARCHAR (255) NULL,
    [ExternalId]         INT            NULL,
    [SearchAccountId]    INT            NULL,
    CONSTRAINT [PK_SearchCampaign] PRIMARY KEY CLUSTERED ([SearchCampaignId] ASC),
    CONSTRAINT [FK_SearchCampaign_Advertiser] FOREIGN KEY ([AdvertiserId]) REFERENCES [dbo].[Advertiser] ([AdvertiserId]),
    CONSTRAINT [FK_SearchCampaign_SearchAccount] FOREIGN KEY ([SearchAccountId]) REFERENCES [dbo].[SearchAccount] ([SearchAccountId])
);



