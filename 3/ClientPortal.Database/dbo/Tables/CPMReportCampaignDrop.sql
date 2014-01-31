CREATE TABLE [dbo].[CPMReportCampaignDrop] (
    [CPMReportId]    INT NOT NULL,
    [CampaignDropId] INT NOT NULL,
    CONSTRAINT [PK_CPMReportCampaignDrop] PRIMARY KEY CLUSTERED ([CPMReportId] ASC, [CampaignDropId] ASC),
    CONSTRAINT [FK_CPMReportCampaignDrop_CampaignDrop] FOREIGN KEY ([CampaignDropId]) REFERENCES [dbo].[CampaignDrop] ([CampaignDropId]),
    CONSTRAINT [FK_CPMReportCampaignDrop_CPMReport] FOREIGN KEY ([CPMReportId]) REFERENCES [dbo].[CPMReport] ([CPMReportId])
);

