CREATE TABLE [dbo].[Metric] (
    [id]   INT           IDENTITY (1, 1) NOT NULL,
    [name] NVARCHAR (50) NOT NULL,
    CONSTRAINT [PK_Metric] PRIMARY KEY CLUSTERED ([id] ASC)
);

