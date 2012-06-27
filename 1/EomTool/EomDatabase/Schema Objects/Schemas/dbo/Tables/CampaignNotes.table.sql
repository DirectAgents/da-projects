CREATE TABLE [dbo].[CampaignNotes] (
    [id]                   INT           IDENTITY (1, 1) NOT NULL,
    [campaign_id]          INT           NOT NULL,
    [note]                 VARCHAR (MAX) NOT NULL,
    [added_by_system_user] VARCHAR (255) NULL,
    [created]              DATETIME      DEFAULT (getdate()) NOT NULL
);

