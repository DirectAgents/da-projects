CREATE TABLE [dbo].[Campaign] (
    [CampaignId]      INT            NOT NULL,
    [OfferId]         INT            NOT NULL,
    [AffiliateId]     INT            NOT NULL,
    [CampaignName]    NVARCHAR (255) NULL,
    [PriceFormatName] NVARCHAR (50)  NULL,
    CONSTRAINT [PK_Campaign] PRIMARY KEY CLUSTERED ([CampaignId] ASC),
    CONSTRAINT [FK_Campaign_Affiliate] FOREIGN KEY ([AffiliateId]) REFERENCES [dbo].[Affiliate] ([AffiliateId]),
    CONSTRAINT [FK_Campaign_Offer] FOREIGN KEY ([OfferId]) REFERENCES [dbo].[Offer] ([OfferId])
);

