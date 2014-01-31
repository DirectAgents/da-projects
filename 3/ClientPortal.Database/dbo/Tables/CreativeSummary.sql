CREATE TABLE [dbo].[CreativeSummary] (
    [CreativeId]  INT      NOT NULL,
    [Date]        DATETIME NOT NULL,
    [Views]       INT      NOT NULL,
    [Clicks]      INT      NOT NULL,
    [Conversions] INT      NOT NULL,
    CONSTRAINT [PK_CreativeSummary] PRIMARY KEY CLUSTERED ([CreativeId] ASC, [Date] ASC)
);

