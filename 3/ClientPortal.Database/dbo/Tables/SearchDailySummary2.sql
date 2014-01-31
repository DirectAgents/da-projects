CREATE TABLE [dbo].[SearchDailySummary2] (
    [SearchCampaignId] INT       NOT NULL,
    [Date]             DATETIME  NOT NULL,
    [Revenue]          MONEY     NOT NULL,
    [Cost]             MONEY     NOT NULL,
    [Orders]           INT       NOT NULL,
    [Clicks]           INT       NOT NULL,
    [Impressions]      INT       NOT NULL,
    [CurrencyId]       INT       NOT NULL,
    [Network]          NCHAR (1) NOT NULL,
    [Device]           NCHAR (1) NOT NULL,
    [ClickType]        NCHAR (1) NOT NULL,
    CONSTRAINT [PK_SearchDailySummary2] PRIMARY KEY CLUSTERED ([SearchCampaignId] ASC, [Date] ASC, [Network] ASC, [Device] ASC, [ClickType] ASC),
    CONSTRAINT [FK_SearchDailySummary2_SearchCampaign] FOREIGN KEY ([SearchCampaignId]) REFERENCES [dbo].[SearchCampaign] ([SearchCampaignId])
);

