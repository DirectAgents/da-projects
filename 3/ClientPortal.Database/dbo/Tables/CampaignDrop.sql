CREATE TABLE [dbo].[CampaignDrop] (
    [CampaignDropId] INT             IDENTITY (1, 1) NOT NULL,
    [CampaignId]     INT             NOT NULL,
    [Date]           DATETIME        NOT NULL,
    [Cost]           DECIMAL (18, 2) NULL,
    [Volume]         INT             NULL,
    [Opens]          INT             NULL,
    [Subject]        NVARCHAR (255)  NULL,
    [CopyOf]         INT             NULL,
    [FromEmail]      NVARCHAR (100)  NULL,
    CONSTRAINT [PK_CampaignDrop] PRIMARY KEY CLUSTERED ([CampaignDropId] ASC),
    CONSTRAINT [FK_CampaignDrop_Campaign] FOREIGN KEY ([CampaignId]) REFERENCES [dbo].[Campaign] ([CampaignId]),
    CONSTRAINT [FK_CampaignDrop_CampaignDrop] FOREIGN KEY ([CopyOf]) REFERENCES [dbo].[CampaignDrop] ([CampaignDropId])
);

