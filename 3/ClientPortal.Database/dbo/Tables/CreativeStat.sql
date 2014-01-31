CREATE TABLE [dbo].[CreativeStat] (
    [CreativeStatId] INT IDENTITY (1, 1) NOT NULL,
    [CampaignDropId] INT NOT NULL,
    [CreativeId]     INT NOT NULL,
    [Clicks]         INT NULL,
    [Leads]          INT NULL,
    CONSTRAINT [PK_CreativeStat] PRIMARY KEY CLUSTERED ([CreativeStatId] ASC),
    CONSTRAINT [FK_CreativeStat_CampaignDrop] FOREIGN KEY ([CampaignDropId]) REFERENCES [dbo].[CampaignDrop] ([CampaignDropId]),
    CONSTRAINT [FK_CreativeStat_Creative] FOREIGN KEY ([CreativeId]) REFERENCES [dbo].[Creative] ([CreativeId])
);

