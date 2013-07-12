CREATE TABLE [dbo].[AdvertiserContact] (
    [AdvertiserId] INT NOT NULL,
    [ContactId]    INT NOT NULL,
    [Order]        INT NULL,
    CONSTRAINT [PK_dbo.AdvertiserContact] PRIMARY KEY CLUSTERED ([AdvertiserId] ASC, [ContactId] ASC),
    CONSTRAINT [FK_dbo.AdvertiserContact_dbo.Advertiser_AdvertiserId] FOREIGN KEY ([AdvertiserId]) REFERENCES [dbo].[Advertiser] ([AdvertiserId]) ON DELETE CASCADE,
    CONSTRAINT [FK_dbo.AdvertiserContact_dbo.Contact_ContactId] FOREIGN KEY ([ContactId]) REFERENCES [dbo].[Contact] ([ContactId]) ON DELETE CASCADE
);

