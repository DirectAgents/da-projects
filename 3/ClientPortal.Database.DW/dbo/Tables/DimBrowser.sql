CREATE TABLE [dbo].[DimBrowser] (
    [BrowserKey]  INT            NOT NULL,
    [BrowserName] NVARCHAR (250) NOT NULL,
    CONSTRAINT [PK_DimBrowser] PRIMARY KEY CLUSTERED ([BrowserKey] ASC)
);

