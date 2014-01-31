CREATE TABLE [dbo].[SearchChannel] (
    [SearchChannelId] INT           IDENTITY (1, 1) NOT NULL,
    [Prefix]          NVARCHAR (5)  NULL,
    [Name]            NVARCHAR (50) NULL,
    [Device]          NVARCHAR (5)  NULL,
    CONSTRAINT [PK_SearchChannel] PRIMARY KEY CLUSTERED ([SearchChannelId] ASC)
);

