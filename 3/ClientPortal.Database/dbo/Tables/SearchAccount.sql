CREATE TABLE [dbo].[SearchAccount] (
    [SearchAccountId] INT            IDENTITY (1, 1) NOT NULL,
    [AdvertiserId]    INT            NOT NULL,
    [Name]            NVARCHAR (255) NULL,
    [Channel]         NVARCHAR (50)  NULL,
    [AccountCode]     NVARCHAR (50)  NULL,
    [ExternalId]      NVARCHAR (50)  NULL,
    CONSTRAINT [PK_SearchAccount] PRIMARY KEY CLUSTERED ([SearchAccountId] ASC),
    CONSTRAINT [FK_SearchAccount_Advertiser] FOREIGN KEY ([AdvertiserId]) REFERENCES [dbo].[Advertiser] ([AdvertiserId])
);

