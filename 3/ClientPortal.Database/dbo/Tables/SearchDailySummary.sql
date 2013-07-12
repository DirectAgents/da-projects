CREATE TABLE [dbo].[SearchDailySummary] (
    [SearchCampaignId] INT      NOT NULL,
    [Date]             DATETIME NOT NULL,
    [Revenue]          MONEY    NOT NULL,
    [Cost]             MONEY    NOT NULL,
    [Orders]           INT      NOT NULL,
    [Clicks]           INT      NOT NULL,
    [Impressions]      INT      NOT NULL,
    [CurrencyId]       INT      NOT NULL,
    CONSTRAINT [PK_SearchDailySummary] PRIMARY KEY CLUSTERED ([SearchCampaignId] ASC, [Date] ASC),
    CONSTRAINT [FK_SearchDailySummary_SearchCampaign] FOREIGN KEY ([SearchCampaignId]) REFERENCES [dbo].[SearchCampaign] ([SearchCampaignId])
);

