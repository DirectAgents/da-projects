CREATE TABLE [dbo].[GoogleAnalyticsSummary] (
    [SearchCampaignId] INT   NOT NULL,
    [Date]             DATE  NOT NULL,
    [Transactions]     INT   NOT NULL,
    [Revenue]          MONEY NOT NULL,
    CONSTRAINT [PK_GoogleAnalyticsSummary] PRIMARY KEY CLUSTERED ([SearchCampaignId] ASC, [Date] ASC),
    CONSTRAINT [FK_GoogleAnalyticsSummary_SearchCampaign] FOREIGN KEY ([SearchCampaignId]) REFERENCES [dbo].[SearchCampaign] ([SearchCampaignId])
);

