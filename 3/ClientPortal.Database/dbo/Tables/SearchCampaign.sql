CREATE TABLE [dbo].[SearchCampaign] (
    [SearchCampaignId]   INT            IDENTITY (1, 1) NOT NULL,
    [SearchCampaignName] NVARCHAR (255) NOT NULL,
    [AdvertiserId]       INT            NOT NULL,
    [Channel] NVARCHAR(255) NULL, 
    CONSTRAINT [PK_SearchCampaign] PRIMARY KEY CLUSTERED ([SearchCampaignId] ASC),
    CONSTRAINT [FK_SearchCampaign_Advertiser] FOREIGN KEY ([AdvertiserId]) REFERENCES [dbo].[Advertiser] ([AdvertiserId])
);

