CREATE TABLE [dbo].[Goal] (
    [Id]           INT             IDENTITY (1, 1) NOT NULL,
    [AdvertiserId] INT             NOT NULL,
    [OfferId]      INT             NULL,
    [Name]         NVARCHAR (MAX)  NULL,
    [TypeId]       INT             NOT NULL,
    [MetricId]     INT             NOT NULL,
    [Target]       DECIMAL (18, 2) NOT NULL,
    [StartDate]    DATETIME        NULL,
    [EndDate]      DATETIME        NULL,
    CONSTRAINT [PK_dbo.Goal] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_Goal_Advertiser] FOREIGN KEY ([AdvertiserId]) REFERENCES [dbo].[Advertiser] ([AdvertiserId]),
    CONSTRAINT [FK_Goal_Offer] FOREIGN KEY ([OfferId]) REFERENCES [dbo].[Offer] ([OfferId])
);



